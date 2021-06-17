﻿using UnityEngine;

// 摄像机连接器的转换器基类,用于实现切换连接器时的各种过渡效果
public abstract class CameraLinkerSwitch : FrameBase
{
	protected CameraLinker mLinker;
	protected Vector3 mOriginRelative;
	protected Vector3 mTargetRelative;
	// 转换器的速度,不同的转换器速度含义不同
	// 直线转换器是直线速度
	// 环形转换器是角速度
	// 绕目标旋转转换器是角速度
	protected float mSpeed;
	public virtual void init(Vector3 origin, Vector3 target, float speed)
	{
		mOriginRelative = origin;
		mTargetRelative = target;
		mSpeed = speed;
	}
	public abstract void update(float elapsedTime);
	public virtual void destroy()
	{
		mLinker = null;
	}
	public void setLinker(CameraLinker parentLinker)	{ mLinker = parentLinker; }
	public void setOriginRelative(Vector3 origin)		{ mOriginRelative = origin; }
	public void setTargetRelative(Vector3 target)		{ mTargetRelative = target; }
	public void setSwitchSpeed(float speed)				{ mSpeed = speed; }
	public CameraLinker getLinker()						{ return mLinker; }
	public Vector3 getOriginRelative()					{ return mOriginRelative; }
	public Vector3 getTargetRelative()					{ return mTargetRelative; }
	public float getSwitchSpeed()						{ return mSpeed; }
}