﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour 
{
	/// <summary>
	/// Creates a map in the game scene as a plane
	/// </summary>
	/// <param name="map">Map.</param>
	public static IEnumerator CreateMap(Map map)
	{
		//if there is an existing map, destroy it
		GameObject previousMap = GameObject.Find("Map");
		if(previousMap != null)
		{
			if(Application.isPlaying)
			{
				GameObject.Destroy(previousMap);
			}
			else
				GameObject.DestroyImmediate(previousMap);
		}

		GameObject mapPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);

		mapPlane.name = "NewMap";

		//Set size of the plane to be resonable in the game world
		float mapScaleX = map.width / 32.0f;
		float mapScaleY = map.height / 32.0f;

		mapPlane.transform.localScale =  new Vector3(mapScaleX, mapScaleY, 1.0f);

		//Generate the texture and apply it to the object
		List<Texture2D> mapLayers = Map.GenerateMapTextures(map);

		//Combine all map layers into one texture
		Texture2D mapTexture = TyleEditorUtils.NewTransparentTexture(map.width, map.height);

		foreach(Texture2D layer in mapLayers)
		{
			Color[] layerPixels = layer.GetPixels();

			for(int i = 0; i < layerPixels.Length; i++)
			{
				int pixelX;
				int pixelY;

				if(i == map.width)
					pixelX = map.width;
				else
					pixelX = i % map.width;

				pixelY = i / (map.width + 1);

				if(layerPixels[i].a > 0)
					mapTexture.SetPixel(pixelX, pixelY, layerPixels[i]);
			}
		}

		mapTexture.Apply();

		//Use the shared material only in the editor
#if UNITY_EDITOR
		mapPlane.renderer.sharedMaterial.mainTexture = mapTexture;
#else
		mapPlane.renderer.material = mapMat;
#endif

		//Add the colliders into the scene
		CreateColliders(map);

		yield return null;
	}

	private static void CreateColliders(Map map)
	{
		foreach(Tile t in map.Tiles)
		{
			if(!t.Passable || t.Trigger)
			{
				GameObject specialTile = new GameObject();
				specialTile.AddComponent("BoxCollider2D");
				specialTile.AddComponent("Rigidbody2D");

				if(!t.Passable)
					specialTile.name = "ImpassibleTile";
				else if(t.Trigger)
					specialTile.name = "Trigger";

				BoxCollider2D collider = specialTile.GetComponent<BoxCollider2D>();
				Rigidbody2D rigidbody = specialTile.GetComponent<Rigidbody2D>();

				float x = t.Position.x;
				float y = t.Position.y;
				float width = t.Size.x;
				float height = t.Size.y;
				float mapWidth = map.width / 32.0f;
				float mapHeight = map.height / 32.0f;


				//Rescale and reorient
				width = (width/32.0f);
				height = (height/32.0f);
				x = (x/32.0f - mapWidth/2 + width/2);
				y = (y/32.0f - mapHeight/2 + height/2);

				//Set up object
				specialTile.transform.position = new Vector3(x,y,0.0f);

				//Setup collider and rigidbody
				collider.size = new Vector2(width,height);

				if(t.Trigger)
				{
					//If it's a trigger, mark it as such, also add a trigger component to the game object
					collider.isTrigger = true;
					TriggerProperties triggerProperties = (TriggerProperties)specialTile.AddComponent<TriggerProperties>();
					triggerProperties.type = t.Type;

					triggerProperties.eventName = t.EventName;
					triggerProperties.travelTo = t.TravelTo;
					triggerProperties.travelFrom = t.TravelFrom;
				}

				rigidbody.isKinematic = true;

				//Parent to the map for organization
				specialTile.transform.parent = GameObject.Find("NewMap").transform;
			}
		}

		GameObject.Find("NewMap").name = "Map";
	}

}
