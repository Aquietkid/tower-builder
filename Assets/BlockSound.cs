using UnityEngine;

public class BlockSound : MonoBehaviour
{
    [SerializeField] private AudioClip collisionSound;
    [SerializeField] private float minVelocity = 1f;
    [SerializeField] private float maxVelocity = 10f;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            float velocity = collision.relativeVelocity.magnitude;
            if (velocity > minVelocity)
            {
                float volume = Mathf.InverseLerp(minVelocity, maxVelocity, velocity);
                audioSource.PlayOneShot(collisionSound, volume);
            }
        }
    }

}
