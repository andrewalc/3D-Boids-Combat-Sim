using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class NormalizeBoost : Task
{
    public NormalizeBoost(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();
        
        if (this.bb.GetBoolean("Boosted"))
        {
            agent.velocity = Mathf.Lerp(agent.velocity, agent.oVelocity,  Time.deltaTime);
            agent.slerpConstant = Mathf.Lerp(agent.slerpConstant, agent.oSlerp, .3f* Time.deltaTime);
            if (Math.Abs(agent.velocity - agent.oVelocity) < 1 && Math.Abs(agent.slerpConstant - agent.oSlerp) < 1)
            {
                this.bb.PutBoolean("Boosted", false);
            }
        }

        return true;
    } 
}
