using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    // --- CONFIGURACIÓN DE META ---
    public int metaParaGanar = 10; // Total de enemigos que debes matar para ganar
    public static int enemigosDerrotados = 0; // Cuenta cuántos has matado
    public static bool juegoTerminado; 

    // --- SALUD DEL JUGADOR ---
    public static int vidas;
    public int vidasIniciales = 100; // Démosle más vida al jugador (ej. 100)

    // --- UI ---
    public TMP_Text txtEnemigos; // Mostrará "Eliminados: 3 / 10"
    public TMP_Text txtVidas;

    // --- CONTROL DE FÁBRICA ---
    // Mantenemos esto solo para que la fábrica sepa cuántos hay vivos ahora mismo
    public static int enemigosVivosActualmente = 0; 

    void Start()
    {
        vidas = vidasIniciales;
        enemigosDerrotados = 0;
        enemigosVivosActualmente = 0;
        juegoTerminado = false; 
    }

    void Update()
    {
        JuegoControl();
    }

    public void JuegoControl()
    {
        if (juegoTerminado) return; 

        // Actualizar UI
        if (txtEnemigos != null) 
            txtEnemigos.text = "Misión: " + enemigosDerrotados + " / " + metaParaGanar;
        
        if (txtVidas != null) 
            txtVidas.text = "Salud: " + vidas;

        // --- NUEVA LÓGICA DE VICTORIA ---
        // Ganas solo si has matado la cantidad necesaria
        if (enemigosDerrotados >= metaParaGanar)
        {
            GanarJuego();
        }

        // --- DERROTA ---
        if (vidas <= 0)
        {
            PerderJuego();
        }
    }

    void GanarJuego()
    {
        juegoTerminado = true;
        Debug.Log("¡Misión Cumplida!");
        SceneManager.LoadScene("Victoria"); 
    }

    void PerderJuego()
    {
        juegoTerminado = true;
        Debug.Log("Jugador Eliminado");
        SceneManager.LoadScene("Derrota"); 
    }
}