using UnityEngine;
using System.Collections;

public class EnemyAnimation : MonoBehaviour {
    //死角的度数（当机器人与玩家之间的夹角非常小时就不会再转动）
    public float deadZone = 4;
    //动画参数的延迟时间
    public float dampSpeedTime = 0.1f;
    public float dampAngularSpeedTime = 0.1f;
    //导航组件
    private NavMeshAgent nav;
    //脚本组件
    private EnemySight enemySight;
    //玩家
    private Transform player;
    //动画控制器组件
    private Animator anim;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        //把角度转成弧度
        deadZone *= Mathf.Deg2Rad;
        player = GameObject.FindWithTag(Tags.Player).transform;
        enemySight = GetComponent<EnemySight>();
    }

   //回调方法，当有动画执行时
   void OnAnimatorMove()
    {
        //计算每一帧导航的速度
        nav.velocity = anim.deltaPosition / Time.deltaTime;
        //通过动画的根动作的旋转使机器人进行旋转
        transform.rotation = anim.rootRotation;
    }

    void Update()
    {
        //速度和角速度
        float speed = 0;
        float angularSpeed = 0;
        //如果看到了玩家
        if (enemySight.playerInSight)
        {
            speed = 0;
            //角速度赋值，用来进行旋转
            angularSpeed = FindAngle();
            //如果旋转的角度小于死角角度
            if (Mathf.Abs(angularSpeed)<deadZone)
            {
                //置为0，即不再发生旋转
                angularSpeed = 0;
                //小机器人看向玩家
                transform.LookAt(player);
            }
        }
        else
        {
            //机器人的正前方与期望速度之间的投影，投影的值越小，表示小机器人向前的速度越小
            speed = Vector3.Project(nav.desiredVelocity, transform.forward).magnitude;
            angularSpeed = FindAngle();
        }
        //设置动画参数
        anim.SetFloat(HashID.speed, speed, dampSpeedTime, Time.deltaTime);
        anim.SetFloat(HashID.angularSpeed, angularSpeed, dampAngularSpeedTime, Time.deltaTime);
    }
    //得到旋转的角度
    float FindAngle()
    {
        //期望速度
        Vector3 dir = nav.desiredVelocity;
        //期望速度和小机器人正前方之间的夹角
        float angle = Vector3.Angle(dir, transform.forward);
        //小机器人的正前方和dir之间的法向量，用来判定小机器人旋转角度和方向
        Vector3 normal = Vector3.Cross(transform.forward, dir);
        if (normal.y<0)
        {
            angle = -angle;
        }
        //把角度转换为弧度
        angle *= Mathf.Deg2Rad;
        //如果期望速度为0
        if (dir==Vector3.zero)
        {
            angle = 0;
        }
        return angle;
    }
}
