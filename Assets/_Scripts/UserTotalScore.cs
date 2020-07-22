using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserTotalScore : MonoBehaviour
{
    UnityEngine.UI.Text mText;
    // Start is called before the first frame update

    private void Awake()
    {
        mText = GetComponent<UnityEngine.UI.Text>();
    }
    void Start()
    {

        mText.text = GlobalVar.UserCurrentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
