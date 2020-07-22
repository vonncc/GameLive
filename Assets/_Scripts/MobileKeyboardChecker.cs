using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileKeyboardChecker : MonoBehaviour
{
    Vector2 origPosition;
    public bool wholeKeyboard;
    RectTransform mPositon;
    // Start is called before the first frame update
    void Start()
    {
        origPosition = GetComponent<RectTransform>().position;
        mPositon = GetComponent<RectTransform>();
    }

    void UpdatePosition(Vector2 pPosition)
    {
        mPositon.position = pPosition;
    }

    public static int GetKeyboardHeight(bool includeInput)
    {
#if UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");

            var result = 0;

            if (view != null)
            {
                using (var rect = new AndroidJavaObject("android.graphics.Rect"))
                {
                    view.Call("getWindowVisibleDisplayFrame", rect);
                    result = Display.main.systemHeight - rect.Call<int>("height");
                }

                if (includeInput)
                {
                    var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                    var decorView = dialog?.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                    if (decorView != null)
                    {
                        var decorHeight = decorView.Call<int>("getHeight");
                        result += decorHeight;
                    }
                }
            }

            return result;
        }
#else
            var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
            return height >= Display.main.systemHeight ? 0 : height;
#endif
    }

    // Update is called once per frame
    void Update()
    {
        //print("Height/Y");
        //print(TouchScreenKeyboard.area.height);
        //print(TouchScreenKeyboard.area.y);
        if (TouchScreenKeyboard.visible == true)
        {
            if (wholeKeyboard)
                UpdatePosition(new Vector2(origPosition.x, origPosition.y + (GetKeyboardHeight(true))));
            else
                UpdatePosition(new Vector2(origPosition.x, origPosition.y + (GetKeyboardHeight(true)/2)));
        } else
        {
            UpdatePosition(origPosition);
        }

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    UpdatePosition(new Vector2(origPosition.x, origPosition.y + 50));
        //} else
        //{
        //    UpdatePosition(origPosition);
        //}

    }
}
