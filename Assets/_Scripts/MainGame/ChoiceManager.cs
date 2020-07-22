using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceManager : MonoBehaviour
{
    public enum ChoiceLetterEnum {
        A,B,C,D
    }

    public GameObject confirmationGameObject;
    public Text confirmText;
    ConfirmationHandler mConfrimationAnswer;
    public ChoiceLetterEnum ChoiceLetter;
    Vonn_GameManager vonn_GameManager;
    public Vonn_TouchManager vonn_TouchManager;
    Plane objPlane;
    Vector3 mO;

    public bool IsCorrectAnswer;

    public bool IsDebug = false;

    TextMeshPro textMeshPro;

   
    float timerCounter = 15;
    float currentTimer;

    private void Start()
    {
        vonn_GameManager = GameObject.FindGameObjectWithTag("VonnGameManager").GetComponent<Vonn_GameManager>();
        textMeshPro = GetComponentInChildren<TextMeshPro>();
    }

    Ray GenerateMouseRay()
    {
        Vector3 mousePosFar = new Vector3(Input.mousePosition.x,
                                            Input.mousePosition.y,
                                            Camera.main.farClipPlane);

        Vector3 mousePosNear = new Vector3(Input.mousePosition.x,
                                            Input.mousePosition.y,
                                            Camera.main.nearClipPlane);
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        Ray mr = new Ray(mousePosN, mousePosF-mousePosN);
        return mr;
    }

    void SetFinalAnswer()
    {
        if (IsCorrectAnswer == true)
        {
            vonn_GameManager.AddScore(currentTimer, ChoiceLetter.ToString());
            //Vonn_GameManager.CanAnswer = false;
            //timerCounter = 15;
            //print("Correct");
        }
        else
        {
            vonn_GameManager.UserPickedAnswer(false, ChoiceLetter.ToString());
            //print("Wrong");
        }
    }

    //ConfirmationAnswer.OnAnswerSelected SelectAnswer()
    //{
    //    SetFinalAnswer();
    //}

    public void OnClick()
    {
        if (GlobalVar.canAnswer == true)
        {
            //print("asdadsd");
            vonn_TouchManager.DeselectAll();
            currentTimer = vonn_GameManager.cdTimer;
            textMeshPro.color = Color.green;
            confirmText.text = "IS '" + ChoiceLetter.ToString() + "' YOUR FINAL ANSWER?";
            confirmationGameObject.SetActive(true);
            mConfrimationAnswer = confirmationGameObject.GetComponent<ConfirmationHandler>();
            mConfrimationAnswer.onAnswerSelected = () =>
            {
                SetFinalAnswer();
            };

            Vonn_TouchManager.canPick = false;
            //SetFinalAnswer();
        }
        
    }

    

    // Update is called once per frame
    void Update()
    {
        if (IsDebug == true)
        {
            if (IsCorrectAnswer == true)
                textMeshPro.text = "T";
            else
                textMeshPro.text = "F";
        }
        
        //if (Vonn_GameManager.runTimer == true)
        //{
        //    timerCounter -= Time.deltaTime;

        //    if (timerCounter <= 0)
        //    {
        //        timerCounter = 0;
        //    } else
        //    {

        //    }
        //}

        //if (IsCorrectAnswer == true)
        //{
        //    textMeshPro.color = Color.red;
        //}

    }
}
