﻿using System;

public class CmdCharacterManagerCreateCharacter : Command
{
	public long mID;
	public Type mCharacterType;
	public string mName;
	public bool mCreateNode;
	public override void resetProperty()
	{
		base.resetProperty();
		mCharacterType = null;
		mName = null;
		mCreateNode = true;
		mID = 0;
	}
	public override void execute()
	{
		if(mCharacterType == null)
		{
			return;
		}
		if (mID == 0)
		{
			mID = generateGUID();
		}
		mCharacterManager.createCharacter(mName, mCharacterType, mID, mCreateNode);
	}
	public override void debugInfo(MyStringBuilder builder)
	{
		builder.Append(": mName:", mName).
				Append(", mCharacterType:", mCharacterType.ToString()).
				Append(", mID:", mID).
				Append(", mCreateNode:", mCreateNode);
	}
}
