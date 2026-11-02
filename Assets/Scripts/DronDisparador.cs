using UnityEngine;

public class DronDisparador : MonoBehaviour
{
    public enum DireccionDisparo { Izquierda, Derecha, Arriba, Abajo }

    [Header("Configuración de Dirección")]
    [Tooltip("Selecciona hacia dónde saldrá disparado el láser")]
    public DireccionDisparo direccion = DireccionDisparo.Derecha;

    [Header("Configuración de Disparo")]
    public GameObject prefabLaser;
    public Transform puntoDisparo; // Opcional: crea un hijo en el dron para la boca del cañón
    public float tiempoEntreDisparos = 1.5f;

    private float cronometro;

    void Update()
    {
        cronometro += Time.deltaTime;
        if (cronometro >= tiempoEntreDisparos)
        {
            Disparar();
            cronometro = 0f;
        }
    }

    void Disparar()
    {
        if (prefabLaser == null) return;

        Vector3 posicionOrigen = puntoDisparo != null ? puntoDisparo.position : transform.position;
        GameObject nuevoLaser = Instantiate(prefabLaser, posicionOrigen, Quaternion.identity);

        Vector2 vectorDireccion = ObtenerVectorDireccion();
        
        ProyectilLaser scriptLaser = nuevoLaser.GetComponent<ProyectilLaser>();
        if (scriptLaser != null)
        {
            scriptLaser.Inicializar(vectorDireccion);
        }
    }

    private Vector2 ObtenerVectorDireccion()
    {
        switch (direccion)
        {
            case DireccionDisparo.Izquierda: return Vector2.left;
            case DireccionDisparo.Derecha:   return Vector2.right;
            case DireccionDisparo.Arriba:    return Vector2.up;
            case DireccionDisparo.Abajo:     return Vector2.down;
            default: return Vector2.right;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 pos = puntoDisparo != null ? puntoDisparo.position : transform.position;
        Gizmos.DrawRay(pos, (Vector3)ObtenerVectorDireccion() * 2f);
    }
}