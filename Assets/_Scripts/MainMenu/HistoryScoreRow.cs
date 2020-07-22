using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HistoryScoreRow : MonoBehaviour
{
    public Text dateText;
    public Text scoreText;

    public void UpdateText(string pDateText, string pScoreText)
    {
        dateText.text = pDateText;
        scoreText.text = pScoreText;
    }

}
