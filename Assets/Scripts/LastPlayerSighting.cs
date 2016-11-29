using UnityEngine;
using System.Collections;

public class LastPlayerSighting : MonoBehaviour {
    //报警位置
    public Vector3 alarmPosition = new Vector3(1000, 1000, 1000);
    //非报警位置
    public Vector3 normalPosition = new Vector3(1000, 1000, 1000);
    //声音切换的速度
    public float turnSpeed = 3f;
    //警报灯脚本
    private AlarmLight alarmLight;
    //主背景音乐
    private AudioSource mainAudio;
    //触发警报时的背景音乐
    private AudioSource panicAudio;
    //警报喇叭的声音(共有6个)
    private AudioSource[] alarmAudios;

    void Awake()
    {
        mainAudio = GetComponent<AudioSource>();
        panicAudio = transform.GetChild(0).GetComponent<AudioSource>();     
    }

    void Start()
    {
        //通过tag值找到所有喇叭的对象
        GameObject[] sirens = GameObject.FindGameObjectsWithTag(Tags.Siren);
        //初始化数组
        alarmAudios = new AudioSource[sirens.Length];
        //获取AlarmLight脚本组件
        alarmLight = GameObject.FindWithTag(Tags.AlarmLight).GetComponent<AlarmLight>();
        //获取所有喇叭对象中的audiosource组件
        for (int i = 0; i < sirens.Length; i++)
        {
            alarmAudios[i] = sirens[i].GetComponent<AudioSource>();
        }        
    }

    void Update()
    {
        //解除警报
        if (alarmPosition==normalPosition)
        {
            //关闭警报灯
            alarmLight.alarmOn = false;
            //逐个关闭警报喇叭的声音
            for (int i = 0; i < alarmAudios.Length; i++)
            {
                alarmAudios[i].volume = Mathf.Lerp(alarmAudios[i].volume, 0, Time.deltaTime * turnSpeed);
                if (alarmAudios[i].volume<0.05f)
                {
                    alarmAudios[i].volume = 0;
                }
            }
            //主背景音乐音量加大
            mainAudio.volume = Mathf.Lerp(mainAudio.volume, 1, Time.deltaTime * turnSpeed);
            if (mainAudio.volume>9.8f)
            {
                mainAudio.volume = 1;
            }
            //把触发警报后的背景音乐音量变为最小
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0, Time.deltaTime * turnSpeed);
            if (panicAudio.volume<0.05f)
            {
                panicAudio.volume = 0;
            }
        }
        //触发警报
        else
        {
            alarmLight.alarmOn = true;
            for (int i = 0; i < alarmAudios.Length; i++)
            {
                alarmAudios[i].volume = Mathf.Lerp(alarmAudios[i].volume, 1, Time.deltaTime * turnSpeed);
                if (alarmAudios[i].volume>0.98f)
                {
                    alarmAudios[i].volume = 1;
                }
            }
            mainAudio.volume = Mathf.Lerp(mainAudio.volume, 0, Time.deltaTime * turnSpeed);
            if (mainAudio.volume<0.05f)
            {
                mainAudio.volume = 0;
            }
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 1, Time.deltaTime * turnSpeed);
            if (panicAudio.volume>9.8f)
            {
                panicAudio.volume = 1;
            }
        }
    }
}
