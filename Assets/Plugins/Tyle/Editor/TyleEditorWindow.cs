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
using System.Collections.Generic;
using System.IO;

public enum BrushType
{
	Textures,
	BoundingBoxes,
	Triggers
}

public enum BrushMode
{
	Painting,
	Erasing,
	Selecting
}

[ExecuteInEditMode]
public class TyleEditorWindow : EditorWindow 
{
	public static TyleEditorWindow windowInstance;

	private Event currentEvent;

	private BrushType currentBrush = BrushType.Textures;
	private BrushMode currentMode = BrushMode.Painting;

	//Selected tiles for modifying later
	private List<Tile> selectedTiles = new List<Tile>();
	
	//Textures for various things we need to display on the string
	private Texture2D mapTexture = new Texture2D(0,0);
	private Texture2D selectionSpaceTexture = new Texture2D(0,0);
	private Texture2D detailTexture = new Texture2D(0,0);

	private Texture2D backgroundTexture = TyleEditorUtils.NewBasicTexture(Color.gray,1,1);
	
	private Rect selectionOutlineBox = new Rect(0,0,0,0);
	private Rect detailBox = new Rect(0,0,0,0);

	//Selectable and/or indexed variables

	//The tile set chosen at the top of the tools panel
	private List<string> tileSetList = new List<string>();
	private int tileSetIndex = 0;

	//The texture we chose from the loaded textures
	private static List<Texture2D> textureList = new List<Texture2D>();
	private static List<string> textureNameList = new List<string>();
	private Texture2D selectedTexture;
	private int selectedTextureIndex;

	//Scroll positions
	private Vector2 textureScrollPosition = Vector2.zero;
	private Vector2 selectionScrollPosition = Vector2.zero;
	private Vector2 inspectorScrollPosition = Vector2.zero;

	//Window dimensions 
	private float scrollViewWidth; 
	private float scrollViewHeight;
	private float inspectorWidth;

	//Map and texture dimensions
	private int brushSize = 32;
	private int mapWidth = 1024;
	private int mapHeight = 1024;

	//The map
	private Map tileMap;

	/// <summary>
	/// Init an editor window
	/// </summary>
	[MenuItem ("Tyle Editor/Window")]
	public static void Init()
	{
		windowInstance = (TyleEditorWindow)EditorWindow.GetWindow (typeof(TyleEditorWindow));
	
		windowInstance.GatherTileSets();
		windowInstance.GatherTextures();
	}

	/// <summary>
	/// Called when the window is in focus; use it to make sure we're displaying the right textures
	/// </summary>
	void OnFocus()
	{
		GatherTextures();
	}

	/// <summary>
	/// For displaying all of the GUI in the window
	/// </summary>
	void OnGUI()
	{
		//Store the current event
		currentEvent = Event.current;

		//Keep things up to date when events fire
		if(currentEvent != null)
			Repaint();

		//get some sizes that we'll use later
		scrollViewWidth = position.width * .75f;
		scrollViewHeight = position.height - 24;

		inspectorWidth = position.width * .25f - 12;

		DrawMap();

		DrawMapControls();
	}

