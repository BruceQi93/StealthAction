using UnityEngine;
using System.Collections;

public class LaserController : MonoBehaviour {
    //激光门是否闪烁
    public bool isBlinking = false;
    //闪烁的时间间隔
    public float blinkDeltaTime = 3;
    //计时器
    private float timer = 0;
    //GameController上的脚本
    private LastPlayerSighting lastPlayerSighting;

    void Start()
    {
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();
    }
	
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= blinkDeltaTime && isBlinking)
        {
            timer = 0;
            //控制激光门的隐藏
            GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;
            GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
            GetComponent<AudioSource>().enabled = !GetComponent<AudioSource>().enabled;
        }
    }

    //触发检测
    void OnTriggerEnter(Collider other)
    {
        if (other.tag==Tags.Player)
        {
            //设置警报位置
            lastPlayerSighting.alarmPosition = other.transform.position;
        }
    }
}
