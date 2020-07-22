using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPaletteManager : MonoBehaviour
{
    public AvatarManagerScript avatarManagerScript;
    public ColorPalette[] colorPalettes;

    public int currentSelectedColor = 0;
    Color colorGot;
    public void OnClick(Color pGotColor, int pIndexColor)
    {
        for (int i = 0; i < colorPalettes.Length; i++)
        {
            colorPalettes[i].isSelected = false;
            colorPalettes[i].UpdateFrame();
        }
        currentSelectedColor = pIndexColor;
        colorGot = pGotColor;
    }

    public Color GetColor()
    {
        return colorGot;
    }

    public void StartOnThisColor(int pStartColor)
    {
        currentSelectedColor = pStartColor;
        for (int i = 0; i < colorPalettes.Length; i++)
        {
            colorPalettes[i].avatarManagerScript = avatarManagerScript;
            colorPalettes[i].numberInGroup = i;
            colorPalettes[i].isSelected = false;
            if (i == pStartColor)
            {
                colorGot = colorPalettes[i].GetColor();
                colorPalettes[i].isSelected = true;
            }
            colorPalettes[i].UpdateFrame();
        }
    }

    public void ResetPalette()
    {
        currentSelectedColor = 0;
        StartOnThisColor(0);
        colorGot = new Color(1, 1, 1);
    }
    public void CanColorChange(bool pStatus, int pStartColor)
    {
        for (int i = 0; i < colorPalettes.Length; i++)
        {
            colorPalettes[i].CanInteract(pStatus);
        }

        if (pStatus == true)
        {
            StartOnThisColor(pStartColor);
        }
        else
        {
            ResetPalette();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetPalette();
    }

    // Update is called once per frame
    void Update()
    {
        //print(currentSelectedColor);
    }
}
