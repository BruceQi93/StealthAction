using UnityEngine;
using System.Collections;
using System;

public class CameraMove : MonoBehaviour {
    //摄像机移动的速度
    public float moveSpeed = 3;
    //摄像机旋转的速度
    public float turnSpeed = 10;
    //得到player
    private Transform player;
    //摄像机与player之间的方向向量
    private Vector3 dir;
    //射线碰撞信息
    private RaycastHit hit;
    //摄像机与玩家之间的距离
    private float distance;
    //摄像机的观察点
    private Vector3[] currentPoints;

    void Awake()
    {
        player = GameObject.FindWithTag(Tags.Player).transform;
        currentPoints = new Vector3[5];
    }

    void Start()
    {
        //游戏开始时 摄像机与玩家之间的距离
        distance = Vector3.Distance(transform.position, player.position);
        //游戏开始时玩家与摄像机之间的方向
        dir = player.position - transform.position;
    }

    void LateUpdate()
    {
        //摄像机观察的第一个点
        Vector3 startPoint = player.position - dir;
        //最后一个点
        Vector3 endPoint = player.position + Vector3.up * (distance-1);
        //第二个点
        currentPoints[1] = Vector3.Lerp(startPoint, endPoint, 0.25f);
        //第三个点
        currentPoints[2] = Vector3.Lerp(startPoint, endPoint, 0.5f);       
        //第四个点
        currentPoints[3] = Vector3.Lerp(startPoint, endPoint, 0.75f);
        //把第一个点和最后一个点放到数组里
        currentPoints[0] = startPoint;
        currentPoints[4] = endPoint;
        //定义一个变量来临时存储可以看到玩家的点
        Vector3 viewPosition = currentPoints[0];
        //遍历数组里的五个点
        for (int i = 0; i < currentPoints.Length; i++)
        {
            if (CheckView(currentPoints[i]))
            {
                viewPosition = currentPoints[i];
                break;
            }
        }
        //把摄像机移动到可以看到玩家的观察点上
        transform.position = Vector3.Lerp(transform.position, viewPosition, Time.deltaTime * moveSpeed);        
        //摄像机进行平滑旋转
        SmoothRotate();
    }
    /// <summary>
    /// 平滑旋转
    /// </summary>
    private void SmoothRotate()
    {
        //方向向量
        Vector3 dir = player.position - transform.position;
        //要旋转的角度
        Quaternion qua = Quaternion.LookRotation(dir);        
        //摄像机旋转到指定的角度
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * turnSpeed);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, 0);
    }

    /// <summary>
    /// 检测某个点是否可以看到玩家
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    bool CheckView(Vector3 pos)
    {
        //计算玩家与要观测的点之间的方向向量
        Vector3 dir = player.position - pos;
        //发射射线
        if (Physics.Raycast(pos,dir,out hit))
        {
            //如果射线碰撞到的事玩家，表示可以看到玩家
            if (hit.collider.tag == Tags.Player)
            {
                return true;
            }
        }
        return false;
    }	
}
