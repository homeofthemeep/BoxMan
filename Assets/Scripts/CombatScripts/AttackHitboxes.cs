using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitboxes : MonoBehaviour {

    // Use this for initialization
    Component[] hitboxes;

    private void Awake()
    {
        //hitboxes = GetComponents(typeof(Collider));
    }

    public void activateHitboxes()
    {
        for (int  i = 0; i < hitboxes.Length; i++)
        {
            //((Collider)hitboxes[i]) = new Component();
        }
    }
}
