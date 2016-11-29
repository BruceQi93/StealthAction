using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 该脚本加到playerMove上，功能：控制主角移动以及播放脚步声
/// </summary>
public class PlayerMove : MonoBehaviour {
    //玩家是否拿到钥匙
    public bool hasKey = false;
    //横纵轴的值
    private float hor, ver;
    //player转身的速度
    public float turnSpeed = 10;
    //是否潜行
    private bool isSneak = false;
    //动画控制组件
    private Animator anim;
    //声音组件
    private AudioSource au;
    //另外一个脚本组件，里面有血量
    private PlayerHealth playerHealth;

	void Awake () {
        anim = GetComponent<Animator>();
        au = GetComponent<AudioSource>();
        playerHealth = GetComponent<PlayerHealth>();
	}
	 
	void Update () {
        //如果血量大于0
        if (playerHealth.hp>0)
        {
            hor = Input.GetAxis("Horizontal");
            ver = Input.GetAxis("Vertical");
        }else
        {
            hor = 0;ver = 0;
        }
        //如果按下左shift键，潜行
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSneak = true;
        }else
        {
            isSneak = false;
        }
        //设置潜行动画过渡的条件
        anim.SetBool(HashID.sneak, isSneak);
        //设置player运动的过渡条件、
        if (hor!=0||ver!=0)
        {
            //设置参数
            anim.SetFloat(HashID.speed, 1, 0.1f, Time.deltaTime);
            //转身
            Turn(hor,ver);          
        }else
        {
            anim.SetFloat(HashID.speed, 0);
        }
        //播放脚步声
        FootStep(); 
    }
    
    //player转身的方法
    void Turn(float hor, float ver)
    {
        //得到一个向量
        Vector3 dir = new Vector3(hor, 0, ver);
        //要旋转的四元数
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * turnSpeed);
    }

    //播放脚步声的方法
    void FootStep()
    {
        //如果当前正在播放LocalMotion里面的动作片段时，说明player在移动
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("LocalMotion"))
        {
            //如果没有播放声音
            if (au.isPlaying==false)
            {
                au.Play();
            }
        }else
        {
            au.Stop();
        }
    }
}
