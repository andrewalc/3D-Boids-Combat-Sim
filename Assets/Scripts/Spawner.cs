using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
  public GameObject boidPrefab;
  public LayerMask allyLayer;
  public int spawnCount;
  public float spawnRadius;
  public bool respawn = false;

  // Start is called before the first frame update
  void Start()
  {
    for (var i = 0; i < spawnCount; i++) {
      Spawn();
    }
  }

  void Update()
  {
    if (!respawn)
    {
      return;
    }
    
    Collider[] boids = Physics.OverlapSphere(this.transform.position, 2000, allyLayer);
    if (boids.Length < spawnCount)
    {
      Spawn();
    }
  }

  public void Spawn()
  {
    Vector3 position = this.transform.position + (Random.insideUnitSphere * spawnRadius);
    var boid = Instantiate(boidPrefab, position, transform.rotation);
  }
}
