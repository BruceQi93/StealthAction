using UnityEngine;
using System.Collections;

public class HashID : MonoBehaviour {

    public static int speed = 0;
    public static int dead = 0;
    public static int sneak = 0;
    public static int turn = 0;
    public static int doorOpen = 0;
    public static int angularSpeed = 0;
    public static int playerInSight = 0;
    public static int aimWight = 0;
    public static int shot = 0;
    public static int LocomotionState = 0;

    void Awake()
    {
        speed = Animator.StringToHash("Speed");
        dead = Animator.StringToHash("Dead");
        sneak = Animator.StringToHash("Sneak");
        turn = Animator.StringToHash("Turn");
        doorOpen = Animator.StringToHash("DoorOpen");
        angularSpeed = Animator.StringToHash("AngularSpeed");
        playerInSight = Animator.StringToHash("PlayerInSight");
        aimWight = Animator.StringToHash("AimWight");
        shot = Animator.StringToHash("Shot");
        LocomotionState = Animator.StringToHash("Locomotion");
    }    	
}
