using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            Debug.Log($"Bullet hit: {other.tag}");

            if (GameManager.Instance != null)
            {
                GameManager.Instance.SwitchItStatus();
            }

            Destroy(gameObject);
        }
    }
}
