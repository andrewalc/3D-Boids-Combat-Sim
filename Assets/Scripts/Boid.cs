using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
  public LayerMask allyLayer;
  public LayerMask enemyLayer;
  public LayerMask wallLayer;
  [Range(0.0f, 20.0f)]
  public float allyRange = 4;    
  [Range(0.0f, 200.0f)]
  public float enemyRange = 20;
  [Range(0.0f, 200.0f)]
  public float markingRange = 50;
  [Range(0.0f, 20.0f)]
  public float wallRange = 10;
  [Range(0.0f, 25.0f)]
  public float alignmentConstant = 7.0f;
  [Range(0.0f, 20.0f)]
  public float cohesionConstant = 7.0f;
  [Range(0.0f, 20.0f)]
  public float separationConstant;
  [Range(0.0f, 50.0f)]
  public float wallSeparationConstant = 16.0f;
  [Range(0.1f, 10.0f)]
  public float slerpConstant = 3.0f;
  public float Minvelocity = 0.0f;
  public float Maxvelocity = 0.0f;
  public float velocity = 0.0f;
  public Blackboard blackboard;
  public Task behaviorTree;
  public GameObject bulletPrefab;
  public bool movingBoid = true;

  public float oVelocity;
  public float oSlerp;
  private void Start()
  {
    this.blackboard.PutGameObject("Agent", this.gameObject);
    if (movingBoid)
    {
      this.velocity = Random.Range(Minvelocity, Maxvelocity);
    }
    this.oVelocity = this.velocity;
    this.oSlerp = this.slerpConstant;
  }

  Vector3 GetAlignmentVector(Collider[] boids) {
      Vector3 alignment = Vector3.zero;

      // Accumulates the vectors.
      foreach (Collider boid in boids)
      {
        alignment += boid.transform.forward;
      }
      alignment /= boids.Length;
      return alignment.normalized * alignmentConstant;
    }

    Vector3 GetCohesionVector(Collider[] boids) {
      Vector3 center = Vector3.zero;

      // Accumulates the positions.
      foreach (Collider boid in boids)
      {
        center += boid.transform.position;
      }

      // Find center of boids
      center /= boids.Length;
      return (center - this.transform.position).normalized * cohesionConstant;
    }
    
    Vector3 GetBoidSeparationVector(Collider[] boids) {
      Vector3 result = Vector3.zero;

      // Accumulates the vectors.
      foreach (Collider boid in boids)
      {
        Vector3 diff = this.transform.position - boid.transform.position;
        float diffLen = diff.magnitude;
        float scaler = Mathf.Clamp(1.0f - diffLen / allyRange, 0.0f, 1.0f);
        result += diff * scaler;
      }
      result /= boids.Length;
      result = result.normalized * separationConstant;

      return result;
    }


    Vector3 GetWallSeparationVector() {
      Vector3 result = Vector3.zero;

      Ray ray = new Ray (transform.position, transform.forward);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, wallRange, wallLayer))
      {
          result += getVerticalWallSeparation();
          result += getHorizontalWallSeparation();
      }

      return result;
    }

    private Vector3 getVerticalWallSeparation() {
      Vector3 result = Vector3.zero;

      // Up Ray
      Ray ray = new Ray (transform.position, transform.up);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, wallRange, wallLayer))
      {
        float proportionalHeight = (wallRange - hit.distance) / wallRange;
        result += -transform.up * proportionalHeight * wallSeparationConstant;
      }
      
      // Down Ray
      ray = new Ray (transform.position, -transform.up);
      if (Physics.Raycast(ray, out hit, wallRange, wallLayer))
      {
        float proportionalHeight = (wallRange - hit.distance) / wallRange;
        result += transform.up * proportionalHeight * wallSeparationConstant;
      }
      return result;
    }

    private Vector3 getHorizontalWallSeparation() {
      Vector3 result = Vector3.zero;

      // Right Ray
      Ray ray = new Ray (transform.position, transform.right);
      RaycastHit hit;

      if (Physics.Raycast(ray, out hit, wallRange, wallLayer))
      {
        float proportionalHeight = (wallRange - hit.distance) / wallRange;
        result += -transform.right * proportionalHeight * wallSeparationConstant;
      }
      
      // Left Ray
      ray = new Ray (transform.position, -transform.right);
      if (Physics.Raycast(ray, out hit, wallRange, wallLayer))
      {
        float proportionalHeight = (wallRange - hit.distance) / wallRange;
        result += transform.right * proportionalHeight * wallSeparationConstant;
      }
      return result;
    }

  public void FireBullet()
  {
    Vector3 position = this.transform.position + this.transform.forward * 2;
    var bullet = Instantiate(bulletPrefab, position, transform.rotation);
    bullet.GetComponent<Rigidbody>().velocity = this.transform.forward * Mathf.Max(20, this.velocity * 2.0f);
    blackboard.PutGameObject("Bullet", bullet);
  }

    void FixedUpdate() {
      // Current position
      Vector3 currentPosition = transform.position;
      Quaternion currentRotation = transform.rotation;

      // Looks up nearby boids
      Collider[] nearbyBoids = Physics.OverlapSphere(currentPosition, allyRange, allyLayer);

      // Get behavior vectors
      Vector3 separation = this.GetBoidSeparationVector(nearbyBoids) + this.GetWallSeparationVector();
      Vector3 alignment = this.GetAlignmentVector(nearbyBoids);
      Vector3 cohesion = this.GetCohesionVector(nearbyBoids);

      // Calculates a rotation from the vectors
      Vector3 direction = separation + alignment + cohesion;
      Quaternion rotation = Quaternion.FromToRotation(transform.forward, direction.normalized);
      this.GetComponent<Rigidbody>().velocity = Vector3.zero;
      // Update our position and rotation with interpolation
      this.transform.position = currentPosition + transform.forward.normalized * (velocity * Time.deltaTime);
//      Vector3 targetPosition = currentPosition + transform.forward.normalized * velocity;
//      this.transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 0.5f);
      Quaternion targetRotation = rotation * transform.rotation;
      transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, slerpConstant * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
      Gizmos.color = Color.green;
      Vector3 direction = transform.forward * wallRange;
      Gizmos.DrawRay(transform.position, direction);
      GameObject mObj = this.blackboard.GetGameObject("Marked");
      if (mObj != null)
      {
        Boid marked = mObj.GetComponent<Boid>();
        Gizmos.color = Color.yellow;
        Vector3 markedDir = marked.transform.position - transform.position;
        Gizmos.DrawRay(transform.position, markedDir/2);
      }
//
//      Gizmos.color = Color.red;
//      Gizmos.DrawWireSphere(transform.position, allyRange);
//      Gizmos.color = Color.blue;
//      Gizmos.DrawWireSphere(transform.position, enemyRange);
    }
}