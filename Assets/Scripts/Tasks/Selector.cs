using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Task
{
    public List<Task> tasks = new List<Task>();
  
    public Selector(Blackboard bb) : base(bb){}
  
    public override bool execute() {
        foreach (Task task in tasks) {
            if (task.execute()) {
                return true;
            }
        }
        return false;
    } 
  
    public void AddTask(Task task) {
        this.tasks.Add(task);
    }
}
