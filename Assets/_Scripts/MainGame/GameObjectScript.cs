using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectScript : MonoBehaviour
{
    public enum Condition
    {
        win,
        lose
    }

    public enum Scenario
    {
        wait,
        roundResult,
        gameResult
    }

    public Text resultText;
    public Text userGotScore;

    public Text winGotText;
    public Text winTotalScore;
    public Text winGotAnswer;
    public Text winCorrectAnswer;
    public Text loseGotText;
    public Text loseTotalScore;
    public Text loseGotAnswer;
    public Text loseCorrectAnswer;
    Text gotScore;
    Text totalScore;
    Text wasAnswer;
    Text correctAnswer;

    public GameObject waitingForServerAnswer;

    public GameObject WinGameObject;
    public GameObject LoseGameObject;

    public GameObject WinComicsGameObject;
    public GameObject LoseComicsGameObject;

    public GameObject waitingForServer;
    public GameObject roundResult;
    public GameObject gameResult;

    ComicAnimationScript comicAnimationScript;
    Condition mCondition;
    // Start is called before the first frame update
    public void SetCondition(Condition pCondition, int pScore, string pAnswer, string pCorrectAnswer)
    {
        switch(pCondition)
        {

            case Condition.win:
                comicAnimationScript = WinComicsGameObject.GetComponent<ComicAnimationScript>();
                resultText.text = "WIN";
                gotScore = winGotText;
                totalScore = winTotalScore;
                wasAnswer = winGotAnswer;
                correctAnswer = winCorrectAnswer;
                //WinGameObject.SetActive(true);
                WinComicsGameObject.SetActive(true);
                break;
            case Condition.lose:
                comicAnimationScript = LoseComicsGameObject.GetComponent<ComicAnimationScript>();
                resultText.text = "LOSE";
                gotScore = loseGotText;
                totalScore = loseTotalScore;
                wasAnswer = loseGotAnswer;
                correctAnswer = loseCorrectAnswer;
                //LoseGameObject.SetActive(true);
                LoseComicsGameObject.SetActive(true);
                break;
        }

        comicAnimationScript.ChangeExtraImage(pAnswer);

        correctAnswer.text = "THE CORRECT ANSWER IS " + pCorrectAnswer;
        wasAnswer.text = "YOUR ANSWER WAS: " + pAnswer;
        gotScore.text = pScore.ToString();
        totalScore.text = GlobalVar.UserCurrentScore.ToString();
        //userGotScore.text = "YOU GOT: " + pScore.ToString();
    }

    public void ShowScenario(Scenario pScenario)
    {
        //waitingForServerAnswer.SetActive(false);
        waitingForServer.SetActive(false);
        roundResult.SetActive(false);
        gameResult.SetActive(false);
        switch (pScenario)
        {
            case Scenario.wait:
                waitingForServer.SetActive(true);
                break;
            case Scenario.roundResult:
                roundResult.SetActive(true);
                break;
        }
    }
}
