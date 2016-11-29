using UnityEngine;
using System.Collections;

public class DoorAnimation : MonoBehaviour {
    //打开门时是否需要钥匙
    public bool needKey = false;
    //拒绝开门的声音片段
    public AudioClip refuseClip;
    //玩家是否进入触发范围之内
    private bool playerIn = false;
    //进入触发器内的人数
    private int count = 0;
    private Animator anim;
    private AudioSource au;
    //得到玩家脚本里的hasKey,用来判断玩家是否有钥匙
    private PlayerMove playerMove;
    
    void Awake()
    {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();      
    }

    void Start()
    {
        playerMove = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerMove>();
    }
    //触发检测
    void OnTriggerEnter(Collider other)
    {
        //如果进入触发器的是玩家或小机器人
        if (other.tag==Tags.Player || (other.tag==Tags.Enemy&&other.GetType()==typeof(CapsuleCollider)))
        {
            //进入触发范围的人数加1
            count++;
            //如果是玩家
            if (other.tag==Tags.Player)
            {
                playerIn = true;
                //如果需要钥匙而玩家没有钥匙
                if (needKey&&!playerMove.hasKey)
                {
                    //在门的位置播放拒绝开门的声音
                    AudioSource.PlayClipAtPoint(refuseClip, transform.position);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag==Tags.Player || (other.tag == Tags.Enemy && other.GetType() == typeof(CapsuleCollider)))
        {
            count--;
            //如果是玩家
            if (other.tag==Tags.Player)
            {
                playerIn = false;
            }
        }
    }

    void Update()
    {
        //如果不需要钥匙
        if (!needKey)
        {
            //表示有人在触发范围之内
            if (count>0)
            {
                //把门打开
                anim.SetBool(HashID.doorOpen, true);
            }else
            {
                //把门关上
                anim.SetBool(HashID.doorOpen, false);
            }
        }else
        {
            //如果玩家有钥匙
            if (playerIn&&playerMove.hasKey)
            {
                anim.SetBool(HashID.doorOpen, true);
            }else
            {
                anim.SetBool(HashID.doorOpen, false);
            }
        }
    }
    //动画帧事件
    public void PlayVoice()
    {
        if (Time.time>0.1f)
        {
            au.Play();
        }
    }
}
