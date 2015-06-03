using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/**
 * Trasa jest generowana automatycznie,
 * gracz w trybie emocjonalnym ma zmienną kulkę (sterowalność i rozmiar) w zależności od /collisions in last 10 seconds/
 * a w trybie bez emocji może wybrać sterowalność i rozmiar kulki
 * 
 */
using Erf;
using System;

public class PlayerController : MonoBehaviour
{
    public ScoreController scoreController;
    public float speed;
    public static float initialZSpeed = 1;
    public float zSpeed = initialZSpeed;
    public float zAcc = 1.0001f;
    public float acceleration;
    private Rigidbody rb;
    public static float distanceTraveled;
    public static GameSettings gameSettings;
    private CharacterModel playerModel;//nasz link do ERF
    private DateTime lastReportTime;//miernik zmiany poziomu trudności dla trybu z emocjami
    private int interval = 1700;//co ile sprawdzać zmianę poziomu trudności dla trybu z emocjami
    private float minFear = 5, maxFear = 20;//wstępne wskaźniki mapowania strachu na poziom trudności



    AudioSource audio;//odnośnik do dźwięku kolizji
    

    void Start()
    {


        audio = GetComponent<AudioSource>();


        //init ERF ACTOR
        ExternalExpert msiBallEmotionModel = ErfContext.GetInstance().FindExpert("MSIBallEmotionModel");
        playerModel = new CharacterModel();
        playerModel.RegisterExpert(msiBallEmotionModel);
        lastReportTime = DateTime.Now;


        rb = GetComponent<Rigidbody>();

        if (MainMenu.gameSettings == null)
        {
            //jeśli w debug initial screen był Game a nie Menu
            MainMenu.gameSettings = new GameSettings();
            MainMenu.gameSettings.gameMode = GameMode.EMOTIONAL;

        }
        gameSettings = MainMenu.gameSettings;

        if (gameSettings.gameMode == GameMode.FIXED)//ustaw poziom trudności
        {
            setDifficultyEnum(gameSettings.gameDifficulty, false);
        } else
        {
            setDifficultyEnum(GameDifficulty.HARD, false);//ustaw na początek na poziom trudny w trybie rozpoznawania emocji
        }
    }

    void setDifficultyEnum(GameDifficulty difficulty, bool play=true)
    {
        if (gameSettings.gameDifficulty != difficulty && play)
        {
            scoreController.GetComponent<AudioSource>().Play();
        }
        gameSettings.gameDifficulty = difficulty;
        var m = GetComponent<Renderer>().material;
        var light = 0.8f;
        var dark = 0.2f;
        var alpha = 0.86f;
        switch (difficulty)
        {
            case GameDifficulty.EASY:
                setDifficulty(0.5f);
                m.color = new Color(dark, light, dark, alpha);
                break;
            case GameDifficulty.MEDIUM:
                setDifficulty(0.7f);
                m.color = new Color(dark, dark, light, alpha);
                break;
            case GameDifficulty.HARD:
                setDifficulty(1.1f);
                m.color = new Color(light, dark, dark, alpha);
                break;
        }
    }

    void updateSpeed()
    {
        zSpeed *= zAcc;//lekko przyśpieszamy
                
    }

    private float difficulty;
    private float targetScale;

    void setDifficulty(float dif)
    {
        difficulty = dif;
        targetScale = dif;//robimy transition do targetScale
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Obstacle(Clone)")//tylko kolizje z przeszkodami a nie np ścianą
        {
            zSpeed = initialZSpeed;
            scoreController.collisionDetected(other);
            rb.AddExplosionForce(140 * zSpeed * difficulty, other.contacts [0].point, 0);//odbij się od przeszkody
            audio.Play();
        }
    }
    
    void FixedUpdate()
    {
        float fac;
        //transition scale
        float curScale = transform.localScale.x;
        float newScale = 0.05f * (targetScale - curScale) + curScale;
        transform.localScale = new Vector3(newScale, newScale, newScale);
        //
        if (gameSettings.gameMode == GameMode.EMOTIONAL)
        {
            //erf
            DateTime now = DateTime.Now;
           
            if ((now-lastReportTime).TotalMilliseconds >= interval)//czy należy rozpatrzeć zmianę emocji?
            {
                lastReportTime = now;
                //tutaj wysyłamy emocję
                EecEvent timeElapsedEvent = new EecEvent((int)GameEvent.STD_TIME_ELAPSED);
                timeElapsedEvent.AddValue("NO_OF_COLLISIONS", Variant.Create(scoreController.buffer.movingScore));
                timeElapsedEvent.AddValue("CURRENT_SPEED", Variant.Create(zSpeed));
                playerModel.HandleEvent(timeElapsedEvent);
                //odpowiedz z erf
                float fear = playerModel.GetEmotionVector().GetValue(OccEmotions.FEAR).AsFloat();
                //adaptacyjny zakres
                if (fear < minFear)
                {
                    minFear = fear;
                } else if (fear > maxFear)
                {
                    maxFear = fear;
                }

                float fearLength = maxFear - minFear;
                float fearRatio = (fear - minFear) / fearLength;
                if (fearRatio < 0.1f)
                {
                    setDifficultyEnum(GameDifficulty.HARD);
                } else if (fearRatio < 0.4)
                {
                    setDifficultyEnum(GameDifficulty.MEDIUM);
                } else
                {
                    setDifficultyEnum(GameDifficulty.EASY);
                }
                Debug.LogFormat("ratio: {0} [{1}  -  {2}  -  {3}", fearRatio, minFear, fear, maxFear);
            }
        } else
        {
            //nie zmieniamy poziomu trudności
        }

        if (Input.GetKeyDown("escape"))//wychodzimy do menu?
        {
            Application.LoadLevel("MainMenuScene");
        }


        //obsługa poruszania kulą przez użytkownika
        float moveHorizontal = Input.GetAxis("Horizontal");

        float moveVertical = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(moveHorizontal / difficulty, 0.0f, zSpeed);
        rb.AddForce(movement * speed);
        distanceTraveled = transform.localPosition.z;
        updateSpeed();
    }
}