using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    Vector3 startingPosition;
    [SerializeField] Vector3 movementVector;
    float movementFactor;
    [SerializeField] float period = 2f;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        OscilateMovement();
    }

    void OscilateMovement()
    {
        if (period <= Mathf.Epsilon) {return;}
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float rawSineWave = Mathf.Sin(cycles * tau); // sine wave will cycle back and forth between -1 to 1
        movementFactor = (rawSineWave + 1f) / 2f; // recalculated to go from 0 to 1, so it's cleaner
        //movementFactor = rawSineWave;
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
