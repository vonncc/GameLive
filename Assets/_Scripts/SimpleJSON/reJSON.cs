using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

#if UNITY_IOS
using UnityEngine.iOS;
#endif
using System.Runtime.Serialization.Formatters.Binary;

public class reJSON : MonoBehaviour
{


    public static string version = "1.0.0";
    public static JSONObject jSONObject = new JSONObject();

    static bool isSaveBinary;
    static bool isSaveBinaryFile;
    //static bool hasInit;
    static string JSON_path;
    static string JSON_file;
    static FileStream streamer;
    static JSONNode jSONVar;

    public string JSONFileName;
    public bool loadABinary;
    public bool saveAsBinary;

    
    

    [HideInInspector]
    public bool isSaving = false;

    
    
    delegate void BinaryConditionFunctions();   


    // Start is called before the first frame update

#region PUBLIC_STATIC_METHODS

    public static void AddObject(string p_ObjectName, JSONNode p_ObjectValue)
    {
        jSONObject.Add(p_ObjectName, p_ObjectValue);
    }

    public static void GetObject(object[] p_Params)
    {
        //foreach ()
    }

#endregion

#region PUBLIC_METHODS

    public static void Init(string p_JSONFileName, bool p_isBinary = false)
    {
        JSON_file = p_JSONFileName;
        isSaveBinary = p_isBinary;
        isSaveBinary = true;
        JSON_path = Path.Combine(Application.persistentDataPath, p_JSONFileName);
        if (File.Exists(JSON_path))
        {
            if (isSaveBinary == true)
            {
                LoadJSONSerialized();
            }
            else
            {
                LoadJSON();
            }
        }
        else
        {
            AddObject("version", "1.0.0");
            SaveJSON(p_JSONFileName);
        }
        //hasInit = true;
    }

    public static void SaveJSON( string p_JSONFileName = "jsonFile.acuf" )
    {
        //print("SAVING");
        string save_JSON_path = Path.Combine(Application.persistentDataPath, JSON_file);
        FileStream streamer_save = new FileStream(save_JSON_path, FileMode.Create);
        BinaryConditionFunctions loadFromBinary = () =>
        {
            //print("YARE YARE");
            //streamer_save = new FileStream(save_JSON_path, FileMode.Create);
            jSONObject.SaveToBinaryStream(streamer_save);
            streamer_save.Close();
        };

        BinaryConditionFunctions loadFromRegular = () =>
        {
            streamer_save.Close();
            File.WriteAllText(save_JSON_path, jSONObject.ToString());

        };
        CheckIfBinary(loadFromBinary, loadFromRegular);

#if UNITY_IOS
            Device.SetNoBackupFlag(save_JSON_path);
#endif
    }


#endregion


#region PRIVARE_METHODS

    static void CheckIfBinary(BinaryConditionFunctions p_isBinary, BinaryConditionFunctions p_isNotBinary)
    {
#if UNITY_EDITOR
        isSaveBinary = false;
        isSaveBinary = true;
#else
        isSaveBinary = true;
#endif
        if (isSaveBinary == true)
        {
            p_isBinary();
        }
        else
        {
            p_isNotBinary();
        }
    }

#endregion


#region PRIVATE_PROTECTED_METHODS

    protected static private void LoadJSONSerialized(string p_JSONFileName = "jsonFile.acuf")
    {
        streamer = new FileStream(JSON_path, FileMode.Open);
        jSONObject = (JSONObject)JSONNode.LoadFromBinaryStream(streamer);
        streamer.Close();
    }

    protected static private void LoadJSON(string p_JSONFileName = "jsonFile.acuf")
    {
        //JSON_path = Path.Combine(Application.persistentDataPath, p_JSONFileName);

#if UNITY_EDITOR
        string json_retrieved = File.ReadAllText(JSON_path);

        jSONVar = JSON.Parse(json_retrieved);
        jSONObject = (JSONObject)jSONVar;
#else
        streamer = new FileStream(JSON_path, FileMode.Open);
        jSONObject = (JSONObject)JSONNode.LoadFromBinaryStream(streamer);
        streamer.Close();
#endif
        //try
        //{

        //    string json_retrieved = File.ReadAllText(JSON_path);

        //    jSONVar = JSON.Parse(json_retrieved);
        //    jSONObject = (JSONObject)jSONVar;

        //    //File.
        //    print("is regular JSON");
        //    print(jSONObject["arrayTest"][0]["asdass"]);
        //} catch {

        //    try
        //    {
        //        //JSON_path = Path.Combine(Application.persistentDataPath, p_JSONFileName);
        //        streamer = new FileStream(JSON_path, FileMode.Open);
        //        jSONObject = (JSONObject)JSONNode.LoadFromBinaryStream(streamer);
        //        streamer.Close();
        //        print(jSONObject["bisitali"]);
        //        print("Serialized JSON");
        //    } finally
        //    {

        //    }

        //}
        //finally
        //{

        //}
        //JSON_path = Path.Combine(Application.persistentDataPath, p_JSONFileName);
        //streamer = new FileStream(JSON_path, FileMode.Open);

        //BinaryConditionFunctions loadFromBinary = () =>
        //{

        //};

        //BinaryConditionFunctions loadFromRegular = () =>
        //{
        //    streamer.Close();
        //    string json_retrieved = File.ReadAllText(JSON_path);
        //    jSONVar = JSON.Parse(json_retrieved);
        //    print(jSONVar);

        //};
        //CheckIfBinary(loadFromBinary, loadFromRegular);

    }

    #endregion



    void Awake()
    {   
        
        // Database Stuff

        //string JSONFileName = "data.json";
        //try
        //{
        //    print("loaded");
        //    //LoadJSON();
        //    LoadJSONSerialized();
        //    print("fully loaded");
        //} catch
        //{
        //    //puke = "jsonFile.json";
        //    print("There was an error loading, File not found");

        //    AddObject("version", "1.0.0");
        //    SaveJSON();
        //    //throw;
        //} finally
        //{
        //    print("DO something");
        //    //JSONObject
        //}

        //isSaveBinary = saveAsBinary;
        //JSON_path = Path.Combine(Application.persistentDataPath, JSONFileName);


        //print(jSONObject["version"]);

    }


}
