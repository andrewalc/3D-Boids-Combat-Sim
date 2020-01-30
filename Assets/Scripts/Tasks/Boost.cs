using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Boost : Task
{
    public Boost(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        bool boosted = this.bb.GetBoolean("Boosted");
        GameObject aObj = this.bb.GetGameObject("Agent");
        if (aObj == null)
        {
            return false;
        }

        if (!boosted)
        {
            Boid agent = aObj.GetComponent<Boid>();
            agent.velocity = Mathf.Lerp(agent.velocity, agent.oVelocity + 20,  Time.deltaTime * 3);
            agent.slerpConstant = 1;
            this.bb.PutBoolean("Boosted", true);
        }

        return true;
    } 
}
