using UnityEngine;
using UnityEditor;
using System;

public class GameEditorBase : Editor
{
	public SerializedProperty findProperty(string name)
	{
		var property = serializedObject.FindProperty(name);
		if (property == null)
		{
			Debug.LogError("找不到属性:" + name + ",确保属性存在并且访问权限为public");
		}
		return property;
	}
	public void displayProperty(string propertyName, string displayName, string toolTip = "", bool includeChildren = true)
	{
		var property = findProperty(propertyName);
		EditorGUILayout.PropertyField(property, new GUIContent(displayName, toolTip), includeChildren);
	}
	// 直接显示一个bool属性,返回修改以后的值,如果被修改过,则会设置modified为true,否则不改变modefied的值
	public bool toggle(string displayName, string toolTip, bool value)
	{
		return EditorGUILayout.Toggle(new GUIContent(displayName, toolTip), value);
	}
	// 直接显示一个bool属性,并且会改变这个变量本身,返回值为是否改变过
	public bool toggle(string displayName, string toolTip, ref bool value)
	{
		bool retValue = EditorGUILayout.Toggle(new GUIContent(displayName, toolTip), value);
		bool modified = retValue != value;
		value = retValue;
		return modified;
	}
	// 直接显示一个int属性,返回修改以后的值,如果被修改过,则会设置modified为true,否则不改变modefied的值
	public int displayInt(string displayName, string toolTip, int value, ref bool modified)
	{
		int retValue = EditorGUILayout.IntField(new GUIContent(displayName, toolTip), value);
		if (retValue != value)
		{
			modified = true;
		}
		return retValue;
	}
	// 直接显示一个int属性,并且会改变这个变量本身,返回值为是否改变过
	public bool displayInt(string displayName, string toolTip, ref int value)
	{
		int retValue = EditorGUILayout.IntField(new GUIContent(displayName, toolTip), value);
		bool modified = retValue != value;
		value = retValue;
		return modified;
	}
	// 直接显示一个float属性,返回修改以后的值,如果被修改过,则会设置modified为true,否则不改变modefied的值
	public float displayFloat(string displayName, string toolTip, float value, ref bool modified)
	{
		float retValue = EditorGUILayout.FloatField(new GUIContent(displayName, toolTip), value);
		if (retValue != value)
		{
			modified = true;
		}
		return retValue;
	}
	// 直接显示一个float属性,并且会改变这个变量本身,返回值为是否改变过
	public bool displayFloat(string displayName, string toolTip, ref float value)
	{
		float retValue = EditorGUILayout.FloatField(new GUIContent(displayName, toolTip), value);
		bool modified = retValue != value;
		value = retValue;
		return modified;
	}
	// 直接显示一个枚举属性,返回修改以后的值,如果被修改过,则会设置modified为true,否则不改变modefied的值
	public T displayEnum<T>(string displayName, string toolTip, T value, ref bool modified) where T : Enum
	{
		Enum retValue = EditorGUILayout.EnumPopup(new GUIContent(displayName, toolTip), value);
		if (retValue.CompareTo(value) != 0)
		{
			modified = true;
		}
		return (T)retValue;
	}
	// 直接显示一个枚举属性,返回修改以后的值,如果被修改过,则会设置modified为true,否则不改变modefied的值
	public T displayEnum<T>(string displayName, string toolTip, T value) where T : Enum
	{
		return (T)EditorGUILayout.EnumPopup(new GUIContent(displayName, toolTip), value);
	}
	// 直接显示一个枚举属性,返回值表示是否已经修改过
	public bool displayEnum<T>(string display, string tip, ref T value) where T : Enum
	{
		string[] names = Enum.GetNames(typeof(T));
		GUIContent[] labels = new GUIContent[names.Length];
		int[] values = new int[names.Length];
		int valueIndex = 0;
		for (int i = 0; i < labels.Length; ++i)
		{
			values[i] = i;
			labels[i] = new GUIContent(UnityUtility.getEnumLabel(typeof(T), names[i]), UnityUtility.getEnumToolTip(typeof(T), names[i]));
			if (value.ToString() == names[i])
			{
				valueIndex = i;
			}
		}
		int retValue = EditorGUILayout.IntPopup(new GUIContent(display, tip), valueIndex, labels, values);
		bool modified = retValue != valueIndex;
		value = (T)Enum.Parse(typeof(T), names[retValue]);
		return modified;
	}
	// 显示整数的下拉框
	public int intPopup(string displayName, string[] valueDisplay, int[] values)
	{
		return EditorGUILayout.IntPopup(displayName, 0, valueDisplay, values);
	}
	public void beginContents()
	{
		EditorGUILayout.BeginHorizontal(GUILayout.MinHeight(10f));
		GUILayout.Space(10f);
		GUILayout.BeginVertical();
		GUILayout.Space(2f);
	}
	public void endContents()
	{
		GUILayout.Space(3f);
		GUILayout.EndVertical();
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(3f);
	}
}