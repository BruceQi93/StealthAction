using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour {
    //是否开启警报灯
    public bool alarmOn = false;
    //警报灯光切换速度
    public float turnSpeed = 3;
    //灯光的最大强度
    public float highIntensity = 1.5f;
    //灯光的最小强度
    public float lowIntensity = 0;
    //灯光的目标强度
    private float targetIntensity = 0;
    //灯光组件
    private Light alarmLight;

    void Awake()
    {
        alarmLight = GetComponent<Light>();
        //初始值，把灯光的目标强度设置为最高强度
        targetIntensity = lowIntensity;
    }
	
	void Update () {
        //如果警报灯开启
        if (alarmOn)
        {
            //使用数学差值，不断得到新的灯光强度值
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, Time.deltaTime * turnSpeed);
            //如果当前灯光的强度与目标强度的差的绝对值小于0.05，说明灯光的强度已达到目标强度
            if (Mathf.Abs(targetIntensity-alarmLight.intensity)<=0.05f)
            {
                //如果当前目标强度的值是最大强度值，则将其改成最小值
                if (targetIntensity==highIntensity)
                {
                    targetIntensity = lowIntensity;
                }
                //否则改成最大值
                else
                {
                    targetIntensity = highIntensity;
                }
            }
        }
        else
        {
            //把警报灯当前灯光的强度慢慢变为0
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0, Time.deltaTime * turnSpeed);
            if (alarmLight.intensity<=0.05f)
            {
                alarmLight.intensity = 0;
            }
        }
	}
}
