using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraPosition : MonoBehaviour
{
    public PlayerInput playerInput;
    public SimulationController simulationController;
    public GameObject system;
    public float y;
    public float z;
    public float angle;

    // Start is called before the first frame update
    void Start(){
        playerInput = GetComponent<PlayerInput>();
        simulationController = new SimulationController();
        simulationController.Simulation.Enable();

        system = GameObject.Find("System(Clone)");
        y = 35.7f;
        z = -6.5f;
        angle = 60;
    }

    // Update is called once per frame
    void Update(){
        transform.position = new Vector3(system.transform.position.x, y, z);
        transform.rotation = Quaternion.identity;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.right);
    }
}
