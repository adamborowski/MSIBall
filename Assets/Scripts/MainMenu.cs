using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{
	public GUISkin guiSkin;
	public Texture2D background, LOGO;
	public bool DragWindow = false;
	
	
	private string clicked = "";
	private Rect WindowRect = new Rect((Screen.width / 2) - 100, Screen.height / 2, 200, 200);
	
	private void Start()
	{

	}

	private void OnGUI()
	{

		if (background != null)
			GUI.DrawTexture(new Rect(0,0,Screen.width , Screen.height),background);
		if (LOGO != null)
			GUI.DrawTexture(new Rect((Screen.width / 2) - 100, 30, 200, 200), LOGO);
		
		GUI.skin = guiSkin;
		if (clicked == "")
		{
			WindowRect = GUI.Window(0, WindowRect, menuFunc, "Main Menu");
		}

	}

	
	private void menuFunc(int id)
	{

		GUILayout.Label ("z emocjami:");
		if (GUILayout.Button("Graj"))
		{
			clicked = "emotions";
			Application.LoadLevel("BallGame");
		}

		GUILayout.Label ("bez emocji:");
		if (GUILayout.Button("Latwy"))
		{
			clicked = "easy";
			Application.LoadLevel("BallGame");
		}
		if (GUILayout.Button("Sredni"))
		{
			clicked = "medium";
			Application.LoadLevel("BallGame");
		}
		if (GUILayout.Button("Trudny"))
		{
			clicked = "hard";
			Application.LoadLevel("BallGame");
		}

		GUILayout.Space(10);
		if (GUILayout.Button("Wyjdz"))
		{
			Application.Quit();
		}

		if (DragWindow)
			GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));

	}
	
	private void Update()
	{
		if (clicked == "about" && Input.GetKey (KeyCode.Escape))
			clicked = "";
	}
}