using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.5f;
    public float bulletSpeed = 20f;
    public Camera playerCamera;

    private float nextFireTime = 0f;

    void Update()
    {
        // Check if it's time to shoot again
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Get the direction the camera is facing at the time of shooting
        Vector3 shootDirection = playerCamera.transform.forward;

        // Rotate the bullet to face the direction of the shootDirection
        bullet.transform.forward = shootDirection;

        // Set the bullet's velocity to move in the shootDirection
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            bulletRb.velocity = shootDirection * bulletSpeed;
        }
    }
}
