using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class Controller : MonoBehaviour {

    public enum PlayerState
    {
        WAIT = 0,
        RUN = 100,
        DASH = 110,
        JUMPSQUAT = 300,
        UPFALL = 310,
        FREEFALL = 320,
        DOWNFALL = 330,
        SPECIALFALL = 340,
        ATTACK_JAB1 = 400,
        ATTACK_FTILT = 410,
        LAND_J = 600,
        LAND_AD = 610,
        AIRDODGE = 7,
        DASHBRAKE = 111,
        RUNBRAKE = 101
    }

    public abstract void ReadInput(ref InputData data);

    public PlayerState playerState, prevSate;
    protected int PlayerStateSize = 7;
    protected Rigidbody rb; 
    protected Collider coll;
    protected Animator anim;
    protected Transform trsfrm;



    protected void Awake()
    {
        playerState = 0;
        rb = GetComponent<Rigidbody>();
        //coll = GetComponent<Collider>();
        coll = rb.transform.Find("ECB/ECB Bone/ECB Object").GetComponent<Collider>();
        anim = GetComponent<Animator>();
        trsfrm = GetComponent<Transform>();
    }

}
