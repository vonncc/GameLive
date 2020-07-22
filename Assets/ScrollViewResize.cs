using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewResize : MonoBehaviour
{

    RectTransform mRectTransform;

    Vector2 origPosition;

    // Start is called before the first frame update
    void Start()
    {
        mRectTransform = GetComponent<RectTransform>();
        origPosition = new Vector2(mRectTransform.offsetMin.x, mRectTransform.offsetMin.y);
        print(origPosition);
        //mRectTransform.sizeDelta = new Vector2(0,0);
    }

    void UpdatePosition(Vector2 pPosition)
    {
        mRectTransform.sizeDelta = pPosition;
    }

    public static void SetLeft(RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }


    // Update is called once per frame
    void Update()
    {
        if (TouchScreenKeyboard.visible == true)
        {
            //UpdatePosition(new Vector2(origPosition.x, origPosition.y + (MobileKeyboardChecker.GetKeyboardHeight(true))));
            SetBottom(mRectTransform, origPosition.y + (MobileKeyboardChecker.GetKeyboardHeight(true)));
        }
        else
        {
            SetBottom(mRectTransform, origPosition.y);
            //UpdatePosition(origPosition);
        }
        //if (Input.GetKey(KeyCode.A))
        //{
        //    SetBottom(mRectTransform, 200);
        //} else if (Input.GetKey(KeyCode.S))
        //{
        //    //SetBottom(mRectTransform, 100);
        //}
    }
}
