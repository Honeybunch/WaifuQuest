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
using System.Linq;

[CustomEditor (typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor : Editor 
{
	private Tile[] tileComponents;

	private Material[] attachedMaterials;
	
	private static List<string> materialNames;
	private static List<Material> materialList;

	private int materialIndex = 0;

	/// <summary>
	/// Called when the object is selected
	/// </summary>
	void OnEnable()
	{
		if(materialList == null)
		{
			materialList = new List<Material>();
			materialNames = new List<string>();
		}

		//Gets all selected targets
		tileComponents = new Tile[targets.Length];
		for(int i =0; i < targets.Length; i++)
		{
			tileComponents[i] = (Tile)targets[i];
		}

		//Get the material on each object
		attachedMaterials = new Material[tileComponents.Length];
		for(int i =0; i < tileComponents.Length; i++)
		{
			attachedMaterials[i] = tileComponents[i].renderer.sharedMaterial;
		}

		//Find all available materials for tiles if there aren't any
		if(materialNames.Count == 0)
			GatherMaterials();

		//Find what index our current material is at
		for(int i = 0; i < materialList.Count; i++)
		{
			//Just match to the first tile's material, they'll all be edited anyway
			if(materialList[i].name == attachedMaterials[0].name)
			{
				materialIndex = i;
				break;
			}
		}

		//Turn off the selection tool because it gets in the way
		if(Tools.current != Tool.None)
			Tools.current = Tool.None;
	}

	/// <summary>
	/// Called when a tile is deselected
	/// </summary>
	void OnDisable()
	{
		Tools.current = Tool.Move;
	}

	/// <summary>
	/// Making some custom inspector GUI 
	/// </summary>
	public override void OnInspectorGUI()
	{
		//Call default inspector
		base.DrawDefaultInspector();

		GUILayout.Label("Tile Editor");
		EditorGUILayout.Space();

		//Popup (dropdown) for selecting the material
		materialIndex = EditorGUILayout.Popup("Tile Type:",materialIndex, materialNames.ToArray());

		EditorGUILayout.Space();

		//Button for changing tiles to the selection
		if(GUILayout.Button("Change Material"))
		{
			//Change the applied materials
			foreach(Tile t in tileComponents)
			{
				t.renderer.sharedMaterial = materialList[materialIndex];
			}
		}

		//Button for refreshing tile materials

		if(GUILayout.Button("Refresh Materials"))
		{
			GatherMaterials();
		}

	}

	/// <summary>
	/// Search for all materials ending with "Tile" in the name and add them to the tileMaterials
	/// </summary>
	private void GatherMaterials()
	{
		DirectoryInfo materialsPath = new DirectoryInfo("Assets/Resources/Materials");
		FileInfo[] files = materialsPath.GetFiles();
		
		foreach(FileInfo f in files)
		{
			//If we have a tile material add it to the dictionary
			if(f.Name.EndsWith("Tile.mat"))
			{
				string textureName = f.Name.Substring(0,f.Name.Length - 4);
				Material mat = (Material)Resources.Load("Materials/" + textureName);

				materialNames.Add(textureName);
				materialList.Add (mat);
			}
		}
	}
}
