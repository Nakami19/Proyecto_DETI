using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime); // Move the bullet forward at the specified speed
    }

    private void onEnable()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject); // Destroy the bullet when it collides with any object
        gameObject.SetActive(false); // Deactivate the bullet instead of destroying it
    }
}
