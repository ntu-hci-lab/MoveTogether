using System.Collections;
using Fusion;
using UnityEngine;
using UnityEngine.Video; // For VideoPlayer

public class PositionalControl : NetworkBehaviour
{
    public static PositionalControl instance;
    public GameObject screen;
    public GameObject targetObject; // Reference to the object to move
    public float targetYPosition = 1.59f; // The target Y position
    public float moveDuration = 2f; // Duration of the move in seconds

    public GameObject[] objectsToActivate; // Array to store the objects to activate
    public GameObject particle; // The new object to activate after button press
    public GameObject npc;
    public GameObject videoCube; // The cube that contains the VideoPlayer
    private VideoPlayer videoPlayer; // The VideoPlayer component
    private bool localVideoPlaying = false;

    [Networked] public bool isVideoPlaying { get; set; } = false; // To track if the video is playing
    
    private bool canStartPlaying = false;

    public bool get_IsVideoPlaying()
    {
        return isVideoPlaying;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        // Initialize the VideoPlayer from the videoCube
        videoPlayer = videoCube.GetComponent<VideoPlayer>();
        if (videoPlayer == null)
        {
            Debug.LogError("VideoPlayer component not found on the cube.");
        }
    }

    public bool get_canStartPlaying()
    {
        return canStartPlaying;
    }

    public void set_canStartPlaying(bool startPlaying)
    {
        canStartPlaying = startPlaying;
    }

    void Update()
    {
        // Check if Enter is pressed (keyboard input)
        if (OVRInput.GetDown(OVRInput.Button.One)) // KeyCode.Return is for the Enter key
        {
            Debug.Log("Enter key pressed!");
            isVideoPlaying = true;
        }
        if (isVideoPlaying && !localVideoPlaying)
        {
            // isVideoPlaying = false;
            // Start playing the video when the Enter key is pressed
            // StartCoroutine(PlayVideoAndMove());
            StartCoroutine(ActivateNewObjectAndPlayVideo());
        }
    }
    IEnumerator ActivateNewObjectAndPlayVideo()
    {
        localVideoPlaying = true;

        // Step 1: Activate the new object immediately
        particle.SetActive(true);
        
        // Step 2: Wait for 5 seconds before playing the video
        yield return new WaitForSeconds(3f);
        npc.SetActive(true);
        // Step 3: Start the video
        videoPlayer.Play();

        // Wait for the video to start (to be sure the video has been triggered)
        while (!videoPlayer.isPlaying)
        {
            yield return null; // Wait until the video starts playing
        }

        // Wait for the video to finish playing
        while (videoPlayer.isPlaying)
        {
            yield return null; // Wait until the video finishes
        }

        
        videoCube.SetActive(false);
        npc.SetActive(false);
        screen.SetActive(false);
        // Now, start moving the object
        StartCoroutine(MoveToTargetY());
    }

    // Coroutine to move the object slowly to targetYPosition
    IEnumerator MoveToTargetY()
    {

        float startY = targetObject.transform.position.y;  // The object's starting Y position
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Interpolate between the start position and the target position
            float newY = Mathf.Lerp(startY, targetYPosition, elapsedTime / moveDuration);
            targetObject.transform.position = new Vector3(targetObject.transform.position.x, newY, targetObject.transform.position.z);

            elapsedTime += Time.deltaTime; // Increase elapsed time
            yield return null; // Wait for the next frame
        }

        // Ensure it exactly reaches the target Y position after the duration
        targetObject.transform.position = new Vector3(targetObject.transform.position.x, targetYPosition, targetObject.transform.position.z);

        // Now activate the objects sequentially with a 0.5-second delay
        StartCoroutine(ActivateObjectsSequentially());
    }

    // Coroutine to activate objects sequentially with a 0.5-second delay between each activation
    IEnumerator ActivateObjectsSequentially()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            // Activate the object
            obj.SetActive(true);
            Debug.Log("Activated: " + obj.name);

            // Wait for 0.5 second before activating the next object
            yield return new WaitForSeconds(0.5f);
        }
        canStartPlaying = true;
        ChangeLevel.instance.SetLevel(1);
        isVideoPlaying = false;
    }
}