using UnityEngine;
using System.Collections;

public class word {
	
	private string name;
	private Vector2 startPosition;
	private Vector2 endPosition;
	private bool found;
	private bool used;

	public string Name {
		get {return name;}
		set {name = value;}
	}

	public int Length {
		get {return name.Length;}
	}

	public Vector2 StartPosition {
		get { return startPosition;}
		set { startPosition = value;}
	}

	public Vector2 EndPosition {
		get { return endPosition;}
		set { endPosition = value;}
	}

	public bool Found {
		get { return found;}
		set { found = value;}
	}

	public bool Used {
		get { return used;}
		set { used = value;}
	}

	public word(string Name) {
		name = Name;
		found = false;
		used = false;
	}
}
