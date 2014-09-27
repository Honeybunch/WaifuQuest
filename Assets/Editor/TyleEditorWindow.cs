using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TyleEditorWindow : EditorWindow 
{
	public static TyleEditorWindow windowInstance;

	public Texture2D texture = new Texture2D(0,0);
	public static List<Texture2D> textureList = new List<Texture2D>();
	public static List<string> textureNameList = new List<string>();
	private int selectedTexture;

	private Vector2 textureScrollPosition = Vector2.zero;
	private Vector2 selectionScrollPosition = Vector2.zero;

	private int brushSize = 32;
	private int mapWidth = 1024;
	private int mapHeight = 1024;

	private bool displayGrid = false;
	private Color gridColor = Color.red;

	private Bounds mapBounds = new Bounds();

	/// <summary>
	/// Init an editor window
	/// </summary>
	[MenuItem ("Tyle Editor/Window")]
	public static void Init()
	{
		windowInstance = (TyleEditorWindow)EditorWindow.GetWindow (typeof(TyleEditorWindow));
	
		windowInstance.GatherTextures();
	}

	/// <summary>
	/// For displaying all of the GUI in the window
	/// </summary>
	void OnGUI()
	{
		//get some sizes that we'll use later
		float scrollViewWidth = position.width * .75f;
		float scrollViewHeight = position.height - 24;

		float inspectorWidth = position.width * .25f - 12;

		//Scroll view for displaying the texture
		GUILayout.BeginArea(new Rect(6,6, scrollViewWidth, scrollViewHeight));
			textureScrollPosition = GUILayout.BeginScrollView(textureScrollPosition);

				//Debug.Log(texture.GetPixel(0,0));
				GUILayout.Box (texture, GUIStyle.none, GUILayout.Width(texture.width), GUILayout.Height(texture.height));

				//Do this here because the events are gathered relative to the layout scope
				
				//Get Events
				Event current = Event.current;
				
				//If the mouse is clicked while we're over the map bounds, lets start drawing
				if(mapBounds.Contains(current.mousePosition))
				{
					DrawOnMap(current.mousePosition);
				}

			GUILayout.EndScrollView();

		GUILayout.EndArea();                 

		//Draw an inspector like set of controls on the right side
		GUILayout.BeginArea(new Rect(scrollViewWidth + 6, 6, inspectorWidth, scrollViewHeight));

			//Draw anther scrollable panel for all the possible textures
			selectionScrollPosition = GUILayout.BeginScrollView(selectionScrollPosition, GUILayout.Height(400));

				selectedTexture = GUILayout.SelectionGrid(selectedTexture, 
		                                    textureList.ToArray(),
		                                    1,
		                                    GUILayout.Width(inspectorWidth - 12),
		                                    GUILayout.Height(128));

			GUILayout.EndScrollView();

			EditorGUILayout.Space();

			//Brush controls
			brushSize = EditorGUILayout.IntField("Brush Size:", brushSize);
			brushSize = Mathf.ClosestPowerOfTwo(brushSize);//Round the brush size to the next power of two

			EditorGUILayout.Space ();
			EditorGUILayout.Space ();	

			//Map Creation Tools
			GUILayout.Label("Map Creation");
			mapWidth = EditorGUILayout.IntField("Map Width: ", mapWidth);
			mapHeight = EditorGUILayout.IntField("Map Height: ", mapHeight);

			//Round mapWidth and mapHeight to nearest power of two
			mapWidth = Mathf.ClosestPowerOfTwo(mapWidth);
			mapHeight = Mathf.ClosestPowerOfTwo(mapHeight);

			//Recalculate the bounds of the map
			float boundsWidth;
			float boundsHeight;
			
			if(texture.width > scrollViewWidth)
				boundsWidth = scrollViewWidth;
			else
				boundsWidth = texture.width;
			
			if(texture.height > scrollViewHeight)
				boundsHeight = scrollViewHeight;
			else
				boundsHeight = texture.height;
	
			mapBounds.size = new Vector3(boundsWidth, boundsHeight, 0);
			mapBounds.center = new Vector3(6 + (boundsWidth/2),6 + (boundsHeight/2),0);

			if(GUILayout.Button("Create Map"))
		    {

				//Double check that we want to overwrite the map
				if(texture.width > 0 && texture.height > 0)
				{
					if(EditorUtility.DisplayDialog("Overwrite Map", "Are you sure you want to overwrite the current map?", "Yes", "No"))
						texture = new Texture2D(mapWidth, mapHeight);
				}
				else
				{
					texture = new Texture2D(mapWidth, mapHeight);
				}
			}

			EditorGUILayout.Space ();
			
			//Display Grid controls
			
			GUILayout.Label("Grid Settings");
			displayGrid = EditorGUILayout.Toggle("Display Grid:",displayGrid);
			gridColor = EditorGUILayout.ColorField("Grid Color:",gridColor);

			GUILayout.FlexibleSpace();

			//A button for reloading textures from the disk
			if(GUILayout.Button("Reload Tiles"))
			{
				GatherTextures();
			}

			//A button for saving out the map
			
			//A button for loading a map

		GUILayout.EndArea();
	}

	/// <summary>
	/// Populate the textureList with all textures ending with Tile
	/// </summary>
	void GatherTextures()
	{
		Texture2D[] textures = Resources.LoadAll<Texture2D>("Textures");

		textureList.Clear();

		foreach(Texture2D t in textures)
		{
			if(t.name.EndsWith("TileTexture"))
			{
				textureList.Add(t);
				textureNameList.Add (t.name);
			}
		}
	}

	/// <summary>
	/// Draws the on map at the given position, taking into account the scrolling offsets
	/// </summary>
	/// <param name="mousePos">Mouse position.</param>
	void DrawOnMap(Vector2 mousePos)
	{
		//Center position of where to draw the texture
		Vector2 texturePos = mousePos + textureScrollPosition - new Vector2(6,6);

		Color[] pixels = texture.GetPixels(0,0,64,64,0);

		for(int i =0; i < pixels.Length; i++)
		{
			pixels[i] = Color.green;
		}

		Debug.Log(pixels[0]);

		texture = new Texture2D(64,64);
		texture.SetPixels(pixels);
	}
}
