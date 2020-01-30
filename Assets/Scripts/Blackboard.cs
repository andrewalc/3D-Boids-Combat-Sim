using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
  private Dictionary<string, GameObject> lookupGameObjects = new Dictionary<string, GameObject>();
  private Dictionary<string, bool> lookupBooleans = new Dictionary<string, bool>();
  private Dictionary<string, float> lookupFloats = new Dictionary<string, float>();

  private void Start()
  {
    
  }

  public GameObject GetGameObject(string key) {
    GameObject value = null;
    lookupGameObjects.TryGetValue(key, out value);
    return value;
  }

  public void PutGameObject(string key, GameObject val) {
    if (lookupGameObjects.ContainsKey(key))
    {
      lookupGameObjects.Remove(key);
    }
    
    lookupGameObjects.Add(key, val);
  }

  public bool GetBoolean(string key) {
    bool value = false;
    lookupBooleans.TryGetValue(key, out value);
    return value;
  }

  public void PutBoolean(string key, bool val) {
    if (lookupBooleans.ContainsKey(key))
    {
      lookupBooleans.Remove(key);
    }
    
    lookupBooleans.Add(key, val);
  }
  
  public float GetFloat(string key) {
    float value = 0.0f;
    lookupFloats.TryGetValue(key, out value);
    return value;
  }

  public void PutFloat(string key, float val) {
    if (lookupFloats.ContainsKey(key))
    {
      lookupFloats.Remove(key);
    }
    
    lookupFloats.Add(key, val);
  }

}
