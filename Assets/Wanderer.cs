using BBUnity.Actions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Wanderer : MonoBehaviour
{
    public WanderState currentState;
    Animator anim;

    [Header("Movement")]
    public float roamSpeed;
    public float chaseSpeed;
    public Vector3 point;
    public GameObject wanderingToRoom;

    private Action OnDestinationReached;
    private bool moving;

    [Header("Search")]
    public float lookAroundTime;

    [Header("Door")]
    public float doorFindDistance;
    public LayerMask findDoorMask;
    public float openDoorAnimationDelay;
    public float openDoorDelay;

    private GameObject doorToOpen;

    [Header("Chase")]
    public float chaseRange;
    public float chaseAngle;
    public float minRange;
    public float stopRange;

    public bool chasing;

    [Header("Misc")]
    public GameObject target;
    public GameObject dirObject;
    public LayerMask findPlayerMask;
    public GameObject currentRoom;

    private NavMeshAgent agent;
    private List<GameObject> rooms;

    [Header("Attack settings")]
    public float lastAttackTime;
    public float attackDelay;
    public float attackVisionRange;
    public float attackAngle;
    public float attackRange;
    public int attackDamage;
    public float attackDamageDelay;

    [Header("Find Player Again")]
    public Vector3 lastPlayerLocation;
    public bool justChased;
    public bool hasSeenPlayer;

    [Header("Sound settings")]
    private AudioSource monsterSource, footstepSource, attackSource;
    public GameObject footstepSourceGO, attackSourceGO;

    public List<AudioClip> screamSounds;
    public List<AudioClip> footstepSounds;
    public List<AudioClip> runningFootstepSounds;

    private List<AudioClip> tempFootsteps = new List<AudioClip>();
    private List<AudioClip> tempRunningFootsteps = new List<AudioClip>();


    // Start is called before the first frame update
    void Start()
    {
        rooms = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        monsterSource = GetComponent<AudioSource>();
        footstepSource = footstepSourceGO.GetComponent<AudioSource>();
        attackSource = attackSourceGO.GetComponent<AudioSource>();
        target = GameObject.FindGameObjectWithTag("Player");

        ChangeState(WanderState.Roaming);
    }

    // Update is called once per frame
    void Update()
    {
        //When destination reached
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && moving)
        {
            moving = false;
            agent.isStopped = true;
            Debug.Log("Destination reached");
            OnDestinationReached?.Invoke();
        }

        //If door in front & moving to other room
        var door = IsDoorInFront();
        if (doorToOpen != null) return;
        if (door != null && doorToOpen == null && currentRoom != wanderingToRoom)
        {
            doorToOpen = door;
            OpenDoor(door);
            return;
        }

        //If target is nearby and in sight - attack
        if (IsTargetClose(attackAngle, attackVisionRange) && CanAttack())
        {
            Debug.Log("Attack");
            LookAtTarget();
            Attack();
        }

        //Continuous follow player when chasing
        if (chasing)
        {
            if (DistanceToTarget() >= stopRange) MoveToPoint(target.transform.position);
            else
            {
                Debug.Log("Stop chasing");
                anim.SetBool("Chase", false);
                anim.SetTrigger("Search");
                moving = false;
                agent.isStopped = true;
                OnDestinationReached?.Invoke();
            }
        }

        //If target is nearby and in sight - chase
        if (IsTargetClose(chaseAngle, chaseRange))
        {
            hasSeenPlayer = true;
            justChased = true;
            lastPlayerLocation = target.transform.position;
            if (DistanceToTarget() >= minRange)
            {
                Debug.Log("Chase");
                ChangeState(WanderState.Chasing);
            }
            return;
        }

        if (justChased)
        {
            justChased = false;

            //Just chased, go to last location. Play scream when arrive and he isnt there

            MoveToPoint(lastPlayerLocation);
            ChangeState(WanderState.LookingForPlayer);

            //Make him scream as well. Maybe other stuff too
            OnDestinationReached = () => ChangeState(WanderState.Roaming);
        }


        //Logic to switch between states
    }

    void ChangeState(WanderState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }

        //Current state logic
        switch (currentState)
        {
            case WanderState.Roaming:
                Roam();
                break;
            case WanderState.SearchingRoom:
                SearchRoom();
                break;
            case WanderState.Chasing:
                Chase();
                break;
            case WanderState.Attacking:
                break;
            case WanderState.LookingForPlayer:
                break;
            case WanderState.Idle:
                break;
        }
    }

    void Roam()
    {
        anim.SetTrigger("Wander");
        agent.speed = roamSpeed;

        OnDestinationReached = () => ChangeState(WanderState.SearchingRoom);

        var room = GetRandomRoom();
        var roomScript = room.GetComponent<Room>();

        Debug.Log("Wandering to room");
        MoveToPoint(roomScript.centerObject.transform.position);
    }

    void Chase()
    {
        anim.SetBool("Chase", true);
        agent.speed = chaseSpeed;

        OnDestinationReached = () =>
        {
            anim.SetBool("Chase", false);
            anim.SetTrigger("Search");
            chasing = false;
        };

        chasing = true;
    }

    float DistanceToTarget()
    {
        return Vector3.Distance(target.transform.position, transform.position);
    }

    void Attack()
    {
        StartCoroutine(DoAttack());
    }


    IEnumerator DoAttack()
    {
        anim.SetTrigger("Attack");
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(attackDamageDelay);

        if (DistanceToTarget() <= attackRange)
        {
            target.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
        }
    }

    void LookAtTarget()
    {
        gameObject.transform.LookAt(target.transform.position);
    }

    void MoveToPoint(Vector3 point)
    {
        moving = true;
        agent.SetDestination(point);
        agent.isStopped = false;

        wanderingToRoom = GetRoomAtLocation(point);
    }

    GameObject GetRoomAtLocation(Vector3 point)
    {
        var hits = Physics.OverlapSphere(point, 0.1f);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Room")) return hit.transform.gameObject;
        }
        return null;
    }

    void SearchRoom()
    {
        var room = currentRoom.GetComponent<Room>();

        if (room.isCorridor)
        {
            ChangeState(WanderState.Roaming);
            return;
        }

        Debug.Log("Searching room");

        var dest = GetRandomPoint(room.centerObject.transform.position, (room.bounding_x / 2) - 1, (room.bounding_z / 2) - 1);
        OnDestinationReached = BeginLookAround;
        MoveToPoint(dest);
    }

    void BeginLookAround()
    {
        StartCoroutine(LookAround());
    }

    IEnumerator LookAround()
    {
        anim.SetTrigger("Search");
        agent.speed = roamSpeed;

        yield return new WaitForSeconds(lookAroundTime);
        ChangeState(WanderState.Roaming);
    }


    //Get a random room, but remove from list after. Ensures entire dungeon will be roamed
    private GameObject GetRandomRoom()
    {
        //When room count is 0 (has roamed entire map), resets the list and begins roaming again
        if (rooms.Count <= 0) rooms = new List<GameObject>(GameObject.FindObjectOfType<DungeonGenerator>().spawnedRooms);
        rooms = rooms.OrderBy(r => Vector3.Distance(new Vector3(0, gameObject.transform.position.y, 0),
        new Vector3(0, r.transform.position.y, 0)))
            .OrderBy(r => Vector3.Distance(new Vector3(gameObject.transform.position.x, 0, gameObject.transform.position.z),
            new Vector3(r.transform.position.x, 0, r.transform.position.z))).ToList();

        var room = rooms[0];
        rooms.Remove(room);
        return room;
    }

    Vector3 GetRandomPoint(Vector3 origin, float distancex, float distancez)
    {
        Vector3 randomDirection = new Vector3(Random.Range(0.5f, 0.9f) * distancex / 2, origin.y, Random.Range(0.5f, 0.9f) * distancez / 2);

        return randomDirection += origin;
    }

    private bool IsTargetClose(float angle, float dist)
    {
        if (target == null) return false;
        var pos = gameObject.transform.position;
        var tempPos = new Vector3(pos.x, 0, pos.z);
        var playerPos = Camera.main.transform.position;
        var tempPlayerPos = new Vector3(playerPos.x, 0, playerPos.z);
        if (Vector3.Distance(tempPos, tempPlayerPos) > dist) return false;

        var dirobjectcenter = dirObject.GetComponent<SkinnedMeshRenderer>().bounds.center;
        Vector3 dir = (playerPos - dirobjectcenter);


        RaycastHit hit;
        if (Physics.Raycast(dirobjectcenter, dir, out hit, Mathf.Infinity, findPlayerMask))
        {
            Debug.DrawLine(dirobjectcenter, hit.point);
            return hit.collider.gameObject == target && Vector3.Angle(dir, dirObject.transform.forward) < angle * 0.5f;
        }
        return false;
    }

    GameObject IsDoorInFront()
    {
        RaycastHit hit;
        if (Physics.Raycast(dirObject.transform.position, dirObject.transform.forward, out hit, doorFindDistance, findDoorMask))
        {
            var dirobjectcenter = dirObject.GetComponent<SkinnedMeshRenderer>().bounds.center;
            Debug.DrawLine(dirobjectcenter, hit.point, Color.red);

            if (hit.transform.CompareTag("Door"))
            {
                var doorscript = hit.transform.GetComponent<ToggleDoor>();
                if (doorscript.open) return null;

                return hit.transform.gameObject;
            }
        }
        return null;
    }

    bool CanAttack()
    {
        return ((lastAttackTime + attackDelay) <= Time.time);
    }

    void OpenDoor(GameObject door)
    {
        Debug.Log("Opening door");
        StartCoroutine(OpenDoorWait(door));
    }

    IEnumerator OpenDoorWait(GameObject door)
    {
        var prevSpeed = agent.speed;
        agent.speed = 0;
        anim.SetTrigger("Search");

        yield return new WaitForSeconds(openDoorAnimationDelay);

        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(openDoorDelay);

        var doorScript = door.GetComponent<ToggleDoor>();
        doorScript?.Interact(gameObject);

        //THIS NEEDS TO BE UPDATED BASED ON THE PREVIOUS STATE
        anim.SetTrigger("Wander");

        doorToOpen = null;
        agent.speed = prevSpeed;
    }

    public void PlayFootstepSound()
    {
        if (tempFootsteps.Count <= 0) tempFootsteps = new List<AudioClip>(footstepSounds);

        var clip = tempFootsteps[Random.Range(0, tempFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 0.6f;
        footstepSource.PlayOneShot(clip);
        tempFootsteps.Remove(clip);
    }

    public void PlayRunningFootstepSound()
    {
        if (tempRunningFootsteps.Count <= 0) tempRunningFootsteps = new List<AudioClip>(runningFootstepSounds);

        var clip = tempRunningFootsteps[Random.Range(0, tempRunningFootsteps.Count)];
        footstepSource.clip = clip;
        footstepSource.pitch = 1;
        footstepSource.PlayOneShot(clip);
        tempRunningFootsteps.Remove(clip);
    }

    public void PlayScreechSound()
    {
        var clip = screamSounds[Random.Range(0, screamSounds.Count)];
        monsterSource.clip = clip;
        monsterSource.Play();
    }

    public void PlayAttackSound()
    {
        attackSource.Play();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            currentRoom = other.gameObject;
        }
    }

    public enum WanderState
    {
        Roaming,
        SearchingRoom,
        Chasing,
        Attacking,
        LookingForPlayer,
        Idle
    }
}
