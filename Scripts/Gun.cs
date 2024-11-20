using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public Camera playerCamera;

    private float nextFireTime = 0f;
    private AudioSource audioSource;

    public bool isIt;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource is missing on the gun object. Please add one.");
        }
    }

    void Update()
    {
        if (isIt && Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        Vector3 shootDirection = playerCamera.transform.forward;
        bullet.transform.forward = shootDirection;

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootDirection * bulletSpeed;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }

        Destroy(bullet, 2f);
    }

    public void SetIt(bool status)
    {
        isIt = status;
        Debug.Log($"Gun: Player is now {(isIt ? "it" : "not it")}!");
    }
}
