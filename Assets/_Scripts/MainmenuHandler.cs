using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.SceneManagement;
#endif

public class MainmenuHandler : MonoBehaviour
{

    public GameObject chatBox;
    public GameObject listeningMode;

    bool dontDetectAnymore = false;
    // Start is called before the first frame update
    void Start()
    {
        GlobalVar.canAnswer = false;
        GlobalVar.correctAnswer = -1;

        chatBox.SetActive(GlobalVar.withChat);
        listeningMode.SetActive(!GlobalVar.withChat);

        if (GlobalVar.withChat == false)
        {
            dontDetectAnymore = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dontDetectAnymore == false)
        {
            if (GlobalVar.withChat == true)
            {
                dontDetectAnymore = false;
            }
            else
            {
                dontDetectAnymore = true;
            }
            chatBox.SetActive(GlobalVar.withChat);
            listeningMode.SetActive(!GlobalVar.withChat);
        }

#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.A))
        {
            //SceneManager.LoadScene("Scenario_2");
        }
#endif

    }
}
