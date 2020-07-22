using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class SignInValidation : MonoBehaviour
{
    public InputField userNameField;
    public Text userNameFieldNote;

    public Button submitButton;
    public int minUserName = 3;
    //public InputField maxUserName ;


    void UpdateNote(string pNote)
    {
        userNameFieldNote.text = pNote;
    }
    string ValidateUserName()
    {
        string _uNameField = userNameField.text;

        string regexString = "^[a-zA-Z0-9_]{2,10}$";
        Regex regex = new Regex(regexString);

        //print(regex.IsMatch(_uNameField));
        if (_uNameField.Length < minUserName || _uNameField.Length > userNameField.characterLimit)
        {
            //username must be 3 to 10 characters.
            UpdateNote("Username must be " + minUserName.ToString() + " to " + userNameField.characterLimit + " characters.");
            if (_uNameField.Length <= 0)
            {
                UpdateNote("dont use your real name");
                return "empty";
            }
            return "length";
        }   
        else if (_uNameField == "")
        {
            UpdateNote("dont use your real name");
            return "empty";
        } else if (regex.IsMatch(_uNameField) == false )
        {
            UpdateNote("No special '!@.,:<()$%*' characters.");
            return "regexInvalid";
        } else if (_uNameField.StartsWith("_"))
        {
            UpdateNote("username must not start with '_'.");
            return "invalidStart";
        }
            
        return "ok";
    }
    public void UserNameValidate()
    {
        string _gotResult = ValidateUserName();
        if (_gotResult != "ok")
        {
            submitButton.interactable = false;
            if (_gotResult == "empty")
            {
                userNameFieldNote.color = Color.black;
            } else
            {
                userNameFieldNote.color = Color.red;
            }
            
        } else
        {
            if (_gotResult == "empty")
            {
                submitButton.interactable = false;
            } else
            {
                UpdateNote("");
                submitButton.interactable = true;
            }
            
            userNameFieldNote.color = Color.black;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
