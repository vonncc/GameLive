using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Text;

public class Vonn_GameManager : MonoBehaviour
{

    public static readonly string Accept = "Accept";
    public static readonly string Content_Type = "Content-Type";
    public static readonly string ApplicationJson = "application/json";
    public static readonly string ErrorOccurred = "Error occurred";

    //public static bool CanAnswer;

    public static bool runTimer;
    public static bool internalTimerBool;
    public int ScoreToSend;

    public float maxTimer = 10;
    [HideInInspector]
    public float cdTimer = 10;
    float internalTImer;
    public ChoiceManager[] choices;

    LeaderboardSDK mLeaderboardSDK;
    MessageHandler messageHandler;
    GameObjectScript gameOverScript;
    public Text timerText;
    public Text correctAnswerDebug;
    public Text userCurrentScore;

    public GameObject ConfirmGO;

    
    public GameResultScript gameResultScript;
    

    bool isGameOver;
    public GameObject waitingForServerGO;
    public GameObject GameOverObject;
    float counterForGoingBack = 0;
    int gotScore;
    string gotAnswer;
    string correctAnswer;
    bool CorrectAnswerPicked;

    bool showWaiting;
    bool showResult;
    [Serializable]
    public class User
    {
        public string _id;
        public int scoreValue;
        public int badgesValue;
    }

    public static void UpdateScoreHistory()
    {
        // Get Date And Time MM/DD/YYYY
        //string MM = DateTime.Now.Month.ToString();
        //string DD = DateTime.Now.Day.ToString();
        //string YYYY = DateTime.Now.Year.ToString();

        //string MMDDYYYY = MM + "/" + DD + "/" + YYYY;

        string MMDDYYYY = DateTime.Now.ToString("MM/dd/yyyy");
        reJSON.jSONObject["score_history"][MMDDYYYY] = GlobalVar.UserCurrentScore;
        reJSON.SaveJSON("ud.kuf");
    }

    void DebugCorrectAnswer(string pCorrectAnswer)
    {
        correctAnswerDebug.text = "Correct Answer is: " + pCorrectAnswer;
    }

    void TriggerWhenChange()
    {

    }

    //MessageHandler.DoOutside myFunc = () => { };

