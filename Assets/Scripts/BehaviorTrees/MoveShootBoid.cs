﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveShootBoid : MonoBehaviour
{
    public Blackboard bb;

    private Task bt;
    
    // Start is called before the first frame update
    void Start()
    {
        Sequence root = new Sequence(bb);
            NoDanger noDanger = new NoDanger(bb);
            Selector targetSel = new Selector(bb);
                Help help = new Help(bb);
                ViewMark mark = new ViewMark(bb);
            Sequence actionSeq = new Sequence(bb);
                Selector ramSel = new Selector(bb);
                    Sequence ramSeq = new Sequence(bb);
                        LineOfSight los = new LineOfSight(bb);
                        Boost boost = new Boost(bb);
                    NormalizeBoost norm = new NormalizeBoost(bb);
                Selector shootOrChase = new Selector(bb);
                    Sequence shootSeq = new Sequence(bb);
                       WithinFortyDist wtd = new WithinFortyDist(bb);
                       KinematicAim aim = new KinematicAim(bb);
                       Fire fire = new Fire(bb);
                    Chase chase = new Chase(bb);
                

        shootSeq.AddTask(wtd);
        shootSeq.AddTask(aim);
        shootSeq.AddTask(fire);
        
        shootOrChase.AddTask(shootSeq);
        shootOrChase.AddTask(chase);
        
        ramSeq.AddTask(los);
        ramSeq.AddTask(boost);
      
        ramSel.AddTask(ramSeq);
        ramSel.AddTask(norm);
        
        actionSeq.AddTask(ramSel);
        actionSeq.AddTask(shootOrChase);
       
        targetSel.AddTask(help);
        targetSel.AddTask(mark);
        
        root.AddTask(noDanger);
        root.AddTask(targetSel);
        root.AddTask(actionSeq);

        this.bt = root;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bt.execute();
    }
}