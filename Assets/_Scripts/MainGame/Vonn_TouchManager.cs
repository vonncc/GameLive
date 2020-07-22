using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Vonn_TouchManager : MonoBehaviour
{
    public static bool canPick;

    public GameObject confirmGO;
    Plane objPlane;
    Vector3 mO;
    GameObject gObj;

    public TextMeshPro[] texts;

    // Start is called before the first frame update
    void Start()
    {
        canPick = true;
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

        Ray mr = new Ray(mousePosN, mousePosF - mousePosN);
        return mr;
    }

    public void DeselectAll(bool pSome = false)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].color = Color.white;
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        //print(GlobalVar.canAnswer);
        if (GlobalVar.canAnswer == true && confirmGO.activeSelf == false)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                //Ray mouseRay = GenerateMouseRay();
                //RaycastHit hit;

                //if (Physics.Raycast(mouseRay.origin, mouseRay.direction, out hit))
                //{
                //    //DeselectAll();
                //    //gObj = hit.transform.gameObject;
                //    //objPlane = new Plane(Camera.main.transform.forward * -1, gObj.transform.position);

                //    //Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //    //float rayDistance;
                //    //objPlane.Raycast(mRay, out rayDistance);
                //    //mO = gObj.transform.position - mRay.GetPoint(rayDistance);
                //    ////print("did hit something");

                //    //ChoiceManager mChoiceManager = gObj.GetComponent<ChoiceManager>();
                //    //mChoiceManager.OnClick();
                //    //canPick = false;
                //}
                gObj = null;
                //print("nothing hit");
            }
            else if (Input.GetMouseButtonUp(0) && gObj)
            {
                gObj = null;
                //if (IsCorrectAnswer == true)
                //{
                //    vonn_GameManager.AddScore(GetAnswerScore());
                //    Vonn_GameManager.CanAnswer = false;
                //    timerCounter = 15;
                //    print("Correct");
                //} else
                //{
                //    print("Wrong");
                //}
            }
        } else
        {
            
        }

    }
}
