using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Facebook.Unity;
using System;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.SceneManagement;

public class SignIn : MonoBehaviour
{
    public enum SignInType
    {
        regular,
        facebook,
        google
    }
    FacebookComponent fbComponent;

    public SignInType signInType;
    public LeaderboardSDK leaderboardSDK;
    public InputField userNameField;
    public Text gradeLevelText;

    public GameObject login;
    public GameObject register;
    public GameObject autoLogin;
    public GameObject loadingGameObject;

    public MessageBoxScript messageBoxScript;

    delegate void SignInTypeFunction();
    delegate void FacebookFunction();
    delegate void GoogleFunction();

    [Header("Validation Extra Layer")]
    public Text userNameText;
    public int minCharacterAllowed = 3;


    void RunSignInFunction(SignInTypeFunction pRegularFunction, SignInTypeFunction pFacebookFunction, SignInTypeFunction pGoogleFunction)
    {
        switch (signInType)
        {
            case SignInType.regular:
                pRegularFunction();
                break;
            case SignInType.facebook:
                pFacebookFunction();
                break;
            case SignInType.google:
                pGoogleFunction();
                break;
        }
    }

    public void GoToMainScene()
    {
        SceneManager.LoadScene("LoadingScene");
        //SceneManager.LoadScene("AvaterEditorScene");
    }
    public void OnClick()
    {
        loadingGameObject.SetActive(true);
        userNameText.color = Color.black;

        if (userNameField.text == ""
            || userNameField.text.Length < minCharacterAllowed)
        {
            messageBoxScript.Open("WARNING", "INVALID DATA.");
            return;
        }

        SignInTypeFunction regularFunction = () =>
        {
            GlobalVar.username = userNameField.text;

            JSONObject newJsonObject = new JSONObject();
            JSONObject leaderboardDetailsObj = new JSONObject();

            string idCopy;
            if (reJSON.jSONObject["ID"] == null)
            {
                idCopy = "";
            }
            else
            {
                idCopy = reJSON.jSONObject["ID"];
            }

            newJsonObject.Add("id", idCopy);
            //print(gradeLevelText.text.ToUpper());
            newJsonObject.Add("gradelevel", gradeLevelText.text.ToUpper());
            newJsonObject.Add("category", "testing");
            newJsonObject.Add("UserID", GlobalVar.username.ToUpper());
            newJsonObject.Add("Score", GlobalVar.UserCurrentScore);
            newJsonObject.Add("Badges", 0);


            JSONObject currentLeaderboard = new JSONObject();
            JSONObject avatar = new JSONObject();

            string[] itemNames = { "BackHair" ,
            "Face",
            "Mouth",
            "Nose",
            "Eyes",
            "Brows",
            "MidHair",
            "FrontHair",
            "Clothes",
            "Frame",
            "BG" };

            for (int i = 0; i < itemNames.Length; i ++)
            {
                JSONObject jsonColor = new JSONObject();
                JSONObject jsonDetails = new JSONObject();
                jsonDetails.Add("itemNumber", 0);
                jsonColor.Add("r", 1);
                jsonColor.Add("g", 1);
                jsonColor.Add("b", 1);
                jsonColor.Add("a", 1);
                jsonDetails.Add("itemColor", jsonColor);
                jsonDetails.Add("itemName", "empty");
                avatar.Add(itemNames[i], jsonDetails);
            }
            //avatar.Add("BackHair",0);
            //avatar.Add("Face",0);
            //avatar.Add("Mouth",0);
            //avatar.Add("Nose",0);
            //avatar.Add("Eyes",0);
            //avatar.Add("Brows",0);
            //avatar.Add("MidHair",0);
            //avatar.Add("FrontHair",0);
            //avatar.Add("Clothes",0);
            //avatar.Add("Frame", 0);
            //avatar.Add("BG", 0);

            //currentLeaderboard.Add("leaderboardId", GlobalVar.leaderboardID);
            //currentLeaderboard.Add("ScoreForDay", GlobalVar.UserCurrentScore.ToString());
            //currentLeaderboard.Add("BadgesForDay", "0");

            //leaderboardDetailsObj.Add(GlobalVar.leaderboardID, currentLeaderboard);

            newJsonObject.Add("Avatar", avatar);
            newJsonObject.Add("LeaderboardDetails", leaderboardDetailsObj);

            //print(newJsonObject);
            //print(newJsonObject.ToString());

            //if (PlayerPrefs.GetString("ID") == "")
            LeaderboardSDK.CallbackListener onListener = (string fromListener) =>
            {
                loadingGameObject.SetActive(false);
                if (fromListener == "300")
                {
                    //print("user already exists");
                    userNameText.color = Color.red;
                    messageBoxScript.Open("WARNING", "USERNAME IS INVALID.");
                    
                } else if (fromListener == "301")
                {
                    //print("user already exists");
                    userNameText.color = Color.red;
                    messageBoxScript.Open("WARNING", "USER ALREADY EXISTS.");
                }
                else
                {
                    //print("create new user");
                    MessageBoxScript.OnClickOkay onClickOkay = () =>
                    {
                        GoToMainScene();
                    };
                    messageBoxScript.Open("WELCOME", "WELCOME " + userNameField.text.ToUpper() + ". HAVE FUN LEARNING.", onClickOkay);
                    reJSON.AddObject("username", userNameField.text.ToUpper());
                    reJSON.AddObject("gradelevel", gradeLevelText.text.ToUpper());
                    reJSON.AddObject("Avatar", avatar);
                    reJSON.SaveJSON("ud.kuf");
                    
                }
            };
            leaderboardSDK.CreateScore(newJsonObject.ToString(), onListener);

            //reJSON.AddObject("username", userNameField.text);
            //reJSON.SaveJSON("ud.kuf");
            //GoToMainScene();
        };

        SignInTypeFunction facebookFunction = () =>
        {
            if (fbComponent.Initialized == true)
            {
                //FB.LogInWithReadPermissions();
            }    
        };

        SignInTypeFunction googleFunction = () =>
        {

        };

        RunSignInFunction(regularFunction, facebookFunction, googleFunction);

        
    }
    // Start is called before the first frame update
    void Start()
    {
        //print(PlayerPrefs.GetString("ID"));

        reJSON.Init("ud.kuf", true);
        //leaderboardSDK = GetComponent<LeaderboardSDK>();
        //print(reJSON.jSONObject["username"]);
        if (reJSON.jSONObject["username"] == null)
        {
            login.SetActive(true);
        } else
        {
            GlobalVar.username = reJSON.jSONObject["username"];
            //print(reJSON.jSONObject["Avatar"]);
            autoLogin.SetActive(true);
        }

        if (reJSON.jSONObject["score_history"] == null)
        {
            //reJSON.jSONObject["score_history"];
            JSONObject defaultJSON = new JSONObject();
            reJSON.AddObject("score_history", defaultJSON);
            reJSON.SaveJSON("ud.kuf");
        } else
        {
            //print(reJSON.jSONObject["score_history"]);
            //reJSON.jSONObject["score_history"]["01/01/2020"] = "9999";
            //reJSON.jSONObject["score_history"]["01/02/2020"] = "9999";

            //print(reJSON.jSONObject["score_history"].Count);
            //print(reJSON.jSONObject["score_history"][0]);
            
            //print(reJSON.jSONObject["score_history"]);
        }
        //print(DateTime.Now.ToString());
        //print(DateTime.Now.ToString("MM/dd/yyyy"));
        SignInTypeFunction regularFunction = () =>
        {

        };

        SignInTypeFunction facebookFunction = () =>
        {
            fbComponent = GameObject.FindGameObjectWithTag("FacebookComponent").GetComponent<FacebookComponent>();
        };

        SignInTypeFunction googleFunction = () =>
        {

        };

        RunSignInFunction(regularFunction, facebookFunction, googleFunction);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
