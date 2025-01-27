﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

// 3D场景管理器,管理unity场景资源
public class SceneSystem : FrameSystem
{
	protected Dictionary<Type,List<SceneRegisteInfo>> mScriptMappingList;	// 场景脚本类型与场景注册信息的映射,允许多个相似的场景共用同一个场景脚本
	protected Dictionary<string, SceneRegisteInfo> mSceneRegisteList;		// 场景注册信息
	protected Dictionary<string, SceneInstance> mSceneList;					// 已经加载的所有场景
	protected HashSet<SceneScriptCallback> mSceneScriptCallbackList;		// 脚本回调列表
	protected List<SceneInstance> mLoadList;								// 即将加载的场景列表
	protected AssetBundleLoadCallback mSceneLoadCallback;					// 避免GC的委托
	protected WaitForEndOfFrame mWaitEndFrame;								// 用于避免GC
	public SceneSystem()
	{
		mScriptMappingList = new Dictionary<Type, List<SceneRegisteInfo>>();
		mSceneRegisteList = new Dictionary<string, SceneRegisteInfo>();
		mSceneList = new Dictionary<string, SceneInstance>();
		mSceneScriptCallbackList = new HashSet<SceneScriptCallback>();
		mLoadList = new List<SceneInstance>();
		mWaitEndFrame = new WaitForEndOfFrame();
		mSceneLoadCallback = onSceneAssetBundleLoaded;
	}
	public override void destroy()
	{
		base.destroy();
		foreach (var item in mSceneList)
		{
			unloadSceneOnly(item.Key);
		}
		mSceneList.Clear();
	}
	public override void update(float elapsedTime)
	{
		base.update(elapsedTime);
		// 场景AssetBundle加载完毕时才开始加载场景
		for (int i = 0; i < mLoadList.Count; ++i)
		{
			if (mLoadList[i].mState == LOAD_STATE.UNLOAD)
			{
				mGameFramework.StartCoroutine(loadSceneCoroutine(mLoadList[i]));
			}
			else if (mLoadList[i].mState == LOAD_STATE.LOADED)
			{
				mLoadList.RemoveAt(i--);
			}
		}
		foreach (var item in mSceneList)
		{
			if (item.Value.getActive())
			{
				item.Value.update(elapsedTime);
			}
		}
	}
	public void addScriptCallback(SceneScriptCallback callback)
	{
		if (!mSceneScriptCallbackList.Contains(callback))
		{
			mSceneScriptCallbackList.Add(callback);
		}
	}
	public void removeScriptCallback(SceneScriptCallback callback)
	{
		mSceneScriptCallbackList.Remove(callback);
	}
	// filePath是场景文件所在目录,不含场景名,最好该目录下包含所有只在这个场景使用的资源
	public void registeScene(Type type, string name, string filePath)
	{
		// 路径需要以/结尾
		validPath(ref filePath);
		SceneRegisteInfo info = new SceneRegisteInfo();
		info.mName = name;
		info.mScenePath = filePath;
		info.mSceneType = type;
		mSceneRegisteList.Add(name, info);
		if (!mScriptMappingList.TryGetValue(type,out List<SceneRegisteInfo> list))
		{
			list = new List<SceneRegisteInfo>();
			mScriptMappingList.Add(type, list);
		}
		list.Add(info);
	}
	public string getScenePath(string name)
	{
		if (mSceneRegisteList.TryGetValue(name, out SceneRegisteInfo info))
		{
			return info.mScenePath;
		}
		return EMPTY;
	}
	public T getScene<T>(string name) where T : SceneInstance
	{
		mSceneList.TryGetValue(name, out SceneInstance scene);
		return scene as T;
	}
	public int getScriptMappingCount(Type classType)
	{
		return mScriptMappingList[classType].Count;
	}
	public void setMainScene(string name)
	{
		if (!mSceneList.TryGetValue(name, out SceneInstance scene))
		{
			return;
		}
		SceneManager.SetActiveScene(scene.mScene);
	}
	public void hideScene(string name)
	{
		if (!mSceneList.TryGetValue(name, out SceneInstance scene))
		{
			return;
		}
		scene.setActive(false);
		scene.onHide();
	}
	public void showScene(string name, bool hideOther = true, bool mainScene = true)
	{
		if (!mSceneList.TryGetValue(name, out SceneInstance scene))
		{
			return;
		}
		// 如果需要隐藏其他场景,则遍历所有场景设置可见性
		if (hideOther)
		{
			foreach (var item in mSceneList)
			{
				item.Value.setActive(name == item.Key);
				if (name == item.Key)
				{
					item.Value.onShow();
				}
				else
				{
					item.Value.onHide();
				}
			}
		}
		// 不隐藏其他场景则只是简单的将指定场景显示
		else
		{
			scene.setActive(true);
			scene.onShow();
		}
		if (mainScene)
		{
			setMainScene(name);
		}
	}
	// 目前只支持异步加载,因为SceneManager.LoadScene并不是真正地同步加载
	// 该方法只能保证在这一帧结束后场景能加载完毕,但是函数返回后场景并没有加载完毕
	public void loadSceneAsync(string sceneName, bool active, Action<float, bool> callback)
	{
		// 如果场景已经加载,则直接返回
		if (mSceneList.ContainsKey(sceneName))
		{
			showScene(sceneName);
			if (callback != null)
			{
				delayCall(callback, 1.0f, true);
			}
			return;
		}
		SceneInstance scene = createScene(sceneName);
		scene.mState = LOAD_STATE.UNLOAD;
		scene.mActiveLoaded = active;
		scene.mLoadCallback = callback;
		mSceneList.Add(scene.mName, scene);
		// scenePath + sceneName表示场景文件AssetBundle的路径,包含文件名
		mResourceManager.loadAssetBundleAsync(getScenePath(sceneName) + sceneName, mSceneLoadCallback, scene);
	}
	// unloadPath表示是否将场景所属文件夹的所有资源卸载
	public void unloadScene(string name, bool unloadPath = true)
	{
		// 销毁场景,并且从列表中移除
		unloadSceneOnly(name);
		mSceneList.Remove(name);
		if (unloadPath)
		{
			mResourceManager.unloadPath(mSceneRegisteList[name].mScenePath);
		}
	}
	// 卸载除了dontUnloadSceneName以外的其他场景,初始默认场景除外
	public void unloadOtherScene(string dontUnloadSceneName)
	{
		var tempList = new Dictionary<string, SceneInstance>(mSceneList);
		foreach (var item in tempList)
		{
			if (item.Key != dontUnloadSceneName)
			{
				unloadScene(item.Key);
			}
		}
	}
	//------------------------------------------------------------------------------------------------------------------------------
	protected void onSceneAssetBundleLoaded(AssetBundleInfo bundle, object userData)
	{
		mLoadList.Add(userData as SceneInstance);
	}
	protected IEnumerator loadSceneCoroutine(SceneInstance scene)
	{
		scene.mState = LOAD_STATE.LOADING;
		// 所有场景都只能使用叠加的方式来加载,方便场景管理器来管理所有场景的加载和卸载
		scene.mOperation = SceneManager.LoadSceneAsync(scene.mName, LoadSceneMode.Additive);
		// allowSceneActivation指定了加载场景时是否需要调用场景中所有脚本的Awake和Start,以及贴图材质的引用等等
		scene.mOperation.allowSceneActivation = true;
		while (true)
		{
			scene.mLoadCallback?.Invoke(scene.mOperation.progress, false);
			// 当allowSceneActivation为true时,加载到progress为1时停止,并且isDone为true,scene.isLoaded为true
			// 当allowSceneActivation为false时,加载到progress为0.9时就停止,并且isDone为false, scene.isLoaded为false
			// 当场景被激活时isDone变为true,progress也为1,scene.isLoaded为true
			if (scene.mOperation.isDone || scene.mOperation.progress >= 1.0f)
			{
				break;
			}
			yield return mWaitEndFrame;
		}
		// 首先获得场景
		scene.mScene = SceneManager.GetSceneByName(scene.mName);
		// 获得了场景根节点才能使场景显示或隐藏,为了尽量避免此处查找节点错误,所以不能使用容易重名的名字
		scene.mRoot = getGameObject(scene.mName + "_Root", true, false);
		// 加载完毕后就立即初始化
		scene.init();
		if (scene.mActiveLoaded)
		{
			showScene(scene.mName);
		}
		else
		{
			hideScene(scene.mName);
		}
		scene.mState = LOAD_STATE.LOADED;
		scene.mLoadCallback?.Invoke(1.0f, true);
	}
	protected SceneInstance createScene(string sceneName)
	{
		if (!mSceneRegisteList.TryGetValue(sceneName, out SceneRegisteInfo info))
		{
			logError("scene :" + sceneName + " is not registed!");
			return null;
		}
		SceneInstance scene = createInstance<SceneInstance>(info.mSceneType);
		scene.setName(sceneName);
		scene.setType(info.mSceneType);
		notifySceneChanged(scene, true);
		return scene;
	}
	// 只销毁场景,不从列表移除
	protected void unloadSceneOnly(string name)
	{
		if (!mSceneList.TryGetValue(name, out SceneInstance scene))
		{
			return;
		}
		notifySceneChanged(scene, false);
		scene.destroy();
		SceneManager.UnloadSceneAsync(name);
	}
	protected void notifySceneChanged(SceneInstance scene, bool isLoad)
	{
		foreach(var item in mSceneScriptCallbackList)
		{
			item.Invoke(scene, isLoad);
		}
	}
}