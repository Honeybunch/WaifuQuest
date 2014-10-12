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
using System.Collections.Generic;
using WyrmTale;

public class Map {

	public List<Tile> Tiles;

	public int width;
	public int height;
	public int layers;

	/*
	 * Constructors
	 */

	/// <summary>
	/// Creates a new map with no tiles and a given width and height
	/// </summary>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	public Map(int w, int h)
	{
		Tiles = new List<Tile>();

		width = w;
		height = h;
	}

	/// <summary>
	/// Creates a new map with a list of tiles and a given width and height
	/// </summary>
	/// <param name="w">The width.</param>
	/// <param name="h">The height.</param>
	/// <param name="tiles">A given list of tiles.</param>
	public Map(int w, int h, int l, List<Tile> tiles)
	{
		width = w;
		height = h;
		layers = l;
		Tiles = tiles;
	}

	/*
	 * Public Methods
	 */

	/// <summary>
	/// Adds a tile to the map, will remove any existing tiles
	/// </summary>
	public void AddTile(int layer, int xPos, int yPos, int width, int height, string tileSet, string textureName)
	{
		Tile existingTile = GetTile(layer, xPos, yPos, width, height);

		if(existingTile != null && textureName != existingTile.TextureName)
		{
			//Replace the tile
			Tiles.Remove(existingTile);
			Tile newTile = new Tile(layer, xPos, yPos, width, height, tileSet, textureName);
			Tiles.Add(newTile);
		}
		else if(existingTile == null)
		{
			Tile newTile = new Tile(layer, xPos, yPos, width, height, tileSet, textureName);
			Tiles.Add(newTile);
		}
	}

	/// <summary>
	/// Return a tile at this point with this width and height
	/// </summary>
	/// <returns>The tile</returns>
	/// <param name="xPos">X position</param>
	/// <param name="yPos">Y position</param>
	/// <param name="width">Width</param>
	/// <param name="height">Height</param>
	public Tile GetTile(int layer, int xPos, int yPos, int width, int height)
	{
		foreach(Tile t in Tiles)
		{
			bool sameLayer = (layer == t.Layer);
			bool sameX = (xPos == t.Position.x);
			bool sameY = (yPos == t.Position.y);
			bool sameWidth = (width == t.Size.x);
			bool sameHeight = (height == t.Size.y);
			
			if(sameLayer && sameX && sameY && sameWidth && sameHeight)
				return t;
		}
		
		//If we get here without returning a tile, it doesn't exist so return null
		return null;
	}

	/// <summary>
	/// Return a tile at the given position
	/// </summary>
	/// <returns>The tile</returns>
	/// <param name="xPos">X position</param>
	/// <param name="yPos">Y position</param>
	public Tile GetTile(int layer, int xPos, int yPos)
	{
		foreach(Tile t in Tiles)
		{
			bool sameLayer = (layer == t.Layer);
			bool sameX = (xPos == t.Position.x);
			bool sameY = (yPos == t.Position.y);
			
			if(sameLayer && sameX && sameY)
				return t;
		}
		
		//If we get here without returning a tile, it doesn't exist so return null
		return null;
	}

	/// <summary>
	/// Removes the tile.
	/// </summary>
	/// <param name="t">The tile to remove</param>
	public void RemoveTile(Tile t)
	{
		Tiles.Remove(t);
	}

	/// <summary>
	/// Removes the tile at the given x and y with the given width and height
	/// </summary>
	/// <param name="xPos">X position.</param>
	/// <param name="yPos">Y position.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public void RemoveTile(int layer, int xPos, int yPos, int width, int height)
	{
		foreach(Tile t in Tiles)
		{
			bool sameLayer = (layer == t.Layer);
			bool sameX = (xPos == t.Position.x);
			bool sameY = (yPos == t.Position.y);
			bool sameWidth = (width == t.Size.x);
			bool sameHeight = (height == t.Size.y);
			
			if(sameLayer && sameX && sameY && sameWidth && sameHeight)
			{
				Tiles.Remove(t);
				return;
			}
		}
	}

	/*
	 * Static Functions
	 */

