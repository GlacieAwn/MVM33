using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;

    public void Launch(Vector3 firingDirection, float projectileSpeed, float lifetime)
    {
        direction = firingDirection.normalized;
        speed = projectileSpeed;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
