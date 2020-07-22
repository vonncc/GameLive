using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollCategory : MonoBehaviour
{
    ScrollRect mScrollRect;

    public enum ScrollDirection
    {
        left,
        right
    }

    public ScrollDirection scrollDirection;

    public float numberToAdd;
    float currentPosition = 0;

    int positionCounter = 0;
    public void Scroll(int pScrollDirection)
    {
        float maxWidth = 85 * 10;
        float numberOfCells = 10;
        float cellPerClick = maxWidth / numberOfCells;

        positionCounter += 1 * pScrollDirection;

        if (positionCounter <= 0)
            positionCounter = 0;
        else if (positionCounter >= numberOfCells)
            positionCounter = (int)numberOfCells;

        float currentPosition = ((positionCounter * cellPerClick) / maxWidth );
        //print(currentPosition);

        //float perIndention = 1 / maxWidth;

        //float indentionToAdd = 0;
        //indentionToAdd = perIndention * pScrollDirection;
        //print(currentPosition);
        //currentPosition += indentionToAdd;




        //if (currentPosition < 0)
        //    currentPosition = 0;
        //else if (currentPosition > 1)
        //    currentPosition = 1;


        mScrollRect.horizontalNormalizedPosition = currentPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = 0;
        positionCounter = 0;
        mScrollRect = GetComponent<ScrollRect>();
        mScrollRect.horizontalNormalizedPosition = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
