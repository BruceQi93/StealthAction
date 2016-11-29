using UnityEngine;
using System.Collections;

public class LaserSwitchDeactivation : MonoBehaviour {
    //要控制的激光
    public GameObject controllerLaser;
    //解锁的材质
    public Material unlockMat;

	void OnTriggerStay(Collider other)
    {
        //当玩家按下Z键
        if (Input.GetKeyDown(KeyCode.Z)&&other.tag==Tags.Player&&controllerLaser.activeSelf)
        {
            //关闭激光
            controllerLaser.SetActive(false);
            //切换材质
            transform.GetChild(0).GetComponent<MeshRenderer>().material = unlockMat;
            //播放声音
            GetComponent<AudioSource>().Play();
        }
    }
}
