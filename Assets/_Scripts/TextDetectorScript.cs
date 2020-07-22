using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDetectorScript : MonoBehaviour
{
    Text mText;
    Outline mOutline;
    public InputField userChatBox;
    public string concatText = "/30";

    string generatedText;
    int maxChar;
    int currentCharNum;


    int textLength;
    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<Text>();
        mOutline = GetComponent<Outline>();
        maxChar = userChatBox.characterLimit;
    }

    // Update is called once per frame
    void Update()
    {
        textLength = userChatBox.text.Length;
        currentCharNum = maxChar - textLength;
        generatedText = currentCharNum + concatText;
        mText.text = generatedText;

        if (currentCharNum <= 0)
        {
            mText.color = Color.red;
            mOutline.effectColor = Color.red;
        } else
        {
            mText.color = Color.white;
            mOutline.effectColor = new Color(0.286f, 0.509f, 0.301f);
        } 
    }
}
