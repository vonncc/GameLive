using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UserDataSystem : MonoBehaviour
{
    //public static string version = "1.0.0";
    //public static bool saveAsBinary = false;

    //string file_path;
    //FileStream streamer;
    //JSONNode jSONVar;
    //delegate void BinaryConditionFunctions();

    //static JSONObject jSONObject = new JSONObject();
    // Start is called before the first frame update
    public string[] fileNames;
    
    JSONNode mJSONNode;
    JSONNode StampboardCopyToUseAsReference;
    static string stampboard_id;
    static string stampboard_title;
    int[] t;
    void CopyNumbers(int[] p_test)
    {
        t = p_test;
    }

    int[] PasteNumbers()
    {
        return t;
    }

    public static string GetStampboardID()
    {
        return stampboard_id;
    }

    public static void SetStampboardID(string p_id)
    {
        stampboard_id = p_id;
    }

    public static string GetStampboardTitle()
    {
        return stampboard_title;
    }

    public static void SetStampboardTitle(string p_title)
    {
        stampboard_title = p_title;
    }


    public void ResetStampboardData(int p_targetStampboard = 0)
    {
        print("p_targetStampboard");
        print(p_targetStampboard);
        SyncGotJsonToArray(p_targetStampboard);
        //var numberSet = GlobalVar.NonRepeatingNumberSet(0, 15);
        //var numberSet = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        //CopyNumbers(numberSet);
        JSONArray cellXLocation = new JSONArray();
        JSONArray cellYLocation;
        JSONObject cellContenttion;
        int currentFileToPut = 0;

        for (int i = 0; i < 4; i++)
        {
            cellYLocation = new JSONArray();
            for (int j = 0; j < 4; j++)
            {
                //string promoIdTest = "ID_" + i.ToString() + j.ToString();
                cellContenttion = new JSONObject();
                currentFileToPut += 1;
                //cellContenttion.Add("filename", fileNames[numberSet[currentFileToPut - 1]]);
                //cellContenttion.Add("state", false);
                //cellContenttion.Add("isPromo", false);
                //cellContenttion.Add("taskId", "none");
                //cellContenttion.Add("instructions", "none");
                //cellContenttion.Add("instructionImg", "none");
                //cellContenttion.Add("title", "none");
                //cellContenttion.Add("promoId", fileNames[numberSet[currentFileToPut - 1]]); // temporary

                string stampImage = StampboardCopyToUseAsReference[i][j]["stamp_img"];
                string instructionsImage = StampboardCopyToUseAsReference[i][j]["instruction_img"];

                //print(mJSONNode);
                //print(mJSONNode[i]);
                //print(mJSONNode[i][j]);   
                //print(mJSONNode[i][j]["stamp_img"]);
                int dotLocation = stampImage.IndexOf(".", System.StringComparison.CurrentCulture);
                string newString = stampImage.Substring(0,dotLocation);

                int dotLocationInstructions = instructionsImage.IndexOf(".", System.StringComparison.CurrentCulture);
                string newStringInstructionsImage = instructionsImage.Substring(0, dotLocationInstructions);
                //print("newString");
                //print(newString);
                cellContenttion.Add("filename", newString);
                cellContenttion.Add("state", false);
                cellContenttion.Add("isPromo", StampboardCopyToUseAsReference[i][j]["sponsored"]);
                cellContenttion.Add("taskId", StampboardCopyToUseAsReference[i][j]["id"]);
                cellContenttion.Add("instructions", StampboardCopyToUseAsReference[i][j]["instructions"]);
                cellContenttion.Add("instruction_Img", newStringInstructionsImage);
                cellContenttion.Add("title", StampboardCopyToUseAsReference[i][j]["title"]);
                cellContenttion.Add("promoId", "promo_" + StampboardCopyToUseAsReference[i][j]["id"] + "_" + StampboardCopyToUseAsReference[i][j]["title"]); // temporary

                cellYLocation.Add(cellContenttion);

              //  "id": 1,
              //"title": "Jollibee",
              //"instructions": "Jollibee instruction updatesadfasdsdfsadfsdfs again 2 3 4 5 6 7 8",
              //"stamp_img": "1571363794119_patatas_2.jpg",
              //"instruction_img": "",
              //"img": "https://devdcitdashboard.z23.web.core.windows.net/assets/momalland/1571363794119_patatas_2.jpg?sp=r&sv=2018-03-28&sr=b&sig=IyTbXTWAe3whK3XmOwZC%2FyfpZSb1VvWZh3RWy15VBVI%3D",
              //"instructionImg": null,
              //"sponsored": true
            }
            cellXLocation.Add(cellYLocation);
        }
        reJSON.AddObject("cellDefinedArray", cellXLocation);

        JSONArray toBeAllocated = new JSONArray();
        JSONArray gainedItem = new JSONArray();
        JSONObject pointGained = new JSONObject();
        for (int i = 0; i <= 9; i++)
        {
            toBeAllocated.Add(i);
        }
        //pointGained.Add("pointGained", 0);
        reJSON.AddObject("toBeAllocated", toBeAllocated);
        reJSON.AddObject("gainedItem", gainedItem);
        //print(reJSON.jSONObject.ToString());
    }


    void SyncGotJsonToArray(int p_portion)
    {
        //print("GetStampboardID()");
        //print(p_portion);
        SetStampboardID(mJSONNode["data"]["boards"][p_portion]["id"]);
        SetStampboardTitle(mJSONNode["data"]["boards"][p_portion]["title"]);
        //print(GetStampboardID());
        StampboardCopyToUseAsReference = mJSONNode["data"]["boards"][p_portion]["stampboard"];
        //print(mJSONNode[0][0]["stamp_img"]);
        //print("asdsdd");

        //print(p_jsonNode["data"]["boards"][0]["stampboard"][0][0]["id"]);
        //print(p_jsonNode["data"]["boards"][0]["stampboard"][0][1]["id"]);
        //print(p_jsonNode["data"]["boards"][0]["stampboard"][1][0]["id"]);
        //print(mJSONNode.ToString());
        //print(mJSONNode[0][0]["instructions"]);
        //bodyTextToJSON[0]
    }
    public void DataToSave()
    {
        //mJSONNode = jsonNode;
        string pathToSave = Path.Combine(Application.persistentDataPath + "/stmpbd.kuf");
        var streamer = new FileStream(pathToSave, FileMode.Open);
        mJSONNode = JSONNode.LoadFromBinaryStream(streamer);
        streamer.Close();
        //string JSONString = File.ReadAllText(pathToSave);
        //print(JSONString);
        //mJSONNode = JSON.Parse(JSONString);
        print(mJSONNode);
       
        //var Haroro = mJSONNode["data"]["boards"][1]["stampboard"];
        //print(Haroro[0][0]["stamp_img"]);
        if (reJSON.jSONObject["isTutorial"] == null)
        {
            reJSON.jSONObject["isTutorial"] = true;
            reJSON.jSONObject["isFirstTime"] = true;
        }

        if (reJSON.jSONObject["isSynced"] == null)
        {
            reJSON.jSONObject["isSynced"] = false;
        }

        if (reJSON.jSONObject["stampProgression"] == null) // the stamp progression counter from 1-16
        {
            reJSON.jSONObject["stampProgression"] = 0;
        }

        if (reJSON.jSONObject["tryingToGetAVoucher"] == null)
        {
            reJSON.jSONObject["tryingToGetAVoucher"] = false;
        }

        if (reJSON.jSONObject["shownNotifCompleteBoard"] == null)
        {
            reJSON.jSONObject["shownNotifCompleteBoard"] = false;
        }

        if (reJSON.jSONObject["ifBoardComplete"] == null)
        {
            reJSON.jSONObject["ifBoardComplete"] = false;
        }

        if (reJSON.jSONObject["keykey"] == null)
        {
            reJSON.jSONObject["keykey"] = "";
        }

        //print("asdasd");
        //print(1 % 2);
        if (reJSON.jSONObject["currentStampItemCounter"] == null)
        {
            reJSON.jSONObject["currentStampItemCounter"] = 0;
        } else
        {
            int sample = reJSON.jSONObject["currentStampItemCounter"];
            SetStampboardID(mJSONNode["data"]["boards"][sample]["id"]);
            SetStampboardTitle(mJSONNode["data"]["boards"][sample]["title"]);
        }

        if (reJSON.jSONObject["alignmentData"] == null)
        {
            // 1
            JSONArray mainAlignment = new JSONArray();
            JSONArray arrayPerMain;
            int widthNumber = 4;
            int heightNumber = 4;
            int groupNumber = 0;
            for (int i = 0; i < widthNumber; i++)
            {
                arrayPerMain = new JSONArray();
                for (int j = 0; j < heightNumber; j++)
                {
                    arrayPerMain.Add(false);
                }
                mainAlignment.Add(arrayPerMain);
                groupNumber += 1;
            }

            for (int i = 0; i < heightNumber; i++)
            {
                arrayPerMain = new JSONArray();
                for (int j = 0; j < widthNumber; j++)
                {
                    arrayPerMain.Add(false);
                }
                mainAlignment.Add(arrayPerMain);
                groupNumber += 1;
            }

            int currentScan = 0;
            int targetLocator = 0;
            int adder = 1;

            arrayPerMain = new JSONArray();
            while (currentScan >= 0)
            {
                arrayPerMain.Add(false);
                targetLocator += 1;

                if (targetLocator >= 4)
                {
                    targetLocator = 0;
                    currentScan += adder;
                    adder = -1;

                    mainAlignment.Add(arrayPerMain);
                    groupNumber += 1;
                    arrayPerMain = new JSONArray();
                }
                currentScan += adder;
            }
            reJSON.AddObject("alignmentData", mainAlignment);



            // 2 eto ang ginamit
            
            ResetStampboardData(0);
            //print("sabasjvdaygcbvs hjduio");

            //  upto here
            // 3
            //var numberSet = GlobalVar.NonRepeatingNumberSet(0, 15);
            //var numberSet = GlobalVar.NonRepeatingNumberSet(0, 15);
            //JSONArray cellArray = new JSONArray();

            //int currentX = 0;
            //int currentY = 0;
            //var nsSet = PasteNumbers();
            //for (int i = 0; i < nsSet.Length; i++)
            //{
            //    JSONObject cellContents = new JSONObject();
            //    JSONObject cellContentsMore = new JSONObject();
            //    cellContentsMore.Add("x", currentX);
            //    cellContentsMore.Add("y", currentY);
            //    currentX += 1;
            //    if (currentX > 3)
            //    {
            //        currentX = 0;
            //        currentY += 1;
            //    }
            //    cellContents.Add("filename", fileNames[nsSet[i]]);
            //    cellContents.Add("positions", cellContentsMore);
            //    //cellContents.Add("state", false);
            //    //cellContents.Add("isPromo", false);
            //    //cellContents.Add("promoId", "none");
            //    //cellContents.Add(fileNames[numberSet[i]], cellContentsMore);
            //    cellArray.Add("contents", cellContents);

            //}

            //reJSON.AddObject("cellArray", cellArray);



            //JSONArray cellXLocation = new JSONArray();
            //JSONArray cellYLocation;
            //JSONObject cellContenttion;
            //int currentFileToPut = 0;

            //for (int i = 0; i < widthNumber; i++)
            //{
            //    cellYLocation = new JSONArray();
            //    for (int j = 0; j < heightNumber; j++)
            //    {
            //        cellContenttion = new JSONObject();
            //        currentFileToPut += 1;

            //        cellContenttion.Add("filename", fileNames[numberSet[currentFileToPut-1]]);
            //        cellContenttion.Add("state", false);
            //        cellYLocation.Add(cellContenttion);


            //        //cellScriptCollections[i, j] = csScript;
            //        //boolTest[i, j] = false;
            //        //cellScriptCollection[]
            //    }
            //    cellXLocation.Add(cellYLocation);
            //}
            //reJSON.AddObject("cellDefinedArray", cellXLocation);

            //JSONArray toBeAllocated = new JSONArray();
            //JSONArray gainedItem = new JSONArray();
            //JSONObject pointGained = new JSONObject();
            //for (int i = 0; i <= 9; i++)
            //{
            //    toBeAllocated.Add(i);
            //}
            ////pointGained.Add("pointGained", 0);
            //reJSON.AddObject("toBeAllocated", toBeAllocated);
            //reJSON.AddObject("gainedItem", gainedItem);


            reJSON.AddObject("pointGained", 0);
            reJSON.AddObject("pointSent", 0);
            reJSON.AddObject("totalPointGained", 0);

            
            reJSON.jSONObject["isMuted"] = false;
            //pointGained.Add("pointGained", 0);


            //for (int i = 0; i < widthNumber; i++)
            //{
            //    for (int j = 0; j < heightNumber; j++)
            //    {
            //        GameObject ClonedImage = Instantiate(cellObjects, transform);
            //        //reJSON.jSONObject[""]
            //        CellScript csScript = ClonedImage.GetComponent<CellScript>();
            //        csScript.cellX = i;
            //        csScript.cellY = j;
            //        cellScriptCollections[i, j] = csScript;
            //        boolTest[i, j] = false;
            //        //cellScriptCollection[]
            //    }
            //}
        }

        if (reJSON.jSONObject["voucherGainedCollector"] == null)
        {
            //print("re creaye");
            JSONArray voucherCollector = new JSONArray();

            for (int i = 0; i < 4; i++) {
                JSONObject emptyVoucher = new JSONObject();
                emptyVoucher.Add("instructions", "instructions goes here");
                emptyVoucher.Add("id", "");
                emptyVoucher.Add("image", "image goes here");
                emptyVoucher.Add("referenceNumber", "reference number goes here");
                emptyVoucher.Add("isClaimed", false);
                emptyVoucher.Add("isGiven", false);
                emptyVoucher.Add("scannableImageTag", "");
                emptyVoucher.Add("voucher_name", "");
                JSONObject dateAndTime = new JSONObject();
                dateAndTime.Add("hour", 0);
                dateAndTime.Add("min", 0);
                dateAndTime.Add("sec", 0);
                dateAndTime.Add("month", 0);
                dateAndTime.Add("day", 0);
                dateAndTime.Add("year", 0);
                emptyVoucher.Add("dateObtained", dateAndTime);
                JSONObject expiryDate = new JSONObject();
                expiryDate.Add("hour", 0);
                expiryDate.Add("min", 0);
                expiryDate.Add("sec", 0);
                expiryDate.Add("month", 0);
                expiryDate.Add("day", 0);
                expiryDate.Add("year", 0);
                emptyVoucher.Add("expiryDate", expiryDate);
                emptyVoucher.Add("voucherChanceGiven", false);

                emptyVoucher.Add("wonVouhcer", false);
                emptyVoucher.Add("voucherGot", false);
                emptyVoucher.Add("voucherNotif", false);
                emptyVoucher.Add("voucherClaimed", false);
                //reJSON.AddObject("voucherGained", emptyVoucher);
                voucherCollector.Add(emptyVoucher);
                
            }

            reJSON.AddObject("voucherGainedCollector", voucherCollector);

            reJSON.AddObject("tierProgress", -1);
            //reJSON.jSONObject["wonVouhcer"] = false; // Coin Toss
            //reJSON.jSONObject["voucherGot"] = false; // Request To Get Vouhcer
            //reJSON.jSONObject["voucherNotif"] = false; // Voucher Got Show Notif
            //reJSON.jSONObject["voucherClaimed"] = false; // Voucher Claimed by user on scanning

            reJSON.jSONObject["stampboardID_toUse"] = 0; // Stampboard ID to get Voucher
        }

        if (reJSON.jSONObject["raffleData"] == null)
        {
            JSONObject userDataDefaults = new JSONObject();
            userDataDefaults.Add("f", "");
            userDataDefaults.Add("l", "");
            userDataDefaults.Add("adrs", "");
            userDataDefaults.Add("cn", "");
            userDataDefaults.Add("e", "");

            userDataDefaults.Add("tnc", false);
            userDataDefaults.Add("dp", false);
            reJSON.AddObject("raffleData", userDataDefaults);
        }


        //print(reJSON.jSONObject["cellArray"][1]);
        //print(reJSON.jSONObject["cellArray"].Count);
        //print(reJSON.jSONObject["cellDefinedarray"][0][0]["filename"]);

        reJSON.SaveJSON("data.kuf");
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);

#if UNITY_EDITOR
        reJSON.Init("data.kuf", true);
#else
        reJSON.Init("data.kuf", true);
#endif


        //print("kuf");
        //print(3 / 4);
    }
    void Start()
    {
        print("start");
        //print(reJSON.jSONObject["version"]);
        
        //reJSON.jSONObject["version"] = "0.1.1";
        //reJSON.AddObject("");
        //reJSON.AddObject("bisitali", 12345);
        //JSONObject jsonObj = new JSONObject();
        //jsonObj.Add("asdass", "asdsdas");
        //jsonObj.Add("asdsadda", "sdas");
        //jsonObj.Add("wqeqwe", "2131312");
        //jsonObj.Add("ertet", "sdasdrtyry");
        //jsonObj.Add("hfngfghf", "rtyrtyt");
        //reJSON.AddObject("sprikitonkitong", jsonObj);
        //JSONArray jSONArray = new JSONArray();
        //jSONArray.Add("Kptex", jsonObj);
        //jSONArray.Add("AR", jsonObj);
        //reJSON.AddObject("arrayTest", jSONArray);
        //print("asdasd");

        //reJSON.SaveJSON();

        //reJSON.AddObject("username", "sprikitong");
        //reJSON.AddObject("password", 2345617236514);

        //JSONObject userDataObject = new JSONObject();
        //userDataObject.Add("STR", 10);
        //userDataObject.Add("AGI", 10);
        //userDataObject.Add("VIT", 10);
        //userDataObject.Add("INT", 10);
        //userDataObject.Add("DEX", 10);
        //userDataObject.Add("LUK", 10);

        //reJSON.AddObject("userData", userDataObject);
        //reJSON.SaveJSON();
        //print("saving?");
    }

}
