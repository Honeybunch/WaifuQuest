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

using UnityEditor;
using UnityEngine;
using System.Collections;

public class TyleEditorUtils
{
	/// <summary>
	/// Creates a texture and makes every pixel transparent
	/// </summary>
	/// <returns>The transparent texture.</returns>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	public static Texture2D NewTransparentTexture(int width, int height)
	{
		Texture2D texture = new Texture2D(width, height);

		Color[] pixels = texture.GetPixels();

		for(int i =0 ; i < pixels.Length; i++)
			pixels[i].a = 0;

		texture.SetPixels(pixels);
		texture.Apply();

		return texture;
	}
}


