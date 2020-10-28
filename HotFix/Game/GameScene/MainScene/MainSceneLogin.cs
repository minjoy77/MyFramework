﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainSceneLogin : ILRSceneProcedure
{
	protected override void onInit(SceneProcedure lastProcedure, string intent)
	{
		LT.LOAD_UGUI_SHOW(LAYOUT_ILR.LOGIN);
	}
	protected override void onExit(SceneProcedure nextProcedure)
	{
		LT.HIDE_LAYOUT(LAYOUT_ILR.LOGIN);
	}
}