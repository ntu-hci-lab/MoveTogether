using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class FireSpiritControl : MonoBehaviour
{
    public NetworkObject networkObject;
    private bool isDying = false;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {   
        Debug.Log("Fire Collided with " + collision.gameObject.name);
        
        if (collision.gameObject.name == "FLOOR_EffectMesh" && !isDying)
        {
            Debug.Log("Fire hit the floor. Starting death sequence.");
            isDying = true;
            // Set the animation bool to trigger the dying animation
            Animator fireAnimator = GetComponent<Animator>();
            fireAnimator.SetBool("Die", true);

            // Start a coroutine to despawn after 5 seconds
            StartCoroutine(DespawnAfterDelay(5f));
        }
    }
    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Fire Despawned");
        FireObjectSpawner.instance.DespawnObject(networkObject);
    }
}
