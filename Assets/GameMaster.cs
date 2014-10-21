using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture enemySprite;
	public GUISkin buttonSkin;

	public enum GameState{
		Map,
		Battle
	}
	GameState state = GameState.Battle;

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
			break;
		}
	}



	/*void SetUpBattle(){
		//Load in enemy image
		//Reset Enemy HP
	}
	*/

}