    void SetTheAnswer()
    {
        SetRightAnswer(GlobalVar.correctAnswer - 1);

        if (GlobalVar.correctAnswer <= 0)
        {
            GlobalVar.correctAnswer = 0;
        }
        switch (GlobalVar.correctAnswer - 1)
        {
            case 0:
                correctAnswer = "A";
                DebugCorrectAnswer("A");
                break;
            case 1:
                correctAnswer = "B";
                DebugCorrectAnswer("B");
                break;
            case 2:
                correctAnswer = "C";
                DebugCorrectAnswer("C");
                break;
            default:
                correctAnswer = "D";
                DebugCorrectAnswer("D");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        showWaiting = false;
        showResult = false;
        if (GlobalVar.durationForScenario <= 0)
        {
            cdTimer = maxTimer;
        }
        else
        {
            cdTimer = GlobalVar.durationForScenario;
        }

        timerText.text = GlobalVar.durationForScenario.ToString();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        userCurrentScore.text = GlobalVar.UserCurrentScore.ToString();

        UpdateScoreHistory();


        internalTImer = cdTimer;
        isGameOver = false;
        counterForGoingBack = 0;
        gameOverScript = GameOverObject.GetComponent<GameObjectScript>();
        GlobalVar.showResultScreen = false;
#if UNITY_EDITOR
        GlobalVar.leaderboardID = "testing";
        //GlobalVar.username = "Vu";
        GlobalVar.receivedAnswer = true;
        GlobalVar.canAnswer = true;
#else
        messageHandler = GameObject.FindGameObjectWithTag("FirebaseControl").GetComponent<MessageHandler>();
#endif
        //messageHandler.doOnMessage(myFunc);
        mLeaderboardSDK = GetComponent<LeaderboardSDK>();
        CorrectAnswerPicked = false;
        //CanAnswer = true;
        runTimer = false;
        
        //int correctAnswerVar = Random.Range(0, choices.Length);
        //print(GlobalVar.correctAnswer);

#if UNITY_EDITOR
        runTimer = true;
        GlobalVar.correctAnswer = 1;
        SetRightAnswer(GlobalVar.correctAnswer - 1);

        if (GlobalVar.correctAnswer <= 0)
        {
            GlobalVar.correctAnswer = 0;
        }
        switch (GlobalVar.correctAnswer - 1)
        {
            case 0:
                DebugCorrectAnswer("A");
                break;
            case 1:
                DebugCorrectAnswer("B");
                break;
            case 2:
                DebugCorrectAnswer("C");
                break;
            default:
                DebugCorrectAnswer("D");
                break;
        }



#endif

    }

    int GetAnswerScore(float timerCount)
    {
        //if (timerCount >= 12)
        //{
        //    return 1000;
        //}
        //else if (timerCount >= 7 && timerCount < 12)
        //{
        //    return 800;
        //}
        //else if (timerCount >= 3 && timerCount < 7)
        //{
        //    return 500;
        //}
        //else if (timerCount >= 1 && timerCount < 3)
        //{
        //    return 100;
        //}

        float maxScore = 1000;
        float maxDuration = 10;
        float scoreToReturn;

        float currentSfore = timerCount / maxDuration;
        scoreToReturn = maxScore * currentSfore;

        return Mathf.RoundToInt(scoreToReturn);
    }

    public void SetRightAnswer(int pRightAnswer)
    {
        //print("RIGHT ANSQWER");
        //print(pRightAnswer);
        for (int i = 0; i < choices.Length; i++)
        {
            choices[i].IsCorrectAnswer = false;
        }
        choices[pRightAnswer].IsCorrectAnswer = true;
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                //Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    public void UserPickedAnswer(bool didWin, string pGotAnswer)
    {
        GlobalVar.canAnswer = false;
        gotAnswer = pGotAnswer;
    }
    

    public void AddScore(float currentTimer, string pGotAnswer)
    {
        CorrectAnswerPicked = true;
        gotAnswer = pGotAnswer;
        int userGotScore = GetAnswerScore(currentTimer);
        gotScore = userGotScore;
        GlobalVar.UserCurrentScore += userGotScore;
        UpdateScoreHistory();
        UserPickedAnswer(true, pGotAnswer);
        //Dictionary<string, ScoreModel.LeaderboardIDToSend> leaderboardData = new Dictionary<string, ScoreModel.LeaderboardIDToSend>();
        //ScoreModel.LeaderboardIDToSend dataToSend = new ScoreModel.LeaderboardIDToSend();
        //dataToSend.leaderboardId = GlobalVar.leaderboardID;
        //dataToSend.ScoreForDay = GlobalVar.UserCurrentScore.ToString();
        //dataToSend.BadgesForDay = "0";
        //print("gago");
        //print(dataToSend.leaderboardId);
        //leaderboardData.Add(GlobalVar.leaderboardID, dataToSend);
        //print("asda");
        //ScoreModel mScore = new ScoreModel
        //{
        //    // _id = "1ab24ec18f7",
        //    id = PlayerPrefs.GetString("ID"),
        //    category = "testing",
        //    UserID = "GlobalVar.username",
        //    Score = GlobalVar.UserCurrentScore,
        //    Badges = 0,
        //    LeaderboardDetails = leaderboardData
        //};
        //print(leaderboardData);
        //print(mScore.LeaderboardDetails.Values);
        //string json = JsonUtility.ToJson(leaderboardData);
        //print(json);IsCorrectAnswer

        JSONObject newJsonObject = new JSONObject();
        JSONObject leaderboardDetailsObj = new JSONObject();

        string idCopy;

        if (reJSON.jSONObject["ID"] == null)
        {
            idCopy = "";
        } else
        {
            idCopy = reJSON.jSONObject["ID"];
        }
        //newJsonObject.Add("id", idCopy);
        //newJsonObject.Add("category", "testing");
        //newJsonObject.Add("UserID", GlobalVar.username);
        //newJsonObject.Add("Score", GlobalVar.UserCurrentScore);
        //newJsonObject.Add("Badges", 0);

        
        JSONObject currentLeaderboard = new JSONObject();
        currentLeaderboard.Add("leaderboardId", GlobalVar.leaderboardID);
        currentLeaderboard.Add("ScoreForDay", GlobalVar.UserCurrentScore.ToString());
        currentLeaderboard.Add("BadgesForDay", "0");

        leaderboardDetailsObj.Add(GlobalVar.leaderboardID, currentLeaderboard);

        newJsonObject.Add("LeaderboardDetails", leaderboardDetailsObj);

        //print(newJsonObject.ToString());

        //if (PlayerPrefs.GetString("ID") == "")

        if (reJSON.jSONObject["ID"] == null)
            mLeaderboardSDK.CreateScore(newJsonObject.ToString());
        else
            mLeaderboardSDK.UpdateScore(currentLeaderboard.ToString());
        //print(mScore.LeaderboardDetails.);



        //print(JsonUtility.ToJson(mScore));


        //print("try adding score");
        //mLeaderboardSDK.UpdateScore(mScore);

        //mLeaderboardSDK.CreateScore(mScore);

        //if (PlayerPrefs.GetString("ID") == "")
        //mLeaderboardSDK.CreateScore(mScore);
        //else
        //    mLeaderboardSDK.UpdateScore(mScore);

        //StartCoroutine(GetRequest("http://192.168.254.108/TestGameShow/test.php?username=" + GlobalVar.username + "&score=" + userGotScore));
        //StartCoroutine(PostRequest("https://dev-atei.azurewebsites.net/api/leaderboard"));
        //ScoreToSend = GetAnswerScore();
    }

    public string GetScoreToSend()
    {
        int currenyScoreToSend = ScoreToSend;
        ScoreToSend = 0;
        return currenyScoreToSend.ToString();
    }

    public void ShowResultScreen()
    {
        GameOverObject.SetActive(true);

        GameObjectScript.Condition conditionSetter;
        if (CorrectAnswerPicked)
            conditionSetter = GameObjectScript.Condition.win;
        else
            conditionSetter = GameObjectScript.Condition.lose;
        isGameOver = true;
        correctAnswerDebug.text = "Going back to waiting scene";

        gameOverScript.SetCondition(conditionSetter, gotScore, gotAnswer, correctAnswer);
    }
    void GoBackToMainScene()
    {
        counterForGoingBack = 0;
        GlobalVar.canAnswer = false;
        GlobalVar.receivedAnswer = false;
        GlobalVar.correctAnswer = -1;
        GlobalVar.roundEnd = false;
        GlobalVar.showResultScreen = false;
        SceneManager.LoadScene("MainMenu");
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver == false)
        {
            if (GlobalVar.receivedAnswer == true && GlobalVar.correctAnswer >= 0)
            {
                waitingForServerGO.SetActive(false);
                SetTheAnswer();
                runTimer = true;
                internalTimerBool = true;
            }
            else
            {
                runTimer = false;
                //internalTimerBool = false;
                //waitingForServerGO.SetActive(true);
                correctAnswerDebug.text = "Please Stand By";
            }

        } else
        {
            //counterForGoingBack += Time.deltaTime;
            //if (counterForGoingBack >= 2f)
            //{
            //    GoBackToMainScene();
            //}

            if (GlobalVar.roundEnd == true)
            {
                GoBackToMainScene();
                GlobalVar.roundEnd = false;
            } else
            {
                ConfirmGO.SetActive(false);
                correctAnswerDebug.text = "Waiting for server";
            }
        }
        

        if (runTimer == true)
        {
            cdTimer -= Time.deltaTime;
            //GlobalVar.canAnswer = true;
            //CanAnswer = false;
            if (cdTimer <= 0)
            {
                internalTImer = 0;
                cdTimer = 0;
                timerText.text = "0";
                counterForGoingBack = 0;
                GlobalVar.canAnswer = false;
                runTimer = false;
                //ShowResultScreen();
                //gameResultScript.UpdateCurrentScore(GlobalVar.UserCurrentScore.ToString());
                ShowResultScreen();
                if (showWaiting == false)
                {
                    showWaiting = true;
                    gameOverScript.ShowScenario(GameObjectScript.Scenario.wait);
                }

            }
            else
            {

                //timerText.text = (Mathf.Ceil(cdTimer * 100) / 100).ToString();
                timerText.text = ((int)cdTimer + 1).ToString();
            }
        } else
        {
            if (GlobalVar.showResultScreen == true)
            {
                if (showResult == false) {
                    showResult = true;
                    gameResultScript.UpdateCurrentScore(GlobalVar.UserCurrentScore.ToString());
                    gameOverScript.ShowScenario(GameObjectScript.Scenario.roundResult);
                }
                
            }
        }

        
    }
}
