﻿using System;
using System.Collections;
using System.Collections.Generic;

public class SQLiteRegister : FrameBase
{
	public static void registeAllTable()
	{
		registeTable<SQLiteDemo, TDDemo>(ref mSQLiteDemo, "Demo");
	}
	//-------------------------------------------------------------------------------------------------------------
	protected static void registeTable<Table, Data>(ref Table table, string tableName) where Table : SQLiteTable where Data : SQLiteData
	{
		table = mSQLiteManager.registeTable(Typeof<Table>(), Typeof<Data>(), tableName) as Table;
	}
}