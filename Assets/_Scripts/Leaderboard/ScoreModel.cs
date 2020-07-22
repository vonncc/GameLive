using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json;

[Serializable()]
public class ScoreModel
{
    public string UserID;

    public string id;

    public string category;

    public int Score;

    public int Badges;

    [Serializable()]
    public class LeaderboardIDToSend
    {

        public string leaderboardId;

        public string ScoreForDay;

        public string BadgesForDay;

    }

    public Dictionary<string, LeaderboardIDToSend> LeaderboardDetails { get; set; }
}

