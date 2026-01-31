using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
    public float bounceForce = 10f;
    public string playerTag = "spirit"; // change to elf tag
    public float velocityRetention = 0.5f; // How much of the incoming velocity to retain
    public float minBounceSpeed = 2f; // Minimum speed to bounce with

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Get the collision contact point
                ContactPoint contact = collision.contacts[0];

                // Get incoming velocity and surface normal
                Vector3 incomingVelocity = playerRigidbody.velocity;
                Vector3 surfaceNormal = contact.normal;

                // Calculate the reflection direction
                Vector3 reflectionDirection;
                if (incomingVelocity.magnitude < 0.1f) // Check for near-zero velocity
                {
                    // If incoming velocity is near zero, bounce in the direction of the surface normal
                    reflectionDirection = - surfaceNormal;
                }
                else
                {
                    reflectionDirection = Vector3.Reflect(incomingVelocity.normalized, surfaceNormal);
                }

                // Calculate retained velocity
                float retainedSpeed = Mathf.Max(incomingVelocity.magnitude * velocityRetention, minBounceSpeed);

                // Calculate the final bounce velocity
                Vector3 bounceVelocity = reflectionDirection * (retainedSpeed + bounceForce);

                // Apply the bounce velocity
                playerRigidbody.velocity = bounceVelocity;

                // Debug information
                //Debug.Log($"Bounce applied to {collision.gameObject.name}");
                //Debug.Log($"Incoming velocity: {incomingVelocity}");
                //Debug.Log($"Surface normal: {surfaceNormal}");
                //Debug.Log($"Reflection direction: {reflectionDirection}");
                //Debug.Log($"Final bounce velocity: {bounceVelocity}");

                // Visualize the bounce direction
                //Debug.DrawRay(contact.point, reflectionDirection * 2, Color.red, 2f);
            }
        }
    }
}