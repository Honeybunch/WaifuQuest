using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapLoader : MonoBehaviour 
{
	/// <summary>
	/// Creates a map in the game scene as a plane
	/// </summary>
	/// <param name="map">Map.</param>
	public static void CreateMap(Map map)
	{
		//if there is an existing map, destroy it
		GameObject previousMap = GameObject.Find("Map");
		if(previousMap != null)
			GameObject.DestroyImmediate(previousMap);

		GameObject mapPlane = GameObject.CreatePrimitive(PrimitiveType.Quad);

		mapPlane.name = "Map";

		//Set size of the plane to be resonable in the game world
		float mapScaleX = map.width / 32.0f;
		float mapScaleY = map.height / 32.0f;

		mapPlane.transform.localScale =  new Vector3(mapScaleX, mapScaleY, 1.0f);

		//Generate the texture and apply it to the object
		Texture2D mapTexture = Map.GenerateMapTexture(map);
		Material mapMat = new Material(mapPlane.renderer.sharedMaterial);
		mapMat.mainTexture = mapTexture;

		//Use the shared material only in the editor
#if UNITY_EDITOR
		mapPlane.renderer.sharedMaterial = mapMat;
#else
		mapPlane.renderer.material = mapMat;
#endif

		//Add the colliders into the scene
		CreateColliders(map);
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
					collider.isTrigger = true;

				rigidbody.isKinematic = true;

				//Parent to the map for organization
				specialTile.transform.parent = GameObject.Find("Map").transform;
			}
		}
	}

}
