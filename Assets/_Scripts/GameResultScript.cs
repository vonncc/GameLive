using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultScript : MonoBehaviour
{

    public Text totalScore;
    public Text totalScoreUser;

    public void UpdateCurrentScore(string pScore)
    {
        totalScore.text = pScore;
        totalScoreUser.text = GlobalVar.UserCurrentScore.ToString();
    }

}
