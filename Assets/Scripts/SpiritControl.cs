using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;

public class SpiritControl : MonoBehaviour
{
    public NetworkObject networkObject;
    private bool isDying = false;
    bool flag = true;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {   
        Debug.Log("Earth Collided with " + collision.gameObject.name);
        
        if (collision.gameObject.name == "FLOOR_EffectMesh" && !isDying)
        {
            Debug.Log("Earth hit the floor. Starting death sequence.");
            isDying = true;
            // Set the animation bool to trigger the dying animation
            Animator earthAnimator = GetComponent<Animator>();
            earthAnimator.SetBool("Die", true);

            // Start a coroutine to despawn after 5 seconds
            StartCoroutine(DespawnAfterDelay(5f));
        }


        if (collision.gameObject.CompareTag("Trampoline"))
        {
            Debug.Log("Earth collided with a trampoline");
            //Rigidbody rb = this.GetComponent<Rigidbody>();
            //rb.isKinematic = true;

            // Trigger animation on the trampoline
            Animator earthAnimator = GetComponent<Animator>();
            Animator trampolineAnimator = collision.gameObject.GetComponent<Animator>();
            if (earthAnimator != null)
            {
                earthAnimator.SetTrigger("Bounce"); 
                // Ensure "Bounce" is a valid trigger in Earth's Animator
                Debug.Log("Earth Bounce animation triggered.");
            }
            if (trampolineAnimator != null)
            {
                trampolineAnimator.SetTrigger("Bounce"); // Ensure "Bounce" is a valid trigger in your Animator
            }
            else
            {
                Debug.LogWarning("Earth has no Animator component attached.");
            }
        }
        
        // if (flag && collision.gameObject.name == "FLOOR_EffectMesh")
        // {   
        //     Debug.Log("Change Size");
        //     Collider hitCollider = collision.collider;
        //     Vector3 colliderSize = hitCollider.bounds.size;
        //     PositionalControl.instance.setSize(colliderSize);
        //     ObjectSpawner.instance.DespawnObject(networkObject);
        // }
    }
    private IEnumerator DespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Earth Despawned");
        ObjectSpawner.instance.DespawnObject(networkObject);
    }


    // private void OnCollisionEnter(Collision collision)
    // {
    //     // 獲取碰撞到的 Collider
    //     Collider hitCollider = collision.collider;

    //     if (hitCollider != null)
    //     {
    //         // 獲取 Collider 的包圍盒 (Bounding Box) 大小
    //         Vector3 colliderSize = hitCollider.bounds.size;

    //         // 輸出碰撞到的物件名稱和尺寸
    //         Debug.Log("碰撞到的物件名稱: " + hitCollider.gameObject.name);
    //         Debug.Log("Collider 尺寸: " + colliderSize);
    //     }
    // }
   

}
