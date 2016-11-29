using UnityEngine;
using System.Collections;

public class CCTVPlayerDetection : MonoBehaviour {
    //GameController上的脚本组件，里面有警报位置
    private LastPlayerSighting lastPlayerSighting;

    void Start()
    {
        lastPlayerSighting = GameObject.FindWithTag(Tags.GameController).GetComponent<LastPlayerSighting>();
    }

    void OnTriggerEnter(Collider other)
    {
        //如果是玩家，同步报警位置
        if (other.tag==Tags.Player)
        {
            lastPlayerSighting.alarmPosition = other.transform.position;
        }
    }	
}
