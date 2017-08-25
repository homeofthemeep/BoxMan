using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class WalkingController : Controller {

    protected enum PlayerState { WAIT = 0, RUN = 1 , DASH = 2, JUMPSQUAT = 3, UPFALL = 4,
                                 DOWNFALL = 5, LAND_j =6, AIRDODGE = 6 }

    protected static PlayerState curState, prevState;

    protected static Vector3 walkVel;
    protected static Vector3 airDodgeVel;

    protected static int maxJumps;

    protected static float adjVertVel;
    protected static float curJump;
    protected static float curDash;
    protected static float jumpDelay;
    protected static float dashDelay;
    protected static float groundFriction;

    protected static bool dashQ;// = false;
    protected static bool stillMoving;// = false;
    protected static bool firstJump;// = false;
    protected static bool dashRight;// = true;
    protected static bool resetJump;
    protected static bool facingRight;

    Triangle ECB;
    Vector3[] ECBverts;

    public void Awake()
    {
        //dashWalk_class = new WC_Dashing();        

        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();

        ECBverts = new Vector3[3];
        ECBverts[0] = rb.position - new Vector3(0f, 0.5f, 0f);
        ECBverts[1] = rb.position - new Vector3(0.5f, 0f, 0f);
        ECBverts[2] = rb.position + new Vector3(0.5f, 0f, 0f);
        ECB = new Triangle(ECBverts[0], ECBverts[1], ECBverts[2]);

        //Integer numbers
        maxJumps = 2;
        // Float numbers
        adjVertVel = 0.0f;
        curJump = 0.0f;
        curDash = 0.0f;
        jumpDelay = 4f / 60f;
        dashDelay = 13f / 60f;
        groundFriction = 0.2f;
        //Booleans
        dashQ = false;
        stillMoving = false;
        firstJump = false;
        resetJump = false;
        dashRight = true;
        facingRight = true;
    }


    public override void ReadInput(ref InputData data)
    {

        ECBverts[0] = rb.position - new Vector3(0f, 0.5f, 0f);
        ECBverts[1] = rb.position - new Vector3(0.5f, 0f, 0f);
        ECBverts[2] = rb.position + new Vector3(0.5f, 0f, 0f);
        ECB.update(ECBverts[0], ECBverts[1], ECBverts[2]);

        adjVertVel = 0f;

        if ((data.axes[1] <= (1f / 20f)) && (data.axes[1] >= (-1f / 20f))) { dashQ = false;  }
        if ((data.axes[0] <= (1f / 3f)) && data.buttons[2] == false) { resetJump = true; }
        if (Grounded()) { maxJumps = 2; }

        //Debug.Log("resetJump: " + resetJump);
        //checkDashing(data);
        WC_Dashing.checkDashing(data);
        WC_Jumping.checkJump(data);
        WC_Airdodging.checkAirDodge(data);
        //Debug.Log("canDoubleJump[0 & 1]: { "+ candoubleJump[0] + " ," + candoubleJump[1]+ " }");
        applyGroundFriction(data);
    }

    void LateUpdate()
    {
        rb.velocity = new Vector3(walkVel.x, rb.velocity.y + adjVertVel, 0) + airDodgeVel;

        //walkVel = Vector3.zero;
        adjVertVel = 0f;
        /*
        if (!Grounded())
        {
            get
        }
        */
    //}

    
/*
    void applyGroundFriction(InputData data)
    {
        if (!stillMoving && walkVel != Vector3.zero )
        {
            if (dashRight)
            {
                walkVel -= Vector3.right * groundFriction;
                if (walkVel.x <= 0f) { walkVel = Vector3.zero; }
            }
            else if (!dashRight)
            {
                walkVel -= Vector3.left * groundFriction;
                if (walkVel.x >= 0f) { walkVel = Vector3.zero; }
            }
        }
    }

*/
    #region oldMethods
    /*void checkDashing(InputData data)
    {
        

        if (data.axes[1] != 0f && !dashQ  && Grounded())
        {
            if (data.axes[1] > 0)
                { dashRight = true;
                  walkVel = Vector3.right * 6; }

            else if (data.axes[1] < 0)
                {   dashRight = false;
                    walkVel = Vector3.left * 6; }
                        
            dashQ = true;
            stillMoving = true;
        }

        if (dashQ)
            { walkVel *= 1.02f;
              curDash += Time.deltaTime; }

        if (data.axes[1] != 0f) { stillMoving = true; }
        else                    { stillMoving = false; }
        
        

        if (curDash >= dashDelay)
        {
            //curDash = 0.0f;
            dashQ = false;
            if(stillMoving)
            {
                if (dashRight)
                    { walkVel = Vector3.right * 6; }
                else if (!dashRight)
                    { walkVel = Vector3.left * 6; }
            }
            else
            {
                curDash = 0.0f;
            }            
        }
        
    }
    */
    /*void checkJump(InputData data)
    {
        
        if (Grounded())
        {
            //maxJumps = 2;
            if (curJump >= jumpDelay)
            {
                adjVertVel += 10f;
                firstJump = false;
                //candoubleJump[0] = true;
                curJump = 0.0f;
            }
            if ((data.axes[0] >= (1f / 3f) || data.buttons[2] == true))
            {
                firstJump = true;
                resetJump = false;
                maxJumps--;
            }

            if (firstJump) { curJump += Time.deltaTime; }
        }
        else if (resetJump && !Grounded() && maxJumps >= 1) // Work on getting the reset for the jump
        {
            if ((data.axes[0] >= (1f / 3f) || data.buttons[2] == true))
            {
                resetJump = false;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                adjVertVel += 10f;
                maxJumps--;
            }
        }
    }*/
    #endregion
    
/*
    protected static bool Grounded()
    {        
        return Physics.Raycast
            (transform.position, Vector3.down, bounds.extents.y + 0.0001f);
    }
*/
//}

