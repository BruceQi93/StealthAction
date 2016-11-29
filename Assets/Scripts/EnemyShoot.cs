using UnityEngine;
using System.Collections;
using System;

public class EnemyShoot : MonoBehaviour {
    //枪的伤害值
    public float damage = 10;
    //动画控制器组件
    private Animator anim;
    //射击时的光线
    private Light shootLight;
    //线性渲染组件
    private LineRenderer lineRenderer;
    //是否真正射击
    bool isShooting = false;
    //玩家
    private Transform player;
    //玩家血量脚本
    private PlayerHealth playerHealth;

    void Awake()
    {
        anim = GetComponent<Animator>();
        shootLight = GetComponentInChildren<Light>();
        lineRenderer = GetComponentInChildren<LineRenderer>();
        player = GameObject.FindWithTag(Tags.Player).transform;
        playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        //开始射击
        if (anim.GetFloat(HashID.shot)>0.5f&&isShooting==false)
        {
            Shooting();
        }
        else if (anim.GetFloat(HashID.shot)<0.5f)
        {
            isShooting = false;
            shootLight.enabled = false;
            lineRenderer.enabled = false;
        }
    }

    private void Shooting()
    {
        //打开闪光灯
        shootLight.enabled = true;
        //绘制激光线
        lineRenderer.SetPosition(0, lineRenderer.transform.position);
        lineRenderer.SetPosition(1, player.position+Vector3.up*1.5f);
        lineRenderer.enabled = true;
        isShooting = true;
        //玩家受到伤害
        playerHealth.TakeDamage(damage);
    }

    void OnAnimatorIk(int layer)
    {
        //获取权重
        float weight = anim.GetFloat(HashID.aimWight);
        //设置IK权重
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        anim.SetIKPosition(AvatarIKGoal.RightHand, player.position + Vector3.up * 1.5f);
    }
}
