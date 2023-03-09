using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SimulationManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public SimulationController simulationController;
    public GameObject agentPrefab;
    public GameObject arrowPrefab;
    public GameObject systemPrefab;
    public GameObject reservoirPrefab;
    public GameObject sectorPrefab;

    private string[] dataLines = File.ReadAllLines(@"Assets\simulation_data.txt");
    private int steps;
    private int numberOfAgents;
    private int agentSideLength = 2;
    private int numberOfLines;
    private int simulationStepTextDistance = 12;
    private int maxThreshold;

    public int simulationStep = 0;
    public int agentsPerLine = 20;
    public float agentY = 1.5f;
    public float arrowY = 3.0f;
    public bool canRunSimulationStep = false;
    public int agentsInSector = 0;
    public int gameObjectsInSector = 0;

    // Cube's colors 
    public int[] colorLowThreshold = new int[] {246, 238, 97};
    public int[] colorHighThreshold = new int[] {123, 105, 164};

    // SeparationFactor is the distance between two Agents, 1 = 1 * agentSideLength
    public float separationFactor = 0.75f;

    // Parabola movement variables
    public float height = 10.0f;
    public float speed = 0.5f;

    // Planes parameters
    private float systemLength;
    private float systemWidth;
    private float sectorLength;
    private float sectorWidth;
    private float spacingLength;
    private float spacingWidth;
    private float agentSpacingLength;
    private float agentSpacingWidth;

    private GameObject[] agents;
    private GameObject[] toMoveAgents;
    
    void Awake(){
        // Getting player input
        playerInput = GetComponent<PlayerInput>();
        simulationController = new SimulationController();
        simulationController.Simulation.Enable();

        // Defining functions to each player input
        simulationController.Simulation.Initial_Conditions.performed += InitialConditions;
        simulationController.Simulation.Simulation_Step.performed += SimulationStep;
        simulationController.Simulation.Move_One_Agent.performed += MoveOneAgent;
        simulationController.Simulation.Spawn_Arrow.performed += SpawnArrow;
        //simulationController.Simulation.Move_Arrow.performed += MoveArrow;

        // Simulation and visualization data
        steps = Convert.ToInt32(dataLines[0]);
        numberOfAgents = Convert.ToInt32(dataLines[3]);
        if (numberOfAgents < agentsPerLine){
            agentsPerLine = numberOfAgents;
        }
        numberOfLines = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(((float)numberOfAgents)/agentsPerLine)));

        // Planes parameters
        agentSpacingLength = separationFactor * ((float)agentSideLength);
        agentSpacingWidth = separationFactor * ((float)agentSideLength);
        sectorLength = agentSpacingLength + agentsPerLine * (agentSideLength + agentSpacingLength);
        sectorWidth = agentSpacingWidth + numberOfLines * (agentSideLength + agentSpacingWidth);
        systemLength = (25.0f/23) * sectorLength;
        systemWidth = (25.0f/11) * sectorWidth;
        spacingLength = systemLength/25;
        spacingWidth = systemWidth/25;

        Vector3 systemPosition = new Vector3(systemLength/2,-0.2f,systemWidth/2);
        Vector3 reservoirPosition = new Vector3(systemLength/2,0,(2 * spacingWidth + (3.0f/2) * sectorWidth)); 
        Vector3 sectorPosition = new Vector3(systemLength/2,0,(spacingWidth + (1.0f/2) * sectorWidth));

        // Instantiate planes
        GameObject system = Instantiate(systemPrefab, systemPosition, Quaternion.identity);
        system.transform.localScale = new Vector3(systemLength,1,systemWidth);

        GameObject reservoir = Instantiate(reservoirPrefab, reservoirPosition, Quaternion.identity); 
        reservoir.transform.localScale = new Vector3(sectorLength,1,sectorWidth);

        GameObject sector = Instantiate(sectorPrefab, sectorPosition, Quaternion.identity);
        sector.transform.localScale = new Vector3(sectorLength,1,sectorWidth);
    }

    // Set the simulation to it's initial condition
    void InitialConditions(InputAction.CallbackContext context){
        // Destroys all agents on screen (if there are any)
        agents = GameObject.FindGameObjectsWithTag ("Agent");
        for(int i = 0; i < agents.Length; i++){
            Destroy(agents[i]);
        }

        // Resets simulation steps to 0
        simulationStep = 0;

        // Make it possible to run a simulation step
        canRunSimulationStep = true;

        // Resets agentsInSector and gameObjectsInSector values
        agentsInSector = 0;
        gameObjectsInSector = 0;

        // Spawn agents in initial positions
        for (int i = 0; i < numberOfLines; i++){
            for (int j = 0; j < agentsPerLine && j < numberOfAgents; j++){
                Vector3 position = new Vector3(spacingLength + agentSpacingLength + ((float)agentSideLength)/2 + (j%agentsPerLine)*(agentSideLength + agentSpacingLength),
                                               agentY,
                                               2*spacingWidth + sectorWidth + ((float)agentSideLength)/2 + agentSpacingWidth + (numberOfLines - i - 1)*(agentSideLength + agentSpacingWidth));

                GameObject agent = Instantiate(agentPrefab, position, agentPrefab.transform.rotation);

                int number = i*agentsPerLine + j;

                // Read in the simulation_data text each agent's name and threshold and max threshold
                int agentName = Convert.ToInt32(dataLines[4].Substring(1 + number*7,2));
                int agentThreshold = Convert.ToInt32(dataLines[4].Substring(4 + number*7,2));
                maxThreshold = Convert.ToInt32(dataLines[1]);

                // Assign each agent its name and threshold
                agent.GetComponent<AgentProperties>().agentName = agentName;
                agent.GetComponent<AgentProperties>().agentThreshold = agentThreshold;

                // Paint each agent according to its threshold and write the agent's threshold in its head
                // Linear function (colorLowThreshold to colorHighThreshold)
                
                agent.GetComponent<Renderer>().material.color = new Color(((float)((colorHighThreshold[0] - colorLowThreshold[0]) * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[0]))/255,
                                                                        ((float)((colorHighThreshold[1] - colorLowThreshold[1]) * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[1]))/255,
                                                                        ((float)((colorHighThreshold[2] - colorLowThreshold[2]) * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[2]))/255,
                                                                        1);
                
                                                                          
                // "Bilinear" function (colorLowThreshold to white than white to colorHighThreshold)
                /*
                if (((float)agentThreshold)/maxThreshold < 0.5){
                    agent.GetComponent<Renderer>().material.color = new Color(((float)((255 - colorLowThreshold[0]) * 2 * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[0]))/255,
                                                                          ((float)((255 - colorLowThreshold[1]) * 2 * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[1]))/255,
                                                                          ((float)((255 - colorLowThreshold[2]) * 2 * (((float)agentThreshold)/maxThreshold) + colorLowThreshold[2]))/255,
                                                                          1);
                }
                else{
                    agent.GetComponent<Renderer>().material.color = new Color(((float)((colorHighThreshold[0] - 255) * 2 * (((float)agentThreshold)/maxThreshold - 0.5f) + 255))/255,
                                                                          ((float)((colorHighThreshold[1] - 255) * 2 * (((float)agentThreshold)/maxThreshold - 0.5f) + 255))/255,
                                                                          ((float)((colorHighThreshold[2] - 255) * 2 * (((float)agentThreshold)/maxThreshold - 0.5f) + 255))/255,
                                                                          1);
                }
                */

                // Writes the agent threshold in the cube's surface
                agent.GetComponentInChildren<TextMeshPro>().text = dataLines[4].Substring(4 + number*7,2);
            }
        }

        Debug.Log("Simulation Step Number: " + simulationStep);
    }

    void SpawnArrow(InputAction.CallbackContext context){
        Destroy(GameObject.FindGameObjectWithTag("Arrow"));
        Vector3 position = new Vector3(spacingLength + agentSpacingLength + ((float)agentSideLength)/2,
                                        arrowY,
                                        2*spacingWidth + sectorWidth + ((float)agentSideLength)/2 + agentSpacingWidth + (numberOfLines - 1)*(agentSideLength + agentSpacingWidth));
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
    }

    /*void MoveArrow(InputAction.CallbackContext context){
        GameObject arrow = GameObject.FindGameObjectWithTag("Arrow");
        int xPos = arrow.GetComponent<ArrowProperties>().x;
        int yPos = arrow.GetComponent<ArrowProperties>().y;
        int xMove = (int)context.ReadValue<Vector2>()[0];
        int yMOve = (int)context.ReadValue<Vector2>()[1];

        if (xMove < 0 && ){

        }

        Vector3 position = new Vector3(spacingLength + agentSpacingLength + ((float)agentSideLength)/2 + (xPos%agentsPerLine)*(agentSideLength + agentSpacingLength),
                                        arrowY,
                                        2*spacingWidth + sectorWidth + ((float)agentSideLength)/2 + agentSpacingWidth + (numberOfLines - yPos - 1)*(agentSideLength + agentSpacingWidth));

        arrow.transform.position = position;
    }*/

    void MoveOneAgent(InputAction.CallbackContext context){
        if(canRunSimulationStep){
            if(gameObjectsInSector == agentsInSector){
                simulationStep++;
            }

            // Detects how many agents there are in the sector in this simulation step
            agentsInSector = Convert.ToInt32(dataLines[7 + simulationStep*simulationStepTextDistance]);

            // Stores all agents in agents array
            agents = GameObject.FindGameObjectsWithTag ("Agent");

            // Moves the agent from reservoir to sector
            int agentName = Convert.ToInt32(dataLines[8 + simulationStep*simulationStepTextDistance].Substring(1 + gameObjectsInSector*7,2));
            
            foreach(GameObject agent in agents){
                if(agent.GetComponent<AgentProperties>().agentName == agentName){
                    gameObjectsInSector += 1;
                    Vector3 finalPosition = new Vector3 (spacingLength + agentSpacingLength + ((float)agentSideLength)/2 + ((gameObjectsInSector-1)%agentsPerLine)*(agentSideLength + agentSpacingLength), 
                                                    agentY,
                                                    spacingWidth + agentSpacingWidth + ((float)agentSideLength)/2 + (numberOfLines - Convert.ToInt32(Math.Floor(Convert.ToDouble(((float)(gameObjectsInSector-1))/agentsPerLine))) - 1)*(agentSideLength + agentSpacingWidth));

                    // Code that really moves the agent
                    StartCoroutine(MoveAgentCoroutine(agent, agent.transform.position, finalPosition));
                    break;
                }
            }

            Debug.Log("Agent Moved");

            if(gameObjectsInSector == agentsInSector){
                Debug.Log("STEP FINISHED!");
                if(agentsInSector == numberOfAgents){
                    canRunSimulationStep = false;
                    Debug.Log("SIMULATION FINISHED!");
                }
            }
        }
    }

    // Sort agents according to its threshold
    void SortGameObjectsByThreshold(GameObject[] gameObjects){
        Array.Sort(gameObjects, (go1, go2) => {
            AgentProperties agentProps1 = go1.GetComponent<AgentProperties>();
            AgentProperties agentProps2 = go2.GetComponent<AgentProperties>();
            if (agentProps1 == null || agentProps2 == null)
            {
                // If either GameObject does not have an AgentProperties component,
                // consider them equal in the sorting.
                return 0;
            }
            return agentProps1.agentThreshold.CompareTo(agentProps2.agentThreshold);
        });
    }


    // Describes a parabola with a gameobject
    private IEnumerator MoveAgentCoroutine(GameObject agent, Vector3 start, Vector3 end) {
        float t = 0.0f;
        Vector3 controlPoint = new Vector3((start.x + end.x) / 2.0f, start.y + height, (start.z + end.z) / 2.0f);
        while (t < 1.0f) {
            t += Time.deltaTime * speed;
            agent.transform.position = CalculateParabolaPoint(start, end, controlPoint, t);
            yield return null;
        }
    }

    private Vector3 CalculateParabolaPoint(Vector3 start, Vector3 end, Vector3 control, float t) {
        float u = 1.0f - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 point = (uu * start) + (2.0f * u * t * control) + (tt * end);
        return point;
    }


    void SimulationStep(InputAction.CallbackContext context){
        if(canRunSimulationStep){

            //int aux = -1;

            // Move to next simulation step
            if(gameObjectsInSector == agentsInSector){
                simulationStep++;
            }

            // Detects how many agents there are in the sector in this simulation step
            agentsInSector = Convert.ToInt32(dataLines[7 + simulationStep*simulationStepTextDistance]);

            // Stores all agents in agents array
            agents = GameObject.FindGameObjectsWithTag ("Agent");

            // Resets gameObjectsInSector
            gameObjectsInSector = 0;

            //toMoveAgents
            toMoveAgents = new GameObject[agentsInSector];

            // List the agents that will move from reservoir to sector
            for (int i = 0; i < agentsInSector; i++){
                int agentName = Convert.ToInt32(dataLines[8 + simulationStep*simulationStepTextDistance].Substring(1 + i*7,2));

                foreach(GameObject agent in agents){
                    if(agent.GetComponent<AgentProperties>().agentName == agentName){
                        toMoveAgents[gameObjectsInSector] = agent;
                        gameObjectsInSector += 1;
                        break;
                    }             
                }
            }

            // Sort agents by threshold
            SortGameObjectsByThreshold(toMoveAgents);

            // How many agents were in the sector in the past simulation step
            int a = Convert.ToInt32(dataLines[7 + (simulationStep-1)*simulationStepTextDistance]);

            for (int i = a; i < toMoveAgents.Length; i++){
                Vector3 finalPosition = new Vector3 (spacingLength + agentSpacingLength + ((float)agentSideLength)/2 + (i%agentsPerLine)*(agentSideLength + agentSpacingLength), 
                                                agentY,
                                                spacingWidth + agentSpacingWidth + ((float)agentSideLength)/2 + (numberOfLines - Convert.ToInt32(Math.Floor(Convert.ToDouble(((float)i)/agentsPerLine))) - 1)*(agentSideLength + agentSpacingWidth));

                // Code that really moves the agent
                StartCoroutine(MoveAgentCoroutine(toMoveAgents[i], toMoveAgents[i].transform.position, finalPosition));
            }

            Debug.Log("Simulation Step Number: " + simulationStep);
            
            if(agentsInSector == numberOfAgents){
                canRunSimulationStep = false;
                Debug.Log("SIMULATION FINISHED!");
            }
            
        }
    }

}