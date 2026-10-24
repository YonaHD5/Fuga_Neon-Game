using UnityEngine;
using System.Collections;

public class TituloNivel : MonoBehaviour
{
    private CanvasGroup grupoTexto;
    public float tiempoVisible = 2f; // Cuánto tiempo se queda en pantalla
    public float velocidadFade = 1.5f; // Qué tan rápido aparece y desaparece

    void Start()
    {
        // Busca el Canvas Group automáticamente
        grupoTexto = GetComponent<CanvasGroup>();
        
        if (grupoTexto != null)
        {
            StartCoroutine(AparecerYDesaparecer());
        }
    }

    IEnumerator AparecerYDesaparecer()
    {
        // 1. Aparecer suavemente (Fade In)
        while (grupoTexto.alpha < 1f)
        {
            grupoTexto.alpha += Time.deltaTime * velocidadFade;
            yield return null;
        }

        // 2. Esperar en pantalla
        yield return new WaitForSeconds(tiempoVisible);

        // 3. Desaparecer suavemente (Fade Out)
        while (grupoTexto.alpha > 0f)
        {
            grupoTexto.alpha -= Time.deltaTime * velocidadFade;
            yield return null;
        }
    }
}