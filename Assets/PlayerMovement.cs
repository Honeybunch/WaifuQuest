/*

WaifuQuest the JRPG / Visual Novel Hybridization 

Copyright (C) 2014 Arsen Tufankjian, Timothy Cotanch, Kyle Martin and Dylan Nelkin

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */


using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(PlayerTravel))]
public class PlayerMovement : MonoBehaviour 
{
	public float speed = 200.0f;
	public float distanceTraveled;

	private GameMaster gameMaster;

	private Vector3 velocity;
	private PlayerTravel playerTravel;

	private ScreenFader screenFader;
	private float distanceToBattle;
	private Vector3 previousPosition;

	// Use this for initialization
	void Start () 
	{
		velocity = Vector3.zero;

		//Get reference to game master so we can enter battle
		GameObject gameMasterObject = (GameObject)GameObject.Find("GameMaster");
		gameMaster = gameMasterObject.GetComponent<GameMaster>();

		playerTravel = GetComponent<PlayerTravel>();
		screenFader = Camera.main.GetComponent<ScreenFader>();

		distanceToBattle = NewRandomDistanceToBattle();

		previousPosition = this.transform.position;
	}

	int NewRandomDistanceToBattle()
	{
		return Random.Range(10,30);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Don't update if we're not on the overworld
		if(gameMaster.State != GameMaster.GameState.Map)
		{
			rigidbody2D.velocity = Vector3.zero;
			return;
		}

		HandleMovement();

		CheckForBattle();
	}

	void HandleMovement()
	{
		//Only update movement if we're not traveling between maps
		if(playerTravel.traveling)
		{
			rigidbody2D.velocity = Vector2.zero;
			return;
		}
		
		velocity = Vector3.zero;
		
		//Get Key input in 4 directions
		if(Input.GetKey(KeyCode.W))
			velocity.y = speed;
		
		if(Input.GetKey(KeyCode.A))
			velocity.x = -speed;
		
		if(Input.GetKey(KeyCode.S))
			velocity.y = -speed;
		
		if(Input.GetKey(KeyCode.D))
			velocity.x = speed;
		if(Input.GetKey (KeyCode.Escape))
			Application.LoadLevel ("Main_Menu");
		
		rigidbody2D.velocity = (velocity * Time.deltaTime);
	}

	void CheckForBattle()
	{
		if(transform.position != previousPosition && !playerTravel.traveling)
		{
			//Add on to the distance traveled
			distanceTraveled += Mathf.Abs(Vector3.Distance(transform.position, previousPosition));

			//Store position 
			previousPosition = this.transform.position;
			
			//If we've gone far enough, start a battle
			if(distanceTraveled >= distanceToBattle)
			{
				distanceTraveled = 0;
				velocity = Vector3.zero;
				
				StartCoroutine(StartBattle());
			}
		}

		if(playerTravel.traveling)
		{
			//While we're traveling, update the previous position so that the distance traveled won't change
			previousPosition = transform.position;
		}
	}

	public IEnumerator StartBattle()
	{
		screenFader.fadeSpeed = 0.05f;
		playerTravel.traveling = true;

		//Wait to fade to black
		IEnumerator blackOutScreen = screenFader.FadeToBlack();
		while (blackOutScreen.MoveNext()) yield return blackOutScreen.Current;

		//Set the battle state
		gameMaster.SetupBattle();

		//Wait to clear the screen before we allow the player control again
		IEnumerator clearScreen = screenFader.FadeToClear();
		while (clearScreen.MoveNext()) yield return clearScreen.Current;

		playerTravel.traveling = false;

		yield return null;
	}
}
