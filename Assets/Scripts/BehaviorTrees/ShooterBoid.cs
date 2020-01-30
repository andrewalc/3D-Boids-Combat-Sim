using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBoid : MonoBehaviour
{
    public Blackboard bb;

    private Task bt;
    
    // Start is called before the first frame update
    void Start()
    {
        Sequence root = new Sequence(bb);
            Selector targetSel = new Selector(bb);
                Help help = new Help(bb);
                AllMark mark = new AllMark(bb);
            Sequence shootSeq = new Sequence(bb);
                KinematicAim aim = new KinematicAim(bb);
                WithinFortyDist wtd = new WithinFortyDist(bb);
                Fire fire = new Fire(bb);

        shootSeq.AddTask(aim);
        shootSeq.AddTask(wtd);
        shootSeq.AddTask(fire);
       
        targetSel.AddTask(help);
        targetSel.AddTask(mark);
        
        root.AddTask(targetSel);
        root.AddTask(shootSeq);

        this.bt = root;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bt.execute();
    }
}
