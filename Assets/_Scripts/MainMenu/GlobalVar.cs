using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class GlobalVar : MonoBehaviour
{

    public static bool DebugFlag { get; set; }
    public static readonly string Accept = "Accept";
    public static readonly string Content_Type = "Content-Type";
    public static readonly string ApplicationJson = "application/json";
    public static readonly string ErrorOccurred = "Error occurred";
    public static readonly string PrincipalID = "x-ms-client-principal-id";
    public static readonly string PrincipalName = "x-ms-client-principal-name";
    public static readonly string LibraryVersion = "0.1";

    public static bool receivedAnswer;
    public static string username;
    public static int score;
    public static bool canAnswer;
    public static int correctAnswer;
    public static bool roundEnd;
    public static string leaderboardID;
    public static int UserCurrentScore;
    public static bool showResultScreen;
    public static bool withChat;
    public static float durationForScenario;

    public static class FormatObject
    {

        static bool isFamilyFriendly;
        private static List<string> FilteredWords()
        {
            List<string> Filter = new List<string>();
            Filter.Add("tite");
            Filter.Add("fuck");
            Filter.Add("pota");
            Filter.Add("puta");
            Filter.Add("pukinangina");
            Filter.Add("pukenangina");
            Filter.Add("pokinang");
            Filter.Add("pokenang");
            Filter.Add("putangina");
            Filter.Add("pitangina");
            Filter.Add("puki");
            Filter.Add("pakyu");
            Filter.Add("kantot");
            Filter.Add("kantoot");
            Filter.Add("cantot");
            Filter.Add("cantoot");
            Filter.Add("sex");
            Filter.Add("gago");
            Filter.Add("bobo");
            Filter.Add("tanga");
            Filter.Add("bitch");
            Filter.Add("nigger");
            Filter.Add("nigga");
            Filter.Add("ass");
            Filter.Add("asshole");
            Filter.Add("pekpek");
            Filter.Add("burat");
            Filter.Add("tangina");
            Filter.Add("biatch");
            Filter.Add("suck");
            Filter.Add("retard");
            Filter.Add("idiot");
            Filter.Add("bastard");
            // Add more here!
            return Filter;
        }

        public static string ToFamilyFriendlyString(object input)
        {
            isFamilyFriendly = true;
            string originalWord = (string)input;
            foreach (string fWord in FilteredWords())
            {
                //  Replace the word with *'s (but keep it the same length)
                string strReplace = "";
                for (int i = 0; i <= fWord.Length; i++)
                {
                    strReplace += "*";          
                }
                input = Regex.Replace(input.ToString(), fWord, strReplace, RegexOptions.IgnoreCase);

                if (input.ToString() != originalWord)
                    isFamilyFriendly = false;
            }
            return input.ToString();
        }

        public static bool IsFamilyFreindly(object input)
        {
            // return isFamilyFriendly;
            foreach (string fWord in FilteredWords())
            {
                var didFindBadWord = Regex.Match(input.ToString(), fWord, RegexOptions.IgnoreCase);

                if (didFindBadWord != Match.Empty)
                    return false;
            }
            return true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UserCurrentScore = 0;
        DontDestroyOnLoad(this);
        withChat = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