	/// <summary>
	/// Update code for the window
	/// </summary>
	void Update()
	{
		HandleMouseEvents();
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
				//This if block just prevents a bunch of null pointer errors later on
				if(tileMap != null)
				{

					if(mapTexture && mapTexture.width > 0 && mapTexture.height > 0)
					{
						//Draw background texture
						if(backgroundTexture)
							GUI.DrawTexture(new Rect(0,0, mapTexture.width, mapTexture.height), backgroundTexture, ScaleMode.StretchToFill);

						//Then draw map tiles
						GUILayout.Box (mapTexture, GUIStyle.none, GUILayout.Width(mapTexture.width), GUILayout.Height(mapTexture.height));
					}

					//Draw selection 
					if(selectionSpaceTexture && selectionSpaceTexture.width > 0 && selectionSpaceTexture.height > 0)
						GUI.DrawTexture (selectionOutlineBox, selectionSpaceTexture);

					//Draw bounding and trigger details
					if(detailTexture && detailTexture.width > 0 && detailTexture.height > 0)
						GUI.DrawTexture(detailBox, detailTexture);
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
			inspectorScrollPosition = GUILayout.BeginScrollView(inspectorScrollPosition);
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
					                                               GUILayout.Width(inspectorWidth - 48),
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

				//Brush controls
				currentBrush = (BrushType)EditorGUILayout.EnumPopup("Brush Type", currentBrush);
				currentMode = (BrushMode)EditorGUILayout.EnumPopup("Brush Mode", currentMode);

				//BREATHING ROOM
				EditorGUILayout.Space();
				//BREATHING ROOM

				GUILayout.Label("Brush Size:\t\t\t\t\t " + brushSize);

				EditorGUILayout.Space ();

				//Display info about the selected tiles if there are any
				//ONLY DISPLAY THIS DURING THE LAYOUT EVENT, NOT REPAINT
				if(selectedTiles.Count > 0)
				{
					Tile tileToDisplay = selectedTiles[0];

					if(tileToDisplay != null)
					{
						int x = (int)tileToDisplay.Position.x;
						int y = (int)tileToDisplay.Position.y;
						int width = (int)tileToDisplay.Size.x;
						int height = (int)tileToDisplay.Size.y;
				
						string tileSet = tileToDisplay.TileSet;
						string textureName = tileToDisplay.TextureName;
						bool passable = tileToDisplay.Passable;
						bool trigger = tileToDisplay.Trigger;
						string triggerName = tileToDisplay.TriggerName;
						TriggerType triggerType = tileToDisplay.Type;

						EditorGUILayout.LabelField("Tile Summary");
						EditorGUILayout.LabelField("X Position: " + x);
						EditorGUILayout.LabelField("Y Position: " + y);
						EditorGUILayout.LabelField("Width: " + width);
						EditorGUILayout.LabelField("Height: " + height);
						EditorGUILayout.LabelField("Tile Set: " + tileSet);
						EditorGUILayout.LabelField("Texture Name: " + textureName);

						if(!passable)
							EditorGUILayout.LabelField("Type: Impassible");
						else if(trigger)
							EditorGUILayout.LabelField("Type: Trigger");
						else
							EditorGUILayout.LabelField("Type: Regular");

						//Controls to change trigger properties
						if(trigger)
						{
							string newTriggerName = EditorGUILayout.TextField("Trigger Name: ", triggerName);
							if(newTriggerName != triggerName)
							{
								triggerName = newTriggerName;
								tileToDisplay.TriggerName = triggerName;
							}

							TriggerType newTriggerType = (TriggerType)EditorGUILayout.EnumPopup("Trigger Type: ", triggerType);
							if(newTriggerType != triggerType)
							{
								triggerType = newTriggerType;
								tileToDisplay.Type = triggerType;
							}
						}
					}
				}

				//BREATHING ROOM
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
							detailTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);

							detailBox = new Rect(0,0, mapWidth, mapHeight);
						}
					}
					else
					{
						tileMap = new Map(mapWidth, mapHeight);
						mapTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
						selectionSpaceTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);
						detailTexture = TyleEditorUtils.NewTransparentTexture(mapWidth, mapHeight);

						detailBox = new Rect(0,0, mapWidth, mapHeight);
					}
				}

				//Everything after this will be anchored to the bottom of the area
				GUILayout.FlexibleSpace();
				
				DrawMapControlButtons();
			}
			GUILayout.EndScrollView();
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
			
			mapTexture = Map.GenerateMapTexture(tileMap);
			detailTexture = Map.GenerateDetailTexture(tileMap);

			detailBox = new Rect(0,0, mapWidth, mapHeight);

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
	/// Gets the top left of quadrant that the given vector resides within
	/// </summary>
	/// <returns>The top left of quadrant.</returns>
	/// <param name="position">Position.</param>
	Vector2 GetTopLeftOfQuadrant(Vector2 position)
	{
		position += new Vector2(brushSize/2, 0);
		
		int targetX = (int)(Mathf.Round(position.x / brushSize) * brushSize);
		int targetY = (int)(Mathf.Round(position.y / brushSize) * brushSize);
		
		//Reorient target pos to top left rather than bottom left
		targetX -= brushSize;
		targetY = mapWidth - targetY;
		
		targetX = Mathf.Clamp(targetX, 0, mapWidth - brushSize);
		targetY = Mathf.Clamp(targetY, 0, mapHeight - brushSize);

		return new Vector2(targetX, targetY);
	}

	/// <summary>
	/// Paint the source texture onto the destination texture at the specified position
	/// </summary>
	/// <param name="postion">Postion.</param>
	/// <param name="sourceTexture">Source texture.</param>
	/// <param name="destinationTexture">Destination texture.</param>
	void Paint(Vector2 postion, Texture2D sourceTexture, Texture2D destinationTexture)
	{
		if(sourceTexture == null || destinationTexture == null)
			return;

		int targetX = (int)postion.x;
		int targetY = (int)postion.y;

		//Replace the pixels where we want our texture to be
		Color[] sourcePixels = sourceTexture.GetPixels();

		//Set the pixels and make sure they ACTUALLY APPLY 
		destinationTexture.SetPixels(targetX, targetY, brushSize, brushSize, sourcePixels, 0);
		destinationTexture.Apply();
		
		//Make sure that the display updates
		Repaint();
	}

	/// <summary>
	/// Handles the mouse events.
	/// </summary>
	void HandleMouseEvents()
	{
		//Get Events
		if(currentEvent == null)
			return;

		Vector2 targetPos = GetTopLeftOfQuadrant(currentEvent.mousePosition);

		//If we right click, clear the selection
		if(currentEvent.isMouse && currentEvent.button == 1)
			selectedTiles.Clear();

		//If the mouse is clicked while we're over the map bounds, lets start drawing
		if(currentEvent.isMouse &&
		   currentEvent.mousePosition.x <= mapWidth && currentEvent.mousePosition.y <= mapHeight &&
		   currentEvent.mousePosition.x >= 0 && currentEvent.mousePosition.y >= 0)
		{
			//Mouse events based on current brush type
			switch(currentBrush)
			{ 
			case BrushType.BoundingBoxes:
				switch(currentMode)
				{
				case BrushMode.Painting:
					//Modify Tile at that position
					Tile tileToMakeImpassible = tileMap.GetTile((int)targetPos.x, (int)targetPos.y);
					
					if(tileToMakeImpassible != null)
					{
						tileToMakeImpassible.Passable = false;
						tileToMakeImpassible.Trigger = false;
						
						Texture2D boundingTexture = TyleEditorUtils.NewOutlineTexture(Color.red, brushSize, brushSize, 5, 0);
						Paint (targetPos, boundingTexture, detailTexture);
					}
					break;
				case BrushMode.Erasing:
					//Remove the tile detail at the given position 
					Texture2D erasingTexture = TyleEditorUtils.NewTransparentTexture(brushSize, brushSize);
					Paint (targetPos, erasingTexture, detailTexture);
					
					Tile tileToMakePassible = tileMap.GetTile((int)targetPos.x, (int)targetPos.y, brushSize, brushSize);
					if(tileToMakePassible != null)
					tileToMakePassible.Passable = true;
					break;
				case BrushMode.Selecting:
					break;
				
				}
				break;
			case BrushType.Triggers:
				switch(currentMode)
				{
				case BrushMode.Painting:
					//Modify Tile at that position
					Tile tileToMakeTrigger = tileMap.GetTile((int)targetPos.x, (int)targetPos.y);
					
					if(tileToMakeTrigger != null)
					{
						tileToMakeTrigger.Passable = true;
						tileToMakeTrigger.Trigger = true;
						
						Texture2D triggerTexture = TyleEditorUtils.NewOutlineTexture(Color.blue, brushSize, brushSize, 5, 10);
						Paint (targetPos, triggerTexture, detailTexture);
					}
					break;
				case BrushMode.Erasing:
					//Remove the tile detail at the given position 
					Texture2D erasingTexture = TyleEditorUtils.NewTransparentTexture(brushSize, brushSize);
					Paint (targetPos, erasingTexture, detailTexture);
					
					Tile tileToMakeNotTrigger = tileMap.GetTile((int)targetPos.x, (int)targetPos.y, brushSize, brushSize);
					if(tileToMakeNotTrigger != null)
					tileToMakeNotTrigger.Trigger = false;
					break;
				case BrushMode.Selecting:
					if(currentEvent.type == EventType.MouseDown && currentEvent.button != 1)
					{
						selectedTiles.Clear();
						Tile t = tileMap.GetTile((int)targetPos.x, (int)targetPos.y);
						if(t != null)
							selectedTiles.Add(t);
					}
					break;
				}
				break;
				//Default is painting
			default:
				switch(currentMode)
				{
				case BrushMode.Painting:

					if(selectedTexture != null)
					{
						Paint (targetPos, selectedTexture, mapTexture);
						//Add the tile to the map
						tileMap.AddTile((int)targetPos.x, (int)targetPos.y, brushSize, brushSize, tileSetList[tileSetIndex], selectedTexture.name);
					}
					break;
				case BrushMode.Erasing:
					//Remove the tile at the given position and paint a transparent texture in its place
					Texture2D erasingTexture = TyleEditorUtils.NewTransparentTexture(brushSize, brushSize);
					Paint (targetPos, erasingTexture, mapTexture);
					
					tileMap.RemoveTile((int)targetPos.x, (int)targetPos.y, brushSize, brushSize);
					break;
				}
				break;
			}
		}
		
		//If the mouse is over part of the map, show where a tile may be painted
		if(currentEvent.mousePosition.x <= mapWidth && currentEvent.mousePosition.y <= mapHeight &&
		   currentEvent.mousePosition.x >= 0 && currentEvent.mousePosition.y >= 0 && tileMap != null)
		{
			//Create a texture of the size of the brush
			Color selectionColor = new Color(0.0f, 0.5f, 0.0f, 0.3f);
			
			if(currentMode == BrushMode.Erasing)
				selectionColor = new Color(0.5f, 0.0f, 0.0f, 0.7f);
			if(currentMode == BrushMode.Selecting)
				selectionColor = new Color(0.0f, 0.0f, 0.5f, 0.7f);
			
			selectionSpaceTexture = TyleEditorUtils.NewBasicTexture(selectionColor, brushSize, brushSize);
			selectionSpaceTexture.Apply();
			selectionOutlineBox = new Rect(targetPos.x, mapWidth - targetPos.y - brushSize, brushSize, brushSize);
		}
		else
		{
			//Clear the selection texture
			selectionSpaceTexture = TyleEditorUtils.NewTransparentTexture(brushSize, brushSize);
		}

		//If we're in selection mode
	}
}