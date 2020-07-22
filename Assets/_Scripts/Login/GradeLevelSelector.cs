using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradeLevelSelector : MonoBehaviour
{
    public GameObject gradeLevelSelector;

    public Text placeHolder;
    public Text gradeText;

    public void OnClick()
    {
        gradeLevelSelector.SetActive(true);
    }

    public void HideGradeLevelSelector()
    {
        gradeLevelSelector.SetActive(false);
    }

    public void SelectedGrade(string pGradeText)
    {
        placeHolder.text = "";
        gradeText.text = pGradeText;
        HideGradeLevelSelector();
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
