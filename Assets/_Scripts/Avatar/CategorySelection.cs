using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategorySelection : MonoBehaviour
{
    public AvatarManagerScript avatarManagerScript;
    public AvataerFaceManager avataerFaceManager;
    public Sprite selectedFrame;
    public Sprite unSelecgted;
    Image mImage;
    public int currentSelectionCategory;
    public bool isSelected { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();

        if (currentSelectionCategory == 1)
            ChangeFrame(isSelected = true);
    }

    void ChangeFrame(bool pIsSelected)
    {
        if (mImage == null)
            mImage = GetComponent<Image>();

        isSelected = pIsSelected;
        if (isSelected)
            mImage.sprite = selectedFrame;
        else
            mImage.sprite = unSelecgted;
    }

    public void UpdateFrame(bool pIsSelected)
    {
        ChangeFrame(pIsSelected);
    }

    public void OnClick()
    {
        //print(currentSelectionCategory);
        
        avatarManagerScript.OnChangeCategory(currentSelectionCategory);
        isSelected = true;
        UpdateFrame(isSelected);
        avatarManagerScript.UpdateSelection(currentSelectionCategory, avataerFaceManager.GetCurrentSelected(currentSelectionCategory));
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
