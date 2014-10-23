using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WyrmTale;

public class GameMaster : MonoBehaviour {

	public Texture backgroundTexture;
	public GUISkin buttonSkin;

	public GameState State
	{
		get{return state;}
	}

	public enum GameState{
		Map,
		Battle,
		Dead
	}
	//Where are we in the battle
	private enum BattleState{
		PlayerAttack,
		EnemyAttack,
		PlayerChoice,
		EnemyDefeated
	}
	//Current States
	GameState state = GameState.Map;
	BattleState bState = BattleState.PlayerChoice;
	//Enemy Vars
	int enemyHp;
	int enemyType;
	bool enemyDead;
	public Texture enemySprite;
	int[] numEnemies = {1,1,1,1,1}; //How many enemies of each type have we designed
	//Player Vars
	int playerHp = 10;
	bool playerDead = false;
	int dmg;

	#region Combat Vars
	//Random numbers used to generate different responses
	int randomLine1;
	int randomLine2;
	int randomLine3;
	
	//Random numbers used to generate differnt types
	int dialogType1;
	int dialogType2;
	int dialogType3;
	
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
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), backgroundTexture, ScaleMode.StretchToFill);
			//Draw Enemy
			GUI.DrawTexture(new Rect(Screen.width/2, 100, enemySprite.width, enemySprite.height), enemySprite, ScaleMode.ScaleAndCrop);
			//Battle GUI
			//Check to see if player has defeated the monster, or if he/she has been defeated
			if(playerDead){
				state = GameState.Dead;
			}
			else{
				//Display player health and enemyHps to the screen
				GUI.Label(new Rect(300, 20, 200, 40), "HP: " + playerHp);
				GUI.Label(new Rect(300, 30, 200, 40), "Enemy HP: " + enemyHp);
				switch(bState){
				case BattleState.EnemyAttack:
					if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">The monster thrashes with passion! Deals 1 damage.")){
						playerHp-=1;
						bState = BattleState.PlayerChoice;
						UpdateBattle();
					}
					break;
				case BattleState.PlayerAttack:
					switch(dmg){
					case 0:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">They didn't really seem to like that. Deals 0 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
							UpdateBattle();
						}
						break;
					case 3:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">They kinda liked to hear that. Deals 3 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
							UpdateBattle();
						}
						break;
					case 5:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">They really liked that!! Deals 5 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
							UpdateBattle();
						}
						break;
					}
					break;
				case BattleState.PlayerChoice:
					//Print out the randomized dialogue options
					if( GUI.Button(new Rect(0, Screen.height-100*3, Screen.width, 100), line1) )
					{
						dmg = CalcDamage(dialogType1);
						bState = BattleState.PlayerAttack;
						UpdateBattle();
					}
					
					if( GUI.Button(new Rect(0, Screen.height-100*2, Screen.width, 100), line2) )
					{
						dmg = CalcDamage(dialogType2);
						bState = BattleState.PlayerAttack;
						UpdateBattle();
					}
					if ( GUI.Button(new Rect(0, Screen.height-100, Screen.width, 100), line3) )
					{
						dmg = CalcDamage(dialogType3);
						bState = BattleState.PlayerAttack;
						UpdateBattle();
					}
					break;
				case BattleState.EnemyDefeated:
					if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">You defeated the monster, thanks to the power of love!")){
							state = GameState.Map;
						Debug.Log("aaaa");
					}
					break;
				}
			}
			break;
		case GameState.Dead:
			if( GUI.Button(new Rect(0,0,Screen.width, Screen.height), ">The monster overtook you with lust... you have died...")){
				state = GameState.Map;
			}
			break;
		}
	}

	/// <summary>
	/// Setups the battle.
	/// </summary>
	public void SetupBattle(){
		//playtest only
		playerHp = 10;

		enemyHp = 10;
		enemyType = Random.Range(1,6);
		enemyDead = false;
		//Load Sprite
		switch(enemyType){
		case 1:
			enemySprite = Resources.Load("Textures/Tsundere/" + Random.Range(1,numEnemies[enemyType-1] + 1)) as Texture;
			break;
		case 2:
			enemySprite = Resources.Load("Textures/Genki/" + Random.Range(1,numEnemies[enemyType-1] + 1)) as Texture;
			break;
		case 3:
			enemySprite = Resources.Load("Textures/Moe/" + Random.Range(1,numEnemies[enemyType-1] + 1)) as Texture;
			break;
		case 4:
			enemySprite = Resources.Load("Textures/Kuudere/" + Random.Range(1,numEnemies[enemyType-1] + 1)) as Texture;
			break;
		case 5:
			enemySprite = Resources.Load("Textures/Yandere/" + Random.Range(1,numEnemies[enemyType-1] + 1)) as Texture;
			break;
		}
		state = GameState.Battle;
		bState = BattleState.PlayerChoice;
		UpdateBattle();
	}

	/// <summary>
	/// Updates the battle.
	/// Author: Tim Cotanch
	/// </summary>
	void UpdateBattle(){
		do
		{ 
			dialogType1 = Random.Range(1, 6);
			dialogType2 = Random.Range(1, 6);
			dialogType3 = Random.Range(1, 6);
		}while( dialogType1 == dialogType2 || dialogType1 == dialogType3 || dialogType2 == dialogType3 );
		
		//Ensure two responses are not the same
		do
		{ 
			randomLine1 = Random.Range(1, 8);
			randomLine2 = Random.Range(1, 8);
			randomLine3 = Random.Range(1, 8);
		}while( randomLine1 == randomLine2 || randomLine1 == randomLine3 || randomLine2 == randomLine3 );
		
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
		switch(dialogType1)
		{
		default:
			lines1 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines1 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 2:
			lines1 = LineJSON.ToArray<string>("Genki");
			break;
		case 3:
			lines1 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines1 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 5:
			lines1 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		switch(dialogType2)
		{
		default:
			lines2 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines2 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 2:
			lines2 = LineJSON.ToArray<string>("Genki");
			break;
		case 3:
			lines2 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines2 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 5:
			lines2 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		switch(dialogType3)
		{
		default:
			lines3 = LineJSON.ToArray<string>("Genki");
			break;
		case 1:
			lines3 = LineJSON.ToArray<string>("Tsundere");
			break;
		case 2:
			lines3 = LineJSON.ToArray<string>("Genki");
			break;
		case 3:
			lines3 = LineJSON.ToArray<string>("Moe");
			break;
		case 4:
			lines3 = LineJSON.ToArray<string>("Kuudere");
			break;
		case 5:
			lines3 = LineJSON.ToArray<string>("Yandere");
			break;
		}
		
		line1 = lines1[randomLine1];
		line2 = lines2[randomLine2];
		line3 = lines3[randomLine3];

		if(enemyHp <= 0){
			enemyHp = 0;
			enemyDead = true;
			bState = BattleState.EnemyDefeated;
		}
		if(playerHp <= 0){
			playerHp = 0;
			playerDead = true;
		}
	}

	int CalcDamage(int lineType){
		int dmg = 0;
		//Assign Value to Damage
		switch (enemyType)
		{
		case 1:
			switch (lineType)
			{
			case 1:
				dmg = 5;
				break;
			case 2:
				dmg = 0;
				break;
			case 3:
				dmg = 3;
				break;
			case 4:
				dmg = 3;
				break;
			case 5:
				dmg = 0;
				break;
			default:
				break;
			}
			break;
		case 2:
			switch (lineType)
			{
			case 1:
				dmg = 0;
				break;
			case 2:
				dmg = 5;
				break;
			case 3:
				dmg = 3;
				break;
			case 4:
				dmg = 0;
				break;
			case 5:
				dmg = 3;
				break;
			default:
				break;
			}
			break;
		case 3:
			switch (lineType)
			{
			case 1:
				dmg = 3;
				break;
			case 2:
				dmg = 3;
				break;
			case 3:
				dmg = 5;
				break;
			case 4:
				dmg = 0;
				break;
			case 5:
				dmg = 0;
				break;
			default:
				break;
			}
			break;
		case 4:
			switch (lineType)
			{
			case 1:
				dmg = 3;
				break;
			case 2:
				dmg = 0;
				break;
			case 3:
				dmg = 0;
				break;
			case 4:
				dmg = 5;
				break;
			case 5:
				dmg = 3;
				break;
			default:
				break;
			}
			break;
		case 5:
			switch (lineType)
			{
			case 1:
				dmg = 0;
				break;
			case 2:
				dmg = 3;
				break;
			case 3:
				dmg = 0;
				break;
			case 4:
				dmg = 3;
				break;
			case 5:
				dmg = 5;
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}
		return dmg;
	}
}
