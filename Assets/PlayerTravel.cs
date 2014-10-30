using UnityEngine;
using System.Collections;

public class PlayerTravel : MonoBehaviour
{
	public bool traveling;

	bool canTravel;

	ScreenFader screenFader;
	PlayerMovement playerMovement;

	string lastMapFrom;
	string lastMapTo;

	void Start()
	{
		canTravel = true;

		screenFader = Camera.main.GetComponent<ScreenFader>();
		playerMovement = gameObject.GetComponent<PlayerMovement>();

		//We've loaded from the main menu, lets fade in
		screenFader.alpha = 1.0f;
		StartCoroutine(screenFader.FadeToClear());
	}

	/// <summary>
	/// Called when we run into a trigger
	/// </summary>
	void OnTriggerEnter2D(Collider2D collider)
	{
		GameObject triggerObject = collider.gameObject;
		TriggerProperties trigger = triggerObject.GetComponent<TriggerProperties>();

		if(trigger == null)
			return;

		//Handle travel triggers
		if(trigger.type == TriggerType.TRAVEL)
		{
			//Don't do anything if we're already enroute
			if(!canTravel)
				return;
			
			canTravel = false;
			
			string nextMapName = trigger.travelTo;
			string currentMapName = trigger.travelFrom;
			string mapJson = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Maps/" + nextMapName + ".map"); 
			
			Map map = Map.DeserializeMap(mapJson);
			
			//reset the player movment counter when we hit the trigger so that battles don't accidentally happen
			playerMovement.distanceTraveled = 0;

			StartCoroutine(LoadMap(map, nextMapName, currentMapName));
		}
		else if(trigger.type == TriggerType.EVENT)
		{
			//Handle events specifically
			if(trigger.eventName == "Boss")
			{
				//Start a battle with the enemytype being boss
				GameMaster gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
				PlayerMovement movement = gameObject.GetComponent<PlayerMovement>();

				StartCoroutine(movement.StartBattle(6));
			}
		}
	}

	/// <summary>
	/// Waits for the map loading coroutine to end and then finalizes the player position
	/// </summary>
	IEnumerator LoadMap(Map map, string currentMapName, string mapTarget)
	{		
		screenFader.fadeSpeed = 0.2f;

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

				if(trigger != null && trigger.travelFrom == currentMapName && trigger.travelTo == mapTarget)
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
		lastMapTo = mapTarget;
		lastMapFrom = currentMapName;

		yield return null;
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		//If we leave the map trigger, go back to the previous map
		if(collider.gameObject.name == "Map")
		{
			if(!string.IsNullOrEmpty(lastMapTo))
			{
				canTravel = false;
				traveling = true;
				string mapJson = System.IO.File.ReadAllText(Application.dataPath + "/Resources/Maps/" + lastMapTo + ".map"); 

				Map map = Map.DeserializeMap(mapJson);

				StartCoroutine(LoadMap(map, lastMapTo , lastMapFrom));
			}
		}
		else if(!traveling)
			canTravel = true;
	}

}
