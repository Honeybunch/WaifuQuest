//main menu 
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture optionsTexture;

	public enum menuState
	{
		Main,
		Options,
		Quit
	}
	menuState currentState = menuState.Main;

	void OnGUI()
	{
		//Display background texture
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);

		if(currentState == menuState.Main)	
		{
			//display buttons
			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .3f, Screen.width * .5f, Screen.height * .1f), "Start Game "))
			{
				loadGame ();
			}
			
			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .4f, Screen.width * .5f, Screen.height * .1f), "Options"))
			{
				currentState = menuState.Options;
				//reloadMenu();
		
			}
			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .5f, Screen.width * .5f, Screen.height * .1f), "Quit"))
			{
				currentState = menuState.Quit;
				Application.Quit();
			}
		}
		else if(currentState == menuState.Options)
		{
			if (GUI.Button (new Rect(Screen.width * .33f, Screen.height * .9f, Screen.width * .33f, Screen.height * .1f), "Return to Main Menu"))
			{
				currentState = menuState.Main;
			}
		}
	}

	void loadGame()
	{
		Application.LoadLevel ("test");
	}

	void reloadMenu()
	{
		Application.LoadLevel ("Main_Menu");
	}


}
