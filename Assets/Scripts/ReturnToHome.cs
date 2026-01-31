using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class ReturnToHome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {   
        
        if (collision.gameObject.tag == "spirit")
        {   
            Debug.Log("Return to Home");
            NetworkObject networkObject = collision.gameObject.GetComponent<NetworkObject>();
            ObjectSpawner.instance.DespawnObject(networkObject);
            WinScoreCounting.instance.AddPoints();
        }

    }
}
