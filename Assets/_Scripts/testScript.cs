using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testScript : MonoBehaviour
{
    public GameObject ttx;
    public ScrollRect scrollRect;
    public Transform sometih;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void OnClick()
    {
        GameObject clone = Instantiate(ttx, sometih);
        scrollRect.verticalNormalizedPosition = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
