using UnityEngine;
using System.Collections;
using System;
/// <summary>
/// 实现小机器人的巡逻和追捕玩家的功能
/// </summary>
public class EnemyAI : MonoBehaviour {
    //小机器人的巡逻点
    public Transform[] wayPoints;
    //巡逻速度
    public float patrallintSpeed = 2.5f;
    //追捕速度
    public float chasingSpeed = 6;
    //巡逻等待的时间
    public float chasingWaitTime = 3;
    //追捕等待的计时器
    private float chasingTimer = 0;
    //巡逻等待的计时器
    private float patrallingTimer = 0;
    //用到的脚本组件
    private EnemySight enemySight;
    private LastPlayerSighting lastPlayerSighting;
    private PlayerHealth playerHealth;
    //导航组件
    private NavMeshAgent nav;
    //巡逻目标的索引值
    private int index = 0;

    void Awake()
    {
        enemySight = GetComponent<EnemySight>();
        nav = GetComponent<NavMeshAgent>();
    }
	
    void Start()
    {
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();
        playerHealth = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();
    }

    void Update()
    {
        //如果机器人看到了玩家，并且玩家血量大于0
        if (enemySight.playerInSight&&playerHealth.hp>0)
        {
            //Shooting();
        }
        //如果机器人接收到了报警位置，并且玩家血量大于0
        else if (enemySight.personalAlarmPosition!=lastPlayerSighting.normalPosition&&playerHealth.hp>0)
        {
            Chasing();
        }else
        {
            //巡逻
            Patralling();
        }
    }

    //巡逻
    private void Patralling()
    {
        //巡逻速度赋值给导航速度
        nav.speed = patrallintSpeed;
        //设置巡逻目标
        nav.SetDestination(wayPoints[index].position);
        if (nav.remainingDistance-nav.stoppingDistance<0.5f)
        {
            //计时器开始计时
            patrallingTimer += Time.deltaTime;
            if (patrallingTimer>chasingWaitTime)
            {
                index++;
                index %= wayPoints.Length;
                patrallingTimer = 0;
            }
        }else
        {
            patrallingTimer = 0;
        }
    }

    //追捕
    private void Chasing()
    {
        //恢复导航
        nav.Resume();
        //追捕的速度赋值给导航速度，即速度加快
        nav.speed = chasingSpeed;
        //设置导航目标
        nav.SetDestination(enemySight.personalAlarmPosition);
        //如果到达了警报位置
        if (nav.remainingDistance-nav.stoppingDistance<0.5f)
        {
            //追捕计时器开始计时
            chasingTimer += Time.deltaTime;
            if (chasingTimer>chasingWaitTime)
            {
                chasingTimer = 0;
                //警报解除
                lastPlayerSighting.alarmPosition = lastPlayerSighting.normalPosition;
            }
        }else
        {
            chasingTimer = 0;
        }
    }

    //射击
    private void Shooting()
    {
        nav.Stop();
    }
}
