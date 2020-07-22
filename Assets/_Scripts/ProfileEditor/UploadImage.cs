using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class UploadImage : MonoBehaviour
{
    public GameObject CameraDeviceGameObject;
    public RectTransform sourceRectTransform;
    public Rect sourceRect;
    //Rect sourceRect;
    public RawImage cameraRawImage;
    public Renderer cameraRenderer;
    WebCamTexture webCamTexture;
    public void ShowCamera()
    {
        //sourceRect = sourceRectTransform.rect;
        CameraDeviceGameObject.SetActive(true);
        webCamTexture = new WebCamTexture();
        cameraRawImage.material.mainTexture = webCamTexture;
        //cameraRenderer.material.mainTexture = webCamTexture; //Add Mesh Renderer to the GameObject to which this script is attached to
        webCamTexture.Play();
    }

    public void CaptureCamera()
    {
        StartCoroutine(TakePhoto());
    }

    IEnumerator TakePhoto()  // Start this Coroutine on some button click
    {

        // NOTE - you almost certainly have to do this here:

        yield return new WaitForEndOfFrame();

        // it's a rare case where the Unity doco is pretty clear,
        // http://docs.unity3d.com/ScriptReference/WaitForEndOfFrame.html
        // be sure to scroll down to the SECOND long example on that doco page 
        

        

        // Set the current object's texture to show the
        // extracted rectangle.
        //GetComponent<Renderer>().material.mainTexture = destTex;
        
        Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);

        ////photo.rea
        //photo.SetPixels(webCamTexture.GetPixels());
        //photo.Apply();

        int x = Mathf.FloorToInt(sourceRect.x);
        int y = Mathf.FloorToInt(sourceRect.y);
        int width = Mathf.FloorToInt(sourceRect.width);
        int height = Mathf.FloorToInt(sourceRect.height);

        int offsetX = Mathf.FloorToInt(photo.width / 2);
        int offsetY = Mathf.FloorToInt(photo.height / 2);
        Color[] pix = photo.GetPixels(0 ,0,offsetX + 100, offsetY + 100);

        Texture2D destTex = new Texture2D(width, height);
        destTex.SetPixels(pix);
        destTex.Apply();


        byte[] bytes = destTex.EncodeToPNG();
        //byte[] bytes = photo.EncodeToPNG();

        File.WriteAllBytes(Path.Combine(Application.persistentDataPath + "photo.png"), bytes);
        ////Encode to a PNG
        //
        ////Write out the PNG. Of course you have to substitute your_path for something sensible
        //File.WriteAllBytes(Path.Combine(Application.persistentDataPath + "photo.png"), bytes);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
