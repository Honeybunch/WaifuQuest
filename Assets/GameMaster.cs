using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

	public Texture backgroundTexture;
	public Texture enemySprite;

	public enum GameState{
		Map,
		Battle
	}
	GameState state = GameState.Map;

	void onGUI(){
		switch(state){
		case GameState.Map:
			break;
		case GameState.Battle:
			//Draw BG
			GUI.DrawTexture(Rect(0,0,Screen.width,Screen.height), backgroundTetxure);
			//Draw Enemy
			GUI.DrawTexture(Rect(Screen.width/4, 0, enemySprite.width, enemySprite.height), enemySprite, 1);
			//Battle GUI
			break;
		}
	}
	/*
	 * 
	 * Battle Code
	 * 
	 */

	/*void SetUpBattle(){
		//Load in enemy image
		//Reset Enemy HP
	}
	*/

}
