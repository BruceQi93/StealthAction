using UnityEngine;
using System.Collections;

public class MinimapCam : MonoBehaviour {
    private Transform player;
    public float smoothing = 5;

    void Start()
    {
        player = GameObject.FindWithTag(Tags.Player).transform;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(player.position.x, 5, player.position.z),Time.deltaTime*smoothing);
    }
	
}
