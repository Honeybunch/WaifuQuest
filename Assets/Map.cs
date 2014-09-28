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

public class Map : MonoBehaviour {

	public bool displayGrid = true;
	public Color gridColor = Color.red;

	[HideInInspector]
	public List<GameObject> Tiles = new List<GameObject>();

	[HideInInspector]
	public float startX;
	[HideInInspector]
	public float startY;
	[HideInInspector]
	public float width;
	[HideInInspector]
	public float height;
	[HideInInspector]
	public float tileWidth;

	/// <summary>
	/// Draws gizmos only if displayGrid is true and the game isn't playing
	/// </summary>
	void OnDrawGizmos()
	{
		if(displayGrid == false || Application.isPlaying)
			return;

		Gizmos.color = gridColor;

		Vector3 currentPos = transform.position;

		float leftBound = currentPos.x - (width/2.0f * tileWidth);
		float rightBound = currentPos.x + (width/2.0f * tileWidth);
		float topBound = currentPos.y + (height/2.0f * tileWidth);
		float bottomBound = currentPos.y - (height/2.0f * tileWidth);

		//If the width or height is even, we have the modify the grid slightly
		if(width % 2 == 0)
		{
			leftBound -= (tileWidth / 2);
			rightBound -= (tileWidth/2);
		}

		if(height % 2 == 0)
		{
			topBound -= (tileWidth / 2);
			bottomBound -= (tileWidth/2);
		}

		//Draw vertical lines
		for(int x = 0; x < width + 1; x++)
		{
			Vector3 start = new Vector3(leftBound + (x * tileWidth) , topBound, 0);
			Vector3 end = new Vector3(leftBound + (x * tileWidth), bottomBound, 0);

			Gizmos.DrawLine(start,end);
		}

		//Draw horizontal lines
		for(int y = 0; y < height + 1; y++)
		{
			Vector3 start = new Vector3(leftBound, topBound - (y * tileWidth), 0);
			Vector3 end = new Vector3(rightBound, topBound - (y * tileWidth), 0);
			
			Gizmos.DrawLine(start,end);
		}
	}
}
