using UnityEngine;

public class Moneda : MonoBehaviour
{
    [Header("Identificador Único")]
    public string idMoneda = "Nivel1_Moneda1"; // Asigna un ID diferente a cada moneda

    [Header("Configuración")]
    public int valor = 1;

    [Header("Audio")]
    public AudioClip sonidoRecoger;

    private bool yaRecolectada = false;

    private void Start()
    {
        // Si esta moneda ya fue recogida y guardada al completar el nivel anteriormente, se elimina al cargar
        if (PlayerPrefs.GetInt("MonedaRecolectada_" + idMoneda, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!yaRecolectada && (collider.CompareTag("Player") || collider.GetComponentInParent<PlayerController>() != null))
        {
            yaRecolectada = true;

            if (ControladorMonedas.Instancia != null)
            {
                ControladorMonedas.Instancia.AgregarMonedasNivel(valor, idMoneda);
            }

            if (sonidoRecoger != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRecoger, transform.position);
            }

            Destroy(gameObject);
        }
    }
}