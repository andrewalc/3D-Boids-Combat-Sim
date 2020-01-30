using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Help : Task
{
    public Help(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        
        // If no marked target, fail
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();
        
        Collider[] allies = Physics.OverlapSphere(agent.transform.position, agent.allyRange, agent.allyLayer);
        
        // Check each ally for a mark
        foreach (Collider ally in allies)
        {
            var target = ally.gameObject.GetComponent<Blackboard>().GetGameObject("Marked");
            if (target != null && (target.transform.position - agent.transform.position).magnitude < agent.enemyRange * 1.4f)
            {
                this.bb.PutGameObject("Marked", target);
                return true;
            }
        }

        this.bb.PutGameObject("Marked", null);
        return false;
    } 
}
