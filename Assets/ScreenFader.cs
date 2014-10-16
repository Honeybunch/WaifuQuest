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
