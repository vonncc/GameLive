using UnityEngine;

public class DisplayScoreText : MonoBehaviour
{

    UnityEngine.UI.Text mText;
    // Start is called before the first frame update
    void Start()
    {
        mText = GetComponent<UnityEngine.UI.Text>();
        mText.text = GlobalVar.UserCurrentScore.ToString();
    }

    private void OnEnable()
    {
        mText = GetComponent<UnityEngine.UI.Text>();
        mText.text = GlobalVar.UserCurrentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
