using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicAnimationScript : MonoBehaviour
{
    public GameObject animationToShow;

    public float animationDuration = 2;

    public Image image;
    public Sprite[] extraImages;

    bool didShowAnimation;

    float counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        didShowAnimation = false;
    }


    private void OnEnable()
    {
        counter = 0;
        didShowAnimation = false;
    }

    public void ChangeExtraImage(string pLetterChosen)
    {
        if (image)
        {
            int selectedImage;
            switch (pLetterChosen.ToUpper())
            {
                case "A":
                    selectedImage = 0;
                    break;
                case "B":
                    selectedImage = 1;
                    break;
                case "C":
                    selectedImage = 2;
                    break;
                case "D":
                    selectedImage = 3;
                    break;
                default:
                    selectedImage = 0;
                    break;
            }

            //var selectedImage = (pLetterChosen.ToUpper()) switch
            //{
            //    "A" => 0,
            //    "B" => 1,
            //    "C" => 2,
            //    "D" => 3,
            //    _ => 0,
            //};
            image.sprite = extraImages[selectedImage];
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        if (didShowAnimation == false)
        {
            counter += Time.deltaTime;

            if (counter >= animationDuration)
            {
                didShowAnimation = true;
                animationToShow.SetActive(true);
            }
        }
        
    }
}
