using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;
using System;

public class ChatScript : MonoBehaviour
{
    public Image blockerImage;
    public bool CanSendMessage = true;
    public bool isSpammable = false;
    [HideInInspector]
    public string userID;
    [HideInInspector]
    public string username;

    public ScrollRect scrollView;
    public Transform scrollViewParent;
    public GameObject clonedGameObject;

    public Text messageStatusText;

    public Button submitChatButton;

    public float maxSubmitTimerCounter = 10;

    public InputField inputField;
    JSONNode mJSONNode;
    float submitTImerCounter = 0;
    bool aMessageIsSending;
    string messageToSend;

    public class MessageContent {
        public string UserMessage { get; set; }
        public string UserID { get; set; }

        public MessageContent(string id, string msg)
        {
            UserID = id;
            UserMessage = msg;
        }
    }


    string CheckMessageString(string msg)
    {
        return GlobalVar.FormatObject.ToFamilyFriendlyString(msg);
    }


    bool CheckMessageBool(string pMsg)
    {
        return GlobalVar.FormatObject.IsFamilyFreindly(pMsg);
    }



    public void SendAMessage(string instance)
    {
        StopCoroutine("PostUserInternal");
        StartCoroutine(PostUserInternal(instance));
    }

    private IEnumerator PostUserInternal(string instance)
    {
        Debug.Log("START POST USER INTERNAL");
        aMessageIsSending = true;
        messageStatusText.text = "Posting";
        //string json = JsonUtility.ToJson(instance);
        string json = instance;
        //print(json);
        using (UnityWebRequest www = WebUtility.BuildScoresAPIWebRequest("https://dev-atei.azurewebsites.net/api/firebasemsg",
         HttpMethod.Post.ToString(), json, userID, username))
        {
            yield return www.SendWebRequest();
            if (GlobalVar.DebugFlag) Debug.Log(www.responseCode);

            CallbackResponse<User> response = new CallbackResponse<User>();
            if (WebUtility.IsWWWError(www))
            {
                if (GlobalVar.DebugFlag) Debug.Log(www.error);
                WebUtility.BuildResponseObjectOnFailure(response, www);
            }
            else if (www.downloadHandler != null)  //all OK
            {
                //print("ano ba toh");
                //let's get the new object that was created
                try
                {
                    User newObject = JsonUtility.FromJson<User>(www.downloadHandler.text);
                    //newObject._id
                    if (GlobalVar.DebugFlag) Debug.Log("new object is " + newObject.ToString());
                    response.Status = CallBackResult.Success;
                    response.Result = newObject;
                }
                catch (Exception ex)
                {
                    //Debug.Log("exceptiom");
                    response.Status = CallBackResult.DeserializationFailure;
                    response.Exception = ex;
                }
            }
            //onInsertCompleted(response);
            messageStatusText.text = "Posted";
            aMessageIsSending = false;
            //Debug.Log("DONE RESPONSE");
            www.Dispose();
        }
    }

    public void OnSubmit()
    {

        if (submitChatButton.interactable == true)
        {
            messageToSend = inputField.text;
            messageToSend = CheckMessageString(messageToSend);
            bool isMessageViable = CheckMessageBool(inputField.text);
            
            // check if message is good or not
            if (isMessageViable == false)
            {
                //return;
            }
            
            messageStatusText.text = "Sending";
            GameObject clonedText = Instantiate(clonedGameObject, scrollViewParent);
            Canvas.ForceUpdateCanvases();
            scrollViewParent.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            scrollViewParent.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            
            scrollView.content.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
            scrollView.content.GetComponent<ContentSizeFitter>().SetLayoutVertical();
            scrollView.verticalNormalizedPosition = 0f;
            submitChatButton.interactable = false;
            //clonedText.GetComponent<Transform>().SetAsFirstSibling();
            
            clonedText.GetComponent<TextSetter>().SetText(messageToSend);
            inputField.text = "";

            if (isSpammable == false)
            {
                blockerImage.fillAmount = 1;
                submitTImerCounter = maxSubmitTimerCounter;
            }
                

            JSONObject jSONObject = new JSONObject();
#if UNITY_EDITOR
            GlobalVar.username = "testEditor";
#endif
            //jSONObject.Add("UserID", PlayerPrefs.GetString("ID"));
            jSONObject.Add("UserID", GlobalVar.username);
            jSONObject.Add("UserMessage", messageToSend);

            jSONObject.Add("NotificationTitle", "Title Goes Here");
            jSONObject.Add("NotificationBody", messageToSend);

            string json = jSONObject.ToString();
            scrollView.verticalNormalizedPosition = 0f;

#if UNITY_EDITOR
            CanSendMessage = false;
            if (isMessageViable == true)
            {
                print("send to server");
            }
            else
            {
                print("do not send to server");
            }
#endif
            if (CanSendMessage == true)
            {
                if (isMessageViable == true)
                {
                    SendAMessage(json);
                }
                
            }
                
        }
        

    }


    public void IsTyping()
    {
        if (aMessageIsSending == false)
            messageStatusText.text = "";
    }
    // Start is called before the first frame update
    void Start()
    {
        GlobalVar.DebugFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        submitTImerCounter -= Time.deltaTime;
        blockerImage.fillAmount = submitTImerCounter/ maxSubmitTimerCounter;
        if (submitTImerCounter <= 0)
        {
            submitTImerCounter = 0;
            if (inputField.text == "")
            {
                submitChatButton.interactable = false;
            }
            else
                submitChatButton.interactable = true;
        }
    
        
    }
}
