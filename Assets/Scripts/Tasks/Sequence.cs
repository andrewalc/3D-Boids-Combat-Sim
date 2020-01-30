using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
  public List<Task> tasks = new List<Task>();
  
  public Sequence(Blackboard bb) : base(bb){}
  
  public override bool execute() {
    foreach (Task task in tasks) {
      if (!task.execute()) {
        return false;
      }
    }
    return true;  
  } 
  
  public void AddTask(Task task) {
    this.tasks.Add(task);
  }
}
