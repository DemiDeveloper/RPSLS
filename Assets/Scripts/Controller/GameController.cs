using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPSLS
{
    public class GameController : MonoBehaviour
    {
        public static Action<int> StartGame;
        public static Action<HandType> HandClicked;
        public HandRelationScriptableClass handClassDictionary;

        public float[] timerSpeeds;
        public TimeAndScore.GameTimers selectedTime;
        public HandType AISelectedHand;
        public HandObjectsController objectsController;
        public MenuController menuController;
        public GameResult lastResult;
        public TimeAndScore.GameScore gameScore;
        private void Awake()
        {
            objectsController.PopulatePlayerHands(handClassDictionary.handClassDictionary);
            string savedScore = PlayerPrefs.GetString("GameScore");
            if(string.IsNullOrEmpty(savedScore))
            {
                gameScore = new TimeAndScore.GameScore(0, 0);
            }
            else
            {
                gameScore = JsonUtility.FromJson<TimeAndScore.GameScore>(savedScore);
            }
            menuController.UpdateScore(gameScore);
        }
        private void OnEnable()
        {
            StartGame += OnStartGame;
            HandClicked += OnHandClicked;
        }

        private void OnHandClicked(HandType type)
        {
            selectedTime.StopGame();
            string winString = handClassDictionary.handClassDictionary.GetWinString(type, AISelectedHand, out lastResult);
            menuController.ShowResult(winString, lastResult);
            objectsController.HandButtonClick(type, handClassDictionary.handClassDictionary.FetchHandClassData(AISelectedHand));
            if(lastResult is GameResult.Win)
            {
                gameScore.AddScore();
                SaveScore();
                menuController.UpdateScore(gameScore);
            }
        }
        public void OnResetClick()
        {
            switch (lastResult)
            {
                case GameResult.Win:
                case GameResult.Draw:
                    ResetGame();
                    break;
                case GameResult.Lose:
                case GameResult.TimeOut:
                    MenuController.SwitchMenu?.Invoke(false);
                    break;
            }
        }
        void Update()
        {
            if (selectedTime.IsGameRunning())
            {
                if (selectedTime.RunTimer(Time.deltaTime))
                {
                    RanOutOfTime();
                }
            }
        }
        public void ResetGame()
        {
            AISelectedHand = handClassDictionary.handClassDictionary.GetRandomHand();
            objectsController.ResetButtons();
            menuController.ResetResult();
            selectedTime.Reset();
            selectedTime.StartGame();
        }
        private void OnStartGame(int index)
        {
            selectedTime = new TimeAndScore.GameTimers(timerSpeeds[index]);
            ResetGame();
            gameScore.UpdateScore(0);
            SaveScore();
            menuController.UpdateScore(gameScore);

        }
        public void RanOutOfTime()
        {
            selectedTime.StopGame();
            objectsController.DisableHandButtons();
            menuController.ShowResult("", GameResult.TimeOut);
        }
        public void OpenMenu()
        {
            //MenuController.SwitchMenu?.Invoke(false);
        }
        public void CloseMenu() 
        {
            //MenuController.SwitchMenu?.Invoke(true);
        }
        public void SaveScore()
        {
            string saveScore = JsonUtility.ToJson(gameScore);
            PlayerPrefs.SetString("GameScore", saveScore);
        }
        private void OnDisable()
        {
            StartGame -= OnStartGame;
            HandClicked -= OnHandClicked;

        }
    }
    public enum HandType
    {
        Rock,
        Paper,
        Scissor,
        Lizard,
        Spock,
    }
    public enum GameResult
    {
        Win,
        Lose,
        Draw,
        TimeOut,
    }
    public enum GameState
    {
        Menu,
        Started,
        Idle,
    }
}