﻿#if USE_TMP
using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.Events;

// 对TextMeshPro的InputField的封装
public class myUGUIInputFieldTMP : myUGUIObject
{
	protected UnityAction<string> mThisEditEnd;		// 避免GC的委托
	protected TMP_InputField mInputField;			// TextMeshPro的InputField组件
	protected OnInputField mAction;					// 输入结束时的回调
	public myUGUIInputFieldTMP()
	{
		mThisEditEnd = onEndEdit;
	}
	public override void init()
	{
		base.init();
		mInputField = mObject.GetComponent<TMP_InputField>();
		if (mInputField == null)
		{
			mInputField = mObject.AddComponent<TMP_InputField>();
			// 添加UGUI组件后需要重新获取RectTransform
			mRectTransform = mObject.GetComponent<RectTransform>();
			mTransform = mRectTransform;
		}
		if (mInputField == null)
		{
			logError(Typeof(this) + " can not find " + typeof(TMP_InputField) + ", window:" + mName + ", layout:" + mLayout.getName());
		}
	}
	public override void setAlpha(float alpha, bool fadeChild)
	{
		base.setAlpha(alpha, fadeChild);
		Color color = mInputField.textComponent.color;
		color.a = alpha;
		mInputField.textComponent.color = color;
	}
	public void setOnEndEdit(OnInputField action)
	{
		mAction = action;
		mInputField.onEndEdit.AddListener(mThisEditEnd);
	}
	public void cleanUp() { setText(EMPTY); }
	public void setText(string value) { mInputField.text = value; }
	public void setText(float value) { setText(value.ToString()); }
	public string getText() { return mInputField.text; }
	public bool isFocused() { return mInputField.isFocused; }
	public bool isVisible() { return isActive(); }
	//------------------------------------------------------------------------------------------------------------------------------
	protected void onEndEdit(string value) { mAction?.Invoke(value); }
}
#endif