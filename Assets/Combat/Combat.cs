using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Combat : MonoBehaviour {

	//List for all the dialogue options
	List<string> list = new List<string>();

	//Random numbers used to generate different responses
	int randomNum1;
	int randomNum2;
	int randomNum3;

	//Create strings for files to load in
	string file1;
	string file2;
	string file3;

	//Variables for enemy
	int heart = 0;
	int playerHealth = 10;
	
	// Use this for initialization
	void Start () {
		//Ensure two responses are not the same
		do
		{ 
			randomNum1 = Random.Range(1, 8);
			randomNum2 = Random.Range(1, 8);
			randomNum3 = Random.Range(1, 8);
		}while( randomNum1 == randomNum2 || randomNum1 == randomNum3 || randomNum2 == randomNum3 );

		//Load in files
		file1 = loadFile("lines.json")[randomNum1];
		file2 = loadFile("lines.json")[randomNum2];
		file3 = loadFile("lines.json")[randomNum3];
	}
	
	// Update is called once per frame
	void Update () {
	}

	List<string> loadFile(string fileName)
	{
		//Read in the json file and add each value to the list
		JsonTextReader reader = new JsonTextReader(new StreamReader(fileName));
		while( reader.Read() )
		{
			if( reader.Value != null )
			{
				//print (reader.Value);
				print (reader.TokenType);
				list.Add(reader.Value.ToString());
			}
		}
		return list;
	}

	void OnGUI()
	{
		//Check to see if player has defeated the monster, or if he/she has been defeated
		if( playerHealth <= 0 )
		{
			GUI.Label(new Rect(300, 50, 200, 40), "YOU LOSE!!!!");
		}

		if( heart >= 10 )
		{
			GUI.Label(new Rect(300, 50, 200, 40), "YOU WIN!!!!");
		}

		//Display player health and hearts to the screen
		GUI.Label(new Rect(300, 20, 200, 40), "Health: " + playerHealth);
		GUI.Label(new Rect(300, 30, 200, 40), "Hearts: " + heart);

		//Print out the randomized dialogue options
		if( GUI.Button(new Rect(10, 20, 200, 40), file1) )
		{
			if( randomNum1 < 4 )
			{
				heart += 2;
			}
			else if( randomNum1 == 4 )
			{
				heart++; 
			}
			else
			{
				playerHealth -= 2; 
			}
			Start();
		}

		if( GUI.Button(new Rect(10, 60, 200, 40), file2) )
		{
			if( randomNum2 < 4 )
			{
				heart += 2;
			}
			else if( randomNum2 == 4 )
			{
				heart++; 
			}
			else
			{
				playerHealth -= 2; 
			}
			Start();
		}
		if ( GUI.Button(new Rect(10, 100, 200, 40), file3) )
		{
			if( randomNum2 < 4 )
			{
				heart += 2;
			}
			else if( randomNum2 == 4 )
			{
				heart++; 
			}
			else
			{
				playerHealth -= 2; 
			}
			Start();
		}
	}

}