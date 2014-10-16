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
				StartCoroutine(LoadGame());
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

	/// <summary>
	/// Coroutine to fade out and then load the game scene
	/// </summary>
	IEnumerator LoadGame()
	{
		//Wait to fade to black
		ScreenFader screenFader = GetComponent<ScreenFader>(); 
		IEnumerator blackOutScreen = screenFader.FadeToBlack();
		while (blackOutScreen.MoveNext()){yield return blackOutScreen.Current;}

		Application.LoadLevel ("test");
	}

	void ReloadMenu()
	{
		Application.LoadLevel ("Main_Menu");
	}


}
