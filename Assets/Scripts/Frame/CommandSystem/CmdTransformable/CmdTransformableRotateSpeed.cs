﻿using UnityEngine;

// 使物体旋转
public class CmdTransformableRotateSpeed : Command
{
	public Vector3 mRotateAcceleration;		// 旋转加速度
	public Vector3 mRotateSpeed;			// 旋转起始速度
	public Vector3 mStartAngle;				// 旋转起始角度
	public override void resetProperty()
	{
		base.resetProperty();
		mRotateAcceleration = Vector3.zero;
		mRotateSpeed = Vector3.zero;
		mStartAngle = Vector3.zero;
	}
	public override void execute()
	{
		var obj = mReceiver as Transformable;
#if UNITY_EDITOR
		if (obj is myUIObject)
		{
			var uiObj = obj as myUIObject;
			if ((!isVectorZero(mRotateSpeed) || !isVectorZero(mRotateAcceleration)) && !uiObj.getLayout().canUIObjectUpdate(uiObj))
			{
				logError("想要使窗口播放缓动动画,但是窗口当前未开启更新");
			}
		}
#endif
		obj.getComponent(out COMTransformableRotateSpeed com);
		com.setActive(true);
		com.startRotateSpeed(mStartAngle, mRotateSpeed, mRotateAcceleration);
		// 需要启用组件更新时,则开启组件拥有者的更新,后续也不会再关闭
		obj.setEnable(true);
	}
	public override void debugInfo(MyStringBuilder builder)
	{
		builder.append(": mStartAngle:", mStartAngle).
				append(", mRotateSpeed:", mRotateSpeed).
				append(", mRotateAcceleration:", mRotateAcceleration);
	}
}