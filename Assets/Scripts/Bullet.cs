using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
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
