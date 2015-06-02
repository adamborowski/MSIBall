using UnityEngine;
using System.Collections;
using Erf;
using System.IO;
using System;
public class ErfInitialization : MonoBehaviour {
    
    // Use this for initialization
    void Start () {
        
    }

    void Awake() { 
    //    Debug.LogWarning("Ustawienia: "+MainMenu.gameSettings.gameMode.ToString()+" / "+MainMenu.gameSettings.gameDifficulty.ToString());
        ErfLogger.SetLoggingEnabled (true);
        ErfLogger.SetAllStreamsToLoggingFile (@"d://debug.txt");
        Debug.Log ("Initializing ERF for ErfDemo, trying to load "+Directory.GetCurrentDirectory()+"\\MSIBallErf.dll");

        ErfContext context = ErfContext.GetInstance ();
        /*try {
            ExternalComponentLibrary library = ErfContext.GetInstance ().LoadComponentLibrary("ErfCore.dll");
            if (library == null) { 
                Debug.LogError("Could not load ExternalComponentLibrary library");
                return;
            }
        } catch (Exception e)
        {
            Debug.Log (e.Message);
        }
        */
        
        testMSIBallErf ();
        
        Debug.Log ("Library successfully loaded");
    }
    
    void testMSIBallErf()
    {
        try {
            ExternalComponentLibrary library = ErfContext.GetInstance ().LoadComponentLibrary ("MSIBallErf.dll");
            if (library == null) { 
                Debug.LogError("Could not load MSIBallErf library");
                return;
            }
        } catch (Exception e)
        {
            Debug.Log (e.Message);
            return;
        }
        //init:
        ExternalExpert msiBallEmotionModel = ErfContext.GetInstance ().FindExpert ("MSIBallEmotionModel");
        CharacterModel playerModel = new CharacterModel ();
        playerModel.RegisterExpert (msiBallEmotionModel);


        //collision:
        EecEvent timeElapsedEvent = new EecEvent ((int)GameEvent.STD_TIME_ELAPSED);
        timeElapsedEvent.AddValue ("NO_OF_COLLISIONS", Variant.Create (2));
        timeElapsedEvent.AddValue ("CURRENT_SPEED", Variant.Create (7.0f));
        playerModel.HandleEvent (timeElapsedEvent);
        //odpowiedz z erf
        float fear = playerModel.GetEmotionVector ().GetValue (OccEmotions.FEAR).AsFloat();
        //rzeliczenie na poziom trudnosci
        if(fear > 0.0f)
            Debug.Log ("FIRST I WAS AFRAID, I WAS PETRIFIED!");
        else
            Debug.Log ("I AM FEARLESS! I AM DEATH! I AM FIRE!");
        
    }

    // Update is called once per frame
    void Update () {
        
    }
}
