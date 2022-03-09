using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneController : MonoBehaviour
{
    GameObject player;
    
    NavMeshAgent agent;
    GameManager gameManager;
    AudioSource audio;
    Vector3 safePlace;

    public int participantGroup = 1;
    public AudioClip[] monologues;

    float distance;
    bool playedMonologue;
    bool playerCloseToDrone;

    [HideInInspector]
    public int droneTurnCount;
    [HideInInspector]
    public float followTimeStart;

    bool timeTaken;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
        
        agent = this.GetComponent<NavMeshAgent>();
        distance = agent.stoppingDistance + 1f;
        audio = this.GetComponent<AudioSource>();
        gameManager = GameManager.instance;
        safePlace = GameObject.Find("SafePlace").transform.position;
        playedMonologue = false;

        droneTurnCount = 0;
        timeTaken = false;
        
        

    }


    // Update is called once per frame
    void Update()
    {

        if (gameManager.GetState() == 1)
        {
            ApproachVictim();
        }
        else if (gameManager.GetState() == 2)
        {
            StartMonologue();
        }
        else if (gameManager.GetState() == 3)
        {   
            NavigateToSafePlace();
            if (!timeTaken)
            {
                followTimeStart = Time.time;
                Debug.LogWarning("Start time: " + followTimeStart);
                timeTaken = true;
            }
            
        }
        else 
        {
            Debug.Log("Current state of drone: " + gameManager.GetState());
        }
    }


    void ApproachVictim()
    {
        
        if (Vector3.Distance(player.transform.position, agent.transform.position) < distance)
        {
            if(gameManager.GetState() == 1)
            {
                gameManager.AdvanceState();
            }
        }
        else 
        {
            agent.SetDestination(player.transform.position);
        }
        
    }


    void StartMonologue()
    {
        if (!audio.isPlaying)
        {
            if (playedMonologue)
            {
                gameManager.AdvanceState();
            }
            else
            {
                Debug.Log("Drone plays monologue.");
                audio.clip = monologues[participantGroup - 1];
                audio.Play();
                playedMonologue = true;
            }
        }
        else
        {   
            ApproachVictim();
        }
    }


    void NavigateToSafePlace()
    { 
       if (Vector3.Distance(safePlace, agent.transform.position) < distance)
       {
            Debug.Log("Victim is saved.");
            gameManager.AdvanceState();
            gameManager.EndGame();
       }
       else 
       {   
            if  (playerCloseToDrone)
            {
                if (Vector3.Distance(player.transform.position, agent.transform.position) < distance+10)
                {
                    agent.SetDestination(safePlace);
                }
                else
                {   
                    playerCloseToDrone = false;
                }
            }
            else
            {
                agent.SetDestination(player.transform.position);
                if (Vector3.Distance(player.transform.position, agent.transform.position) < distance)
                {
                    playerCloseToDrone = true;
                    droneTurnCount += 1;
                    Debug.LogWarning("Drone turned: " + droneTurnCount + " times");
                }
            }
       }
    }
}
