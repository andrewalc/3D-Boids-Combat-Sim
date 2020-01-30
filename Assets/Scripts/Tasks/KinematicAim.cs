using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Refered to Predictive Aim Mathematics for AI Targeting by Kain Shin
// https://www.gamasutra.com/blogs/KainShin/20090515/83954/Predictive_Aim_Mathematics_for_AI_Targeting.php
public class KinematicAim : Task
{
    public KinematicAim(Blackboard bb) : base(bb){}

    public override bool execute()
    {
        GameObject aObj = this.bb.GetGameObject("Agent");
        GameObject mObj = this.bb.GetGameObject("Marked");
        
        // If no agent, fail
        if (aObj == null)
        {
            return false;
        }
        Boid agent = aObj.GetComponent<Boid>();
        
        // If no mark, fail
        if (mObj == null) {
            return false;
        }
        Boid marked = mObj.GetComponent<Boid>();

        // Start
        Vector3 origin = agent.transform.position;
        Vector3 targetPosition = marked.transform.position;
        Vector3 diff = origin - targetPosition;
        Vector3 targetVelocity = marked.velocity * marked.transform.forward;
        float diffLength = diff.magnitude;
        float targetSpeed = targetVelocity.magnitude;
        float projectileSpeed = Mathf.Max(20, agent.velocity * 2.0f);
        
        // Get the predictive angle
        float dotProduct = Vector3.Dot(diff.normalized, targetVelocity.normalized);
        float time;
        
        // Quadratic Formula, solve for time
        float a = (projectileSpeed * projectileSpeed) - (targetSpeed * targetSpeed);
        float b = 2.0f * diffLength * targetSpeed * dotProduct;
        float c = -(diffLength * diffLength);
        float discriminant = b * b - 4.0f * a * c;

        // Imaginary, return false
        if (discriminant < 0)
        {
            return false;
        }
        else
        {
            float time1 = 0.5f * (-b + Mathf.Sqrt(discriminant)) / a;
            float time2 = 0.5f * (-b - Mathf.Sqrt(discriminant)) / a;
            
            // Choose a significant time, soonest prefered. 
            time = Mathf.Min(time1, time2);
            if (time < Mathf.Epsilon)
            {
                time = Mathf.Max(time1, time2);
            }
            // Neither time is significant, fail
            if (time < Mathf.Epsilon)
            {
                return false;
            }
        }

        Vector3 resultAim = targetVelocity + (-diff / time);
        Vector3 gAccel = Vector3.down;
        Vector3 gravityCompensation = (0.5f * gAccel * time) * 8;
        resultAim -= gravityCompensation;
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(resultAim, Vector3.up), agent.slerpConstant * Time.deltaTime);
        return true;
    }
}
