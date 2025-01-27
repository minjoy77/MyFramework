﻿using System;

// 在物理更新中执行
public class ComponentKeyFramePhysics : ComponentKeyFrame
{
	public override void fixedUpdate(float elapsedTime)
	{
		base.fixedUpdate(elapsedTime);
		if (mKeyFrame != null && mPlayState == PLAY_STATE.PLAY)
		{
			mCurrentTime += elapsedTime;
			mPlayedTime += elapsedTime;
			bool done = false;
			// 无限播放当前震动
			if (mPlayLength < 0.0f)
			{
				if (mCurrentTime > mOnceLength)
				{
					mCurrentTime = 0.0f;
				}
			}
			// 播放固定长度的震动
			else
			{
				// 超过时间则停止,暂时不播放最后一帧
				if (mPlayedTime > mPlayLength)
				{
					done = true;
					mCurrentTime = mOffset + mPlayLength;
				}
				else if (mCurrentTime > mOnceLength)
				{
					mCurrentTime = 0.0f;
				}
			}
			mCurValue = mKeyFrame.evaluate(mCurrentTime / mOnceLength);
			applyTrembling(mCurValue);
			afterApplyTrembling(done);
		}
	}
}