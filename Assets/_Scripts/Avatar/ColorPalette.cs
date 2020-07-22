using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ColorPalette : MonoBehaviour
{
    public ColorPaletteManager colorPaletteManager;
    [HideInInspector]
    public AvatarManagerScript avatarManagerScript;
    [System.Serializable]
    public class NewColor
    {
        public float r;
        public float g;
        public float b;
    }

    public NewColor colorSet;
    public bool isSelected;
    [HideInInspector]
    public float r;
    [HideInInspector]
    public float g;
    [HideInInspector]
    public float b;

    public Image colorImage;

    [HideInInspector]
    public int numberInGroup;

    [HideInInspector]
    public Color color;

    float selectedFrame = .7f;
    float unselectedFrame = .9f;

    bool canClick = true;
    // Start is called before the first frame update
    void Start()
    {
        
        color = new Color(colorSet.r, colorSet.g, colorSet.b);
        colorImage.color = color;
    }

    public Color GetColor()
    {
        return color;
    }

    public void CanInteract(bool pStatus)
    {
        canClick = pStatus;
        if (canClick == true)
        {
            colorImage.color = new Color(color.r, color.g, color.b, 1f);
        } else
        {
            colorImage.color = new Color(color.r, color.g, color.b, 0.5f);
        }
    }

    public void UpdateFrame()
    {
        Vector3 newVector = new Vector3();
        if (isSelected == true)
            newVector = new Vector3(selectedFrame, selectedFrame, selectedFrame);
        else
            newVector = new Vector3(unselectedFrame, unselectedFrame, unselectedFrame);

        colorImage.transform.localScale = newVector;
    }

    public void OnClick()
    {
        if (canClick == true)
        {
            colorPaletteManager.OnClick(GetColor(), numberInGroup);
            isSelected = true;
            UpdateFrame();
            avatarManagerScript.UpdateAvatar();
        }
        
    }
}
