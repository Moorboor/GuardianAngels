using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

    GameObject player;
    GameObject drone;
    GameObject waterPlane;
    DroneController droneMovement;
    private Transform waterPlaneTrans;
    
    Vector3 waterPlanePos;
    Vector3 endPos;
 

    // State of the experiment
    int state = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        drone = GameObject.FindWithTag("Drone");
        waterPlane = GameObject.FindWithTag("WaterPlane");
        waterPlaneTrans = waterPlane.GetComponent<Transform>();
        droneMovement = drone.GetComponent<DroneController>();
        state = 1;
    }


    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            Debug.Log("State 1: Drone is looking for victim");
        } 
        else if (state == 2)
        {
            Debug.Log("State 2: Drone monologue");
        }
        else if (state == 3)
        {
            Debug.Log("State 3: Drone is showing victim the best way out");

            if (waterPlane.transform.position.y < 18)
            {
                this.RaiseLevel(1.5f, 0.005f);
            }
        }
    }


    public void AdvanceState()
    {
        state = state + 1;
    }


    public int GetState()
    {
        return state;
    }
    

    // Raise the level of the water plane
    public void RaiseLevel(float amount, float speed)
    {
        waterPlanePos = waterPlane.transform.position;
        endPos = waterPlanePos + new Vector3(0, amount, 0);
        waterPlane.transform.position = Vector3.Lerp(waterPlanePos, endPos, speed * Time.deltaTime);
    }


    // Save the data
    public void EndGame()
    {
        Debug.LogWarning("Endgame with drone turns: " + droneMovement.droneTurnCount);
        int droneTurnCount = droneMovement.droneTurnCount;
        float totalTime = Time.time - droneMovement.followTimeStart;
        
        this.WriteToFile(totalTime, droneTurnCount);
    }


    // Write data to file
    public void WriteToFile(float time, int droneTurnCount)
    {
        try
        {
            StreamWriter sw = new StreamWriter("Data.txt", true);
            sw.WriteLine("Time:" + time + ", " + droneTurnCount);
            sw.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e.Message);
        }
        finally
        {
            Console.WriteLine("Executing finally block.");
        }
    }
}
