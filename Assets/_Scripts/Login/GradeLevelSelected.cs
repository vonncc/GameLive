using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class GradeLevelSelected : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GradeLevelSelector gradeLevelSelector;
    public string gradeLevel;

    bool isClicked = false;
    public void OnPointerDown(PointerEventData eventData)
    {
        isClicked = true;
        if (eventData.dragging == true)
            isClicked = false;
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        if (isClicked == true && eventData.dragging == false)
        {
            //throw new System.NotImplementedException();
            gradeLevelSelector.SelectedGrade(gradeLevel);
            isClicked = false;
        }
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
