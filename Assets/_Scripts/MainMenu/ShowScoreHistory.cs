using UnityEngine;
using SimpleJSON;
using System;
using System.Collections.Generic;

public class ShowScoreHistory : MonoBehaviour
{

    public GameObject historyGameObject;
    public GameObject rowItem;
    public Transform scrollRectTransform;

    public GameObject noRecordGameObject;

    //string[] dates = { "01/01/2020", "02/01/2020", "03/01/2020" }; // sample
    //string[] scores = { "1234", "5678", "9101" }; // sample
    public void OnClick(bool pBool)
    {
        historyGameObject.SetActive(pBool);
    }
    // Start is called before the first frame update
    void Start()
    {
        //string date = reJSON.jSONObject["score_history"][]

        //print(reJSON.jSONObject["score_history"].Count);

        if (reJSON.jSONObject["score_history"].Count > 0)
            noRecordGameObject.SetActive(false);
        else
            noRecordGameObject.SetActive(true);

        foreach (KeyValuePair<string, JSONNode> datescore in reJSON.jSONObject["score_history"])
        {
            GameObject clonedGameObject = Instantiate(rowItem, scrollRectTransform);
            HistoryScoreRow historyScoreRow = clonedGameObject.GetComponent<HistoryScoreRow>();
            historyScoreRow.UpdateText(datescore.Key, datescore.Value.ToString());
        }
        historyGameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //for (int i = 0; i < dates.Length; i++)
        //{
        //    GameObject clonedGameObject = Instantiate(rowItem, scrollRectTransform);
        //    HistoryScoreRow historyScoreRow = clonedGameObject.GetComponent<HistoryScoreRow>();
        //    historyScoreRow.UpdateText(dates[i], scores[i]);
        //}
        //historyGameObject.SetActive(false);
    }

    private void OnDisable()
    {   
        historyGameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
