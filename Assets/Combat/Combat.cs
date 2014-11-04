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
using System.Collections.Generic;
using System.IO;
using WyrmTale;

public class Combat : MonoBehaviour
{
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

	//Variables for enemy
	int heart = 0;
	int playerHealth = 10;
	
	// Use this for initialization
	void Start () {
		//Ensure two types are not the same
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
	
	// Update is called once per frame
	void Update () {
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
		if( GUI.Button(new Rect(10, 20, 200, 40), line1) )
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

		if( GUI.Button(new Rect(10, 60, 200, 40), line2) )
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
		if ( GUI.Button(new Rect(10, 100, 200, 40), line3) )
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