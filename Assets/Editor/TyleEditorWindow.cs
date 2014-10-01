﻿/*

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
using System.Collections.Generic;
using System.IO;

public class TyleEditorWindow : EditorWindow 
{
	public static TyleEditorWindow windowInstance;

	public Texture2D mapTexture = new Texture2D(0,0);
	public Texture2D selectionSpaceTexture = new Texture2D(0,0);
	public Texture2D boundingBoxTexture = new Texture2D(0,0);
	
	private Rect selectionOutlineBox = new Rect(0,0,0,0);

	public static List<Texture2D> textureList = new List<Texture2D>();
	public static List<string> textureNameList = new List<string>();

	private int selectedTextureIndex;
	private Texture2D selectedTexture;

	private List<string> tileSetList = new List<string>();
	private int tileSetIndex = 0;

	private Vector2 textureScrollPosition = Vector2.zero;
	private Vector2 selectionScrollPosition = Vector2.zero;

	private float scrollViewWidth; 
	private float scrollViewHeight;
	private float inspectorWidth;

	private int brushSize = 32;
	private int mapWidth = 1024;
	private int mapHeight = 1024;

	private Map tileMap;

	/// <summary>
	/// Init an editor window
	/// </summary>
	[MenuItem ("Tyle Editor/Window")]
	public static void Init()
	{
		windowInstance = (TyleEditorWindow)EditorWindow.GetWindow (typeof(TyleEditorWindow));
	
		windowInstance.GatherTileSets();
	}

	/// <summary>
	/// Called when the window is enabled; use it to make sure we're displaying the right textures
	/// </summary>
	void OnEnable()
	{
		GatherTextures();
	}

	/// <summary>
	/// For displaying all of the GUI in the window
	/// </summary>
	void OnGUI()
	{
		//get some sizes that we'll use later
		scrollViewWidth = position.width * .75f;
		scrollViewHeight = position.height - 24;

		inspectorWidth = position.width * .25f - 12;

		DrawMap();

		DrawMapControls();
	}

	/// <summary>
	/// Draws the map
	/// Should be called from OnGUI
	/// </summary>
	void DrawMap()
	{
		//Scroll view for displaying the texture
		GUILayout.BeginArea(new Rect(6,6, scrollViewWidth, scrollViewHeight));
		{
			textureScrollPosition = GUILayout.BeginScrollView(textureScrollPosition);
			{
				if(mapTexture && mapTexture.width > 0 && mapTexture.height > 0)
				{
					GUILayout.Box (mapTexture, GUIStyle.none, GUILayout.Width(mapTexture.width), GUILayout.Height(mapTexture.height));
				}

				if(selectionSpaceTexture && selectionSpaceTexture.width > 0 && selectionSpaceTexture.height > 0)
					GUI.Box (selectionOutlineBox, selectionSpaceTexture, GUIStyle.none);
				
				//Do this here because the events are gathered relative to the layout scope
				
				//Get Events
				Event current = Event.current;

				//If the mouse is clicked while we're over the map bounds, lets start drawing
				if(current.isMouse &&
				   current.mousePosition.x <= mapWidth && current.mousePosition.y <= mapHeight &&
				   current.mousePosition.x >= 0 && current.mousePosition.y >= 0)
				{
					PaintTexture(current.mousePosition);
				}

				//If the mouse is over part of the map, show where a tile may be painted
				if(current.mousePosition.x <= mapWidth && current.mousePosition.y <= mapHeight &&
				   current.mousePosition.x >= 0 && current.mousePosition.y >= 0)
				{
					PaintTileSelection(current.mousePosition);
				}
			}
			GUILayout.EndScrollView();
		}
		GUILayout.EndArea();
	}

	/// <summary>
	/// Draws the map controls
	/// Should be called from OnGUI
	/// </summary>
	void DrawMapControls()
	{
		GUILayout.BeginArea(new Rect(scrollViewWidth + 6, 6, inspectorWidth, scrollViewHeight));
		{
			
			//Ask for which tileset we want to use
			int newTileIndex = EditorGUILayout.Popup(tileSetIndex, tileSetList.ToArray());

			//If the tile set changes, repopulate the textures
			if(newTileIndex != tileSetIndex)
			{
				tileSetIndex = newTileIndex;
				GatherTextures();
			}

			//BREATHING ROOM
			EditorGUILayout.Space();
			//BREATHING ROOM
			
			//Draw anther scrollable panel for all the possible textures
			selectionScrollPosition = 
				GUILayout.BeginScrollView(selectionScrollPosition, GUILayout.Height(400));
			{
				int newSelectedTextureIndex = GUILayout.SelectionGrid(selectedTextureIndex, 
				                                               textureList.ToArray(),
				                                               1,
				                                               GUILayout.Width(inspectorWidth - 12),
				                                               GUILayout.Height(textureList.Count * 64));
				//If the selected texture changes update some info
				if(newSelectedTextureIndex != selectedTextureIndex)
				{
					selectedTextureIndex = newSelectedTextureIndex;

					GetSelectedTexture();
				}
			}
			GUILayout.EndScrollView();

			//BREATHING ROOM
			EditorGUILayout.Space();
			//BREATHING ROOM
			
			GUILayout.Label("Brush Size:\t\t\t\t\t " + brushSize);

			//BREATHING ROOM
			EditorGUILayout.Space ();
			EditorGUILayout.Space ();	
			//BREATHING ROOM

			//Map Creation Tools
			GUILayout.Label("Map Creation");
			mapWidth = EditorGUILayout.IntField("Map Width: ", mapWidth);
			mapHeight = EditorGUILayout.IntField("Map Height: ", mapHeight);
			
			//Round mapWidth and mapHeight to nearest power of two
			mapWidth = Mathf.ClosestPowerOfTwo(mapWidth);
			mapHeight = Mathf.ClosestPowerOfTwo(mapHeight);
			
			if(GUILayout.Button("Create Map"))
			{
				//Double check that we want to overwrite the map
				if(mapTexture && mapTexture.width > 0 && mapTexture.height > 0)
				{
					if(EditorUtility.DisplayDialog("Overwrite Map", "Are you sure you want to overwrite the current map?", "Yes", "No"))
					{
						tileMap = new Map(mapWidth, mapHeight);
						mapTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
						selectionSpaceTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
					}
				}
				else
				{
					tileMap = new Map(mapWidth, mapHeight);
					mapTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
					selectionSpaceTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
				}
			}

			//Everything after this will be anchored to the bottom of the area
			GUILayout.FlexibleSpace();
			
			DrawMapControlButtons();
		}
		GUILayout.EndArea();
	}

	/// <summary>
	/// Draws the map control buttons
	/// To be called from DrawMapControls
	/// </summary>
	void DrawMapControlButtons()
	{
		//A button for reloading textures from the disk
		if(GUILayout.Button("Reload Tiles"))
		{
			GatherTileSets();
			GatherTextures();
		}
		
		EditorGUILayout.Space();
		
		//A button for saving out the map
		if(GUILayout.Button("Save Map"))
		{
			string mapJson = Map.SerializeMap(tileMap);
			
			string path = EditorUtility.SaveFilePanel("Save Map", "Assets/Resources/Maps", "NewMap","map");
			
			//don't continue of they cancel
			if(string.IsNullOrEmpty(path))
				return;
			
			System.IO.File.WriteAllText(path, mapJson);
		}	
		
		//A button for loading a map
		if(GUILayout.Button("Load Map"))
		{
			string path = EditorUtility.OpenFilePanel("Load Map", "Assets/Resources/Maps", "map");
			
			//don't continue of they cancel 
			if(string.IsNullOrEmpty(path))	
				return;
			
			string mapJson = System.IO.File.ReadAllText(path);
			
			tileMap = Map.DeserializeMap(mapJson);
			
			mapWidth = tileMap.width;
			mapHeight = tileMap.height;				
			
			mapTexture = Map.GenerateMap(tileMap);
			
			Repaint();
		}
	}

	/// <summary>
	/// Populate the tileSetList with every folder inside of Resources/Textures
	/// </summary>
	void GatherTileSets()
	{
		DirectoryInfo texturesDir = new DirectoryInfo("Assets/Resources/Textures/");
		DirectoryInfo[] tileSetDirs = texturesDir.GetDirectories();

		tileSetList.Clear();

		foreach(DirectoryInfo d in tileSetDirs)
		{
			tileSetList.Add(d.Name);
		}

		GatherTextures();
	}

	/// <summary>
	/// Populate the textureList with all textures ending with TileTexture
	/// </summary>
	void GatherTextures()
	{
		//there has to be a tile set directory that we want to get
		if(tileSetList.Count <= 0)
			return;

		selectedTextureIndex = 0;

		string tileSetDirectory = tileSetList[tileSetIndex];

		Texture2D[] textures = Resources.LoadAll<Texture2D>("Textures/" + tileSetDirectory);

		textureList.Clear();

		foreach(Texture2D t in textures)
		{
			if(t.name.EndsWith("TileTexture"))
			{
				textureList.Add(t);

				//Textures have to be square and be a power of two
				if(t.width == t.height && Mathf.IsPowerOfTwo(t.width) && Mathf.IsPowerOfTwo(t.height))
					textureNameList.Add (t.name);
			}
		}

		GetSelectedTexture();
	}

	/// <summary>
	/// Gets the selected texture and other info about it
	/// </summary>
	void GetSelectedTexture()
	{
		//Set the selected texture
		selectedTexture = textureList[selectedTextureIndex];
		
		brushSize = selectedTexture.width;
	}

	/// <summary>
	/// Draws the on map at the given position, taking into account the scrolling offsets
	/// </summary>
	/// <param name="mousePos">Mouse position.</param>
	void PaintTexture(Vector2 mousePos)
	{
		//If there is no selected texture or no tile map, don't try to draw
		if(selectedTexture == null || tileMap == null)
			return;

		//Get top left point of the texture we want to draw
		mousePos += new Vector2(brushSize/2, brushSize/2);

		int targetX = (int)(Mathf.Round(mousePos.x / brushSize) * brushSize);
		int targetY = (int)(Mathf.Round(mousePos.y / brushSize) * brushSize);

		//Reorient target pos to top left rather than bottom left
		targetX -= brushSize;
		targetY = mapWidth - targetY;

		targetX = Mathf.Clamp(targetX, 0, mapWidth - brushSize);
		targetY = Mathf.Clamp(targetY, 0, mapHeight - brushSize);

		//Replace the pixels where we want our texture to be
		Color[] selectedPixels = selectedTexture.GetPixels();

		//Set the pixels and make sure they ACTUALLY APPLY 
		mapTexture.SetPixels(targetX, targetY, brushSize, brushSize, selectedPixels, 0);
		mapTexture.Apply();

		//Add the tile to the map
		tileMap.AddTile(targetX, targetY, brushSize, brushSize, tileSetList[tileSetIndex], selectedTexture.name);

		//Make sure that the display updates
		Repaint();
	}

	/// <summary>
	/// Paints where a tile will be painted 
	/// </summary>
	/// <param name="mousePos">Mouse position.</param>
	void PaintTileSelection(Vector2 mousePos)
	{
		//Get top left point of the texture we want to draw
		mousePos += new Vector2(brushSize/2, brushSize/2);
		
		int targetX = (int)(Mathf.Round(mousePos.x / brushSize) * brushSize);
		int targetY = (int)(Mathf.Round(mousePos.y / brushSize) * brushSize);
		
		//Reorient target pos to top left rather than bottom left
		targetX -= brushSize;
		targetY -= brushSize;
		
		targetX = Mathf.Clamp(targetX, 0, mapWidth - brushSize);
		targetY = Mathf.Clamp(targetY, 0, mapHeight - brushSize);
	
		//Apply the selectionPixels
		Color[] outlinePixels = new Color[brushSize* brushSize];
		for(int i =0; i < outlinePixels.Length; i++)
			outlinePixels[i] = new Color(0.0f, 1.0f, 0.0f, 0.3f);

		selectionSpaceTexture = new Texture2D(brushSize, brushSize);
		selectionSpaceTexture.SetPixels(outlinePixels);
		selectionSpaceTexture.Apply();

		selectionOutlineBox = new Rect(targetX, targetY, brushSize, brushSize);

		Repaint();
	}
}