using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class LineOfSight : Task
{
    public LineOfSight(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        GameObject mObj = this.bb.GetGameObject("Marked");
        
        // If no marked target, fail
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();
        if (mObj == null) {
            return false;
        }

        Boid marked = mObj.GetComponent<Boid>();
        Ray ray = new Ray (agent.transform.position, agent.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, agent.enemyRange, agent.enemyLayer))
        {
            if (hit.collider.gameObject.GetComponent<Boid>().GetInstanceID() == marked.GetInstanceID())
            {
                return true;
            }
        }
        
        return false;
    } 
}
