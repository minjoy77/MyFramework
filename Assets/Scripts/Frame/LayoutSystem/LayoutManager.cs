﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct LayoutAsyncInfo
{
	public LayoutAsyncDone mCallback;
	public GameLayout mLayout;
	public GameObject mLayoutObject;
	public LAYOUT_ORDER mOrderType;
	public GUI_TYPE mGUIType;
	public string mName;
	public bool mIsScene;
	public int mRenderOrder;
	public int mID;
}

public class LayoutManager : FrameSystem
{
	protected Dictionary<string, LayoutAsyncInfo> mLayoutAsyncList;
	protected Dictionary<Type, List<int>> mScriptMappingList;
	protected Dictionary<int, string> mLayoutTypeToName;
	protected Dictionary<string, int> mLayoutNameToType;
	protected Dictionary<int, Type> mScriptRegisteList;
	protected Dictionary<int, GameLayout> mLayoutList;
	protected List<GameLayout> mBackBlurLayoutList;                 // 需要背景模糊的布局的列表
#if USE_NGUI
	protected myNGUIObject mNGUIRoot;
	protected UIRoot mNGUIRootComponent;
#endif
	protected myUGUICanvas mUGUIRoot;
	protected bool mUseAnchor;           // 是否启用锚点来自动调节窗口的大小和位置
	public LayoutManager()
	{
		mUseAnchor = true;
		mScriptMappingList = new Dictionary<Type, List<int>>();
		mScriptRegisteList = new Dictionary<int, Type>();
		mLayoutTypeToName = new Dictionary<int, string>();
		mLayoutNameToType = new Dictionary<string, int>();
		mLayoutList = new Dictionary<int, GameLayout>();
		mLayoutAsyncList = new Dictionary<string, LayoutAsyncInfo>();
		mBackBlurLayoutList = new List<GameLayout>();
		// 在构造中获取UI根节点,确保其他组件能在任意时刻正常访问
		mUGUIRoot = LayoutScript.newUIObject<myUGUICanvas>(null, null, getGameObject(FrameDefine.UGUI_ROOT, true));
#if USE_NGUI
		mNGUIRoot = LayoutScript.newUIObject<myNGUIObject>(null, null, getGameObject(FrameDefine.NGUI_ROOT, true));
		mNGUIRootComponent = mNGUIRoot.getUnityComponent<UIRoot>(false);
#endif
	}
#if USE_NGUI
	public new UIRoot getNGUIRootComponent() { return mNGUIRootComponent; }
#endif
	public new Canvas getUGUIRootComponent() { return mUGUIRoot.getCanvas(); }
	public myUIObject getUIRoot(GUI_TYPE guiType)
	{
		if (guiType == GUI_TYPE.NGUI)
		{
#if USE_NGUI
			return mNGUIRoot;
#endif
		}
		if (guiType == GUI_TYPE.UGUI)
		{
			return mUGUIRoot;
		}
		return null;
	}
	public GameObject getRootObject(GUI_TYPE guiType)
	{
		myUIObject root = getUIRoot(guiType);
		return root?.getObject();
	}
	public void notifyLayoutVisible(bool visible, GameLayout layout) 
	{
		if(visible)
		{
			if (layout.isBlurBack())
			{
				mBackBlurLayoutList.Add(layout);
			}
			// 显示布局时,如果当前正在显示有背景模糊的布局,则需要判断当前布局是否需要模糊
			if (mBackBlurLayoutList.Count > 0)
			{
				CommandLayoutManagerBackBlur cmd = newMainCmd(out cmd, false);
				cmd.mExcludeLayout = mBackBlurLayoutList;
				cmd.mBlur = mBackBlurLayoutList.Count > 0;
				cmd.mGUIType = layout.getGUIType();
				pushCommand(cmd, this);
			}
		}
		else
		{
			if (layout.isBlurBack())
			{
				mBackBlurLayoutList.Remove(layout);
			}
			CommandLayoutManagerBackBlur cmd = newMainCmd(out cmd, false);
			cmd.mExcludeLayout = mBackBlurLayoutList;
			cmd.mBlur = mBackBlurLayoutList.Count > 0;
			cmd.mGUIType = layout.getGUIType();
			pushCommand(cmd, this);
			// 布局在隐藏时都需要确认设置层为UI层
			setGameObjectLayer(layout.getRoot().getObject(), layout.getDefaultLayer());
		}
	}
	public void setUseAnchor(bool useAnchor) { mUseAnchor = useAnchor; }
	public bool isUseAnchor() { return mUseAnchor; }
	public override void update(float elapsedTime)
	{
		base.update(elapsedTime);
		foreach (var layout in mLayoutList)
		{
			UnityProfiler.BeginSample(layout.Value.getName());
			layout.Value.update(elapsedTime);
			UnityProfiler.EndSample();
		}
	}
	public override void onDrawGizmos()
	{
		foreach (var layout in mLayoutList)
		{
			layout.Value.onDrawGizmos();
		}
	}
	public override void lateUpdate(float elapsedTime)
	{
		base.lateUpdate(elapsedTime);
		foreach (var item in mLayoutList)
		{
			item.Value.lateUpdate(elapsedTime);
		}
	}
	public override void destroy()
	{
		foreach (var item in mLayoutList)
		{
			item.Value.destroy();
		}
		mLayoutList.Clear();
		mLayoutTypeToName.Clear();
		mLayoutNameToType.Clear();
		mLayoutAsyncList.Clear();
		// 销毁UI摄像机
		mCameraManager.destroyCamera(mCameraManager.getUICamera(GUI_TYPE.NGUI));
		mCameraManager.destroyCamera(mCameraManager.getUICamera(GUI_TYPE.UGUI));
#if USE_NGUI
		myUIObject.destroyWindowSingle(mNGUIRoot, false);
		mNGUIRoot = null;
#endif
		myUIObject.destroyWindowSingle(mUGUIRoot, false);
		mUGUIRoot = null;
		base.destroy();
	}
	public string getLayoutNameByType(int type)
	{
		if (!mLayoutTypeToName.ContainsKey(type))
		{
			logError("can not find LayoutType: " + type);
			return null;
		}
		return mLayoutTypeToName[type];
	}
	public int getLayoutTypeByName(string name)
	{
		if (!mLayoutNameToType.ContainsKey(name))
		{
			logError("can not  find LayoutName:" + name);
			return LAYOUT.NONE;
		}
		return mLayoutNameToType[name];
	}
	public GameLayout getGameLayout(int id)
	{
		return mLayoutList.ContainsKey(id) ? mLayoutList[id] : null;
	}
	public Dictionary<int, GameLayout> getLayoutList() { return mLayoutList; }
	public LayoutScript getScript(int id)
	{
		GameLayout layout = getGameLayout(id);
		return layout != null ? layout.getScript() : null;
	}
	public int getScriptMappingCount(Type classType)
	{
		return mScriptMappingList[classType].Count;
	}
	// 根据顺序类型,计算实际的渲染顺序
	public int generateRenderOrder(int renderOrder, LAYOUT_ORDER orderType)
	{
		if (orderType == LAYOUT_ORDER.ALWAYS_TOP)
		{
			if (renderOrder < FrameDefine.ALWAYS_TOP_ORDER)
			{
				renderOrder += FrameDefine.ALWAYS_TOP_ORDER;
			}
		}
		else if (orderType == LAYOUT_ORDER.ALWAYS_TOP_AUTO)
		{
			renderOrder = getTopLayoutOrder(true) + 1;
		}
		else if (orderType == LAYOUT_ORDER.AUTO)
		{
			renderOrder = getTopLayoutOrder(false) + 1;
		}
		return renderOrder;
	}
	public GameLayout createLayout(int id, int renderOrder, LAYOUT_ORDER orderType, bool async, LayoutAsyncDone callback, GUI_TYPE guiType, bool isScene)
	{
		if (mLayoutList.ContainsKey(id))
		{
			if (async && callback != null)
			{
				callback(mLayoutList[id]);
				return null;
			}
			return mLayoutList[id];
		}
		string name = getLayoutNameByType(id);
		string path = "";
		if(guiType == GUI_TYPE.NGUI)
		{
			path = FrameDefine.R_NGUI_PREFAB_PATH;
		}
		else if(guiType == GUI_TYPE.UGUI)
		{
			path = FrameDefine.R_UGUI_PREFAB_PATH;
		}
		GameObject layoutParent = getRootObject(guiType);
		if (isScene)
		{
			layoutParent = null;
		}
		string fullPath = path + name + "/" + name;
		// 如果是异步加载则,则先加入列表中
		if (async)
		{
			LayoutAsyncInfo info = new LayoutAsyncInfo();
			info.mName = name;
			info.mID = id;
			info.mRenderOrder = renderOrder;
			info.mLayout = null;
			info.mLayoutObject = null;
			info.mGUIType = guiType;
			info.mIsScene = isScene;
			info.mCallback = callback;
			info.mOrderType = orderType;
			mLayoutAsyncList.Add(info.mName, info);
			bool ret = mResourceManager.loadResourceAsync<GameObject>(fullPath, onLayoutPrefabAsyncDone, null, true);
			if (!ret)
			{
				logError("can not find layout : " + name);
			}
			return null;
		}
		else
		{
			GameObject prefab = mResourceManager.loadResource<GameObject>(fullPath, true);
			instantiatePrefab(layoutParent, prefab, name, true);
			GameLayout layout = new GameLayout();
			layout.setPrefab(prefab);
			addLayoutToList(layout, id);
			layout.setID(id);
			layout.setName(name);
			layout.setGUIType(guiType);
			layout.setIsScene(isScene);
			layout.init(renderOrder, orderType);
			return layout;
		}
	}
	public void destroyLayout(int id)
	{
		GameLayout layout = getGameLayout(id);
		if (layout == null)
		{
			return;
		}
		removeLayoutFromList(layout);
		layout.destroy();
	}
	public LayoutScript createScript(GameLayout layout)
	{
		Type type = mScriptRegisteList[layout.getID()];
		LayoutScript script = createInstance<LayoutScript>(type);
		script.setType(type);
		script.setLayout(layout);
		return script;
	}
	public void getAllLayoutBoxCollider(List<Collider> colliders)
	{
		colliders.Clear();
		foreach (var layout in mLayoutList)
		{
			layout.Value.getAllCollider(colliders, true);
		}
	}
	public void registeLayout(Type classType, int type, string name)
	{
		mLayoutTypeToName.Add(type, name);
		mLayoutNameToType.Add(name, type);
		mScriptRegisteList.Add(type, classType);
		if (!mScriptMappingList.ContainsKey(classType))
		{
			mScriptMappingList.Add(classType, new List<int>());
		}
		mScriptMappingList[classType].Add(type);
	}
	// 获取已注册的布局数量,而不是已加载的布局数量
	public int getLayoutCount() { return mLayoutTypeToName.Count; }
	// 获取当前已经显示的布局中最上层布局的渲染深度,但是不包括始终在最上层的布局
	public int getTopLayoutOrder(bool alwaysTop)
	{
		int maxOrder = 0;
		foreach(var item in mLayoutList)
		{
			if (!item.Value.isVisible() ||
				alwaysTop && item.Value.getRenderOrderType() != LAYOUT_ORDER.ALWAYS_TOP ||
				!alwaysTop && item.Value.getRenderOrderType() == LAYOUT_ORDER.ALWAYS_TOP)
			{
				continue;
			}
			maxOrder = getMax(maxOrder, item.Value.getRenderOrder());
		}
		// 如果没有始终在最上层的布局,则需要确保渲染顺序最低不能小于指定值
		if (alwaysTop && maxOrder == 0)
		{
			maxOrder += FrameDefine.ALWAYS_TOP_ORDER;
		}
		return maxOrder;
	}
	//----------------------------------------------------------------------------------------------------------------------------------------------------
	protected void addLayoutToList(GameLayout layout, int type)
	{
		mLayoutList.Add(type, layout);
	}
	protected void removeLayoutFromList(GameLayout layout)
	{
		if (layout == null)
		{
			return;
		}
		mLayoutList.Remove(layout.getID());
	}
	protected void onLayoutPrefabAsyncDone(UnityEngine.Object asset, UnityEngine.Object[] subAssets, byte[] bytes, object userData, string loadPath)
	{
		LayoutAsyncInfo info = mLayoutAsyncList[asset.name];
		mLayoutAsyncList.Remove(asset.name);
		info.mLayoutObject = instantiatePrefab(null, (GameObject)asset, true);
		info.mLayout = new GameLayout();
		addLayoutToList(info.mLayout, info.mID);
		info.mLayout.setPrefab(asset as GameObject);
		GameObject layoutParent = getRootObject(info.mGUIType);
		if (info.mIsScene)
		{
			layoutParent = null;
		}
		setNormalProperty(info.mLayoutObject, layoutParent, info.mName);
		info.mLayout.setID(info.mID);
		info.mLayout.setName(info.mName);
		info.mLayout.setGUIType(info.mGUIType);
		info.mLayout.setIsScene(info.mIsScene);
		info.mLayout.init(info.mRenderOrder, info.mOrderType);
		info.mCallback?.Invoke(info.mLayout);
	}
}