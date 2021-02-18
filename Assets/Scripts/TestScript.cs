using Assets.Scripts.MotorScripts;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public MotorController moving;
    public MotorController sliding;
    [Range(-1f, 1f)]
    public float signal = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            moving.Working(-1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moving.Working(1);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            moving.Working(0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            sliding.Working(1);
        }
        if (Input.GetKey(KeyCode.S))
        {
            sliding.Working(-1);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            sliding.Working(0);
        }
        moving.Working(signal);
    }
}
