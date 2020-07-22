using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningBG : MonoBehaviour
{
    public float rotationSpeed = 1f;
    Transform mTransform;
    // Start is called before the first frame update
    void Start()
    {
        mTransform = GetComponent<Transform>();
        mTransform.localEulerAngles = new Vector3(0, 0, Random.Range(0,60));
    }

    // Update is called once per frame
    void Update()
    {
        mTransform.localEulerAngles += new Vector3(0, 0, rotationSpeed);
    }
}
