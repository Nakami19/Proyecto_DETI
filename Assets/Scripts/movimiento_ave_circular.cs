using UnityEngine;

public class movimiento_ave_circular : MonoBehaviour
{
    public float radio = 2f;
    public float velocidad = 2f;
    private Vector3 centro;
    private Vector3 posicionAnterior;

    void Start()
    {
        centro = transform.position;
        posicionAnterior = transform.position;
    }

    void Update()
    {
        // Movimiento circular
        float x = Mathf.Cos(Time.time * velocidad) * radio;
        float z = Mathf.Sin(Time.time * velocidad) * radio;
        Vector3 nuevaPosicion = centro + new Vector3(x, 0, z);
        transform.position = nuevaPosicion;

        // Dirección de movimiento
        Vector3 direccionMovimiento = nuevaPosicion - posicionAnterior;

        // Si hay movimiento, rotar hacia la dirección
        if (direccionMovimiento != Vector3.zero)
        {   //roto mirando hacia la posicion
            Quaternion rotacion = Quaternion.LookRotation(direccionMovimiento);
            transform.rotation = rotacion;
        }

        // Guardar la posición para el próximo frame
        posicionAnterior = nuevaPosicion;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.name);
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Dano");
            GameManager.Instance.takeDamage(1);
        }
    }
}
