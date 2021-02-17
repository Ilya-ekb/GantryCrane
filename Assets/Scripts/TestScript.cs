using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public MotorController moving;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float signal;
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
    }
}
