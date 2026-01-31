using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStopArea : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
    // Check if the fire spirit is in the stop area (colliding with the stop area)
    // If it is, stop the fire spirit
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Stop");
        if (other.CompareTag("spirit"))
        {

            // FireSpiritMovement.instance.SetStop();
            FireSpiritMovement fm = other.GetComponent<FireSpiritMovement>();
            fm.SetStop();
        }
    }
}

