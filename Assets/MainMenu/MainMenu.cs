//main menu 
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture logo;

	bool maleSelected = false;
	bool femaleSelected = false;
	bool otherSelected = false;

	string playerName = "";

	//Player pronouns
	string nominative = "";
	string possessive = "";
	string oblique = "";
	string reflexive = "";

	GUIStyle errorStyle;
	bool showNoGenderError = false;

	public enum menuState
	{
		MAIN,
		OPTIONS,
		CHARACTER,
		QUIT
	}
	menuState currentState = menuState.MAIN;

	void Start()
	{
		//Setup error style
		errorStyle = new GUIStyle();
		errorStyle.fontSize = 24;
		errorStyle.normal.textColor = Color.red;
	}

	void OnGUI()
	{
		float aspectRatio = ((float)Screen.height / (float)logo.height)/2.5f;

		//Display background texture
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture, ScaleMode.StretchToFill);

		switch(currentState)
		{
		case menuState.MAIN:
			GUI.DrawTexture(new Rect(Screen.width/2 - (logo.width*aspectRatio)/2, 20, logo.width * aspectRatio , logo.height * aspectRatio), logo);

			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .6f, Screen.width * .5f, Screen.height * .1f), "New Game "))
			{
				currentState = menuState.CHARACTER;
			}

			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .75f, Screen.width * .5f, Screen.height * .1f), "Options"))
			{
				currentState = menuState.OPTIONS;
			}

			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .925f, Screen.width * .5f, Screen.height * .05f), "Quit"))
			{
				currentState = menuState.QUIT;
				Application.Quit();
			}

			break;

		case menuState.CHARACTER:
			GUI.DrawTexture(new Rect(Screen.width/2 - (logo.width*aspectRatio)/2, 20, logo.width * aspectRatio , logo.height * aspectRatio), logo);

			//Buttons for choosing gender
			bool newMaleSelected = GUI.Toggle (new Rect(Screen.width * .26f, Screen.height * .45f, Screen.width * .16f, Screen.height * .06f), maleSelected, "Male", "Button");
			if(newMaleSelected != maleSelected)
			{
				maleSelected = newMaleSelected;

				if(newMaleSelected == true)
				{
					femaleSelected = false;
					otherSelected = false;

					showNoGenderError = false;
				}
			}

			bool newFemaleSelected = GUI.Toggle (new Rect(Screen.width * .42f, Screen.height * .45f, Screen.width * .16f, Screen.height * .06f), femaleSelected, "Female", "Button");
			if(newFemaleSelected != femaleSelected);
			{
				femaleSelected = newFemaleSelected;

				if(newFemaleSelected == true)
				{
					maleSelected = false;
					otherSelected = false;
				
					showNoGenderError = false;
				}
			}

			bool newOtherSelected = GUI.Toggle (new Rect(Screen.width * .58f, Screen.height * .45f, Screen.width * .16f, Screen.height * .06f), otherSelected, "Other", "Button");
			if(newOtherSelected != otherSelected);
			{
				otherSelected = newOtherSelected;
				
				if(newOtherSelected == true)
				{
					maleSelected = false;
					femaleSelected = false;

					showNoGenderError = false;
				}
			}

			//Draw UI for choosing pronouns and name
			if(maleSelected || femaleSelected || otherSelected)
			{
				GUIStyle style = GUI.skin.GetStyle("Label");

				//Setup centering styles
				GUIStyle textAlignCenterStyle = new GUIStyle(GUI.skin.GetStyle("TextField"));
				textAlignCenterStyle.alignment = TextAnchor.MiddleCenter;

				string nameLabelText = "Name:";
				string pronounsLabelText = "Pronouns:";
				string nominativeLabelText = "Nominative:";
				string obliqueLabelText = "Oblique:";
				string possessiveLabelText = "Possessive:";
				string reflexiveLabelText = "Reflexive:";

				Vector2 nameLabelTextSize = style.CalcSize(new GUIContent(nameLabelText));
				Vector2 pronounsLabelTextSize = style.CalcSize(new GUIContent(pronounsLabelText));
				Vector2 nominativeLabelTextSize = style.CalcSize(new GUIContent(nominativeLabelText));
				Vector2 obliqueLabelTextSize = style.CalcSize(new GUIContent(obliqueLabelText));
				Vector2 possessiveLabelTextSize = style.CalcSize(new GUIContent(possessiveLabelText));
				Vector2 reflexiveLabelTextSize = style.CalcSize(new GUIContent(reflexiveLabelText));

				GUI.Box(new Rect(Screen.width * .25f, Screen.height * .52f, Screen.width * .5f, Screen.height * .22f), "");

				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .5325f, nameLabelTextSize.x, nameLabelTextSize.y), nameLabelText);
				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .565f, pronounsLabelTextSize.x, pronounsLabelTextSize.y), pronounsLabelText);
				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .5975f, nominativeLabelTextSize.x, nominativeLabelTextSize.y), nominativeLabelText);
				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .63f, obliqueLabelTextSize.x, obliqueLabelTextSize.y), obliqueLabelText);
				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .6625f, possessiveLabelTextSize.x, possessiveLabelTextSize.y), possessiveLabelText);
				GUI.Label(new Rect(Screen.width * .27f, Screen.height * .695f, reflexiveLabelTextSize.x, reflexiveLabelTextSize.y), reflexiveLabelText);

				playerName = GUI.TextField(new Rect(Screen.width * .35f, Screen.height * .5325f, Screen.width * .38f, 20.0f), playerName, 75, textAlignCenterStyle);

				//If the player chooses Other, make sure we allow editing of pronouns
				if(otherSelected)
				{
					nominative = GUI.TextField(new Rect(Screen.width * .35f, Screen.height * .5975f, Screen.width * .38f, nominativeLabelTextSize.y), nominative, 25, textAlignCenterStyle);
					oblique = GUI.TextField(new Rect(Screen.width * .35f, Screen.height * .63f, Screen.width * .38f, obliqueLabelTextSize.y), oblique, 25, textAlignCenterStyle);
					possessive = GUI.TextField(new Rect(Screen.width * .35f, Screen.height * .6625f, Screen.width * .38f, possessiveLabelTextSize.y), possessive, 25, textAlignCenterStyle);
					reflexive = GUI.TextField(new Rect(Screen.width * .35f, Screen.height * .695f, Screen.width * .38f, reflexiveLabelTextSize.y), reflexive, 25, textAlignCenterStyle);
				}
				else
				{
					if(maleSelected)
					{
						nominative = "he";
						oblique = "him";
						possessive = "his";
						reflexive = "himself";
					}
					else if(femaleSelected)
					{
						nominative = "she";
						oblique = "her";
						possessive = "her";
						reflexive = "herself";
					}

					GUI.Box(new Rect(Screen.width * .35f, Screen.height * .5975f, Screen.width * .38f, 20.0f), nominative);
					GUI.Box(new Rect(Screen.width * .35f, Screen.height * .63f, Screen.width * .38f, 20.0f), oblique);
					GUI.Box(new Rect(Screen.width * .35f, Screen.height * .6625f, Screen.width * .38f, 20.0f), possessive);
					GUI.Box(new Rect(Screen.width * .35f, Screen.height * .695f, Screen.width * .38f, 20.0f), reflexive);
				}
			}

			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .75f, Screen.width * .5f, Screen.height * .1f), "Start"))
			{
				if(maleSelected || femaleSelected || otherSelected)
				{
					//Save player prefs for changing scenes
					PlayerPrefs.SetString("name", playerName);

					if(maleSelected)
						PlayerPrefs.SetString("gender", "male");
					else if(femaleSelected)
						PlayerPrefs.SetString("gender", "female");
					else
						PlayerPrefs.SetString("gender", "other");

					PlayerPrefs.SetString("nominative", nominative);
					PlayerPrefs.SetString("oblique", oblique);
					PlayerPrefs.SetString("possessive", possessive);
					PlayerPrefs.SetString("reflexive", reflexive);

					StartCoroutine(LoadGame());
				}
				else
					showNoGenderError = true;
			}

			if (GUI.Button (new Rect(Screen.width * .25f, Screen.height * .925f, Screen.width * .5f, Screen.height * .05f), "Back"))
			{
				currentState = menuState.MAIN;
			}

			//If the player tried to start the game without defining a gender, display a little error
			if(showNoGenderError)
			{
				string errorText = "You need to choose a gender first!";
				Vector2 errorTextSize = errorStyle.CalcSize(new GUIContent(errorText));

				GUI.Label(new Rect(Screen.width * .5f - (errorTextSize.x/2.0f), Screen.height * .72f - errorTextSize.y, Screen.width * .5f, Screen.height * .1f), errorText , errorStyle);
			}
			

			break;

		case menuState.OPTIONS:

			if (GUI.Button (new Rect(Screen.width * .05f, Screen.height * .925f, Screen.width * .3f, Screen.height * .05f), "Back"))
			{
				currentState = menuState.MAIN;
			}

			if (GUI.Button (new Rect(Screen.width * .95f - Screen.width * .3f, Screen.height * .925f, Screen.width * .3f, Screen.height * .05f), "Apply"))
			{

			}

			break;

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
