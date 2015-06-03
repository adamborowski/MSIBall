using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using UnityEngine.UI;

using System;

public class EndMenu : MonoBehaviour {

    public GUISkin guiSkin;
    public Texture2D background, LOGO;
    public bool DragWindow = false;
    private string clicked = "";
    private Rect WindowRect = new Rect((Screen.width / 2) - 100, Screen.height / 2, 200, 200);


    public int score;

    private void Start()
    {
        score = 0;
    }
    
    private void OnGUI()
    {
        
        if (background != null)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
        if (LOGO != null)
            GUI.DrawTexture(new Rect((Screen.width / 2) - 100, 30, 200, 200), LOGO);
        
        GUI.skin = guiSkin;
        if (clicked == "")
        {
            WindowRect = GUI.Window(0, WindowRect, menuFunc, "MSIBall");
        }
        
    }
    
    private void menuFunc(int id)
    {
        
        GUILayout.Label("Times Up!\nScore: "+ScoreController.totalScore.ToString("0"));
        
        GUILayout.Space(10);
        if (GUILayout.Button("Main Menu"))
        {
            Application.LoadLevel("MainMenuScene");
        }
        
        if (DragWindow)
            GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
        
    }
    
    private void Update()
    {
        if (clicked == "about" && Input.GetKey(KeyCode.Escape))
            clicked = "";
    }
}
