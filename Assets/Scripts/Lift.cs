using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Lift : MonoBehaviour {
    float timer = 0;
    //玩家站在电梯里面后几秒钟开始上升
    public float waitTime = 2;
    //电梯上升时间
    public float moveTime = 3;
    //电梯上升速度
    public float moveSpeed = 3;
    //玩家
    Transform player;
    //声源组件
    AudioSource liftRaiseAu;
    //游戏结束的声源组件
    AudioSource endGameAu;

    void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).transform;
        liftRaiseAu = GetComponent<AudioSource>();
        endGameAu = transform.parent.GetComponent<AudioSource>();
    }
	
    void OnTriggerStay(Collider other)
    {
        if (other.tag==Tags.Player)
        {
            timer += Time.deltaTime;
            if (timer>waitTime)
            {
               //如果电梯没有播放声音
                if (!liftRaiseAu.isPlaying)
                {
                    //播放电梯上升的声音
                    liftRaiseAu.Play();
                }
                //播放游戏结束的声音
                if (!endGameAu.isPlaying)
                {
                    endGameAu.Play();
                }
                //电梯开始上升
                transform.root.position += Vector3.up * Time.deltaTime * moveSpeed;
                player.position += Vector3.up * Time.deltaTime * moveSpeed;
                if (timer>waitTime+moveTime)
                {
                    SceneManager.LoadScene(0);
                }
            }
        }
    }
}
