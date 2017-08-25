using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Fencer_Dash : Fencer_Controller
{
    protected float walkDelay;
    protected float curDashTime;
    protected float groundFriction;
    protected bool isDashing;
    protected bool finishDash;
    protected bool stillMoving;
    protected bool goingRight;
    public Vector3 dashWalkVel;

    public Fencer_Dash(bool grounded_param, bool faceRight_param, Vector3 vel_param, PlayerState playerState_param)
    {
        playerState = playerState_param;
        grounded = grounded_param;
        faceRight = faceRight_param;
        vel = vel_param;

        walkDelay = (14f / 60f);
        curDashTime = 0.0f;
        groundFriction = 0.2f;
        isDashing = false;
        goingRight = false;
        finishDash = false;
        stillMoving = false;
        dashWalkVel = Vector3.zero;
    }

    public void checkDashing(ref InputData data)
    {

        if (!finishDash)
            finishDash = transferableStateDASH();

        if (finishDash && (data.axes[1] <= (1f / 20f)) && (data.axes[1] >= (-1f / 20f)))
            isDashing = false;

        #region DASH

        if (goingRight && isDashing && data.axes[1] < 0)
        {
            isDashing = true;
        }
        if (!goingRight && isDashing && data.axes[1] > 0)
        {
            isDashing = true;
        }

        if (data.axes[1] != 0f && !isDashing && grounded && transferableStateDASH())
        {
            if (data.axes[1] > 0)
            {
                //faceRight = true;
                goingRight = true;
                dashWalkVel = Vector3.right * 6;
                vel.x = dashWalkVel.x;
            }

            else if (data.axes[1] < 0)
            {
                //faceRight = false;
                goingRight = false;
                dashWalkVel = Vector3.left * 6;
                vel.x = dashWalkVel.x;
            }

            isDashing = true;
            finishDash = false;
            stillMoving = true;
        }

        
        if (isDashing)
        {
            if (goingRight)
                faceRight = true;
            else
                faceRight = false;

            playerState = PlayerState.DASH;
            dashWalkVel *= 1.02f;
            curDashTime += Time.fixedDeltaTime;
            vel.x = dashWalkVel.x;
        }
        #endregion

        #region DASHBRAKE
        UnityEngine.Debug.Log("analogX&Y: (" + data.axes[1] + ", " + data.axes[0] + ").");
        if (data.axes[1] != 0f) { stillMoving = true; }
        else
        {
            stillMoving = false;
            if(playerState == PlayerState.DASH)
                playerState = PlayerState.DASHBRAKE;
            if (playerState == PlayerState.RUN)
                playerState = PlayerState.RUNBRAKE;
        }
        #endregion

        #region RUN & RUNBRAKE
        if (curDashTime >= walkDelay)
        {
            //curDash = 0.0f;
            isDashing = false;
            //finishDash = true;
            if (stillMoving)
            {
                if (faceRight)
                { dashWalkVel = Vector3.right * 6; }
                else if (!faceRight)
                { dashWalkVel = Vector3.left * 6; }

                vel.x = dashWalkVel.x;
                playerState = PlayerState.RUN;             
            }
            else
            {
                if (playerState != PlayerState.DASHBRAKE)
                    playerState = PlayerState.RUNBRAKE; 
                curDashTime = 0.0f;
            }
        }
        #endregion

    }

    public void applyGroundFriction()
    {

        if (!stillMoving && dashWalkVel != Vector3.zero)
        {
            if (faceRight)
             {
                dashWalkVel -= Vector3.right * groundFriction;
                if (dashWalkVel.x <= 0f) { dashWalkVel = Vector3.zero; }
                vel.x = dashWalkVel.x;
                if (dashWalkVel == Vector3.zero)
                    playerState = PlayerState.WAIT;
             }   
             else if (!faceRight)
             {
                dashWalkVel -= Vector3.left * groundFriction;
                if (dashWalkVel.x >= 0f) { dashWalkVel = Vector3.zero; }
                vel.x = dashWalkVel.x;
                if (dashWalkVel == Vector3.zero)
                    playerState = PlayerState.WAIT;
             }
         }
    }

    private bool transferableStateDASH()
    {
        switch (playerState)
        {
            case PlayerState.RUNBRAKE:
            case PlayerState.DASHBRAKE:
            case PlayerState.JUMPSQUAT: 
            case PlayerState.UPFALL:
            case PlayerState.LAND_J:
            case PlayerState.AIRDODGE: return false; break;
            default:  return true; break;
        }

    }

    
}