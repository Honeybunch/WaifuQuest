using UnityEngine;
using System.Collections;

public class PlayerTravel : MonoBehaviour
{
	MapLoader mapLoader;

	void Start()
	{
		//Store the mapLoader from the GameMaster object
		GameObject gameMaster = GameObject.Find("GameMaster");

		if(gameMaster != null)
		{
			mapLoader = gameMaster.GetComponent<MapLoader>();
		}
		else
		{
			Debug.LogError("Could not find game master, it's not possible to run map switching logic!");
		}
	}

	/// <summary>
	/// Called when we run into a trigger
	/// </summary>
	void OnTriggerEnter2D()
	{

	}

}
