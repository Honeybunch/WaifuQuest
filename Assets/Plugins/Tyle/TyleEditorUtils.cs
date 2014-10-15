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
		texture.hideFlags = HideFlags.HideAndDontSave;

		Color[] pixels = texture.GetPixels();

		for(int i =0 ; i < pixels.Length; i++)
			pixels[i].a = 0;

		texture.SetPixels(pixels);
		texture.Apply();

		return texture;
	}

	/// <summary>
	/// Creates a basic texture of a given color
	/// </summary>
	/// <returns>The background texture.</returns>
	public static Texture2D NewBasicTexture(Color color, int width, int height)
	{
		//Create a 1x1 texture and just set one pixel, then have it repeat
		Texture2D texture = new Texture2D(width, height);
		texture.hideFlags = HideFlags.HideAndDontSave;
		
		Color[] colors = new Color[width * height];

		for(int i = 0; i < colors.Length; i++)
			colors[i] = color;

		texture.SetPixels(colors);
		texture.Apply();
		
		return texture;
	}

	/// <summary>
	/// Creates a new texture that is transparent with an outline
	/// </summary>
	/// <returns>The outline texture.</returns>
	/// <param name="color">Color.</param>
	/// <param name="width">Width.</param>
	/// <param name="height">Height.</param>
	/// <param name="lineWidth">Line width.</param>
	/// <param name="distanceFromEdge">Distance from edge.</param>
	public static Texture2D NewOutlineTexture(Color color, int width, int height, int lineWidth, int distanceFromEdge)
	{
		//We need a transparent starting texture
		Texture2D texture = NewTransparentTexture(width, height);
		texture.hideFlags = HideFlags.HideAndDontSave;

		int verticalBlockWidth = lineWidth; 
		int verticalBlockHeight = height - (2*distanceFromEdge);

		int horizontalBlockWidth = width - (2*distanceFromEdge);
		int horizontalBlockHeight = lineWidth;

		//Each of these will be used twice to create a radial sort of outline around the texture
		Color[] verticalBlock = new Color[verticalBlockWidth * verticalBlockHeight];
		Color[] horizontalBlock = new Color[horizontalBlockWidth * horizontalBlockHeight];

		//Fill both blocks with the color
		for(int i = 0; i < verticalBlock.Length; i++)
			verticalBlock[i] = color;
		for(int i = 0; i < horizontalBlock.Length; i++)
			horizontalBlock[i] = color;

		//Set them to the parts of the texture
		texture.SetPixels(distanceFromEdge, distanceFromEdge, verticalBlockWidth, verticalBlockHeight, verticalBlock);
		texture.SetPixels(width - distanceFromEdge - verticalBlockWidth, distanceFromEdge, verticalBlockWidth, verticalBlockHeight, verticalBlock);
		texture.SetPixels(distanceFromEdge, distanceFromEdge, horizontalBlockWidth, horizontalBlockHeight, horizontalBlock);
		texture.SetPixels(distanceFromEdge, height - distanceFromEdge - horizontalBlockHeight, horizontalBlockWidth, horizontalBlockHeight, horizontalBlock);

		texture.Apply();

		return texture;
	}
}


