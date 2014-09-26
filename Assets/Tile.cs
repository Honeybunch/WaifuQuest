using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	[HideInInspector]
	public float x;
	[HideInInspector]
	public float y;

	public bool passable;

	[HideInInspector]
	public string textureName;

}
