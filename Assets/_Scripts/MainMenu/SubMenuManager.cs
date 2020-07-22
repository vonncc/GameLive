using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubMenuManager : MonoBehaviour
{
    public GameObject ScoreSummaryGameObject;
    public Button[] buttons;
    public enum SubMenuFunc
    {
        avatar,
        score,
        chat
    }

    int selectedIndex;

    void EnableAll()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }
    }
    void EnableAllExceptOne()
    {

        EnableAll();
        buttons[selectedIndex].interactable = false;
    }

    void DoOnClick()
    {
        ActiveScoreSummaryGameObject(false);
        switch (selectedIndex)
        {
            case 0:
                break;
            case 1:
                ActiveScoreSummaryGameObject(true);
                break;
            case 2:
                break;
            default:
                break;
        }
    }
    public void OnClick(int pIndexNumber)
    {
        selectedIndex = pIndexNumber;
        EnableAllExceptOne();
        DoOnClick();
    }

    public void ActiveScoreSummaryGameObject(bool pBool)
    {
        ScoreSummaryGameObject.SetActive(pBool);

        if (pBool == false)
            EnableAll();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
