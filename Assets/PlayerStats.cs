using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {

	[HideInInspector]
	public string playerName;
	[HideInInspector]
	public string gender;

	//Pronouns
	[HideInInspector]
	public string nominative;
	[HideInInspector]
	public string oblique;
	[HideInInspector]
	public string possessive;
	[HideInInspector]
	public string reflexive;

	//Game stats
	public int health;

	// Use this for initialization
	void Start ()
	{
		health = 10;

		playerName = PlayerPrefs.GetString("name");
		gender = PlayerPrefs.GetString("gender");

		nominative = PlayerPrefs.GetString("nominative");
		oblique = PlayerPrefs.GetString("oblique");
		possessive = PlayerPrefs.GetString("possessive");
		reflexive = PlayerPrefs.GetString("reflexive");
	}

}
