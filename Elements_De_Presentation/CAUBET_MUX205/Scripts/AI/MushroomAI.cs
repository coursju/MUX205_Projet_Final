using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MushroomAI : MonoBehaviour
{
  [Range(0.5f, 50)]
  public float detectDistance = 3;
  public Transform[] points;
  int destinationIndex = 0;
  NavMeshAgent agent;
  GameObject player;
  Transform playerTransform;
  PlayerCollision playerCollision;
  Animator anim;
  float runSpeed = 2.0f;
  float defaultSpeed = 1.5f;

  private void Start() 
  {
    player = GameObject.FindGameObjectWithTag("Player"); 
    playerCollision = player.GetComponent<PlayerCollision>();
    playerTransform = player.transform;

    anim = GetComponent<Animator>();

    agent = GetComponent<NavMeshAgent>();

    if(agent != null)
    {
      agent.destination = points[destinationIndex].position;
    }
  } 

  private void Update() 
  {
    
    if(playerCollision.GetMushroomCanWalk())
    {
      anim.SetBool("hurt", false); //Look at animator
      Walk();
      SearchPlayer();
    }
    else
    {
      anim.SetBool("hurt", true);
    }   
  }

  public void Walk()
  {
    if(agent.remainingDistance <= 0.05f){
      destinationIndex = Random.Range(0, points.Length);
      agent.destination = points[destinationIndex].position;
    }
    
  }

  public void SearchPlayer()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

    if(distanceToPlayer < detectDistance)
    {
      agent.destination = playerTransform.position;
      agent.speed = runSpeed;
    }
    else
    {
      agent.speed = defaultSpeed;
      agent.destination = points[destinationIndex].position;
    }
  }
}

