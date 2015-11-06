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

public class CameraFollowPlayer : MonoBehaviour 
{
	GameObject player;
	GameObject map;

	// Use this for initialization
	void Start () 
	{
		player = GameObject.Find("Player");
		map = GameObject.Find("Map");
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(player == null || map == null)
		{
			//If the map goes null, it has been cleared for reloading, try to find it again
			map = GameObject.Find("Map");
			return;
		}

		Vector3 boundPosition = player.transform.position;

		//Keep the camera so that it will never view outside the map
		float topMapBound = map.transform.GetComponent<Renderer>().bounds.center.y + map.transform.GetComponent<Renderer>().bounds.size.y/2;
		float leftMapBound = map.transform.GetComponent<Renderer>().bounds.center.x - map.transform.GetComponent<Renderer>().bounds.size.x/2;
		float bottomMapBound = map.transform.GetComponent<Renderer>().bounds.center.y - map.transform.GetComponent<Renderer>().bounds.size.y/2;
		float rightMapBound = map.transform.GetComponent<Renderer>().bounds.center.x + map.transform.GetComponent<Renderer>().bounds.size.x/2;

		float verticalExtent = GetComponent<Camera>().orthographicSize + 1;
		float horizontalExtent = verticalExtent * ((float)Screen.width / (float)Screen.height);

		float boundX = Mathf.Clamp(boundPosition.x, leftMapBound + horizontalExtent, rightMapBound - horizontalExtent);
		float boundY = Mathf.Clamp(boundPosition.y, bottomMapBound + verticalExtent, topMapBound - verticalExtent);

		this.transform.position = new Vector3(boundX, boundY, -10.0f);
	}
}
