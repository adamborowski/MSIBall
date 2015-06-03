using System;
using UnityEngine;
using UnityEngine.UI;
using System.Timers;
using System.Collections.Generic;

namespace AssemblyCSharp
{
    /*
     * PointBuffer pozwala nam odpowiedzieć na pytanie: "Ile kolizji w ostatnich 10 sekundach"
     */
    public class PointBuffer
    {

        private class Point
        {
            public int score;
            public DateTime time;

            public Point(int score, DateTime time)
            {
                this.score = score;
                this.time = time;
            }
        }
        public PointBuffer(int stepTime, int windowTime)
        {
            queue = new Queue<Point>();
            timer = new Timer(stepTime);
            timer.Start();
            timer.Elapsed += OnTimedEvent;
            this.windowTime = windowTime;
            
        }
        
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            var currentTime = DateTime.Now;
            var firstTime = currentTime.AddMilliseconds(-windowTime);
            while (queue.Count>0 && queue.Peek().time<firstTime)
            {
                queue.Dequeue();
            }
            var points = 0;
            foreach (var point in queue)
            {
                points += point.score;
            }
            movingScore = points;
        }
        
        public void AddPoint(int score)
        {
            queue.Enqueue(new Point(score, DateTime.Now));
        }
        
        public void Stop()
        {
            timer.Stop();
            
        }
        
        //privates
        private Timer timer;
        private Queue<Point> queue;
        public int windowTime;
        public int movingScore = 0;


    }

    public class ScoreController: MonoBehaviour
    {
        public PlayerController player;
        public Text scoreTextField;
        public PointBuffer buffer;
        public static int totalScore = 0;
        int nextPoints = 1;//przyrostowa punktacja za kolejne udane ominięcie przeszkody

        private float timer;

        private int getDifficultyFactor()
        {
            switch (PlayerController.gameSettings.gameDifficulty)
            {
                case GameDifficulty.EASY:
                    return 1;
                case GameDifficulty.MEDIUM:
                    return 2;
                case GameDifficulty.HARD:
                    return 3;
            }
            return 0;
        }

        public void Start()
        {
            buffer = new PointBuffer(1000, 10000);
            timer = 60.0f;
            updateInfo();

        }

        public void OnDestroy()
        {
            buffer.Stop();
        }

        private int numCollisions = 0;

        public void collisionDetected(Collision collision)
        {
            nextPoints = 1;//resetujemy kumulowany przyrost punktów
            numCollisions++;
            totalScore -= (int)Math.Ceiling(totalScore*0.1f / getDifficultyFactor());//algorytm obliczenia kary
            if (totalScore < 0)
                totalScore = 0;
            buffer.AddPoint(1);//dodaj kolizję do sumatora kroczącego
        }

        public void collisionAvoided()
        {
            Debug.LogWarning("avoided: "+getDifficultyFactor()+ " / "+ nextPoints);
            totalScore += getDifficultyFactor() * nextPoints;//algorytm przyznania nagrody wprost proporcjonalny do poziomu trudności
            nextPoints++;//za kolejne ominięcie przeszkody przyzna więcej punktów
        }

        private void updateInfo()
        {
            //wyświetl informacje o stanie rozgrywki
            timer -= Time.deltaTime;
            if (timer <=0.0f)
                Application.LoadLevel("EndScene");

            if (PlayerController.gameSettings != null)
            {
                scoreTextField.text = ""
                    + "Collisions: " + numCollisions
                    + "\nLast " + (buffer.windowTime / 1000).ToString("0") + "s: " + buffer.movingScore
                    + "\nBoost: " + (player.zSpeed - 1).ToString("0.00")
                    + "\nDifficulty: " + (PlayerController.gameSettings.gameDifficulty.ToString())
                    + "\nScore: " + totalScore.ToString("0")
                    + "\nTime: " + timer.ToString("0")+"s"
                    + "";

            }
        }

        public void Update()
        {
            updateInfo();
        }
    }
}

