using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System;

public class AvataerFaceManager : MonoBehaviour
{

    //public Image backHair;
    //public Image face;
    //public Image mouth;
    //public Image nose;
    //public Image eyes;
    //public Image eyebrow;
    //public Image midHair;
    //public Image frontHair;
    //public Image clothes;

    //[HideInInspector]
    //public int currentBackHair;
    //[HideInInspector]
    //public int currentFace;
    //[HideInInspector]
    //public int currentMouth;
    //[HideInInspector]
    //public int currentNose;
    //[HideInInspector]
    //public int currentEyes;
    //[HideInInspector]
    //public int currentEyebrow;
    //[HideInInspector]
    //public int currentMidHair;
    //[HideInInspector]
    //public int currentFrontHair;
    //[HideInInspector]
    //public int currentClothes;

    [System.Serializable]
    public class PartClass
    {
        [System.Serializable]
        public struct Parts
        {
            public Sprite mainSprite;
            public Sprite highlightSprite;
            public Sprite overlaySprite;
        }
        public Parts[] parts;
    }

    [System.Serializable]
    public class PartStruct : PartClass {
        public Image imageToEdit;
        public Image highlightToEdit;
        public Image overlayToEdit;
        //[HideInInspector]
        //public Sprite[] spritesToUse;
        public Sprite[] spritesForSelection;
        public int currentEquipped;
        public bool canChangeColor;
        public string partName;
        int currentSelected;



        //[HideInInspector]
        public int colorIndex;


        public void UpdatePart(int pSelectedPart)
        {
            currentSelected = pSelectedPart;
            //imageToEdit.sprite = spritesToUse[currentSelected];
            imageToEdit.sprite = parts[currentSelected].mainSprite;

            if (overlayToEdit)
                overlayToEdit.sprite = null;

            if (highlightToEdit)
                highlightToEdit.sprite = null;

            if (parts[currentSelected].highlightSprite != null)
            {
                highlightToEdit.sprite = parts[currentSelected].highlightSprite;
            }

            if (parts[currentSelected].overlaySprite != null)
            {
                overlayToEdit.sprite = parts[currentSelected].overlaySprite;
            }
        }

        public void ChangeColor(Color pColor)
        {
            imageToEdit.color = pColor;
        }

        

        public void SavePart()
        {
            currentEquipped = currentSelected;
        }

        public int GetCurrentSelected()
        {
            return currentSelected;
        }

        public int GetTotalSprites()
        {
            //return spritesToUse.Length;
            return parts.Length;
        }

        public Sprite[] GetCurrentAllImages()
        {
            Sprite[] _sprites = new Sprite[parts.Length];

            for (int i = 0; i < _sprites.Length; i++)
            {
                _sprites[i] = parts[i].mainSprite;
            }

            return _sprites;
        }

        public Sprite[] GetCurrentAllSelection()
        {
            return spritesForSelection;
        }

        public Color GetImageColor()
        {
            return imageToEdit.color;
        }

        public int GetItemColorNumber()
        {
            return colorIndex;
        }

    }

    public PartStruct[] partStruct;

    //public PartClass[] partClass;

    public int GetCurrentlyEquiped(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].GetCurrentSelected();
    }

    public void UpdateCurrentSelected(int pCurrentSelected, int pItemPartSelected)
    {
        partStruct[pCurrentSelected].UpdatePart(pItemPartSelected);
    }

    public void UpdateColor(int pCurrentSelected, Color pGotColor, int pColorIndex)
    {
        partStruct[pCurrentSelected].ChangeColor(pGotColor);
        partStruct[pCurrentSelected].colorIndex = pColorIndex;
    }

    public void UpdateColor(int pCurrentSelected, Color pGotColor)
    {
        partStruct[pCurrentSelected].ChangeColor(pGotColor);
    }

    public int GetCurrentSelected(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].GetCurrentSelected();
    }

    public bool GetCanColorChange(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].canChangeColor;
    }

    public int GetTotalItems(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].GetTotalSprites();
    }

    public Sprite[] GetItemImages(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].GetCurrentAllImages();
    }

    public Sprite[] GetItemSelectImages(int pCurrentSelected)
    {
        if (partStruct[pCurrentSelected].GetCurrentAllSelection().Length > 0)
            return partStruct[pCurrentSelected].GetCurrentAllSelection();
        else
            return GetItemImages(pCurrentSelected);
    }

    public int GetColorIndexNumber(int pCurrentSelected)
    {
        return partStruct[pCurrentSelected].colorIndex;
    }


    class ExtractColor
    {
        public string r;
        public string g;
        public string b;
        public string a;
    }

    int ah = 0;
    public JSONObject SaveToJSON()
    {
        //JSONObject jsonObj = new JSONObject();
        JSONObject jsonNode = new JSONObject();
        //JSONObject jsonDetails = new JSONObject();
        for (int i = 0; i < partStruct.Length;i++)
        {
            JSONObject jsonDetails = new JSONObject();
            JSONObject jsonColor = new JSONObject();

            ah += 1;
            //print(partStruct[i].partName);
            Color partColor = partStruct[i].GetImageColor();

            float r = partColor.r;
            float g = partColor.g;
            float b = partColor.b;


            ExtractColor extractedColor = new ExtractColor();
            extractedColor.r = r.ToString();
            extractedColor.g = g.ToString();
            extractedColor.b = b.ToString();
            extractedColor.a = "1";
            var some = JsonUtility.ToJson(extractedColor);

            var ewan = JSON.Parse(some);
            jsonColor.Add("r", (float)ewan["r"]);
            jsonColor.Add("g", (float)ewan["g"]);
            jsonColor.Add("b", (float)ewan["b"]);
            jsonColor.Add("a", (float)1);

            jsonColor["r"] = Math.Round(float.Parse(jsonColor["r"]), 3);
            jsonColor["g"] = Math.Round(float.Parse(jsonColor["g"]), 3);
            jsonColor["b"] = Math.Round(float.Parse(jsonColor["b"]), 3);

            //jsonColor["r"] = r;
            //jsonColor["r"] = g;
            //jsonColor["r"] = b;

            jsonDetails.Add("itemNumber", partStruct[i].GetCurrentSelected());
            jsonDetails.Add("itemColor", jsonColor);
            jsonDetails.Add("itemColorNumber", partStruct[i].GetItemColorNumber());
            jsonDetails.Add("itemName", "empty");

            jsonNode.Add(partStruct[i].partName, jsonDetails);

        }

        //print(jsonNode);
        //jsonObj.Add("Avatar", jsonNode);
        return jsonNode;
    }
    public void SaveCurrentPart()
    {

    }

}
