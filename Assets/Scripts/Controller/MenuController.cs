using RPSLS;
using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameController gameController;
    public static Action<bool> SwitchMenu;
    public CanvasGroup menuGroup;
    public CanvasGroup gameGroup;
    public float switchSpeed;
    public GameObject ResetButtonObject;
    Coroutine switchCoroutine;

    public ScoreUIHolder menuScoreHolder;
    public ScoreUIHolder inGameScoreHolder;

    public TextMeshProUGUI resultText;
    public TextMeshProUGUI winLoseText;
    public GameObject FillObject;
    public GameObject MessageObject;

    public Image FillImage;

    public ResultColorDictionary colorDictionary;
    void Update()
    {
        if (gameController.selectedTime != null && gameController.selectedTime.IsGameRunning())
        {
            FillImage.fillAmount=gameController.selectedTime.GetTimerPercent();
        }
    }
    public void StartGame(int index)
    {
        GameController.StartGame?.Invoke(index);
        OnSwitchMenu(true);
    }
    private void OnEnable()
    {
        SwitchMenu += OnSwitchMenu;
    }

    private void OnSwitchMenu(bool ToGame)
    {
        if (switchCoroutine != null)
        {
            StopCoroutine(switchCoroutine);
        }
        switchCoroutine = StartCoroutine(SwitchWindow(ToGame ? menuGroup : gameGroup, ToGame ? gameGroup : menuGroup));
    }
    public void ShowResult(string message, GameResult result)
    {
        switch (result)
        {
            case GameResult.Win:
            case GameResult.Lose:
                resultText.text = string.Format(message, "#" + colorDictionary[result].ToHexString());
                winLoseText.text = "you " + result.ToString();
                break;
            case GameResult.Draw:
            case GameResult.TimeOut:
                winLoseText.text = result.ToString();
                break;
        }
        FillObject.SetActive(false);
        MessageObject.SetActive(true);
        ResetButtonObject.SetActive(true);
    }
    public void ResetResult()
    {
        resultText.text = "";
        winLoseText.text = "";
        ResetButtonObject.SetActive(false);

        FillObject.SetActive(true);
        MessageObject.SetActive(false);
    }
    IEnumerator SwitchWindow(CanvasGroup from, CanvasGroup to)
    {
        from.interactable = false;
        from.blocksRaycasts = false;
        while (to.alpha < 1)
        {
            to.alpha += switchSpeed * Time.unscaledDeltaTime;
            from.alpha -= switchSpeed * Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        to.alpha = 1;
        to.interactable = true;
        to.blocksRaycasts = true;
        from.alpha = 0;
    }
    public void UpdateScore(TimeAndScore.GameScore score)
    {
        menuScoreHolder.SetValues(score.CurrentScore, score.TopScore);
        inGameScoreHolder.SetValues(score.CurrentScore, score.TopScore);
    }
    private void OnDisable()
    {
        SwitchMenu -= OnSwitchMenu;
    }
}
[Serializable]
public class ScoreUIHolder
{
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI currentScoreText;

    public void SetValues(int cScore, int mScore)
    {
        highScoreText.text = mScore.ToString();
        currentScoreText.text = cScore.ToString();
    }
    public void SetHighScore(int score)
    {
        highScoreText.text = score.ToString();
    }
    public void SetCurreScore(int score)
    {
        currentScoreText.text = score.ToString();
    }
}