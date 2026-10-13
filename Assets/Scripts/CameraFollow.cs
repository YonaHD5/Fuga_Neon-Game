using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;
    public Vector3 compensacion = new Vector3(0f, 2f, -10f); // El -10 en Z es vital para que la cámara vea los objetos 2D
    public float velocidadSuavizado = 5f;

    void LateUpdate()
    {
        if (objetivo != null)
        {
            // Calcula la posición a la que debe ir la cámara
            Vector3 posicionDeseada = objetivo.position + compensacion;
            
            // Mueve la cámara suavemente hacia esa posición
            transform.position = Vector3.Lerp(transform.position, posicionDeseada, velocidadSuavizado * Time.deltaTime);
        }
    }
}