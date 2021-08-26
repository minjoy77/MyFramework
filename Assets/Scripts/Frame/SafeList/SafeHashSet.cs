﻿using System.Collections.Generic;

// 非线程安全
// 可安全遍历的列表,支持在遍历过程中对列表进行修改
public class SafeHashSet<T> : FrameBase
{
	protected List<SafeListModify<T>> mModifyList;  // 记录操作的列表,按顺序存储所有的操作
	protected HashSet<T> mUpdateList;               // 用于遍历更新的列表
	protected HashSet<T> mMainList;                 // 用于存储实时数据的列表
	public SafeHashSet()
	{
		mModifyList = new List<SafeListModify<T>>();
		mUpdateList = new HashSet<T>();
		mMainList = new HashSet<T>();
	}
	public override void resetProperty()
	{
		base.resetProperty();
		mModifyList.Clear();
		mUpdateList.Clear();
		mMainList.Clear();
	}
	// 获取用于更新的列表,会自动从主列表同步
	public HashSet<T> startForeach()
	{
		// 获取更新列表前,先同步主列表到更新列表,为了避免当列表过大时每次同步量太大
		// 所以单独使用了添加列表和移除列表,用来存储主列表的添加和移除的元素
		int mainCount = mMainList.Count;
		// 主列表为空,则直接清空即可
		if (mainCount == 0)
		{
			mUpdateList.Clear();
		}
		else
		{
			// 操作记录较少,则根据操作进行增删
			int modifyCount = mModifyList.Count;
			if (modifyCount < mainCount)
			{
				for (int i = 0; i < modifyCount; ++i)
				{
					var value = mModifyList[i];
					if (value.mAdd)
					{
						mUpdateList.Add(value.mValue);
					}
					else
					{
						mUpdateList.Remove(value.mValue);
					}
				}
			}
			// 主列表元素较少,则直接同步主列表到更新列表
			else
			{
				mUpdateList.Clear();
				mUpdateList.UnionWith(mMainList);
			}
		}
		if (mUpdateList.Count != mMainList.Count)
		{
			logError("同步失败");
		}
		mModifyList.Clear();
		return mUpdateList;
	}
	// 获取主列表,存储着当前实时的数据列表,所有的删除和新增都会立即更新此列表
	// 不能用主列表进行遍历,要遍历应该使用GetUpdateList,但是也需要注意避免遍历UpdateList再次GetUpdateList
	// 也不能使用主列表直接进行添加或删除
	public HashSet<T> getMainList() { return mMainList; }
	public bool contains(T value) { return mMainList.Contains(value); }
	public int count() { return mMainList.Count; }
	public bool add(T value)
	{
		if (!mMainList.Add(value))
		{
			return false;
		}
		mModifyList.Add(new SafeListModify<T>(value, true));
		return true;
	}
	public bool remove(T value)
	{
		if (!mMainList.Remove(value))
		{
			return false;
		}
		mModifyList.Add(new SafeListModify<T>(value, false));
		return true;
	}
	// 清空所有数据,不能正在遍历时调用
	public void clear()
	{
		mMainList.Clear();
		mUpdateList.Clear();
		mModifyList.Clear();
	}
}