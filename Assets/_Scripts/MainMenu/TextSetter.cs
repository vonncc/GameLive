using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextSetter : MonoBehaviour
{
    public UnityEngine.UI.Text mText;

    public void SetText(string pString)
    {
        //mText = GetComponent<UnityEngine.UI.Text>();
        mText.text = pString;
    }

}
