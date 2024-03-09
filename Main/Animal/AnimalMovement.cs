using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public Vector3 destination;
    public Island myIsland;

    private AnimalState animalState;

    private Animator animalAnimator;

    private BoxCollider animalBoxCollider;

    public Vector3 boxColliderSize;
    public Vector3 boxColliderCenter;

    private float stoppingDistance = 1f;

    private NavMeshAgent pathFinder;

    public float navSpeed;
    public float navRadius;
    public float navHeight;

    private bool canMove;
    private bool isMove;


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;
        isMove = false;

        animalBoxCollider = GetComponent<BoxCollider>();
        pathFinder = GetComponent<NavMeshAgent>();
        animalState = GetComponent<AnimalState>();
        animalAnimator = GetComponent<Animator>();

        animalBoxCollider.size = boxColliderSize;
        animalBoxCollider.center = boxColliderCenter;

        pathFinder.radius = 0.5f;
        pathFinder.height = navHeight;
        pathFinder.stoppingDistance = stoppingDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (animalState.IsSpawn == true)
        {
            if (canMove == true)
            {
                destination = myIsland.GetDestination(transform.position);
                StartCoroutine(Move());
            }
            if (isMove == true)
            {
                StopCheck();
            }
        }
    }

    private IEnumerator Move()
    {
        canMove = false;
        pathFinder.isStopped = false;
        isMove = true;
        animalAnimator.SetTrigger("FirstWalk");
        animalAnimator.SetBool("IsMove", isMove);
        pathFinder.speed = navSpeed * Time.deltaTime * 4f;
        pathFinder.SetDestination(destination);
        yield return new WaitForSeconds(60f);
        canMove = true;
    }

    void StopCheck()
    {
        if (!pathFinder.pathPending)
        {
            if (pathFinder.remainingDistance <= pathFinder.stoppingDistance)
            {
                Stop();
            }
        } 
    }

    void Stop()
    {
        pathFinder.isStopped = true;
        isMove = false;
        animalAnimator.SetBool("IsMove", isMove);
    }
    
}
