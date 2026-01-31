using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Playables;
using System.Collections;

public class PlayVideoAndTimelineOnButtonPress : MonoBehaviour
{
    // Reference to the VideoPlayer attached to the Cube
    public VideoPlayer videoPlayer;

    // Reference to the PlayableDirector for the Timeline
    // public PlayableDirector playableDirector;

    // Reference to the object that will be moved
    public GameObject objectToMove;

    // Target Y position to move the object to
    public float targetYPosition = 1.59f;

    // Duration of the move (in seconds)
    public float moveDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //MoveObjectToTargetY();
        // Register to the VideoPlayer's loopPointReached event
        //videoPlayer.loopPointReached += OnVideoFinished;
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Check for the A button press on Oculus controller or the P key on keyboard
    //    if (OVRInput.GetDown(OVRInput.Button.One) || Input.GetKeyDown(KeyCode.B))
    //    {
    //        Debug.Log("input B");
    //        // Play the video and wait for it to finish
    //        PlayVideo();
    //    }
    //}

    //// Function to start playing the video
    //private void PlayVideo()
    //{
    //    if (videoPlayer != null && !videoPlayer.isPlaying)
    //    {
    //        videoPlayer.Play();
    //        Debug.Log("Video is playing...");
    //    }
    //    else
    //    {
    //        Debug.LogWarning("VideoPlayer is either not assigned or already playing.");
    //    }
    //}

    //// This method is called when the video finishes playing
    //private void OnVideoFinished(VideoPlayer vp)
    //{
    //    // Start the Timeline once the video is done
    //    //if (playableDirector != null)
    //    //{
    //    //    playableDirector.Play();
    //    //    Debug.Log("Timeline is playing...");
    //    //}
    //    //else
    //    //{
    //    //    Debug.LogWarning("PlayableDirector is not assigned.");
    //    //}

    //    // Start moving the object on the Y-axis
    //    StartCoroutine(MoveObjectToTargetY());
    //}

    // Coroutine to move the object gradually on the Y-axis
    private IEnumerator MoveObjectToTargetY()
    {
        if (objectToMove == null)
        {
            Debug.LogWarning("No object assigned to move.");
            yield break;
        }

        // Record the object's current position
        Vector3 startPosition = objectToMove.transform.position;
        float startY = startPosition.y;

        // The time elapsed during the movement
        float elapsedTime = 0f;

        // While the object has not reached the target Y position
        while (elapsedTime < moveDuration)
        {
            // Interpolate the Y position smoothly
            float newY = Mathf.Lerp(startY, targetYPosition, elapsedTime / moveDuration);

            // Update the object's position
            objectToMove.transform.position = new Vector3(startPosition.x, newY, startPosition.z);

            // Increment elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the final position is exactly the target Y position
        objectToMove.transform.position = new Vector3(startPosition.x, targetYPosition, startPosition.z);

        Debug.Log("Object has moved to Y = " + targetYPosition);
    }
}
