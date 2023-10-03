using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioClip[] footstepSoundsOnBrick; // Array to hold footstep sound clips on bricks
    public AudioClip[] footstepSoundsOnDirt; // Array to hold foot step sound clips on dirt
    public float minTimeBetweenFootsteps = 0.3f; // Minimum time between footstep sounds
    public float maxTimeBetweenFootsteps = 0.6f; // Maximum time between footstep sounds

    private AudioSource audioSource; // Reference to the Audio Source component
    private bool isWalking = false; // Flag to track if the player is walking
    private float timeSinceLastFootstep; // Time since the last footstep sound
    private float raycastDistance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); // Get the Audio Source component
    }

    private void Update()
    {
        // Check if the player is walking
        if (isWalking)
        {
            // Check if enough time has passed to play the next footstep sound
            if (Time.time - timeSinceLastFootstep >= Random.Range(minTimeBetweenFootsteps, maxTimeBetweenFootsteps))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, Vector2.down, out hit, raycastDistance))
                {
                    // detect the floor, play different arrays of footstep sounds
                    if (hit.collider.CompareTag("brick"))
                    {
                        // Update the time since the last footstep sound
                        // Play a random brick footstep sound from the array
                        AudioClip footstepSoundOnBrick = footstepSoundsOnBrick[Random.Range(0, footstepSoundsOnBrick.Length)];
                        audioSource.PlayOneShot(footstepSoundOnBrick);
                        timeSinceLastFootstep = Time.time;
                    }
                    else if (hit.collider.CompareTag("dirt"))
                    {
                        // Update the time since the last footstep sound
                        // Play a random dirt footstep sound from the array
                        AudioClip footstepSoundOnDirt = footstepSoundsOnDirt[Random.Range(0, footstepSoundsOnDirt.Length)];
                        audioSource.PlayOneShot(footstepSoundOnDirt);
                        timeSinceLastFootstep = Time.time;
                    }

                }

            }
        }
    }

    // Call this method when the player starts walking
    public void StartWalking()
    {
        isWalking = true;
    }

    // Call this method when the player stops walking
    public void StopWalking()
    {
        isWalking = false;
    }
}