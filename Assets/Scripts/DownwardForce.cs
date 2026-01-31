using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownwardForce : MonoBehaviour
{
    public float forceAmount = 3f; // The strength of the downward force

    void FixedUpdate()
    {
        AddDownwardForce();
    }

    void AddDownwardForce()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply a downward force
            rb.AddForce(Vector3.down * forceAmount, ForceMode.Force);
        }
        else
        {
            Debug.LogWarning("Rigidbody component not found on this GameObject!");
        }
    }
}
