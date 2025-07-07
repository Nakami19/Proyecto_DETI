using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float radius = 0.1f;           // Radius of the sphere cast (small enough to simulate a "circular" path)
    public float lifetime = 5f;           // Optional: bullet lifetime
    private float lifeTimer = 0f;

    void OnEnable()
    {
        lifeTimer = 0f; // Reset life
    }

    void Update()
    {
        Vector3 direction = transform.forward;
        float distance = speed * Time.deltaTime;

        // Cast a sphere forward to detect potential hit
        //if (Physics.SphereCast(transform.position, radius, direction, out RaycastHit hit, distance))
        //{
        //    Debug.Log("Bullet hit: " + hit.collider.name);
        //    // Verifica si el objeto golpeado es un enemigo
        //    if (hit.collider.CompareTag("Enemy")) 
        //    {
        //        // Destruye al enemigo
        //        Destroy(hit.collider.gameObject);
        //        // aumento puntaje
        //        GameManager.Instance.score += 10;
        //    }
        //    gameObject.SetActive(false); // Deactivate bullet
        //    return;
        //}

        // Move forward if nothing hit
        transform.Translate(direction * distance, Space.World);

        // Optional: lifetime auto-deactivate
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name != "Player")
        {
            Debug.Log("Bullet hit: " + collision.collider.name);
            if (collision.collider.tag  == "Enemy")
            {
                Destroy(collision.collider.gameObject);
                GameManager.Instance.score += 10;
            }
            gameObject.SetActive(false);
        }
    }
}
