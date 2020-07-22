using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationAnswer : MonoBehaviour
{
    public GameObject gameOverGO;
    GameObjectScript gameOverScript;
    public GameObject waitingForAnswerGameObject;
    public bool isCorrectAnswerSelected;
    public Vonn_TouchManager vonn_TouchManager;
    ConfirmationHandler parentCOnfirmationHandler;
    public enum ButtonStatus
    {
        yes,
        no
    }

    public ButtonStatus buttonStatus;

    public void OnClick()
    {
        transform.parent.gameObject.SetActive(false);
        switch (buttonStatus)
        {
            case ButtonStatus.yes:
                //onAnswerSelected();
                //waitingForAnswerGameObject.SetActive(true);
                gameOverGO.SetActive(true);
                gameOverScript = gameOverGO.GetComponent<GameObjectScript>();
                gameOverScript.ShowScenario(GameObjectScript.Scenario.wait);
                parentCOnfirmationHandler.onAnswerSelected();
                if (isCorrectAnswerSelected == true)
                {
                    //onAnswerSelected();
                    


                } else
                {
                    
                }
                break;
            case ButtonStatus.no:
                //canCount = true;
                //counter = 0;
                GlobalVar.canAnswer = true;
                //StartCoroutine(ReEnableTouch());
                vonn_TouchManager.DeselectAll(true);
                break;
        }
        

    }

    private void OnEnable()
    {
        GlobalVar.canAnswer = false;
        parentCOnfirmationHandler = transform.parent.gameObject.GetComponent<ConfirmationHandler>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //canCount = false;
        isCorrectAnswerSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
