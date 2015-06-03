using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MainMenu : MonoBehaviour
{
    public GUISkin guiSkin;
    public Texture2D background, LOGO;
    public bool DragWindow = false;
    private string clicked = "";
    public static GameSettings gameSettings;
    private Rect WindowRect = new Rect((Screen.width / 2) - 100, Screen.height / 2, 200, 200);


    /*
     * Menu ktrore pojawia sie przed rozpoczeciem rozgrywki
     * 
     * sklada sie z kilku prostych labeli
     * do wybory poziom trydnosci oraz tryb gry
     * ustawienia przekazywane sa do GameSettings po zatwierdzeniu
     * 
     */

    private void Start()
    {
        gameSettings = new GameSettings();
        gameSettings.gameDifficulty = GameDifficulty.MEDIUM;
        gameSettings.gameMode = GameMode.FIXED;
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

        GUILayout.Label("with ERF:");
        if (GUILayout.Button("PLAY"))
        {
            clicked = "emotions";
            gameSettings.gameMode=GameMode.EMOTIONAL;

            Application.LoadLevel("BallGame");
        }

        GUILayout.Label("without ERF:");
        if (GUILayout.Button("EASY"))
        {
            clicked = "easy";
            gameSettings.gameMode=GameMode.FIXED;
            gameSettings.gameDifficulty=GameDifficulty.EASY;
            Application.LoadLevel("BallGame");
        }
        if (GUILayout.Button("MEDIUM"))
        {
            clicked = "medium";
            gameSettings.gameMode=GameMode.FIXED;
            gameSettings.gameDifficulty=GameDifficulty.MEDIUM;
            Application.LoadLevel("BallGame");
        }
        if (GUILayout.Button("HARD"))
        {
            clicked = "hard";
            gameSettings.gameMode=GameMode.FIXED;
            gameSettings.gameDifficulty=GameDifficulty.HARD;
            Application.LoadLevel("BallGame");
        }

        GUILayout.Space(10);
        if (GUILayout.Button("Exit"))
        {
            Application.Quit();
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