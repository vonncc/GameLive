using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingScript : MonoBehaviour
{
    Image mImage;

    float directionIndicator = 1;
    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();
    }

    void ChangeRotation()
    {
        directionIndicator *= -1;
        mImage.fillClockwise = !mImage.fillClockwise;
    }
    // Update is called once per frame
    void Update()
    {
        mImage.fillAmount += .01f * directionIndicator;

        if (mImage.fillAmount >= 1f)
        {
            ChangeRotation();
        }
        else if (mImage.fillAmount <= 0f)
        {
            ChangeRotation();
        }
    }
}
