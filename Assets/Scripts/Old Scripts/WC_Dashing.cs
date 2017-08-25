using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class WC_Dashing : WalkingController {

    static int dashHash = Animator.StringToHash("Base Layer.Dashing");

    public static void checkDashing(InputData data)
    {
        PlayerState ps = 0;

        if (data.axes[1] != 0f && !dashQ && Grounded())
        {
            if (data.axes[1] > 0)
            {
                dashRight = true;
                walkVel = Vector3.right * 6;
            }

            else if (data.axes[1] < 0)
            {
                dashRight = false;
                walkVel = Vector3.left * 6;
            }

            dashQ = true;
            stillMoving = true;
        }

        if (dashQ)
        {
            walkVel *= 1.02f;
            curDash += Time.deltaTime;
            //anim.SetTrigger(dashHash);
            ps = (PlayerState)2;
        }

        if (data.axes[1] != 0f) { stillMoving = true; }
        else { stillMoving = false; }

        if (curDash >= dashDelay)
        {
            //curDash = 0.0f;
            dashQ = false;
            if (stillMoving)
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


        setPlayerState(ps);
    }

    private static void setPlayerState(PlayerState updateState)
    {
        curState = updateState;
    }
}
*/