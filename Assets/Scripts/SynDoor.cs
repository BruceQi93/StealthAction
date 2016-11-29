using UnityEngine;
using System.Collections;

public class SynDoor : MonoBehaviour {
    public Transform outterLeft;
    public Transform outterRight;
    private Transform innerLeft;
    private Transform innerRight;

    void Awake()
    {
        innerLeft = transform.GetChild(0);
        innerRight = transform.GetChild(1);
    }

    void Update()
    {
        //里面左右的门同步外面左右门
        innerLeft.localPosition = new Vector3(innerLeft.localPosition.x, innerLeft.localPosition.y, outterLeft.localPosition.z);
        innerRight.localPosition = new Vector3(innerRight.localPosition.x, innerRight.localPosition.y, outterRight.localPosition.z);
    }	
}
