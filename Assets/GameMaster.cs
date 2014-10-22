using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WyrmTale;

public class GameMaster : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture enemySprite;
	public GUISkin buttonSkin;

	public enum GameState{
		Map,
		Battle
	}

	//Current State
	GameState state = GameState.Battle;

	//Enemy Vars
	int enemyHp;
	int enemyType;
	//Player Vars
	int playerHp = 10;

	#region Combat Vars
	//Random numbers used to generate different responses
	int randomNum1;
	int randomNum2;
	int randomNum3;
	
	//Random numbers used to generate differnt types
	int randomType1;
	int randomType2;
	int randomType3;
	
	//Create strings for files to load in
	string line1;
	string line2;
	string line3;
	#endregion Combat Vars

	void OnGUI(){
		GUI.skin = buttonSkin;
		switch(state){
		case GameState.Map:
			break;
		case GameState.Battle:
			//Draw BG
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), backgroundTexture, ScaleMode.ScaleToFit);
			//Draw Enemy
			GUI.DrawTexture(new Rect(Screen.width - Screen.width/2, 200, enemySprite.width, enemySprite.height), enemySprite, ScaleMode.StretchToFill);
			//Battle GUI
			//Check to see if player has defeated the monster, or if he/she has been defeated
			if( playerHp <= 0 )
			{
				GUI.Label(new Rect(300, 50, 200, 40), "YOU LOSE!!!!");
			}
			
			if( enemyHp >= 10 )
			{
				GUI.Label(new Rect(300, 50, 200, 40), "YOU WIN!!!!");
			}
			
			//Display player health and enemyHps to the screen
			GUI.Label(new Rect(300, 20, 200, 40), "Health: " + playerHp);
			GUI.Label(new Rect(300, 30, 200, 40), "enemyHps: " + enemyHp);
			
			//Print out the randomized dialogue options
			if( GUI.Button(new Rect(35, Screen.height-100*3, Screen.width, 100), line1) )
			{
				if( randomNum1 < 4 )
				{
					enemyHp += 2;
				}
				else if( randomNum1 == 4 )
				{
					enemyHp++; 
				}
				else
				{
					playerHp -= 2; 
				}
				UpdateBattle();
			}
			
			if( GUI.Button(new Rect(35, Screen.height-100*2, Screen.width, 100), line2) )
			{
				if( randomNum2 < 4 )
				{
					enemyHp += 2;
				}
				else if( randomNum2 == 4 )
				{
					enemyHp++; 
				}
				else
				{
					playerHp -= 2; 
				}
				UpdateBattle();
			}
			if ( GUI.Button(new Rect(35, Screen.height-100, Screen.width, 100), line3) )
			{
				if( randomNum2 < 4 )
				{
					enemyHp += 2;
				}
				else if( randomNum2 == 4 )
				{
					enemyHp++; 
				}
				else
				{
					playerHp -= 2; 
				}
				UpdateBattle();
			}
			break;
		}
	}

	/// <summary>
	/// Setups the battle.
	/// </summary>
	void SetupBattle(){
		enemyHp = 10;
		enemyType = Random.Range(1,6);
		//enemySprite -> load from file (tim or arsen)
		state = GameState.Battle;
	}

	/// <summary>
	/// Updates the battle.
	/// Author: Tim Cotanch
	/// </summary>
	void UpdateBattle(){
		do
		{ 
			randomType1 = Random.Range(1, 5);
			randomType2 = Random.Range(1, 5);
			randomType3 = Random.Range(1, 5);
		}while( randomType1 == randomType2 || randomType1 == randomType3 || randomType2 == randomType3 );
		
		//Ensure two responses are not the same
		do
		{ 
			randomNum1 = Random.Range(1, 8);
			randomNum2 = Random.Range(1, 8);
			randomNum3 = Random.Range(1, 8);
		}while( randomNum1 == randomNum2 || randomNum1 == randomNum3 || randomNum2 == randomNum3 );
		
		//Load in json from file as a string
		//TODO: Change where the lines.json is stored
		string JSONString = System.IO.File.ReadAllText(Application.dataPath + "/../lines.json");
		
		//Pase JSON
		JSON LineJSON = new JSON();
		LineJSON.serialized = JSONString;
		
		//Parse lines from JSON
		string[] lines1;
		string[] lines2;
		string[] lines3;
		
		//Randomly choose enemy type and assign responses
		switch(randomType1)
		{
		default:
			lines1 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines1 = LineJSON.ToArray<string>("Genki");
			break;
		case 2:
			lines1 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 3:
			lines1 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines1 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 5:
			lines1 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		switch(randomType2)
		{
		default:
			lines2 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines2 = LineJSON.ToArray<string>("Genki");
			break;
		case 2:
			lines2 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 3:
			lines2 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines2 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 5:
			lines2 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		switch(randomType3)
		{
		default:
			lines3 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines3 = LineJSON.ToArray<string>("Genki");
			break;
		case 2:
			lines3 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 3:
			lines3 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines3 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 5:
			lines3 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		line1 = lines1[randomNum1];
		line2 = lines2[randomNum2];
		line3 = lines3[randomNum3];
	}
}
