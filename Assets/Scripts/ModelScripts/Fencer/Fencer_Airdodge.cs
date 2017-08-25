using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Fencer_AirDodge : Fencer_Controller {

    protected float curAirDodgeTime;
    protected float undodgeTimeDelay;
    protected float airDodgeSpeed;
    protected float airDodgeFriction;
    protected float airDodgeFrictionCoefficient;
    protected int airDodgeIncrementalExponent;
    protected bool isAirDodge;

    protected Vector3 direction;

    public Fencer_AirDodge(bool grounded_param, bool faceRight_param, Vector3 vel_param, PlayerState playerState_param)
    {
        curAirDodgeTime = 0.0f;
        undodgeTimeDelay = (14f / 60f);
        airDodgeSpeed = 9f;
        airDodgeFriction = 0.2f;
        airDodgeFrictionCoefficient = 1.5f;
        airDodgeIncrementalExponent = 0;
        isAirDodge = false;
        direction = Vector3.zero;
    }

    public void checkAirdodge(ref InputData data)
    {
        #region SETUP AIRDODGNG WITH CHECK
        //Debug.Log("isairDodge: " + isAirDodge + " grounded: " + grounded );
        if (!grounded && !isAirDodge && (data.buttons[5] || data.buttons[6]) && transferableStateAIRDODGE())
        {
            isAirDodge = true;
            direction = new Vector3(data.axes[1], data.axes[0], 0);
            direction.Normalize();
            vel = Vector3.zero;
        }
        #endregion

        #region WHILE AND AFTER AIRDODGING
        if (isAirDodge )
        {
            curAirDodgeTime += Time.fixedDeltaTime;
            playerState = PlayerState.AIRDODGE;
                       
            vel = direction * airDodgeSpeed;
        }

        if (curAirDodgeTime >= undodgeTimeDelay)
        {
            isAirDodge = false;
            playerState = PlayerState.SPECIALFALL;
            //vel.x = 0f; vel.y = 0f;
        }
        #endregion

    }

    public void applyAirDodgeFriction()
    {
        airDodgeFrictionCoefficient++;
        if (playerState == PlayerState.AIRDODGE || playerState == PlayerState.LAND_AD)
        {
            airDodgeSpeed -= (airDodgeFriction* Mathf.Pow(airDodgeFrictionCoefficient, airDodgeIncrementalExponent));
            if (airDodgeSpeed <=  0f)
            {
                airDodgeSpeed = 0f;
            }
        }
        else
        {
            airDodgeIncrementalExponent = 0;
            airDodgeSpeed = 9f;
        }
    }

    private bool transferableStateAIRDODGE()
    {
        switch (playerState)
        {
            case PlayerState.UPFALL:
            case PlayerState.FREEFALL:
            case PlayerState.JUMPSQUAT: return true; break;
            default: return false; break;
        }
    }

}
