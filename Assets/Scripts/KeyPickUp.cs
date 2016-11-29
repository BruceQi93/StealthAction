using UnityEngine;
using System.Collections;

public class KeyPickUp : MonoBehaviour {
    public AudioClip au_pick;
    private PlayerMove playMove;

    void Start()
    {
        playMove = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerMove>();
    }
	
    void OnTriggerEnter(Collider other)
    {
        if (other.tag==Tags.Player)
        {
            playMove.hasKey = true;
            //播放声音
            AudioSource.PlayClipAtPoint(au_pick, transform.position);
            //销毁钥匙
            Destroy(gameObject);
        }
    }
}
