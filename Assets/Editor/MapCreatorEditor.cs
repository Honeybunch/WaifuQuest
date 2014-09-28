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
using UnityEditor;
using System.Collections;

using MiniJSON;

[CustomEditor (typeof(MapCreator))]
public class MapCreatorEditor : Editor
{
	public int mapWidth = 30;
	public int mapHeight = 30;

	/// <summary>
	/// For creating the Inspector GUI
	/// </summary>
	public override void OnInspectorGUI()
	{
		GUILayout.Label("Map Creator");
		EditorGUILayout.Space();

		//Sliders for creating new maps
		mapWidth = EditorGUILayout.IntSlider("Map Width:", mapWidth, 1, 60);
		mapHeight = EditorGUILayout.IntSlider("Map Height:", mapHeight, 1, 60);
	
		EditorGUILayout.Space();

		//Buttons for loading, saving, etc
		if(GUILayout.Button("Create Map"))
		{
			MapCreator.CreateNewMap(mapWidth, mapHeight);
		}

		EditorGUILayout.Space();

		GUILayout.Button("Load Map");
		GUILayout.Button("Save Map");

		//Save editor changes
		EditorUtility.SetDirty((MapCreator)target);
	}
}
