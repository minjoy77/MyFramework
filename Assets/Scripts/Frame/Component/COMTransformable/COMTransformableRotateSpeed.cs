﻿using UnityEngine;
using System;

// 使物体按速度旋转的组件
public class COMTransformableRotateSpeed : ComponentRotateSpeedNormal  
{
	//------------------------------------------------------------------------------------------------------------------------------
	protected override void applyRotation(ref Vector3 rotation)
	{
		(mComponentOwner as Transformable).setRotation(rotation);
	}
	protected override Vector3 getCurRotation() { return (mComponentOwner as Transformable).getRotation(); }
}