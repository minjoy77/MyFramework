﻿using System;
using System.Collections.Generic;

// 状态组,状态组可以指定哪些状态是互斥的,如果是互斥的,则添加属于该组的状态时,会移除已有的该组中的所有状态
public class StateGroup : FrameBase
{
	public HashSet<Type> mStateList;	// 组中的状态类型
	public StateGroupMutex mMutex;		// 该组中的状态是否可以共存
	public Type mMainState;				// 主状态类型
	public StateGroup()
	{
		mStateList = new HashSet<Type>();
	}
	public override void resetProperty()
	{
		base.resetProperty();
		mStateList.Clear();
		mMutex = null;
		mMainState = null;
	}
	public void destroy()
	{
		UN_CLASS(mMutex);
		mMutex = null;
	}
	public void setMutex(GROUP_MUTEX mutex)
	{
		mMutex = CLASS(mStateManager.getGroupMutex(mutex)) as StateGroupMutex;
		mMutex.setMutexType(mutex);
		mMutex.setGroup(this);
	}
	public void setMainState(Type type)
	{
		if (mMainState != null)
		{
			logError("state group's main state is not empty!");
			return;
		}
		mMainState = type;
	}
	public void addState(Type type) { mStateList.Add(type); }
	public bool hasState(Type type) { return mStateList.Contains(type); }
	public bool allowKeepState(Type newState, Type existState) { return mMutex.allowKeepState(newState, existState); }
	public bool allowAddState(Type newState, Type existState) { return mMutex.allowAddState(newState, existState); }
}