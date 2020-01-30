using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class WithinFortyDist : Task
{
    public WithinFortyDist(Blackboard bb) : base(bb){}
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
        Vector3 diff = marked.transform.position - agent.transform.position;
        return diff.magnitude < 40;
    } 
}
