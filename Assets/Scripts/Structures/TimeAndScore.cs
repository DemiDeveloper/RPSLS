using System;
using UnityEngine;

namespace RPSLS
{
    public class TimeAndScore
    {
        [Serializable]
        public class GameTimers
        {
            [SerializeField] float MaxTimer;
            [SerializeField] float CurrentTimer;
            bool IsRunning;
            public bool RunTimer(float cTime)
            {
                CurrentTimer -= cTime;
                return CurrentTimer < 0;
            }
            public GameTimers(float MTime)
            {
                MaxTimer = MTime;
                CurrentTimer = MTime;
            }

            public void Reset()
            {
                CurrentTimer = MaxTimer;
            }
            public void StartGame()
            {
                IsRunning = true;
            }
            public void StopGame()
            {
                IsRunning = false;
            }
            public bool IsGameRunning()
            {
                return IsRunning;
            }
            public float GetTimerPercent()
            {
                return CurrentTimer / MaxTimer;
            }
        }
        [Serializable]
        public class GameScore
        {
            public int CurrentScore;
            public int TopScore;
            public GameScore(int cScore, int mScore)
            {
                CurrentScore = cScore;
                TopScore = mScore;
            }
            public void UpdateTopSore(int mScore)
            {
                TopScore = mScore;
            }
            public void UpdateScore(int cScore)
            {
                CurrentScore = cScore;
                if (CurrentScore > TopScore)
                {
                    TopScore = CurrentScore;
                }
            }

            public void AddScore(int cScore = 1)
            {
                CurrentScore += cScore;
                if (CurrentScore > TopScore)
                {
                    TopScore = CurrentScore;
                }
            }
        }
    }
}
