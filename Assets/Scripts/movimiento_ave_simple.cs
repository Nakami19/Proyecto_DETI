using UnityEngine;

public class movimiento_ave_simple : MonoBehaviour
{

    public float velocidad = 2f;
    public float distancia = 5f;
    private Vector3 puntoA;
    private Vector3 puntoB;
    private Vector3 destino;
    private bool mirandoA = true;

    void Start()
    {
        puntoA = transform.position;
        puntoB = puntoA + transform.forward * distancia;
        destino = puntoB;
    }

    void Update()
    {
        // Mover a destino
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        // Si llegó al destino
        if (Vector3.Distance(transform.position, destino) < 0.01f)
        {
            // Rotar
            transform.Rotate(0, 180f, 0);

            // Cambiar destino para volver
            mirandoA = !mirandoA;
            destino = mirandoA ? puntoB : puntoA;
        }
    }
}

