using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {
    public float hp = 100;
    //游戏结束的音乐片段
    public AudioClip gameEndClip;
    //玩家动画控制器组件
    private Animator anim;
    //玩家是否死亡
    private bool isDead = false;
    //游戏是否结束
    private bool isGameEnd = false;

    void Awake()
    {
        anim = GetComponent<Animator>();      
    }

    void Update()
    {
        //如果当前血量小于0且player没死
        if (hp <= 0 && isDead == false)
        {
            //播放玩家死亡的动画
            anim.SetTrigger(HashID.dead);
            isDead = true;
        }
        //player死了，但游戏还没结束
        else if (isDead&&isGameEnd==false)
        {
            //调用协程，游戏结束并转场景
            StartCoroutine(EndGame());
            isGameEnd = true;
        }
    }

	IEnumerator EndGame()
    {
        //在英雄所在的位置播放游戏结束的音乐片段
        AudioSource.PlayClipAtPoint(gameEndClip, transform.position);
        //等待四秒
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// player受攻击的方法
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        hp -= damage;
    }
}
