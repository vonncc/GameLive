using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class LeaderboardSDK : MonoBehaviour
{
    public string Url = "https://dev-atei.azurewebsites.net/api/leaderboardmongo";
    //public string Url = "https://localhost:5001/api/leaderboardmongo";
    //1ab24ec18f7
    public static LeaderboardSDK Instance;
    [HideInInspector]
    public string userID;
    [HideInInspector]
    public string username;

    public delegate void CallbackListener(string code);

    CallbackListener callbackFunction;
    void Awake()
    {
        Instance = this;
        GlobalVar.DebugFlag = false;
        WebUtility.ValidateForNull(Url);
    }
    // Equivalent of POST https://functionURL/api/scores
    //public void CreateScore(Score instance, Action<CallbackResponse<User>> oncreateScoreCompleted)
    //public void CreateScore(ScoreModel instance)
    public void CreateScore(string instance, CallbackListener newDelegate = null)
    {
        //Debug.Log("START COROUTINE");
        //print(JsonUtility.ToJson(instance));
        //instance._id = Instance.userID;
        //instance.username = Instance.username;
        //WebUtility.ValidateForNull(instance, oncreateScoreCompleted);
        //WebUtility.ValidateForNull(instance);
        //StartCoroutine(PostScoreInternal(instance, oncreateScoreCompleted));
        callbackFunction = newDelegate;
        StartCoroutine(PostScoreInternal(instance));
    }

    //public void UpdateScore(ScoreModel instance)
    public void UpdateScore(string instance)
    {
        StartCoroutine(PutScoreInternal(instance, "score"));
    }

    //public void UpdateScore(ScoreModel instance)
    public void UpdateAvatar(string instance, CallbackListener newDelegate)
    {
        callbackFunction = newDelegate;
        StartCoroutine(PutScoreInternal(instance, "avatar"));
    }
    // Equivalent of GET https://functionURL/api/scores/top/:count
    public void ListTopScores(int count, int skipCount, Action<CallbackResponse<ScoreModel[]>> callback)
    {
        WebUtility.ValidateForNull(callback);
        StartCoroutine(GetStuffArray<ScoreModel>("/scores/top/" + count, skipCount, callback));
    }

    //private IEnumerator PostScoreInternal(Score instance, Action<CallbackResponse<User>> onIns/ertCompleted)
    //private IEnumerator PostScoreInternal(ScoreModel instance)
    private IEnumerator PostScoreInternal(string instance)
    {
        //Debug.Log("START POST SCORE INTERNAL");
        //string json = JsonUtility.ToJson(instance);
        string json = instance;
        //print(json);
        using (UnityWebRequest www = WebUtility.BuildScoresAPIWebRequest(GetLeaderboardsAPIURL(),
         HttpMethod.Post.ToString(), json, userID, username))
        {
            yield return www.SendWebRequest();
            if (GlobalVar.DebugFlag) Debug.Log(www.responseCode);

            //print(www.responseCode);
            //print(www.downloadHandler);
            CallbackResponse<User> response = new CallbackResponse<User>();
            if (WebUtility.IsWWWError(www))
            {
                //print("gago");
                //print(www.error);
                if (GlobalVar.DebugFlag) Debug.Log(www.error);
                WebUtility.BuildResponseObjectOnFailure(response, www);
            }
            else if (www.downloadHandler != null)  //all OK
            {
                //print("ano ba toh");
                //let's get the new object that was created
                try
                {
                    //print("all okay");
                    //print(www.downloadHandler.text);
                    //print(www.responseCode);

                    if (www.responseCode == 204)
                    {
                        //print("No Response");
                        callbackFunction("204");
                    } else if (www.responseCode == 600 || www.responseCode == 601)
                    {
                        // invalid username
                        callbackFunction(www.responseCode.ToString());
                    }
                    else
                    {
                        //print("create new user");
                        callbackFunction("201");
                        User newObject = JsonUtility.FromJson<User>(www.downloadHandler.text);
                        //newObject._id
                        //print(newObject.id);
                        //PlayerPrefs.SetString("ID", newObject.id);
                        reJSON.AddObject("ID", newObject.id);
                        reJSON.SaveJSON("ud.kuf");
                        if (GlobalVar.DebugFlag) Debug.Log("new object is " + newObject.ToString());
                        response.Status = CallBackResult.Success;
                        response.Result = newObject;
                    }
                    
                }
                catch (Exception ex)
                {
                    //Debug.Log("exceptiom");
                    print(www.downloadHandler.text);
                    print(www.responseCode);
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
            }
            //onInsertCompleted(response);

            //Debug.Log("DONE RESPONSE");
            www.Dispose();
        }
    }


    // Exclusive for this leaderboard
    //private IEnumerator PutScoreInternal(ScoreModel instance)
    private IEnumerator PutScoreInternal(string instance, string updateScore)
    {
        //Debug.Log("START PUT SCORE INTERNAL");
        //string json = JsonUtility.ToJson(instance.LeaderboardDetails);
        string json = instance;
        //ScoreModel.LeaderboardIDToSend _instance = ;
        byte[] myData = System.Text.Encoding.UTF8.GetBytes(json);
        string userID = reJSON.jSONObject["ID"];
        //print(reJSON.jSONObject["ID"]);

        string link;
        if (updateScore == "score")
        {
            link = GetLeaderboardsAPIURL() + "/" + userID + "/"
            + GlobalVar.leaderboardID;
        } else
        {
            link = GetLeaderboardsAPIURL() + "/avataredit/" + userID;
        }
        UnityWebRequest www = UnityWebRequest.Put(link, myData);
        www.SetRequestHeader(GlobalVar.Accept, GlobalVar.ApplicationJson);
        www.SetRequestHeader(GlobalVar.Content_Type, GlobalVar.ApplicationJson);
        www.SetRequestHeader(GlobalVar.PrincipalID, userID);
        www.SetRequestHeader(GlobalVar.PrincipalName, username);
        yield return www.SendWebRequest();



        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
            if (updateScore != "score")
                callbackFunction("404");
        }
        else
        {
            if (updateScore != "score")
                callbackFunction("204");
            //Debug.Log("Upload complete!");
        }
        //using (UnityWebRequest www = WebUtility.BuildScoresAPIWebRequest(GetLeaderboardsAPIURL() + "/9a8e17c6a4526a1",
        // HttpMethod.Put.ToString(), json, userID, username))
        //{
        //    yield return www.SendWebRequest();
        //    if (GlobalVar.DebugFlag) Debug.Log(www.responseCode);

        //    CallbackResponse<User> response = new CallbackResponse<User>();
        //    if (WebUtility.IsWWWError(www))
        //    {
        //        if (GlobalVar.DebugFlag) Debug.Log(www.error);
        //        WebUtility.BuildResponseObjectOnFailure(response, www);
        //    }
        //    else if (www.downloadHandler != null)  //all OK
        //    {
        //        //let's get the new object that was created
        //        try
        //        {
        //            User newObject = JsonUtility.FromJson<User>(www.downloadHandler.text);
        //            if (GlobalVar.DebugFlag) Debug.Log("new object is " + newObject.ToString());
        //            response.Status = CallBackResult.Success;
        //            response.Result = newObject;
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.Log("exceptiom");
        //            response.Status = CallBackResult.DeserializationFailure;
        //            response.Exception = ex;
        //        }
        //    }
        //    //onInsertCompleted(response);

        //    Debug.Log("DONE RESPONSE");
        //    www.Dispose();
        //}
    }
    private IEnumerator GetStuffSingle<T>(string url, Action<CallbackResponse<T>> callback)
    {
        using (UnityWebRequest www = WebUtility.BuildScoresAPIWebRequest
         (GetLeaderboardsAPIURL() + url, HttpMethod.Get.ToString(), null, userID, username))
        {
            yield return www.Send();
            if (GlobalVar.DebugFlag) Debug.Log(www.responseCode);
            CallbackResponse<T> response = new CallbackResponse<T>();
            if (WebUtility.IsWWWError(www))
            {
                if (GlobalVar.DebugFlag) Debug.Log(www.error);
                WebUtility.BuildResponseObjectOnFailure(response, www);
            }
            else
            {
                try
                {
                    T data = JsonUtility.FromJson<T>(www.downloadHandler.text);
                    response.Result = data;
                    response.Status = CallBackResult.Success;
                }
                catch (Exception ex)
                {
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
            }
            callback(response);
            www.Dispose();
        }
    }
    private IEnumerator GetStuffArray<T>(string url, int skipCount, Action<CallbackResponse<T[]>> callback)
    {
        string fullurl = GetLeaderboardsAPIURL() + url;
        if (skipCount < 0)
            throw new ArgumentException("skipCount cannot be less than zero");
        else if (skipCount > 0)
            fullurl += "?skip=" + skipCount;
        using (UnityWebRequest www = WebUtility.BuildScoresAPIWebRequest
           (fullurl, HttpMethod.Get.ToString(), null, userID, username))
        {
            yield return www.SendWebRequest();
            if (GlobalVar.DebugFlag) Debug.Log(www.responseCode);
            var response = new CallbackResponse<T[]>();
            if (WebUtility.IsWWWError(www))
            {
                if (GlobalVar.DebugFlag) Debug.Log(www.error);
                WebUtility.BuildResponseObjectOnFailure(response, www);
            }
            else
            {
                try
                {
                    //T[] data = JsonHelper.GetJsonArray<T>(www.downloadHandler.text);
                    //response.Result = data;
                    response.Status = CallBackResult.Success;
                }
                catch (Exception ex)
                {
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
            }
            callback(response);
            www.Dispose();
        }
    }
    private string GetLeaderboardsAPIURL()
    {
        //return string.Format("{0}/api/", Url);
        return Url;
    }
}
