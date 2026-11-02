using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ControladorMonedas : MonoBehaviour
{
    public static ControladorMonedas Instancia { get; private set; }

    [Header("UI (Opcional)")]
    public TextMeshProUGUI textoMonedasUI;

    private int monedasNivelActual = 0;
    private int monedasTotalesGuardadas = 0;
    private List<string> idsMonedasPendientes = new List<string>();

    private const string CLAVE_MONEDAS = "MonedasTotalesGuardadas";

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        monedasTotalesGuardadas = PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);
        ActualizarUI();
    }

    public void AgregarMonedasNivel(int cantidad, string idMoneda)
    {
        monedasNivelActual += cantidad;

        if (!string.IsNullOrEmpty(idMoneda) && !idsMonedasPendientes.Contains(idMoneda))
        {
            idsMonedasPendientes.Add(idMoneda);
        }

        ActualizarUI();
    }

    public int GuardarMonedasAlFinalizarNivel()
    {
        int monedasGanadasEnEsteNivel = monedasNivelActual;

        // 1. Acumular las monedas ganadas en PlayerPrefs
        monedasTotalesGuardadas += monedasNivelActual;
        PlayerPrefs.SetInt(CLAVE_MONEDAS, monedasTotalesGuardadas);

        // 2. Registrar cada ID de moneda como recolectado de forma permanente
        foreach (string id in idsMonedasPendientes)
        {
            PlayerPrefs.SetInt("MonedaRecolectada_" + id, 1);
        }

        PlayerPrefs.Save();

        // 3. Reiniciar contadores del nivel
        monedasNivelActual = 0;
        idsMonedasPendientes.Clear();
        ActualizarUI();

        Debug.Log("Nivel completado. Monedas guardadas. Total acumulado: " + monedasTotalesGuardadas);
        return monedasGanadasEnEsteNivel;
    }

    public bool GastarMonedas(int cantidad)
    {
        int actuales = PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);
        if (actuales >= cantidad)
        {
            actuales -= cantidad;
            PlayerPrefs.SetInt(CLAVE_MONEDAS, actuales);
            PlayerPrefs.Save();

            monedasTotalesGuardadas = actuales;
            ActualizarUI();
            return true;
        }
        return false;
    }

    public int ObtenerMonedasNivelActual()
    {
        return monedasNivelActual;
    }

    public int ObtenerMonedasTotales()
    {
        return PlayerPrefs.GetInt(CLAVE_MONEDAS, 0);
    }

    private void ActualizarUI()
    {
        if (textoMonedasUI != null)
        {
            int totalVisual = monedasTotalesGuardadas + monedasNivelActual;
            textoMonedasUI.text = "Monedas: " + totalVisual;
        }
    }
}