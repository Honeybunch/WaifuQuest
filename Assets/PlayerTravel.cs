﻿using UnityEngine;
using System.Collections;

public class PlayerTravel : MonoBehaviour
{
	public bool traveling;

	bool canTravel;
	ScreenFader screenFader;

	void Start()
	{
		canTravel = true;
		traveling = false;

		screenFader = Camera.main.GetComponent<ScreenFader>();
	}

	/// <summary>
	/// Called when we run into a trigger
	/// </summary>
	void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject triggerObject = collider.gameObject;
		TriggerProperties triggerProperties = triggerObject.GetComponent<TriggerProperties>();

		//Don't do anything if we're already enroute
		if(!canTravel)
			return;

		string mapName = triggerProperties.travelTo;
		string mapJson = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Maps/" + mapName + ".map"); 
		
		Map map = Map.DeserializeMap(mapJson);
		canTravel = false;

		StartCoroutine(LoadMap(map, mapName));
	}

	/// <summary>
	/// Waits for the map loading coroutine to end and then finalizes the player position
	/// </summary>
	IEnumerator LoadMap(Map map, string tileToLoadAt)
	{
		//Wait a bit just so it doesn't immediately interrupt movement
		yield return new WaitForSeconds(0.3f);
		traveling = true;

		//Wait to fade to black
		IEnumerator blackOutScreen = screenFader.FadeToBlack();
		while (blackOutScreen.MoveNext()) yield return blackOutScreen.Current;

		//Wait to create the map first
		IEnumerator createMap = MapLoader.CreateMap(map);
		while (createMap.MoveNext()) yield return createMap.Current;

		//Now that we've created the new map, set the position of the player to the exit location
		GameObject[] triggerObjects = GameObject.FindObjectsOfType<GameObject>();
		
		if(triggerObjects.Length > 0)
		{
			foreach(GameObject g in triggerObjects)
			{
				TriggerProperties trigger = g.GetComponent<TriggerProperties>();

				if(trigger != null && trigger.travelFrom == tileToLoadAt)
				{
					transform.position = new Vector3(g.transform.position.x, g.transform.position.y, -0.01f);
					break;
				}
			}
		}

		//Wait to clear the screen before we allow the player control again
		IEnumerator clearScreen = screenFader.FadeToClear();
		while (clearScreen.MoveNext()) yield return clearScreen.Current;

		traveling = false;

		yield return null;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		canTravel = true;
	}

}
