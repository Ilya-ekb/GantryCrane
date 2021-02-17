using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public MotorController moving;
    public MotorController sliding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            moving.Working(-1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moving.Working(1);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            moving.Working(0);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            sliding.Working(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            sliding.Working(-1);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            sliding.Working(0);
        }
    }
}