	/// <summary>
	/// Serializes a given map to a JSON string
	/// </summary>
	/// <returns>The map as a JSON string</returns>
	public static string SerializeMap(Map map)
	{
		JSON js = new JSON();
		JSON[] tilesArray = new JSON[map.Tiles.Count];

		for(int i =0; i < map.Tiles.Count; i++)
		{
			 tilesArray[i] = (JSON)map.Tiles[i];
		}
		js["MapWidth"] = map.width;
		js["MapHeight"] = map.height;
		js["Layers"] = map.layers;
		js["Tiles"] = tilesArray;

		string mapJson = js.serialized;
		return mapJson;
	}

	/// <summary>
	/// Deserializes a given json string to a map
	/// </summary>
	/// <returns>The map.</returns>
	/// <param name="mapJson">Map json.</param>
	public static Map DeserializeMap(string mapJson)
	{
		JSON js = new JSON();
		js.serialized = mapJson;

		int w = js.ToInt("MapWidth");
		int h = js.ToInt("MapHeight");
		int l = js.ToInt("Layers");

		Tile[] tilesArray = Tile.Array(js.ToArray<JSON>("Tiles"));

		List<Tile> tiles = new List<Tile>();

		for(int i =0; i < tilesArray.Length; i++)
		{
			tiles.Add (tilesArray[i]);
		}

		Map map = new Map(w, h, l, tiles);

		return map;
	}

	/// <summary>
	/// Generates the map texture at a given layer
	/// </summary>
	/// <returns>The map.</returns>
	/// <param name="map">The texture of the map</param>
	/// <param name="layer">The layer of the texture we want to generate</param>
	public static Texture2D GenerateMapTexture(Map map, int layer)
	{
		Texture2D textureMap = TyleEditorUtils.NewTransparentTexture(map.width, map.height);

		//Draw every tile onto the texture
		foreach(Tile t in map.Tiles)
		{
			int tileLayer = (int)t.Layer;
			int x = (int)t.Position.x;
			int y = (int)t.Position.y;
			int w = (int)t.Size.x;
			int h = (int)t.Size.y;
			string tileSet = t.TileSet;
			string textureName = t.TextureName;

			if(tileLayer == layer)
			{
				//Load the texture from the Resources folder
				Texture2D tileTexture = (Texture2D)Resources.Load("Textures/" + tileSet + "/" + textureName);

				Color[] tilePixels = tileTexture.GetPixels();
				
				//Set the pixels 
				textureMap.SetPixels(x, y, w, h, tilePixels, 0);
			}
		}

		textureMap.Apply();

		return textureMap;
	}

	/// <summary>
	/// Generates the map textures at every layer
	/// </summary>
	/// <returns>The map textures.</returns>
	public static List<Texture2D> GenerateMapTextures(Map map)
	{
		List<Texture2D> mapLayers = new List<Texture2D>();
		int layers = map.layers;

		for(int i = 0; i < layers; i++)
		{
			Texture2D mapLayer = GenerateMapTexture(map, i);
			mapLayers.Add(mapLayer);
		}

		return mapLayers;
	}

	public static Texture2D GenerateDetailTexture(Map map)
	{
		Texture2D detailMapTexture = TyleEditorUtils.NewTransparentTexture(map.width, map.height);

		//Draw every trigger and collider onto the map
		foreach(Tile t in map.Tiles)
		{
			int x = (int)t.Position.x;
			int y = (int)t.Position.y;
			int w = (int)t.Size.x;
			int h = (int)t.Size.y;
			bool passable = t.Passable;
			bool trigger = t.Trigger;

			Texture2D detailTexture;

			//Determine wheter to draw bounding box or trigger
			if(!passable)
				detailTexture = TyleEditorUtils.NewOutlineTexture(Color.red, w, h, 5, 0);			
			else if(trigger)
				detailTexture = TyleEditorUtils.NewOutlineTexture(Color.blue, w, h, 5, 10);
			//Give it a transparent texture in case that it's neither
			else
				detailTexture = TyleEditorUtils.NewTransparentTexture(w,h);

			Color[] detailPixels = detailTexture.GetPixels();

			//Set the pixels
			detailMapTexture.SetPixels(x, y, w, h, detailPixels);
		}
		
		detailMapTexture.Apply();

		return detailMapTexture;
	}
}
