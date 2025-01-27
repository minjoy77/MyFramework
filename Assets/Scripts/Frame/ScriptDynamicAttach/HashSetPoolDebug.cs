﻿using System.Collections.Generic;
using UnityEngine;

// HashSet对象池的调试信息
public class HashSetPoolDebug : MonoBehaviour
{
	public List<string> PersistentInuseList = new List<string>();	// 持久使用的列表
	public List<string> InuseList = new List<string>();				// 单帧使用的列表
	public List<string> UnuseList = new List<string>();				// 未使用列表
	public void Update()
	{
		if (!FrameBase.mGameFramework.mEnableScriptDebug)
		{
			return;
		}
		PersistentInuseList.Clear();
		var persistentInuse = FrameBase.mHashSetPool.getPersistentInusedList();
		foreach (var item in persistentInuse)
		{
			if (item.Value.Count > 0)
			{
				PersistentInuseList.Add(item.Key.ToString() + ", 数量:" + StringUtility.IToS(item.Value.Count));
			}
		}

		InuseList.Clear();
		var inuse = FrameBase.mHashSetPool.getInusedList();
		foreach(var item in inuse)
		{
			if (item.Value.Count > 0)
			{
				InuseList.Add(item.Key.ToString() + ", 数量:" + StringUtility.IToS(item.Value.Count));
			}
		}

		UnuseList.Clear();
		var unuse = FrameBase.mHashSetPool.getUnusedList();
		foreach (var item in unuse)
		{
			if (item.Value.Count > 0)
			{
				UnuseList.Add(item.Key.ToString() + ", 数量:" + StringUtility.IToS(item.Value.Count));
			}
		}
	}
}