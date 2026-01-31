using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
public class WaterSpiritController : MonoBehaviour
{
    public NetworkObject networkObject;
    bool flag = true;
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Water Collided with " + collision.gameObject.name);

        if (collision.gameObject.name == "FLOOR_EffectMesh")
        {
            Debug.Log("Water Despawned");
            ObjectSpawner.instance.DespawnObject(networkObject);
        }


        //if (collision.gameObject.CompareTag("Trampoline"))
        //{
        //    Debug.Log("Water collided with a trampoline");
        //    //Rigidbody rb = this.GetComponent<Rigidbody>();
        //    //rb.isKinematic = true;

        //    // Trigger animation on the trampoline
        //    Animator WaterAnimator = GetComponent<Animator>();
        //    Animator trampolineAnimator = collision.gameObject.GetComponent<Animator>();
            //if (WaterAnimator != null)
            //{
            //    WaterAnimator.SetTrigger("Bounce");
            //    // Ensure "Bounce" is a valid trigger in Water's Animator
            //    Debug.Log("Water Bounce animation triggered.");
            //}
            //if (trampolineAnimator != null)
            //{
            //    trampolineAnimator.SetTrigger("Bounce"); // Ensure "Bounce" is a valid trigger in your Animator
            //}
            //else
            //{
            //    Debug.LogWarning("Water has no Animator component attached.");
            //}
        }
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
//     }

//}
