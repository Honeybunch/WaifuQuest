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
