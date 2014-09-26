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
