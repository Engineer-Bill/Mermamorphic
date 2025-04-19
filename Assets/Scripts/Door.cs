using UnityEngine;
using System.Collections;


public class Door : MonoBehaviour
{
    private Vector3 closedPosition;
    public Vector3 openPositionOffset; // The position of the door when it's open
    public float moveSpeed = 2f; // Speed at which the door moves

    [SerializeField]
    private AudioClip _openSound; // SFX Junk
    private AudioSource _audioSource;

    private void Start()
    {
        closedPosition = transform.position; // Store the initial position as the closed position
        _audioSource = GetComponent<AudioSource>(); // SFX Junk
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void OpenDoor()
    {
        StopAllCoroutines(); // Stop any door movement if it was in progress
        if (_openSound != null) // SFX Junk
        {
            _audioSource.PlayOneShot(_openSound);
        }
        StartCoroutine(MoveDoor(closedPosition + openPositionOffset)); // Move the door to the open position
    }

    public void CloseDoor()
    {
        StopAllCoroutines(); // Stop any door movement if it was in progress
        StartCoroutine(MoveDoor(closedPosition)); // Move the door to the closed position
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        transform.position = targetPosition; // Ensure the door reaches the exact target position
    }
}
