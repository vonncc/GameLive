using Firebase.Extensions;
using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MessageHandler : MonoBehaviour
{

    public enum FirebaseTopic
    {
        GameShowBetaLocal,
        GameShowPlayOn,
        TestTopic
    }

    public FirebaseTopic firebaseTopic;
    public GUISkin fb_GUISkin;
    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    private string logText = "";
    const int kMaxLogSize = 16382;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    protected bool isFirebaseInitialized = false;
    //private string topic = "GameShowPlayOn";


    private string topic = "GameShowBetaLocal";
    private bool UIEnabled = false;

    public static bool gotAnswer;
    public static int rightAnswer;

    //public delegate void DoOutside();
    //public delegate void DoOnMessage(DoOutside mOutside);
    

    //public DoOnMessage doOnMessage;
    //public DoOnMessage 

    // Log the result of the specified task, returning true if the task
    // completed successfully, false otherwise.
    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;
        if (task.IsCanceled)
        {
            DebugLog(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            DebugLog(operation + " encounted an error.");
            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
            {
                string errorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;
                if (firebaseEx != null)
                {
                    errorCode = String.Format("Error.{0}: ",
                      ((Firebase.Messaging.Error)firebaseEx.ErrorCode).ToString());
                }
                DebugLog(errorCode + exception.ToString());
            }
        }
        else if (task.IsCompleted)
        {
            DebugLog(operation + " completed");
            complete = true;
        }
        return complete;
    }

    private void Awake()
    {
        topic = firebaseTopic.ToString();
        //print(topic);
        GameObject[] objs = GameObject.FindGameObjectsWithTag("FirebaseControl");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    protected virtual void Start()
    {
        //gotAnswer = false
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        GlobalVar.canAnswer = false;
        GlobalVar.correctAnswer = -1;
        DontDestroyOnLoad(gameObject);
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    

    // Setup message event handlers.
    void InitializeFirebase()
    {
        DebugLog("FireB Messaging Initialized");
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        //Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(task => {
        //    LogTaskCompletion(task, "SubscribeAsync");
        //});
       

        // This will display the prompt to request permission to receive
        // notifications if the prompt has not already been displayed before. (If
        // the user already responded to the prompt, thier decision is cached by
        // the OS and can be changed in the OS settings).
        Firebase.Messaging.FirebaseMessaging.RequestPermissionAsync().ContinueWithOnMainThread(
          task => {
              LogTaskCompletion(task, "RequestPermissionAsync");
          }
        );
        isFirebaseInitialized = true;
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(
            task => {
                LogTaskCompletion(task, "SubscribeAsync");
            }
        );
        DebugLog("Subscribed to " + topic);
    }

    void IterTest(System.Collections.Generic.KeyValuePair<string, string> pDic)
    {
        //doOnMessage();
        //doOnMessage(iterVal);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        DebugLog("Received a new message");
        var notification = e.Message.Notification;
        if (notification != null)
        {
            DebugLog("title: " + notification.Title);
            DebugLog("body: " + notification.Body);
            var android = notification.Android;
            if (android != null)
            {
                DebugLog("android channel_id: " + android.ChannelId);
            }
        }
        if (e.Message.From.Length > 0)
            //DebugLog("from: " + e.Message.From);
            if (e.Message.Link != null)
            {
                //DebugLog("link: " + e.Message.Link.ToString());
            }
        if (e.Message.Data.Count > 0)
        {
            bool canLoadAnotherScene = false;
            string sceneToLoad = "";
            //DebugLog("data:");
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
                     e.Message.Data)
            {
                //DebugLog("  " + iter.Key + ": " + iter.Value);
                //print("  " + iter.Key + ": " + iter.Value);


                if (iter.Key == "scenario_state")
                {
                    if (iter.Value == "startGame")
                    {
                        // disable Chat
                        GlobalVar.withChat = false;
                    }
                    else if (iter.Value == "ready")
                    {
                        //print("readying up");
                        //SceneManager.LoadScene("Scenario_2"); 
                        canLoadAnotherScene = true;
                        //SceneManager.LoadScene("MainGame");
                    }
                    else if (iter.Value == "play")
                    {
                        //IterTest(iter);
                        //GlobalVar.roundEnd = false;
                    }
                    else if (iter.Value == "roundFinish")
                    {
                        GlobalVar.durationForScenario = 0;
                        GlobalVar.showResultScreen = true;
                    }
                    else if (iter.Value == "roundEnd")
                    {
                        GlobalVar.withChat = true;
                        GlobalVar.roundEnd = true;
                    }
                    else if (iter.Value == "roundEndWithoutChat")
                    {
                        GlobalVar.withChat = false;
                        GlobalVar.roundEnd = true;
                    }
                    else if (iter.Value == "finish")
                    {
                        //SceneManager.LoadScene("GameFinishScene");
                        sceneToLoad = "GameFinishScene";
                        canLoadAnotherScene = true;
                    }
                    else
                    {
                        //SceneManager.LoadScene(iter.Value);
                        //sceneToLoad = iter.Value;
                        //canLoadAnotherScene = true;
                    }
                }
                else if (iter.Key == "answer")
                {
                    GlobalVar.canAnswer = true;
                    GlobalVar.receivedAnswer = true;
                    GlobalVar.correctAnswer = int.Parse(iter.Value);
                }
                else if (iter.Key == "leaderboard_id")
                {
                    GlobalVar.leaderboardID = iter.Value;
                }
                else if (iter.Key == "duration")
                {
                    GlobalVar.durationForScenario = float.Parse(iter.Value);
                }
                else if (iter.Key == "scenario_to_load")
                {
                    //print("found a scene to load");
                    if (int.Parse(iter.Value) >= 1)
                    {
                        sceneToLoad = "Scenario_" + iter.Value.ToString();
                    }
                    else
                    {
                        sceneToLoad = "Scenario_2";
                    }
                    canLoadAnotherScene = true;
                }
            }


            // Go To Scene
            if (canLoadAnotherScene == true)
            {
                //print("Scene To Load");
                //print(sceneToLoad);
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }


    public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        DebugLog("Received Registration Token: " + token.Token);
    }

    public void ToggleTokenOnInit()
    {
        bool newValue = !Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled;
        Firebase.Messaging.FirebaseMessaging.TokenRegistrationOnInitEnabled = newValue;
        DebugLog("Set TokenRegistrationOnInitEnabled to " + newValue);
    }

    // Exit if escape (or back, on mobile) is pressed.
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // End our messaging session when the program exits.
    public void OnDestroy()
    {
        Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
        Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
    }

    // Output text to the debug log text field, as well as the console.
    public void DebugLog(string s)
    {
        //print(s);
        logText += s + "\n";

        while (logText.Length > kMaxLogSize)
        {
            int index = logText.IndexOf("\n");
            logText = logText.Substring(index + 1);
        }

        scrollViewVector.y = int.MaxValue;
    }

    // Render the log output in a scroll view.
    void GUIDisplayLog()
    {
        scrollViewVector = GUILayout.BeginScrollView(scrollViewVector);
        GUILayout.Label(logText);
        GUILayout.EndScrollView();
    }

    // Render the buttons and other controls.
    void GUIDisplayControls()
    {
        if (UIEnabled)
        {
            controlsScrollViewVector =
                GUILayout.BeginScrollView(controlsScrollViewVector);
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Topic:", GUILayout.Width(Screen.width * 0.20f));
            topic = GUILayout.TextField(topic);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Subscribe"))
            {
                Firebase.Messaging.FirebaseMessaging.SubscribeAsync(topic).ContinueWithOnMainThread(
                  task => {
                      LogTaskCompletion(task, "SubscribeAsync");
                  }
                );
                //DebugLog("Subscribed to " + topic);
            }
            if (GUILayout.Button("Unsubscribe"))
            {
                Firebase.Messaging.FirebaseMessaging.UnsubscribeAsync(topic).ContinueWithOnMainThread(
                  task => {
                      LogTaskCompletion(task, "UnsubscribeAsync");
                  }
                );
                //DebugLog("Unsubscribed from " + topic);
            }
            if (GUILayout.Button("Toggle Token On Init"))
            {
                ToggleTokenOnInit();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }
    }

    // Render the GUI:
    //void OnGUI()
    //{
    //    GUI.skin = fb_GUISkin;
    //    if (dependencyStatus != Firebase.DependencyStatus.Available)
    //    {
    //        GUILayout.Label("One or more Firebase dependencies are not present.");
    //        GUILayout.Label("Current dependency status: " + dependencyStatus.ToString());
    //        return;
    //    }

    //    Rect logArea;
    //    Rect controlArea;

    //    if (Screen.width < Screen.height)
    //    {
    //        // Portrait mode
    //        controlArea = new Rect(0.0f, 0.0f, Screen.width, Screen.height * 0.5f);
    //        logArea = new Rect(0.0f, Screen.height * 0.5f, Screen.width, Screen.height * 0.5f);
    //    }
    //    else
    //    {
    //        // Landscape mode
    //        controlArea = new Rect(0.0f, 0.0f, Screen.width * 0.5f, Screen.height);
    //        logArea = new Rect(Screen.width * 0.5f, 0.0f, Screen.width * 0.5f, Screen.height);
    //    }

    //    GUILayout.BeginArea(new Rect(0.0f, 0.0f, Screen.width, Screen.height));

    //    GUILayout.BeginArea(logArea);
    //    GUIDisplayLog();
    //    GUILayout.EndArea();

    //    GUILayout.BeginArea(controlArea);
    //    GUIDisplayControls();
    //    GUILayout.EndArea();

    //    GUILayout.EndArea();
    //}
}
