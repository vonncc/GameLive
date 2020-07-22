using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MessageBoxScript : MonoBehaviour
{
    public GameObject messageBoxBody;

    public Text titleText;
    public Text bodyText;

    public UnityEvent OnOkay;

    public delegate void OnClickOkay();

    OnClickOkay onClickOkay;

    void UpdatePanelActive(bool pActive)
    {
        messageBoxBody.SetActive(pActive);
    }
    public void Close()
    {
        if (OnOkay != null)
            OnOkay.Invoke();

        if (onClickOkay != null)
        {
            onClickOkay();
            onClickOkay = null;
        }

        UpdatePanelActive(false);
    }

    public void Open(string pTitle, string pBody)
    {
        titleText.text = pTitle.ToUpper();
        bodyText.text = pBody.ToUpper();
        UpdatePanelActive(true); 
    }

    public void Open(string pTitle, string pBody, OnClickOkay pOnClickOkay)
    {
        titleText.text = pTitle.ToUpper();
        bodyText.text = pBody.ToUpper();
        onClickOkay = pOnClickOkay;
        UpdatePanelActive(true);
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
