using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fencer_GroundAttacks : Fencer_Controller {

    protected float curJabTime;
    protected float endJabDelay;
    protected float[] activeJab;

    bool isJab;

    public Fencer_GroundAttacks(bool grounded_param, bool faceRight_param, Vector3 vel_param, PlayerState playerState_param)
    {
        playerState = playerState_param;
        grounded = grounded_param;
        faceRight = faceRight_param;
        vel = vel_param;

        isJab = false;
        curJabTime = 0.0f;
        endJabDelay = 22f / 60f;
        activeJab = new float[2];
        activeJab[0] = 3f / 60f;
        activeJab[1] = 5f / 60f;
    }

    public void checkJab(ref InputData data)
    {
        if (grounded && !isJab && data.buttons[0] && transferableStateATTACK_JAB1())
        {
            isJab = true;
        }

        if (isJab && playerState == PlayerState.ATTACK_JAB1)
        {
            playerState = PlayerState.ATTACK_JAB1;
            curJabTime += Time.fixedDeltaTime;
        }

        if (curJabTime >= endJabDelay)
        {
            playerState = PlayerState.WAIT;
            curJabTime = 0.0f;
        }

        if (curJabTime >= activeJab[0] && curJabTime <= activeJab[1])
        {

        }
    }

    private bool transferableStateATTACK_JAB1()
    {
        switch (playerState)
        {
            //case PlayerState.ATTACK_JAB1:
            case PlayerState.WAIT: return true; break;
            default: return false;

        }

    }

}
