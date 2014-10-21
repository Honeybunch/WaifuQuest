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

	void OnGUI(){
		switch(state){
		case GameState.Map:
			break;
		case GameState.Battle:
			//Draw BG
			GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), backgroundTexture, ScaleMode.ScaleToFit);
			//Draw Enemy
			GUI.DrawTexture(new Rect(Screen.width/4, 0, enemySprite.width, enemySprite.height), enemySprite, ScaleMode.ScaleToFit);
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
