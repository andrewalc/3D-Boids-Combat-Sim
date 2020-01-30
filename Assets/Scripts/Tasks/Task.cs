using UnityEngine; 
using System.Collections; 
 
public abstract class Task
{
  protected Blackboard bb;

  public Task(Blackboard bb)
  {
    this.bb = bb;
  }

  public abstract bool execute(); 
  
}