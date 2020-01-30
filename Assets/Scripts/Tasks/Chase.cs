using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Chase : Task
{
    public Chase(Blackboard bb) : base(bb){}
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
        if (!this.bb.GetBoolean("Boosted"))
        {
            agent.velocity = Mathf.Lerp(agent.velocity, 0, 0.1f * Time.deltaTime);
//            agent.slerpConstant = Mathf.Lerp(agent.slerpConstant, 10, .3f* Time.deltaTime);
        } 
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(diff, agent.transform.up), agent.slerpConstant * 0.75f*  Time.deltaTime);
        return true;
    } 
}
