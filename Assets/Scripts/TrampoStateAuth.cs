using UnityEngine;
using Fusion;
using Meta.XR.MultiplayerBlocks.Fusion;
using UnityEngine.SceneManagement;

public class TrampoStateAuth : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "spirit" tag
        if (other.CompareTag("spirit"))
        {
            // Attempt to get the NetworkObject component
            NetworkObject networkObject = other.GetComponent<NetworkObject>();
            
            if (networkObject != null)
            {
                // Request State Authority
                Debug.Log($"Requesting State Authority for {other.gameObject.name}");
                networkObject.RequestStateAuthority();
            }
            else
            {
                Debug.LogWarning($"The object {other.gameObject.name} with tag 'spirit' does not have a NetworkObject component.");
            }
        }
    }
}
