using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
#if NETFX_CORE
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using System.Runtime.InteropServices.WindowsRuntime;
#endif
public static class WebUtility
{
    public static UnityWebRequest BuildScoresAPIWebRequest(string url, string method, string json, string userID, string username)
    {
        UnityWebRequest www = new UnityWebRequest(url, method);
        //Debug.Log(method);
        www.SetRequestHeader(GlobalVar.Accept, GlobalVar.ApplicationJson);
        www.SetRequestHeader(GlobalVar.Content_Type, GlobalVar.ApplicationJson);
        www.SetRequestHeader(GlobalVar.PrincipalID, userID);
        www.SetRequestHeader(GlobalVar.PrincipalName, username);
        www.downloadHandler = new DownloadHandlerBuffer();

        if (method.ToUpper() == "PUT")
        {
        } else
        {
            if (!string.IsNullOrEmpty(json))
            {
                //Debug.Log("WITH JSON");
                //Debug.Log(json);
                byte[] payload = Encoding.UTF8.GetBytes(json);
                UploadHandler handler = new UploadHandlerRaw(payload);
                handler.contentType = GlobalVar.ApplicationJson;
                www.uploadHandler = handler;
            }
            else
            {
                //Debug.Log("NO JSON");
            }
        }
        return www;
    }

    
    public static void ValidateForNull(params object[] objects)
    {
        //foreach (object obj in objects)
        //{
        //    if (obj == null)
        //    {
        //        throw new Exception("Argument null");
        //    }
        //}
    }
    public static bool IsWWWError(UnityWebRequest www)
    {
        return www.isNetworkError || (www.responseCode >= 400L && www.responseCode <= 511L);
    }
    public static void BuildResponseObjectOnFailure(CallbackResponse response, UnityWebRequest www)
    {
        if (www.responseCode == 404L)
            response.Status = CallBackResult.NotFound;
        else if (www.responseCode == 409L)
            response.Status = CallBackResult.ResourceExists;
        else
            response.Status = CallBackResult.Failure;
        string errorMessage = www.error;
        if (errorMessage == null && www.downloadHandler != null && !string.IsNullOrEmpty(www.downloadHandler.text))
            errorMessage = www.downloadHandler.text;
        else
            errorMessage = GlobalVar.ErrorOccurred;
        Exception ex = new Exception(errorMessage);
        response.Exception = ex;
    }
    public static void BuildResponseObjectOnException(CallbackResponse response, Exception ex)
    {
        response.Status = CallBackResult.LocalException;
        response.Exception = ex;
    }
}
public enum HttpMethod
{
    Post,
    Get,
    Patch,
    Delete,
    Put,
    Merge
}