﻿using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

// 从AssetDataBase中加载资源
public class AssetDataBaseLoader : FrameBase
{
	protected Dictionary<string, Dictionary<string, ResourceLoadInfo>> mLoadedPath;		// 所有已加载的文件夹
	protected Dictionary<Object, ResourceLoadInfo> mLoadedObjects;						// 所有的已加载的资源
	public AssetDataBaseLoader()
	{
		mLoadedPath = new Dictionary<string, Dictionary<string, ResourceLoadInfo>>();
		mLoadedObjects = new Dictionary<Object, ResourceLoadInfo>();
	}
	public void destroy(){}
	public override void resetProperty()
	{
		base.resetProperty();
		mLoadedPath.Clear();
		mLoadedObjects.Clear();
	}
	public void getFileList(string path, List<string> list)
	{
		List<string> fileList = findResourcesFilesNonAlloc(path);
		// 去除meta文件
		list.Clear();
		int count = fileList.Count;
		for(int i = 0; i < count; ++i)
		{
			string item = fileList[i];
			if (!item.EndsWith(".meta"))
			{
				list.Add(getFileNameNoSuffix(item));
			}
		}
	}
	public bool unloadResource<T>(ref T obj) where T : Object
	{
		if(obj == null)
		{
			return false;
		}
		// 资源已经加载完
		if(!mLoadedObjects.TryGetValue(obj, out ResourceLoadInfo info))
		{
			logWarning("无法卸载资源:" + obj.name + ", 可能未加载,或者已经卸载,或者该资源是子资源,或者正在异步加载");
			return false;
		}
		if (!(obj is GameObject))
		{
			Resources.UnloadAsset(obj);
		}
		mLoadedPath[info.mPath].Remove(info.mResouceName);
		UN_CLASS(info);
		mLoadedObjects.Remove(obj);
		obj = null;
		return true;
	}
	// 卸载指定路径中的所有资源
	public void unloadPath(string path)
	{
		LIST(out List<string> tempList);
		tempList.AddRange(mLoadedPath.Keys);
		int count = tempList.Count;
		for(int i = 0; i < count; ++i)
		{
			string item0 = tempList[i];
			if (!startWith(item0, path))
			{
				continue;
			}
#if UNITY_EDITOR || DEVELOPMENT_BUILD
			log("unload path: " + item0);
#endif
			var list = mLoadedPath[item0];
			foreach (var item in list)
			{
				ResourceLoadInfo info = item.Value;
				if (info.mObject != null)
				{
					mLoadedObjects.Remove(info.mObject);
					if (!(info.mObject is GameObject))
					{
						Resources.UnloadAsset(info.mObject);
					}
					info.mObject = null;
				}
				UN_CLASS(info);
			}
			mLoadedPath.Remove(item0);
		}
		UN_LIST(tempList);
	}
	public bool isResourceLoaded(string name)
	{
		string path = getFilePath(name);
		if (mLoadedPath.TryGetValue(path, out Dictionary<string, ResourceLoadInfo> resList))
		{
			return resList.ContainsKey(name);
		}
		return false;
	}
	public Object getResource(string name)
	{
		string path = getFilePath(name);
		if (mLoadedPath.TryGetValue(path, out Dictionary<string, ResourceLoadInfo> resList) && 
			resList.TryGetValue(name, out ResourceLoadInfo info))
		{
			return info.mObject;
		}
		return null;
	}
	// 同步加载资源,name为Resources下的相对路径,不带后缀
	public Object[] loadSubResource<T>(string name) where T : Object
	{
		string path = getFilePath(name);
		// 如果文件夹还未加载,则添加文件夹
		if (!mLoadedPath.TryGetValue(path, out Dictionary<string, ResourceLoadInfo> resList))
		{
			resList = new Dictionary<string, ResourceLoadInfo>();
			mLoadedPath.Add(path, resList);
		}
		// 资源未加载,则使用Resources.Load加载资源
		if (!resList.TryGetValue(name, out ResourceLoadInfo info))
		{
			if (!load<T>(path, name))
			{
				return null;
			}
			// 加载后需要重新获取一次
			info = resList[name];
			return info.mSubObjects;
		}
		if (info.mState == LOAD_STATE.LOADED)
		{
			return info.mSubObjects;
		}
		else if (info.mState == LOAD_STATE.LOADING)
		{
			logForce("资源正在后台加载,不能同步加载!" + name);
		}
		else if (info.mState == LOAD_STATE.UNLOAD)
		{
			logForce("资源已加入列表,但是未加载" + name);
		}
		return null;
	}
	// 同步加载资源,name为Resources下的相对路径,不带后缀
	public T loadResource<T>(string name) where T : Object
	{
		string path = getFilePath(name);
		// 如果文件夹还未加载,则添加文件夹
		if (!mLoadedPath.TryGetValue(path, out Dictionary<string, ResourceLoadInfo> resList))
		{
			resList = new Dictionary<string, ResourceLoadInfo>();
			mLoadedPath.Add(path, resList);
		}
		// 资源未加载,则使用Resources.Load加载资源
		if (!resList.TryGetValue(name, out ResourceLoadInfo info))
		{
			if (!load<T>(path, name))
			{
				return null;
			}
			// 加载后需要重新获取一次
			info = resList[name];
			return info.mObject as T;
		}
		if(info.mState == LOAD_STATE.LOADED)
		{
			return info.mObject as T;
		}
		else if(info.mState == LOAD_STATE.LOADING)
		{
			logWarning("资源正在后台加载,不能同步加载!" + name);
		}
		else if(info.mState == LOAD_STATE.UNLOAD)
		{
			logWarning("资源已加入列表,但是未加载" + name);
		}
		return null;
	}
	// 异步加载资源,name为GameResources下的相对路径,不带后缀
	public bool loadResourcesAsync<T>(string name, AssetLoadDoneCallback doneCallback, object userData = null) where T : Object
	{
		string path = getFilePath(name);
		// 如果文件夹还未加载,则添加文件夹
		if (!mLoadedPath.TryGetValue(path, out Dictionary<string, ResourceLoadInfo> resList))
		{
			resList = new Dictionary<string, ResourceLoadInfo>();
			mLoadedPath.Add(path, resList);
		}
		// 已经加载,则返回true
		if (resList.TryGetValue(name, out ResourceLoadInfo info))
		{
			// 资源正在下载,将回调添加到回调列表中,等待下载完毕
			if (info.mState == LOAD_STATE.LOADING)
			{	
				info.addCallback(doneCallback, userData, name);
			}
			// 资源已经下载完毕,直接调用回调
			else if(info.mState == LOAD_STATE.LOADED)
			{
				doneCallback(info.mObject, info.mSubObjects, null, userData, name);
			}
		}
		// 还没有加载则开始异步加载
		else
		{
			CLASS(out info);
			info.mPath = path;
			info.mResouceName = name;
			info.mState = LOAD_STATE.LOADING;
			info.addCallback(doneCallback, userData, name);
			resList.Add(name, info);
			mGameFramework.StartCoroutine(loadResourceCoroutine<T>(info));
		}
		return true;
	}
	//------------------------------------------------------------------------------------------------------------------------------
	protected bool load<T>(string path, string name) where T : Object
	{
		var resList = mLoadedPath[path];
		if (resList.ContainsKey(name))
		{
			return true;
		}
		CLASS(out ResourceLoadInfo info);
		info.mPath = path;
		info.mResouceName = name;
		info.mState = LOAD_STATE.LOADING;
		resList.Add(info.mResouceName, info);
#if UNITY_EDITOR
		LIST(out List<string> fileNameList);
		mResourceManager.adjustResourceName<T>(name, fileNameList, false);
		int fileCount = fileNameList.Count;
		for(int i = 0; i < fileCount; ++i)
		{
			string filePath = FrameDefine.P_GAME_RESOURCES_PATH + fileNameList[i];
			if (isFileExist(filePath))
			{
				info.mObject = AssetDatabase.LoadAssetAtPath<T>(filePath);
				info.mSubObjects = AssetDatabase.LoadAllAssetsAtPath(filePath);
				break;
			}
		}
		UN_LIST(fileNameList);
#else
		info.mObject = Resources.Load(name, Typeof<T>());
		info.mSubObjects = Resources.LoadAll(name);
#endif
		info.mState = LOAD_STATE.LOADED;
		if (info.mObject == null)
		{
			// 加载失败则从列表中移除
			logWarning("资源加载失败:" + name);
			resList.Remove(info.mResouceName);
			UN_CLASS(info);
			return false;
		}
		mLoadedObjects.Add(info.mObject, info);
		return true;
	}
	protected IEnumerator loadResourceCoroutine<T>(ResourceLoadInfo info) where T : Object
	{
#if UNITY_EDITOR
		LIST(out List<string> fileNameList);
		mResourceManager.adjustResourceName<T>(info.mResouceName, fileNameList, false);
		int fileCount = fileNameList.Count;
		for (int i = 0; i < fileCount; ++i)
		{
			string filePath = FrameDefine.P_GAME_RESOURCES_PATH + fileNameList[i];
			if (isFileExist(filePath))
			{
				info.mObject = AssetDatabase.LoadAssetAtPath<T>(filePath);
				info.mSubObjects = AssetDatabase.LoadAllAssetsAtPath(filePath);
				break;
			}
		}
		UN_LIST(fileNameList);
		yield return null;
#else
		ResourceRequest request = Resources.LoadAsync<T>(info.mResouceName);
		yield return request;
		info.mObject = request.asset;
#endif
		info.mState = LOAD_STATE.LOADED;
		if(info.mObject != null)
		{
			mLoadedObjects.Add(info.mObject, info);
			info.callbackAll(info.mObject, info.mSubObjects, null);
		}
		else
		{
			// 加载失败则从列表中移除
			logWarning("资源加载失败:" + info.mResouceName);
			mLoadedPath[info.mPath].Remove(info.mResouceName);
			info.callbackAll(info.mObject, info.mSubObjects, null);
			UN_CLASS(info);
		}
	}
}