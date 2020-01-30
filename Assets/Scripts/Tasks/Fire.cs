using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Fire : Task
{
    public Fire(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");                
        // If no marked target, fail
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();

        float firecd = bb.GetFloat("FireCooldown");
        if (firecd <= 0.0f)
        {
            agent.FireBullet();
            bb.PutFloat("FireCooldown", 1.0f);
            return true;
        }

        bb.PutFloat("FireCooldown", firecd - Time.deltaTime);
        return false;
    } 
}
