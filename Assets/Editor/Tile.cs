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
using System;
using System.Collections;
using System.Collections.Generic;
using WyrmTale;

public class Tile {
	
	public Vector2 Position;
	public Vector2 Size;
	public string TileSet;
	public string TextureName;
	public bool Passable;
	public bool Trigger;

	/// <summary>
	/// Create a new Tile.
	/// </summary>
	/// <param name="xPos">X position.</param>
	/// <param name="yPos">Y position.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// <param name="tileSet">Tile set.</param>
	/// <param name="textureName">Texture name.</param>
	public Tile(int xPos, int yPos, int width, int height, string tileSet, string textureName)
	{
		Position = new Vector2(xPos, yPos);
		Size = new Vector2(width, height);
		TileSet = tileSet;
		TextureName = textureName;
		Passable = true;
		Trigger = false;
	}

	/// <summary>
	/// Creates a tile with the passable and trigger flags preset
	/// </summary>
	/// <param name="xPos">X position.</param>
	/// <param name="yPos">Y position.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// <param name="tileSet">Tile set.</param>
	/// <param name="textureName">Texture name.</param>
	/// <param name="passable">If set to <c>true</c> passable.</param>
	/// <param name="trigger">If set to <c>true</c> trigger.</param>
	public Tile(int xPos, int yPos, int width, int height, string tileSet, string textureName, bool passable, bool trigger)
		:this(xPos, yPos, width, height, tileSet, textureName)
	{
		Passable = passable;
		Trigger = trigger;
	}

	//Allows the converstion from Tile to JSON for serialization
	public static implicit operator JSON(Tile tile)
	{
		JSON js = new JSON();
		if (tile!=null)
		{
			js["X"] = tile.Position.x;
			js["Y"] = tile.Position.y;
			js["Width"] = tile.Size.x;
			js["Height"] = tile.Size.y;
			js["TileSet"] = tile.TileSet;
			js["TextureName"] = tile.TextureName;
			js["Passable"] = tile.Passable;
			js["Trigger"] = tile.Trigger;
		}          
		return js;
	}

	//Allows the converstion from JSON to Tile for deserialization
	public static explicit operator Tile(JSON value)
	{
		checked
		{
			int x = value.ToInt("X");
			int y = value.ToInt("Y");
			int width = value.ToInt("Width");
			int height = value.ToInt("Height");
			string tileSet = value.ToString("TileSet");
			string textureName = value.ToString("TextureName");
			bool passable = value.ToBoolean("Passable");
			bool trigger = value.ToBoolean("Trigger");

			return new Tile(x,y,width,height,tileSet, textureName, passable, trigger);
		}
	}

	//Allows the conversion from a JSON array to a Tile array
	public static Tile[] Array(JSON[] array)
	{
		List<Tile> tileList = new List<Tile>();

		for(int i =0; i < array.Length; i++)
			tileList.Add((Tile)array[i]);

		return tileList.ToArray();
	}

}
