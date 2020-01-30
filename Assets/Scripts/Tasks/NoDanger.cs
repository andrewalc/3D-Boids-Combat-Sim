using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class NoDanger : Task
{
    public NoDanger(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();

        Collider[] allies = Physics.OverlapSphere(agent.transform.position, agent.allyRange, agent.allyLayer);
        Collider[] enemies = Physics.OverlapSphere(agent.transform.position, agent.enemyRange, agent.enemyLayer);

        if (allies.Length < enemies.Length)
        {
            agent.velocity = Mathf.Lerp(agent.velocity, agent.oVelocity * 2.7f, Time.deltaTime * 0.3f);
//            agent.slerpConstant = Mathf.Lerp(agent.slerpConstant, agent.oSlerp, .3f* Time.deltaTime);

            this.bb.PutGameObject("Marked", null);
            this.bb.PutBoolean("Scared", true);
            return false;
        }
        else
        {
            agent.velocity = Mathf.Lerp(agent.velocity, agent.oVelocity, Time.deltaTime);
//            agent.slerpConstant = Mathf.Lerp(agent.slerpConstant, agent.oSlerp, .3f* Time.deltaTime);
            if (Math.Abs(agent.velocity - agent.oVelocity) < 1 && Math.Abs(agent.slerpConstant - agent.oSlerp) < 1)
            {
                this.bb.PutBoolean("Scared", false);
            }
            return true;
        }
    } 
}
