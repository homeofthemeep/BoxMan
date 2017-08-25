using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class WC_Jumping : WalkingController  {

    public static void checkJump(InputData data)
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
    }

    private static void setPlayerState(PlayerState updateState)
    {
        curState = updateState;
    }
}
*/