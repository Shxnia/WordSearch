using UnityEngine;
using System.Collections;
using SQLite4Unity3d;

public class puzzleItem  {

	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }
	[Indexed]
	public int CategoryId { get; set; }
	public int Level { get; set; }
	public string ItemName { get; set; }
}
