using UnityEngine;
using UnityEngine.EventSystems; // Necesario para detectar el mouse
using UnityEngine.UI;
using TMPro;

public class BotonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image fondoBoton;
    private TextMeshProUGUI textoBoton;

    void Start()
    {
        // El script busca el fondo y el texto automáticamente
        fondoBoton = GetComponent<Image>();
        textoBoton = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Cuando el mouse entra al botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        fondoBoton.color = Color.black;
        textoBoton.color = Color.magenta;
    }

    // Cuando el mouse sale del botón
    public void OnPointerExit(PointerEventData eventData)
    {
        fondoBoton.color = Color.magenta;
        textoBoton.color = Color.black;
    }
}