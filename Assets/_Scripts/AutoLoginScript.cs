using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class AutoLoginScript : MonoBehaviour
{
    public Text welcomeText;
    public SignIn signInScript;

    float counter;
    // Start is called before the first frame update
    void Start()
    {
        welcomeText.text = "WELCOME\n" + GlobalVar.username;
    }

    private void OnEnable()
    {
        welcomeText.text = "WELCOME\n" + GlobalVar.username;
    }
    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;

        if (counter >= 3)
        {
            signInScript.GoToMainScene();
            counter = -5;
            Destroy(gameObject);
        }
    }
}
