using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapCreator : MonoBehaviour {

	
	/// <summary>
	/// Creates a new map of the given size
	/// </summary>
	/// <param name="x">The width of the new map</param>
	/// <param name="y">The height of the new map</param>
	public static void CreateNewMap(int x, int y)
	{
		//Delete the old map if there is one
		GameObject oldMap = GameObject.Find("Map");

		if(oldMap)
		{
			if(EditorUtility.DisplayDialog("Delete?",
			                            "Only one map allowed, would you like to delete the previous one?",
			                            "Yes", 
			                               "No"))
			{
				GameObject.DestroyImmediate(oldMap);
			}
			else
			{
				return;
			}
		}

		float tileWidth = 1.0f; //Hard coded for now

		float startX = (x/2) * -tileWidth;
		float startY = (y/2) * -tileWidth;

		//The prefab we want to load for tiling
		GameObject tileBase = (GameObject)Resources.Load("TileBase");

		GameObject map = new GameObject();
		Map mapComponent = map.AddComponent<Map>();
		map.name = "Map";

		for(int i =0 ; i < x; i++)
		{
			for(int j=0; j < y; j++)
			{
				GameObject newTile = (GameObject)Instantiate(tileBase);

				float newX = startX + (tileWidth * i);
				float newY = startY + (tileWidth * j);

				Vector3 newPos = new Vector3(newX, newY, 0);

				newTile.transform.position = newPos;

				//Add tile to map so that it's nice and organized
				newTile.transform.parent = map.transform;
				//Also add the new tile to the Map's internal list of tiles

			}
		}

	}
	
	/// <summary>
	/// Loads the map for editing
	/// </summary>
	public static void LoadMap()
	{
		
	}
	
	/// <summary>
	/// Saves the map out to a file in JSON format
	/// </summary>
	public static void SaveMap()
	{
		
	}
}
