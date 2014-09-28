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

public class PlayerMovement : MonoBehaviour 
{
	public float speed = 0.1f;

	private Vector3 velocity;

	// Use this for initialization
	void Start () 
	{
		velocity = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () 
	{
		velocity = Vector3.zero;

		//Get Key input in 4 directions
		if(Input.GetKey(KeyCode.W))
			velocity.y = speed;

		if(Input.GetKey(KeyCode.A))
			velocity.x = -speed;

		if(Input.GetKey(KeyCode.S))
			velocity.y = -speed;

		if(Input.GetKey(KeyCode.D))
			velocity.x = speed;

		transform.position += velocity;
	}
}
