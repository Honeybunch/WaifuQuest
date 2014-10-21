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

[CustomEditor (typeof(MapLoader))]
public class MapLoaderEditor : Editor
{
	MapLoader loader;

	//Get the object we're attached to when the user selects said object
	void OnEnable()
	{
		loader = (MapLoader)target;
	}

	public override void OnInspectorGUI()
	{
		string mapName = loader.mapName;

		EditorGUILayout.Space();

		if(string.IsNullOrEmpty(mapName))
			mapName = "None";

		EditorGUILayout.LabelField("Selected Map:", mapName);

		//Button to select map
		if(GUILayout.Button("Select Map"))
		{
			string path = EditorUtility.OpenFilePanel("Select Map", "Assets/Resources/Maps", "map");

			if(path != null)
			{
				//Trim the name out of the path
				int splitIndex = path.LastIndexOf("/");
				string fileName = path.Substring(splitIndex + 1);

				//Make sure these get saved back to the target script
				loader.mapPath = path;
				loader.mapName = fileName;
			}
		}

		EditorGUILayout.Space();

		//Button to load map
		if(GUILayout.Button("Load Map"))
		{
			LoadMap(loader.mapPath);
		}
	}

	/// <summary>
	/// Loads the map from the saved mapPath
	/// </summary>
	public static void LoadMap(string mapPath)
	{
		//Load the map and call the MapLoader to finish the job
		string mapJson = System.IO.File.ReadAllText(mapPath);
		
		Map map = Map.DeserializeMap(mapJson);
		
		MapLoader.CreateMap(map).MoveNext();
	}
}
