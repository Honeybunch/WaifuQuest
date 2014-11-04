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

public class ScreenFader : MonoBehaviour
{
	public float fadeSpeed = 0.2f;
	
	public float alpha = 0.0f;

	Texture2D blackTexture;

	void Start()
	{
		blackTexture = TyleEditorUtils.NewBasicTexture(Color.black, 1,1);
	}

	void OnGUI()
	{
		//Set this depth so that this will always be in front of any other GUI calls
		GUI.depth = -1;

		//Draw a simple black texture across the whole screen
		GUI.color = new Color(1.0f,1.0f,1.0f,alpha);
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),blackTexture, ScaleMode.StretchToFill);
	}

	/// <summary>
	/// Fades to the GUI texture to black; to be called from a coroutine
	/// </summary>
	public IEnumerator FadeToBlack()
	{
		while(alpha <= 0.995f)
		{
			alpha = Mathf.Lerp(alpha, 1.0f, fadeSpeed);

			yield return null;
		}

		alpha = 1.0f;
	}

	/// <summary>
	/// Fades to the GUI texture to clear; to be called from a coroutine
	/// </summary>
	public IEnumerator FadeToClear()
	{
		while(alpha >= 0.05f)
		{
			alpha = Mathf.Lerp(alpha, 0.0f, fadeSpeed);

			yield return null;
		}

		alpha = 0.0f;
	}


}
