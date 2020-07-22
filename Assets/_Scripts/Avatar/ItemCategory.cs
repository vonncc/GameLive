using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCategory : MonoBehaviour
{

    public AvatarManagerScript avatarManagerScript;
    public ColorPaletteManager colorPaletteManager;
    public Sprite selectedFrame;
    public Sprite unselectedFrame;
    [HideInInspector]
    public Sprite spriteToUse;
    public int currentIndex = 0;
    public bool isSelected;
    Image mImage;
    public Image itemToShow;
    Color color;
    // Start is called before the first frame update


    void ChangeFrame(bool pIsSelected)
    {
        if (mImage == null)
            mImage = GetComponent<Image>();

        isSelected = pIsSelected;
        if (isSelected)
            mImage.sprite = selectedFrame;
        else
            mImage.sprite = unselectedFrame;
    }
    public void UpdateImage(bool pIsSelected)
    {
        ChangeFrame(pIsSelected);
        itemToShow.sprite = spriteToUse;
    }

    public void UpdateFrame(bool pIsSelected)
    {
        ChangeFrame(pIsSelected);
    }

    public void UpdateColor(Color pColor)
    {
        color = pColor;
    }

    public void OnClick()
    {
        avatarManagerScript.currentPartEditSelected = currentIndex;
        avatarManagerScript.UpdateAvatar();

    }

    private void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
