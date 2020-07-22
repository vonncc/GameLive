using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SimpleJSON;

public class AvatarManagerScript : MonoBehaviour
{
    public AvataerFaceManager avataerFaceManager;
    public ColorPaletteManager colorPaletteManager;
    public int currentPartEditing = 0;
    public int currentPartEditSelected = 0;

    public GameObject clonedItemSelection;
    public Transform itemScrollSelection;

    public Transform categoryScrollContent;

    public List<ItemCategory> itemCategoryList;

    CategorySelection[] allCategory;

    LeaderboardSDK mLeaderboardSDK;

    [Header("For loading and saving")]
    public GameObject loadingScreen;
    public MessageBoxScript msgBoxScript;
    string[] partsName =
    {
        "BackHair",
        "Face",
        "Mouth",
        "Nose",
        "Eyes",
        "Brows",
        "MidHair",
        "FrontHair",
        "Clothes",
        "Frame",
        "BG"
    };

    public void UpdateSelection(int pPartToEdit, int pPartSelected)
    {
        colorPaletteManager.ResetPalette();
        currentPartEditing = pPartToEdit;
        currentPartEditSelected = pPartSelected; // change to whatever is equipped or last selected

        //print(avataerFaceManager.GetColorIndexNumber(currentPartEditing));
        colorPaletteManager.CanColorChange(avataerFaceManager.GetCanColorChange(currentPartEditing), avataerFaceManager.GetColorIndexNumber(currentPartEditing));
        int maxItemsOnCategory = avataerFaceManager.GetTotalItems(currentPartEditing);

        Sprite[] sprites = avataerFaceManager.GetItemSelectImages(currentPartEditing);
        if (itemCategoryList.Count > 0)
        {
            foreach(var item in itemCategoryList)
            {
                Destroy(item.gameObject);
            }
        }


        itemCategoryList = new List<ItemCategory>();
        for (int i = 0; i < maxItemsOnCategory; i++)
        {
            GameObject newItem = Instantiate(clonedItemSelection, itemScrollSelection);
            ItemCategory itemCategory = newItem.GetComponent<ItemCategory>();
            itemCategory.avatarManagerScript = this;
            itemCategory.currentIndex = i;
            itemCategory.spriteToUse = sprites[i];
            itemCategory.UpdateImage(i == currentPartEditSelected);
            itemCategoryList.Add(itemCategory);
        }
    }

    public void OnChangeSelection(int pSelected)
    {
        for (int i = 0; i < itemCategoryList.Count; i++)
        {
            itemCategoryList[i].UpdateFrame(false);
        }
        itemCategoryList[pSelected].UpdateFrame(true);
    }

    public void OnChangeCategory(int pSelected)
    {
        

        //foreach (var component in categoryScrollContent.GetComponentsInChildren<CategorySelection>())
        //{
        //    //component.UpdateFrame();
        //}
        for (int i = 0; i < allCategory.Length; i++)
        {
            allCategory[i].UpdateFrame(false);
        }
    }

    public void OnPointerClickDelegate(PointerEventData data)
    {
        //UpdateAvatar(0);
    }

    public void UpdateAvatar()
    {
        OnChangeSelection(currentPartEditSelected);
        
        avataerFaceManager.UpdateCurrentSelected(currentPartEditing, currentPartEditSelected); // change itsura
        //print(colorPaletteManager.currentSelectedColor);
        avataerFaceManager.UpdateColor(currentPartEditing, colorPaletteManager.GetColor(), colorPaletteManager.currentSelectedColor);
    }

    public void OnClickSave()
    {
        //avataerFaceManager.SaveToJSON();
        //print(avataerFaceManager.SaveToJSON());
        reJSON.jSONObject["Avatar"] = avataerFaceManager.SaveToJSON();
        //print(reJSON.jSONObject["Avatar"]);
        reJSON.SaveJSON("ud.kuf");
        loadingScreen.SetActive(true);
        //JSONObject currentLeaderboard = new JSONObject();
        //currentLeaderboard.Add("leaderboardId", GlobalVar.leaderboardID);
        //currentLeaderboard.Add("ScoreForDay", GlobalVar.UserCurrentScore.ToString());
        //currentLeaderboard.Add("BadgesForDay", "0");

        //leaderboardDetailsObj.Add(GlobalVar.leaderboardID, currentLeaderboard);

        //newJsonObject.Add("LeaderboardDetails", leaderboardDetailsObj);

        ////print(newJsonObject.ToString());

        ////if (PlayerPrefs.GetString("ID") == "")
        ///
        LeaderboardSDK.CallbackListener onListener = (string fromListener) =>
        {
            loadingScreen.SetActive(false);
            if (fromListener != "204")
            {
                //print("user already exists");
                msgBoxScript.Open("WARNING", "SAVING FAILED, TRY AGAIN LATER.");
            }
            else
            {
                //print("create new user");
                msgBoxScript.Open("WELCOME", "AVATAR SAVED");

            }
        };

        mLeaderboardSDK.UpdateAvatar(reJSON.jSONObject["Avatar"].ToString(), onListener);
    }


    // Start is called before the first frame update
    void Start()
    {
        mLeaderboardSDK = GetComponent<LeaderboardSDK>();
        allCategory = categoryScrollContent.GetComponentsInChildren<CategorySelection>();

        //print(reJSON.jSONObject["Avatar"]);
        //print(reJSON.jSONObject["Avatar"]["BackHair"]);
        for (int i = 0; i< partsName.Length;i++)
        {
            //print(reJSON.jSONObject["Avatar"][partsName[i]]);
            //print(reJSON.jSONObject["Avatar"][partsName[i]]);
            avataerFaceManager.UpdateCurrentSelected(i, reJSON.jSONObject["Avatar"][partsName[i]]["itemNumber"]);
            Color upColor = new Color(reJSON.jSONObject["Avatar"][partsName[i]]["itemColor"]["r"],
                reJSON.jSONObject["Avatar"][partsName[i]]["itemColor"]["g"],
                reJSON.jSONObject["Avatar"][partsName[i]]["itemColor"]["b"],
                reJSON.jSONObject["Avatar"][partsName[i]]["itemColor"]["a"]);
            avataerFaceManager.UpdateColor(i, upColor, reJSON.jSONObject["Avatar"][partsName[i]]["itemColorNumber"]);
            //print(partsName[i].ToString());

            //avataerFaceManager.UpdateCurrentSelected(currentPartEditing, currentPartEditSelected);
        }

        UpdateSelection(1, reJSON.jSONObject["Avatar"]["Face"]["itemNumber"]);



    }

    // Update is called once per frame
    void Update()
    {
        print(typeof(string).Assembly.ImageRuntimeVersion);
        
    }
}
