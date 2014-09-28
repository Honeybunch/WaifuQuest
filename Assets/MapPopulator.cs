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

public class MapPopulator : MonoBehaviour 
{
	public static MapPopulator Populator;

	//private Map CurrentMap;

	/// <summary>
	/// When the game master is created, find all the maps
	/// Store them in an array so we can load and display them later
	/// </summary>
	public void Start()
	{

	}

	/// <summary>
	/// Loads a map from a given json file
	/// </summary>
	/// <param name="mapName">The file name in the Streaming Assets directory</param>
	public void LoadMap(string mapName)
	{

	}

	/// <summary>
	/// Takes the loaded map and creates the tiles on the sceen
	/// </summary>
	public void DisplayMap()
	{
		
	}

}
