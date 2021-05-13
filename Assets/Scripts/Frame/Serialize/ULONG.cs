﻿using System;
using System.Collections.Generic;

public class ULONG : OBJECT
{
	protected const int TYPE_SIZE = sizeof(long);
	public ulong mValue;
	protected bool mIntReplace;
	public ULONG()
	{
		mType = typeof(ulong);
		mSize = TYPE_SIZE;
	}
	public ULONG(ulong value)
	{
		mValue = value;
		mType = typeof(ulong);
		mSize = TYPE_SIZE;
	}
	public override void setIntReplaceULLong(bool replace)
	{
		mIntReplace = replace;
	}
	public override void zero() { mValue = 0; }
	public void set(ulong value) { mValue = value; }
	public override bool readFromBuffer(byte[] buffer, ref int index)
	{
		bool success;
		// 如果第一个字节的最高位是1,则表示应该读取4字节
		// 因为存储是小端模式,最高的字节在高地址,所以为了方便解析,使低地址的最高位作为标记位,先左移一位,使低字节的最低字节空出来作为标记位
		// 所以解析时首先找到标记位
		if (mIntReplace && getLowestBit(buffer[index]) == 1)
		{
			// 恢复标记位,右移一位
			int value = readInt(buffer, ref index, out success);
			setLowestBit(ref value, 0);
			// 因为右移ing可能会使符号位变为1从而变成负数,最终造成数据错误,所以需要转换为uint进行右移
			mValue = ((uint)value) >> 1;
		}
		else
		{
			mValue = readULong(buffer, ref index, out success);
		}
		return success;
	}
	public override bool writeToBuffer(byte[] buffer, ref int index)
	{
		if (mIntReplace && mValue < 0x7FFFFFFF)
		{
			// 左移一位,设置标记位
			int value = (int)mValue << 1;
			setLowestBit(ref value, 1);
			return writeInt(buffer, ref index, value);
		}
		else
		{
			return writeULong(buffer, ref index, mValue);
		}
	}
}