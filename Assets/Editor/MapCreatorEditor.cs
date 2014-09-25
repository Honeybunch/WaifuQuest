using UnityEngine;
using UnityEditor;
using System.Collections;

using MiniJSON;

[CustomEditor (typeof(MapCreator))]
public class MapCreatorEditor : Editor
{
	public static int mapWidth = 50;
	public static int mapHeight = 50;

	/// <summary>
	/// For creating the Inspector GUI
	/// </summary>
	public override void OnInspectorGUI()
	{
		GUILayout.Label("Map Creator");
		EditorGUILayout.Space();

		//Sliders for creating new maps
		mapWidth = EditorGUILayout.IntSlider("Map Width:", mapWidth, 1, 100);
		mapHeight = EditorGUILayout.IntSlider("Map Height:", mapHeight, 1, 100);
	
		EditorGUILayout.Space();

		//Buttons for loading, saving, etc
		if(GUILayout.Button("Create Map"))
		{
			MapCreator.CreateNewMap(mapWidth, mapHeight);
		}

		EditorGUILayout.Space();

		GUILayout.Button("Load Map");
		GUILayout.Button("Save Map");
	}
}
