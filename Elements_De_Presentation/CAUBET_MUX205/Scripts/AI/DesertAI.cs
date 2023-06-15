using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DesertAI : MonoBehaviour
{
  [Range(0.5f, 50)]
  public float detectDistance = 14;
  public Transform[] points;
  public float launchVelocity = 700f;
  public GameObject coconut;
  int destinationIndex = 0;
  NavMeshAgent agent;
  Transform player;
  float runSpeed = 2.0f;
  float defaultSpeed = 1.5f;
  bool canShoot = true;

  private void Start() 
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;

    agent = GetComponent<NavMeshAgent>();

    if(agent != null)
    {
      agent.destination = points[destinationIndex].position;
    }
  } 

  private void Update() {
    Walk();
    SearchPlayer();
  }

  public void Walk()
  {
     float dist = agent.remainingDistance; 
    if(dist <= 0.05f){
      destinationIndex = Random.Range(0, points.Length);
      agent.destination = points[destinationIndex].position;
    }
  }

  public void SearchPlayer()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if(distanceToPlayer < detectDistance)
    {
      if(canShoot)
      {
      canShoot = false;
      GameObject coconutToShoot = Instantiate(coconut, transform.position, transform.rotation);
      coconutToShoot.transform.position = transform.position + Vector3.up + transform.forward;
      coconutToShoot.GetComponent<Rigidbody>().AddRelativeForce(player.forward * launchVelocity);//(new Vector3(0, player.position.y * launchVelocity, 0));// * launchVelocity * Time.deltaTime);//(new Vector3(player.position.x * launchVelocity * Time.deltaTime, player.position.y * launchVelocity * Time.deltaTime, player.position.z * launchVelocity * Time.deltaTime));//(new Vector3 (0, player.position.y * launchVelocity * Time.deltaTime,0));
      StartCoroutine("LaunchProjectile");
      }
      
      agent.destination = player.position;
      agent.speed = runSpeed;
      }
    else
    {
      agent.speed = defaultSpeed;
      agent.destination = points[destinationIndex].position;
    }
  }

  IEnumerator LaunchProjectile()
  {
    yield return new WaitForSeconds(0.5f);
    canShoot = true;
  }
}
