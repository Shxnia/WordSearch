﻿using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

public class category  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	public string Name { get; set; }
}
