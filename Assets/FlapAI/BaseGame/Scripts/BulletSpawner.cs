using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private float bulletSpeed = 10f;
    private float shootingCooldown = 1f;
    private float lastShootTime;

    void Update() {
        // Check if the space bar is pressed and enough time has passed since the last shot
        if (Input.GetKeyDown(KeyCode.Space) && (GameManager.instance.state != GameManager.States.GameOver) && CanShoot()) {
            SpawnBullet();

            // Update the last shoot time
            lastShootTime = Time.time;
        }
    }

    private void SpawnBullet() {

        // Instantiate a bullet prefab at the spawner's position
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Access the Rigidbody component of the bullet, if it has one
        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        // Check if the bullet has a Rigidbody
        if (bulletRb != null) {
            // Set the velocity of the bullet to move it forward
            bulletRb.velocity = Vector2.right * bulletSpeed;
        }
    }

    private bool CanShoot() {
        return Time.time - lastShootTime >= shootingCooldown;
    }
}
