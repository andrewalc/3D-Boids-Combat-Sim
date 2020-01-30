using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassicBoid : MonoBehaviour
{
    public Blackboard bb;

    private Task bt;
    
    // Start is called before the first frame update
    void Start()
    {
        Sequence root = new Sequence(bb);
            Selector targetSel = new Selector(bb);
                Help help = new Help(bb);
                ViewMark mark = new ViewMark(bb);
            NoDanger noDanger = new NoDanger(bb);
            Sequence actionSeq = new Sequence(bb);
                Selector ramSel = new Selector(bb);
                    Sequence ramSeq = new Sequence(bb);
                        LineOfSight los = new LineOfSight(bb);
                        Boost boost = new Boost(bb);
                    NormalizeBoost norm = new NormalizeBoost(bb);
                Chase chase = new Chase(bb);

        ramSeq.AddTask(los);
        ramSeq.AddTask(boost);
      
        ramSel.AddTask(ramSeq);
        ramSel.AddTask(norm);
        
        actionSeq.AddTask(ramSel);
        actionSeq.AddTask(chase);
       
        targetSel.AddTask(help);
        targetSel.AddTask(mark);
        
        root.AddTask(targetSel);
        root.AddTask(noDanger);
        root.AddTask(actionSeq);

        this.bt = root;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bt.execute();
    }
}
