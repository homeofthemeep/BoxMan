using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fencer_Jump : Fencer_Controller
{
    protected float curJumpTime;
    protected float airTimeDelay;
    protected float grav;
    public float jumpVel;

    protected byte maxJumps;

    protected bool resetJumps;
    protected bool firstJump;


    public Fencer_Jump(bool grounded_param, bool faceRight_param, Vector3 vel_param, PlayerState playerState_param)
    {

        playerState = playerState_param;
        grounded = grounded_param;
        faceRight = faceRight_param;
        vel = vel_param;

        curJumpTime = 0.0f;
        airTimeDelay = (5f / 60f);
        jumpVel = 12f;
        maxJumps = 2;
        grav = 0.2f;
        firstJump = false;
    }
    

    public void checkJump(ref InputData data)
    {

        

        if ((data.axes[0] <= (1f / 3f)) && data.buttons[2] == false) { resetJumps = true; }

        #region UPFALL

        if (transferableStateUPFALL())
        {   //maxJumps = 2;
            if (curJumpTime >= airTimeDelay)
            {
                playerState = PlayerState.UPFALL;
                vel.y = jumpVel;
                //jumpVel = 12f;
                firstJump = false;
                curJumpTime = 0.0f;
            }
        }

        
        #endregion

        #region JUMPSQUAT
        if (grounded & transferableStateJUMPSQUAT())
        {           
            if ((data.axes[0] >= (1f / 3f) || data.buttons[2] == true))
            {
                firstJump = true;
                resetJumps = false;
                maxJumps--;
            }            
        }
        if (grounded && firstJump)
        {
            playerState = PlayerState.JUMPSQUAT;
            curJumpTime += Time.deltaTime;
        }
        #endregion

        #region FREEFALL
        if (!grounded && jumpVel <= 0f)
        {
            playerState = PlayerState.FREEFALL;
        }
        else if (!grounded && jumpVel > 0 && transferableStateUPFALL())
        {
            playerState = PlayerState.UPFALL;
        }
        #endregion

        #region Extrajumps
        else if (resetJumps && !grounded && maxJumps >= 1) // Work on getting the reset for the jump
        {
            if ((data.axes[0] >= (1f / 3f) || data.buttons[2] == true))
            {
                playerState = PlayerState.UPFALL;
                resetJumps = false;
                vel.y = jumpVel;
                //jumpVel = 12f;
                maxJumps--;
            }
        }
        #endregion
    }

    public void applyGravity()
    {
        /*
        RaycastHit rcHit;

        Physics.Raycast
           (trsfrm.position + new Vector3(0, 0.1f, 0), Vector3.down, out rcHit, 0.51f);
           */
        if (!grounded)
            vel.y -= grav;
        if (grounded && vel.y < 0f)
            vel.y = 0f;

        /*if (!grounded)
            jumpVel -= grav;
        if (grounded && jumpVel < 0f)
            jumpVel = 0f;*/

    }

    private bool transferableStateJUMPSQUAT()
    {
        switch (playerState)
        {
            case PlayerState.DASHBRAKE:
            case PlayerState.JUMPSQUAT:
            case PlayerState.UPFALL:
            case PlayerState.LAND_J:
            case PlayerState.AIRDODGE: return false; break;
            default: return true; break;
        }
    }

    private bool transferableStateUPFALL()
    {
        switch (playerState)
        {
            case PlayerState.DASHBRAKE:
            case PlayerState.UPFALL:
            case PlayerState.LAND_J:
            case PlayerState.AIRDODGE: return false; break;
            case PlayerState.JUMPSQUAT:
            default: return true; break;
        }
    }
}
