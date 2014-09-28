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
		GameObject tileBase = (GameObject)Resources.Load("Prefabs/TileBase");

		GameObject map = new GameObject();
		Map mapComponent = map.AddComponent<Map>();
		map.name = "Map";

		mapComponent.startX = startX;
		mapComponent.startY = startY;
		mapComponent.width = x;
		mapComponent.height = y;
		mapComponent.tileWidth = tileWidth;

		for(int i =0 ; i < x; i++)
		{
			for(int j=0; j < y; j++)
			{
				GameObject newTile = (GameObject)Instantiate(tileBase);
				newTile.AddComponent<Tile>();
				Tile tileComponent = newTile.GetComponent<Tile>();

				float newX = startX + (tileWidth * i);
				float newY = startY + (tileWidth * j);

				Vector3 newPos = new Vector3(newX, newY, 0);

				tileComponent.x = newX;
				tileComponent.y = newY;
				tileComponent.passable = true;
				tileComponent.textureName = "GrassTileTexture";

				newTile.transform.position = newPos;

				//Add tile to map so that it's nice and organized
				newTile.transform.parent = map.transform;
				//Also add the new tile to the Map's internal list of tiles
				mapComponent.Tiles.Add(newTile);
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
