using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 3f;

    private Transform destinoActual;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        destinoActual = puntoB;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Mueve al enemigo hacia el destino actual
        transform.position = Vector3.MoveTowards(transform.position, destinoActual.position, velocidad * Time.deltaTime);

        // Si llega al punto de destino, cambia al otro punto
        if (Vector3.Distance(transform.position, destinoActual.position) < 0.1f)
        {
            if (destinoActual == puntoB)
            {
                destinoActual = puntoA;
                if (spriteRenderer != null) spriteRenderer.flipX = true; // Voltea el sprite
            }
            else
            {
                destinoActual = puntoB;
                if (spriteRenderer != null) spriteRenderer.flipX = false;
            }
        }
    }
}