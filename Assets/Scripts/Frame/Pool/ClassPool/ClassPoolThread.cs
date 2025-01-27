﻿using System.Collections.Generic;
using System;

// 不支持带参构造的类,因为在再次利用时参数无法正确传递
// 线程安全的对象池,但是效率较低
public class ClassPoolThread : FrameSystem
{
	protected Dictionary<Type, ClassPoolSingle> mPoolList;	// 对象池列表,对应每一个类型的对象池
	protected ThreadLock mListLock;							// 列表的线程锁
	public ClassPoolThread()
	{
		mPoolList = new Dictionary<Type, ClassPoolSingle>();
		mListLock = new ThreadLock();
		mCreateObject = true;
	}
	public override void init()
	{
		base.init();
#if UNITY_EDITOR
		mObject.AddComponent<ClassPoolThreadDebug>();
#endif
	}
	public void lockList() { mListLock.waitForUnlock(); }
	public void unlockList() { mListLock.unlock(); }
	public Dictionary<Type, ClassPoolSingle> getPoolList() { return mPoolList; }
	// 返回值表示是否是new出来的对象,false则为从回收列表中重复使用的对象
	public ClassObject newClass(Type type, out bool isNewObject)
	{
		isNewObject = false;
		if (type == null)
		{
			return null;
		}
		// 锁定期间不能调用任何其他非库函数,否则可能会发生死锁
		mListLock.waitForUnlock();
		// 先从未使用的列表中查找是否有可用的对象
		if (!mPoolList.TryGetValue(type, out ClassPoolSingle singlePool))
		{
			singlePool = new ClassPoolSingle();
			singlePool.setType(type);
			mPoolList.Add(type, singlePool);
		}
		mListLock.unlock();
		return singlePool.newClass(out isNewObject);
	}
	// 仅用于主工程中的类,否则无法识别
	public T newClass<T>(out T obj) where T : ClassObject
	{
		ClassObject classObj = newClass(Typeof<T>(), out _);
		obj = classObj as T;
		if (obj == null)
		{
			logError("创建类实例失败,可能传入的type类型与目标类型不一致");
		}
		return obj;
	}
	public void destroyClass(ClassObject classObject)
	{
		mListLock.waitForUnlock();
		if (!mPoolList.TryGetValue(Typeof(classObject), out ClassPoolSingle singlePool))
		{
			logError("找不到类对象的对象池");
		}
		singlePool.destroyClass(classObject);
		mListLock.unlock();
	}
	public void destroyClassReally(ClassObject classObject)
	{
		mListLock.waitForUnlock();
		if (!mPoolList.TryGetValue(Typeof(classObject), out ClassPoolSingle singlePool))
		{
			logError("找不到类对象的对象池");
		}
		singlePool.destroyClassReally(classObject);
		mListLock.unlock();
	}
	public bool isInuse(ClassObject classObject)
	{
		mListLock.waitForUnlock();
		if (!mPoolList.TryGetValue(Typeof(classObject), out ClassPoolSingle singlePool))
		{
			logError("找不到类对象的对象池");
		}
		bool inuse = singlePool.isInuse(classObject);
		mListLock.unlock();
		return inuse;
	}
}