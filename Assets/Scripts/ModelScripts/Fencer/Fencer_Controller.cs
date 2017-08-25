using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Fencer_Controller : Controller
{
    
    protected bool grounded;
    protected bool faceRight;
    protected Vector3 vel;
    
    private Fencer_Dash groundMovement;
    private Fencer_Jump jumpMovement;
    private Fencer_GroundAttacks groundAttacks;
    private Fencer_AirDodge airDodgeHandler;

    public void Awake()
    {
        base.Awake();

        this.varAssignment();
 
        groundMovement = new Fencer_Dash(grounded, faceRight, vel, playerState);
        jumpMovement = new Fencer_Jump(grounded, faceRight, vel, playerState);
        groundAttacks = new Fencer_GroundAttacks(grounded, faceRight, vel, playerState);
        airDodgeHandler = new Fencer_AirDodge(grounded, faceRight, vel, playerState);
        //Time.timeScale = 0.5f;
    }

    private void varAssignment()
    {
        playerState = PlayerState.WAIT;
        faceRight = true;
        vel = new Vector3(0, 0, 0);
        grounded = Grounded();
    }

    public override void ReadInput(ref InputData data)
    {        
        //groundAttacks.setParentVars(grounded, faceRight, vel, playerState);
        //groundAttacks.checkJab(ref data);
        //this.getBackVars(groundAttacks);
        handleGroundMovement(ref data);
        handleJumpMovement(ref data);
        handleAirDodge(ref data);
        setGrounded();
    }

    public void LateUpdate()
    {
        
        if (vel.x <= 0.01f && vel.x >= -0.01f) 
        {
            vel.x = 0.0f;
        }
        //Debug.Log("vel.x: " + vel.x);
        rb.velocity = new Vector3(vel.x, vel.y, 0);
        
        handleAnimation();
        
    }

    private bool Grounded()
    {
        RaycastHit rcHit;
        
         Physics.Raycast
            (transform.position + new Vector3(0,0.5f,0), Vector3.down, out rcHit, 0.501f);
        
        /*
        if (rcHit.collider != null)
        {  return true; }
        else
        {  return false; }*/

        if (rcHit.collider != null)
        { trsfrm.position = new Vector3(transform.position.x, rcHit.point.y, 0);
            return true; }
        else
        { return false; }

        /*if (rcHit.collider != null)
        { Debug.Log(rcHit.collider); trsfrm.position = new Vector3(transform.position.x, rcHit.point.y, 0);
            return true; }
        else
        { Debug.Log(rcHit.collider); return false; }*/


    }

    private void setGrounded()
    {
        grounded = Grounded();
        if (grounded)
        {
            switch (playerState)
            {
                case PlayerState.SPECIALFALL: 
                case PlayerState.AIRDODGE: playerState = PlayerState.LAND_AD; break;
                case PlayerState.UPFALL:
                case PlayerState.FREEFALL: playerState = PlayerState.LAND_J; break;
                case PlayerState.LAND_J: playerState = PlayerState.WAIT; break;
            }
        }
        else
        {

        }

        //Debug.Log("grounded: " + grounded);
    }

    private void handleGroundMovement(ref InputData data)
    {
        groundMovement.setParentVars(grounded, faceRight, vel, playerState);
        groundMovement.checkDashing(ref data);
        groundMovement.applyGroundFriction();
        this.getBackVars(groundMovement);
    }

    private void handleJumpMovement(ref InputData data)
    {
        jumpMovement.setParentVars(grounded, faceRight, vel, playerState);
        jumpMovement.checkJump(ref data);
        jumpMovement.applyGravity();
        this.getBackVars(jumpMovement);
    }

    private void handleAirDodge(ref InputData data)
    {
        airDodgeHandler.setParentVars(grounded, faceRight, vel, playerState);
        airDodgeHandler.checkAirdodge(ref data);
        airDodgeHandler.applyAirDodgeFriction();
        this.getBackVars(airDodgeHandler);

    }

    private void handleAnimation()
    {
        Transform roots;
         
        if (prevSate != playerState)
        {
            switch (playerState)
            {
                case PlayerState.WAIT: anim.Play("Idle"); break;
                case PlayerState.RUN: anim.Play("Running"); break;
                case PlayerState.DASH: anim.Play("Dashing"); break;
                case PlayerState.JUMPSQUAT: anim.Play("Kneebend"); break;
                case PlayerState.UPFALL: anim.Play("Upfall"); break;
                case PlayerState.FREEFALL: anim.Play("Freefall"); break;
                case PlayerState.LAND_J: anim.Play("Crouching"); break;
                case PlayerState.ATTACK_JAB1: anim.Play("Jab"); break;
            }
        }

        prevSate = playerState;
        roots = rb.transform.Find("Armature");
        if (!faceRight)
        {
            if (roots != null)
                roots.localEulerAngles = new Vector3(roots.localEulerAngles.x, 180f, roots.localEulerAngles.z);
        }
        else
        {
            if (roots != null)
                roots.localEulerAngles = new Vector3(roots.localEulerAngles.x, 0f, roots.localEulerAngles.z);
        }
            
    }

    #region getBack methods
    protected static Vector3 getFaceRight(ref Vector3 vel)
    {
        return vel;
    }

    protected PlayerState giveBackPlayerState()
    {
        return playerState;
    }

    protected bool giveBackFaceRight()
    {
        return faceRight;
    }

    protected bool giveBackGrounded()
    {
        return grounded;
    }

    protected Vector3 giveBackVel()
    {
        return vel;
    }

    private  void getBackVars(System.Object e)
    {
        Fencer_Controller getter = (Fencer_Controller)e;
        this.playerState = getter.giveBackPlayerState();
        this.grounded = getter.giveBackGrounded();
        this.faceRight = getter.giveBackFaceRight();
        this.vel = getter.giveBackVel();
    }

    public void setParentVars(bool grounded_param, bool faceRight_param, Vector3 vel_param, PlayerState playerState_param)
    {
        playerState = playerState_param;
        grounded = grounded_param;
        faceRight = faceRight_param;
        vel = vel_param;
    }
    #endregion


}

