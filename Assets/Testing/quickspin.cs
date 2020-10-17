using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quickspin : MonoBehaviour
{
    public float spd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, spd * Time.deltaTime, 0);
    }
}
