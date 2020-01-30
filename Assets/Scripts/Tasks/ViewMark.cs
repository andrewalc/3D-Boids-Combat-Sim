using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ViewMark : Task
{
    public ViewMark(Blackboard bb) : base(bb){}
    public override bool execute()
    {
        bool scared = this.bb.GetBoolean("Scared");
        if (scared)
        {
            this.bb.PutGameObject("Marked", null);
            return false;
        }
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
//            Vector3 diff = agent.transform.position - boid.transform.position;
            Vector3 sightOrigin = agent.transform.position + agent.transform.forward * 2;
            Vector3 diff = boid.transform.position - sightOrigin; 

            RaycastHit hit;
            if (Vector3.Angle(diff, agent.transform.forward) < 90 && Physics.Raycast (sightOrigin, diff, out hit, agent.markingRange))
            { 
                    if (hit.collider.gameObject == boid.gameObject) {
//                        Debug.Log("Can see player");
                        float diffLen = diff.magnitude;
                        if (diffLen < closestDist) {
                            closest = boid;
                            closestDist = diffLen;
                        }
                    }else{
                        //Debug.Log("Can not see player");
                    }
                
            }

        }

        if (closest != null)
        {
//            GameObject mObj = this.bb.get("Marked");
//            if (mObj == null) {
//                this.bb.put("Marked", closest.gameObject);
//            }
            this.bb.PutGameObject("Marked", closest.gameObject);

            return true;
        }
        
        this.bb.PutGameObject("Marked", null);
//        agent.velocity = agent.oVelocity;
//        agent.slerpConstant = agent.oSlerp;
        return false;

    } 
}
