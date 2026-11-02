using UnityEngine;

public class Puerta : MonoBehaviour
{
    [Header("Configuración de Apertura")]
    public bool deslizarAlAbrir = true;
    public Vector3 desplazamiento = new Vector3(0, 3f, 0); // Cuánto se eleva en Y
    public float velocidad = 4f;

    private Vector3 posicionDestino;
    private bool estaAbierta = false;

    private void Start()
    {
        posicionDestino = transform.position;
    }

    private void Update()
    {
        if (estaAbierta && deslizarAlAbrir)
        {
            transform.position = Vector3.MoveTowards(transform.position, posicionDestino, velocidad * Time.deltaTime);
        }
    }

    public void Abrir()
    {
        if (!estaAbierta)
        {
            estaAbierta = true;
            posicionDestino = transform.position + desplazamiento;
            
            // Si prefieres que la puerta desaparezca de golpe en vez de deslizarse,
            // desmarca "deslizarAlAbrir" en el Inspector o usa:
            // gameObject.SetActive(false);
        }
    }
}