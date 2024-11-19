using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;

    private void Start()
    {
        // Destroy the bullet after a set amount of time
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move the bullet forward
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet hit the player or enemy
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            // Switch "it" status between player and enemy in GameManager
            GameManager.Instance.SwitchItStatus();
            Destroy(gameObject);
        }
    }
}
