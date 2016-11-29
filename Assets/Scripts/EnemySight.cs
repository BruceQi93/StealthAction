using UnityEngine;
using System.Collections;

public class EnemySight : MonoBehaviour {
    //能否看到玩家
    public bool playerInSight = false;
    //机器人视野的夹角
    public float fieldOfView = 110;
    //机器人视野的距离
    public float distanceOfView;
    //机器人的球形触发器
    private SphereCollider sph;
    //小机器人私有的警报位置
    public Vector3 personalAlarmPosition;
    //前一个警报位置
    private Vector3 PreviousAlarmPosition;
    //脚本组件，里面有触发警报的位置
    private LastPlayerSighting lastPlayerSighting;
    //射线检测
    private RaycastHit hit;
    //导航组件
    private NavMeshAgent nav;
    //玩家血量的脚本组件
    private PlayerHealth playerHealth;
    //动画控制器组件
    private Animator anim;

    void Awake()
    {
        //球形触发器组件
        sph = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();
        playerHealth = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();
        //小机器人的私有警报位置
        personalAlarmPosition = lastPlayerSighting.normalPosition;
        PreviousAlarmPosition = lastPlayerSighting.normalPosition;
        //得到视线距离，即为球形触发器的半径
        distanceOfView = sph.radius;
    }
	
    void Update()
    {
        //公共的警报位置发生改变，表示玩家触发了警报，
        //此时把该警报位置传递给小机器人的私有警报位置
        if (PreviousAlarmPosition!=lastPlayerSighting.alarmPosition)
        {
            personalAlarmPosition = lastPlayerSighting.alarmPosition;
        }
        PreviousAlarmPosition = lastPlayerSighting.alarmPosition;
        //如果玩家的血量大于0
        if (playerHealth.hp>0)
        {
            anim.SetBool(HashID.playerInSight, playerInSight);
        }else
        {
            anim.SetBool(HashID.playerInSight, false);
        }
    }
    /// <summary>
    /// 玩家在触发器内
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerStay(Collider other)
    {
        if (other.tag==Tags.Player)
        {
            playerInSight = false;
            float distance = Vector3.Distance(other.transform.position, transform.position);
            //玩家与机器人的方向向量
            Vector3 dir = other.transform.position - transform.position;
            //计算机器人正前方和该方向向量之间的夹角
            float angle = Vector3.Angle(transform.forward, dir);
            if (angle<fieldOfView/2)
            {
                //模型导入后其旋转还是按原始的对齐点进行即scene视图下的pivot
                if (Physics.Raycast(transform.position+Vector3.up*1.7f,dir,out hit))
                {
                    //射线碰撞到了玩家
                    if (hit.collider.tag==Tags.Player)
                    {
                        //机器人能够看到玩家
                        playerInSight = true;
                        //报警
                        personalAlarmPosition = other.transform.position;
                    }
                }
            }
            //听觉
            if (EnemyListening(other.transform.position))
            {
                //如果玩家发出声音
                if (other.GetComponent<AudioSource>().isPlaying)
                {
                    personalAlarmPosition = other.transform.position;
                }
            }
        }
    }
    //不在触发范围以内，表示看不到玩家
    void OnTriggerExit(Collider other)
    {
        if (other.tag==Tags.Player)
        {
            playerInSight = false;
        }
    }
    //检测小机器人是否可以听到玩家发出的声音
    bool EnemyListening(Vector3 playerPos)
    {
        //路径对象
        NavMeshPath path = new NavMeshPath();
        //如果导航可以到玩家位置
        if (nav.CalculatePath(playerPos,path))
        {
            //用数组获取所有路径上的点
            Vector3[] points = new Vector3[path.corners.Length + 2];
            //把起始点放到数组里(小机器人的位置)
            points[0] = transform.position;
            points[points.Length-1] = playerPos;
            //把路径上的拐点放到数组里
            for (int i = 1; i < points.Length-1; i++)
            {
                points[i] = path.corners[i - 1];
            }
            //导航距离
            float navDistance = 0;
            for (int i = 0; i < points.Length-1; i++)
            {
                navDistance += Vector3.Distance(points[i], points[i + 1]);
            }
            if (navDistance<distanceOfView)
            {
                return true;
            }
        }
        return false;
    }
}
