using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class AllMark : Task
{
    public AllMark(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        
        if (aObj == null)
        {
            return false;
        }

        Boid agent = aObj.GetComponent<Boid>();
        // Looks up nearby boids
        Collider[] nearbyEnemies = Physics.OverlapSphere(agent.transform.position, agent.markingRange, agent.enemyLayer);
        Collider closest = null;
        float closestDist = agent.markingRange;
        foreach (Collider boid in nearbyEnemies)
        {
            Vector3 diff = agent.transform.position - boid.transform.position;
            float diffLen = diff.magnitude;
            if (diffLen < closestDist) {
                closest = boid;
                closestDist = diffLen;
            }
        }

        if (closest != null)
        {
            this.bb.PutGameObject("Marked", closest.gameObject);

            return true;
        }
        
        this.bb.PutGameObject("Marked", null);
        return false;

    } 
}
