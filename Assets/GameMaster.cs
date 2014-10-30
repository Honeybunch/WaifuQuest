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
		Start,
		Map,
		Battle,
		Dead,
		Win
	}
	//Where are we in the battle
	private enum BattleState{
		Start,
		PlayerAttack,
		EnemyAttack,
		PlayerChoice,
		EnemyDefeated,
	}

	//Current States
	GameState state = GameState.Start;
	BattleState bState = BattleState.PlayerChoice;
	//Enemy Vars
	int enemyHp;
	public int enemyType;
	public Texture enemySprite;
	int[] numEnemies = {1,1,1,1,1,1}; //How many enemies of each type have we designed
	//Player Vars
	int playerHp = 10;
	bool playerDead = false;
	int dmg;

	#region Combat Vars
	int choiceNum;

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

	//Variables for boss battle
	string bossLine;
	int bossProgress;
	int correctChoice;
	const int BOSS = 6;
	#endregion Combat Vars
	void OnGUI(){
		GUI.skin = buttonSkin;
		switch(state){
		case GameState.Start:
			if( GUI.Button(new Rect(0,0,Screen.width, Screen.height), ">The love crystal has been shattered! The monsters of this land run wild, consumed with passion!\n Only you can dispell the curse of the shattered crystal, through filling their hearts with love\n and collecting the shards!")){
				state = GameState.Map;
			}
			break;
		case GameState.Map:
			break;
		case GameState.Battle:
			if(enemyHp <= 0){
				enemyHp = 0;
				bState = BattleState.EnemyDefeated;
			}
			else if(playerHp <= 0){
				playerHp = 0;
				playerDead = true;
			}
			//Draw BG
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), backgroundTexture, ScaleMode.StretchToFill);
			//Draw Enemy
			GUI.DrawTexture(new Rect(Screen.width/2, 100, enemySprite.width, enemySprite.height), enemySprite, ScaleMode.ScaleAndCrop);
			//Battle GUI
			Texture2D healthBar = new Texture2D(1,1);
			healthBar.SetPixel(0,0,Color.green);
			healthBar.wrapMode = TextureWrapMode.Repeat;
			healthBar.Apply();
			GUI.DrawTexture(new Rect(20,20,250 * playerHp/10,20), healthBar);
			healthBar.SetPixel(0,0,Color.magenta);
			healthBar.Apply();
			GUI.DrawTexture(new Rect(20,60,250 * enemyHp / 10, 20), healthBar);
			//Check to see if player has defeated the monster, or if he/she has been defeated
			if(playerDead){
				state = GameState.Dead;
			}
			else{
				switch(bState){
				case BattleState.Start:
					string blurb = "";
					switch(enemyType){
					case 1:
						blurb = "They look like they're playing hard to get...";
						break;
					case 2:
						blurb = "They seem really energetic...";
						break;
					case 3:
						blurb = "They're really endearing...";
						break;
					case 4:
						blurb = "They look like they're giving you the cold shoulder...";
						break;
					case 5:
						blurb = "They like they might like you a little too much...";
						break;
					case BOSS:
						blurb = "This demon's been hiding this crystal shard.\nYou've got to sweep her off her feet!";
						break;
					}
					if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">A monster approaches!\n" + blurb)){
						bState = BattleState.PlayerChoice;
						UpdateBattle();
					}
					break;
				case BattleState.EnemyAttack:
					string response = "";
					switch(enemyType){
					case 1:
						response = "Go away! B-b-baka!";
						break;
					case 2:
						response = "Let's do our best today!";
						break;
					case 3:
						response = "Do I look cute? Uguu~";
						break;
					case 4:
						response = "...";
						break;
					case 5:
						response = "I hope you never leave :)";
						break;
					case BOSS:
						response = bossLine;
						break;
					}
					if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">\"" + response + "\"\n>The monster thrashes with passion! Deals 1 damage.")){
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
						}
						break;
					case 2:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">You seem to be getting to them! Deals 2 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
						}
						break;
					case 3:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">They kinda liked to hear that. Deals 3 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
						}
						break;
					case 5:
						if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">They really liked that!! Deals 5 damage!")){
							enemyHp -= dmg;
							bState = BattleState.EnemyAttack;
						}
						break;
					}
					break;
				case BattleState.PlayerChoice:
					//Print out the randomized dialogue options
					if( GUI.Button(new Rect(0, Screen.height-100*3, Screen.width, 100), line1) )
					{
						choiceNum = 1;
						dmg = CalcDamage(dialogType1);
						bState = BattleState.PlayerAttack;
					}
					
					if( GUI.Button(new Rect(0, Screen.height-100*2, Screen.width, 100), line2) )
					{
						choiceNum = 2;
						dmg = CalcDamage(dialogType2);
						bState = BattleState.PlayerAttack;
					}
					if ( GUI.Button(new Rect(0, Screen.height-100, Screen.width, 100), line3) )
					{
						choiceNum = 3;
						dmg = CalcDamage(dialogType3);
						bState = BattleState.PlayerAttack;
					}
					break;
				case BattleState.EnemyDefeated:
					if(GUI.Button(new Rect(0, Screen.height-100 * 3, Screen.width, 300), ">You defeated the monster, thanks to the power of love!")){
							if(enemyType == BOSS)
								state = GameState.Win;
							else
								state = GameState.Map;
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
		case GameState.Win:
			if( GUI.Button(new Rect(0,0,Screen.width, Screen.height), ">You've Collected the first shard of the crystal!\nTo be continued?")){
				state = GameState.Map;
			}
			break;
		}
	}

	/// <summary>
	/// Setups the battle.
	/// </summary>
	public void SetupBattle(int optionalType){
		//playtest only
		//Debug.Log("Setup " + optionalType);
		playerHp = 10;
		enemyHp = 10;
		bossProgress = 0;
		if(optionalType == 0)
			enemyType = Random.Range(1,6);
		else
			enemyType = optionalType;
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
		case BOSS:
			enemySprite = Resources.Load("Textures/Boss/" + Random.Range(1, numEnemies[enemyType-1] + 1)) as Texture;
			break;
		}
		state = GameState.Battle;
		bState = BattleState.Start;
		UpdateBattle();
	}

	/// <summary>
	/// Updates the battle.
	/// Author: Tim Cotanch
	/// </summary>
	void UpdateBattle(){
		if(enemyType!=BOSS){
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
		}
		else
			BossBattle();
	}

	void BossBattle(){
		//Load in json from file as a string
		string JSONString = System.IO.File.ReadAllText(Application.dataPath + "/../bossLines.json");
		
		//Pase JSON
		JSON LineJSON = new JSON();
		LineJSON.serialized = JSONString;
		
		//Parse Lines from JSON
		string[] bossLines;
		string[] playerLines;

		//Assign lines
		bossLines = LineJSON.ToArray<string>("Boss");
		playerLines = LineJSON.ToArray<string>("Player");
		bossLine = bossLines[bossProgress];

		do
		{ 
			randomLine1 = Random.Range(bossProgress * 3, bossProgress * 3 + 3);
			randomLine2 = Random.Range(bossProgress * 3, bossProgress * 3 + 3);
			randomLine3 = Random.Range(bossProgress * 3, bossProgress * 3 + 3);
		}while( randomLine1 == randomLine2 || randomLine1 == randomLine3 || randomLine2 == randomLine3 );

		if(randomLine1 == bossProgress * 3)
			correctChoice = 1;
		if(randomLine2 == bossProgress * 3)
			correctChoice = 2;
		if(randomLine3 == bossProgress * 3)
			correctChoice = 3;
		line1 = playerLines[randomLine1];
		line2 = playerLines[randomLine2];
		line3 = playerLines[randomLine3];
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
		if(enemyType ==  BOSS){
			if(correctChoice == choiceNum){
				dmg = 2;
				bossProgress++;
			}
		}
		return dmg;
	}
	
}
