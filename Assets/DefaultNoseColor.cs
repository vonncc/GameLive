using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultNoseColor : MonoBehaviour
{
    public AvataerFaceManager avataerFaceManager;

    Image mImage;
    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();
    }


    void UpdateColor(Color pColor)
    {
        mImage.color = pColor;
    }
    // Update is called once per frame
    void Update()
    {
        int currentlyFaceEquipped = avataerFaceManager.GetCurrentlyEquiped(0);

        switch(currentlyFaceEquipped)
        {
            case 0:
                UpdateColor(new Color(0.894f, 0.635f, 0.439f));
                break;
            case 1:
                UpdateColor(new Color(0.968f, 0.780f, 0.650f));
                break;
            case 2:
                UpdateColor(new Color(0.890f, 0.662f, 0.498f));
                break;
            case 3:
                UpdateColor(new Color(0.643f, 0.454f, 0.329f));
                break;
            case 4:
                UpdateColor(new Color(0.478f, 0.356f, 0.290f));
                break;
            default:
                UpdateColor(Color.white);
                break;
        }
    }
}
