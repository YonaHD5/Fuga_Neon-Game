using UnityEngine;
using TMPro; // Necesario para controlar TextMeshPro
using System.Collections;

public class TemporizadorNivel : MonoBehaviour
{
    [Header("Configuración de Tiempo")]
    public float tiempoNivel = 100f; // Segundos que dura el nivel
    private float tiempoRestante;
    private bool tiempoAgotado = false;

    [Header("Interfaz (UI)")]
    public TextMeshProUGUI textoTiempoUI;
    public GameObject textoTimeUpUI; 

    [Header("Referencias")]
    public PlayerController jugador;

    void Start()
    {
        tiempoRestante = tiempoNivel;
        if (textoTimeUpUI != null) textoTimeUpUI.SetActive(false); 
    }

    void Update()
    {
        // El tiempo se detiene si hay pausa o si ya se acabó
        if (MenuPausa.juegoPausado || tiempoAgotado) return;

        tiempoRestante -= Time.deltaTime;

        // Actualiza el reloj en pantalla. "F0" sirve para quitar los decimales.
        if (textoTiempoUI != null)
        {
            textoTiempoUI.text = "TIME: " + tiempoRestante.ToString("F0");
        }

        // ¿Se acabó el tiempo?
        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            StartCoroutine(SecuenciaTimeUp());
        }
    }

    private IEnumerator SecuenciaTimeUp()
    {
        tiempoAgotado = true;
        
        // 1. Mostrar el texto gigante de "TIME UP"
        if (textoTimeUpUI != null) textoTimeUpUI.SetActive(true);

        // 2. Le quitamos solo 1 vida al jugador y activamos su animación de muerte
        if (jugador != null)
        {
            jugador.PerderVida();
        }

        // 3. Esperamos 2 segundos a que termine de caer y reaparezca en el checkpoint
        yield return new WaitForSeconds(2f);

        // 4. Si el jugador no ha perdido todas sus vidas, reiniciamos el reloj para que siga jugando
        if (jugador != null && jugador.vidas > 0)
        {
            tiempoRestante = tiempoNivel; // Vuelve a poner los segundos iniciales
            tiempoAgotado = false; // Reactiva el conteo
            if (textoTimeUpUI != null) textoTimeUpUI.SetActive(false); // Oculta el mensaje de Time Up
        }
    }
}