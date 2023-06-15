using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SheepAI : MonoBehaviour
{
  [Range(0.5f, 50)]
  public float detectDistance = 3;
  public Transform[] points;
  int destinationIndex = 0;
  NavMeshAgent agent;
  Transform player;
  Animator anim;
  float runSpeed = 2.0f;
  float defaultSpeed = 1.5f;
  bool isIdle = false;

  private void Start() 
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    anim = GetComponent<Animator>();
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
    if(agent.remainingDistance <= 0.05f && !isIdle)
    {
      isIdle = true;
      anim.SetBool("walking", false);
      StartCoroutine("GoToNextSpot");
    }
  }

  public void SearchPlayer()
  {
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);

    if(distanceToPlayer < detectDistance)
    {
      agent.destination = player.position;
      agent.speed = runSpeed;
    }
    else
    {
      agent.speed = defaultSpeed;
      agent.destination = points[destinationIndex].position;
    }
  }

  IEnumerator GoToNextSpot()
    {
      yield return new WaitForSeconds(2.0f);
      isIdle = false;
      destinationIndex = Random.Range(0, points.Length);
      agent.destination = points[destinationIndex].position;
      anim.SetBool("walking", true);
    }
}
