using UnityEngine;

public class SeguimientoCamara : MonoBehaviour
{
    [Header("Objetivo a seguir")]
    public Transform jugador; // Aquí arrastraremos a tu gato

    [Header("Configuración de Cámara")]
    public Vector3 compensacion = new Vector3(0f, 2f, -10f); // Ajusta la posición para que no quede a ras de suelo
    public float velocidadSuavizado = 5f; // Qué tan rápido lo sigue

    void LateUpdate()
    {
        if (jugador != null)
        {
            // Calcula la posición a la que debe ir la cámara
            Vector3 posicionDeseada = jugador.position + compensacion;

            // Mueve la cámara suavemente hacia esa posición
            Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadSuavizado * Time.deltaTime);
            
            transform.position = posicionSuavizada;
        }
    }
}