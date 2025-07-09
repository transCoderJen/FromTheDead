using System.Collections;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    [SerializeField] private float timetoDissapear; // Time after which the platform is destroyed
    [SerializeField] private float timetoReappear; // Time after which the platform is destroyed
    private Vector3 startingPos;

    private void Start()
    {
        startingPos = transform.position; // Store the initial position of the platform
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.Instance.player.IsGroundDetected())
            {
                StartCoroutine(ShakePlatform());
                StartCoroutine(Disappear()); 
            }
        }
    }

    private IEnumerator ShakePlatform()
    {
        Vector3 originalPosition = transform.position;
        float shakeDuration = 0.5f; // Duration of the shake effect
        float shakeMagnitude = 0.1f; // Magnitude of the shake effect

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            transform.position = new Vector3(originalPosition.x + xOffset, originalPosition.y + yOffset, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        transform.position = originalPosition; // Reset to original position after shaking
    }


    private IEnumerator Disappear()
    {
        yield return Helpers.GetWait(timetoDissapear);
        GetComponent<BoxCollider2D>().enabled = false; // Disable the collider so it doesn't block other objects
        GetComponent<SpriteRenderer>().enabled = false; // Hide the platform
        StartCoroutine(ResetPlatform());
    }

    private IEnumerator ResetPlatform()
    {
        yield return Helpers.GetWait(timetoReappear); // Wait for the specified time before resetting the platform
        transform.position = startingPos; // Reset position
        GetComponent<BoxCollider2D>().enabled = true; // Re-enable the collider
        GetComponent<SpriteRenderer>().enabled = true; // Show the platform again
    }
}
