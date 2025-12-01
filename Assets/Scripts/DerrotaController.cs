using UnityEngine;
using UnityEngine.SceneManagement;

public class DerrotaController : MonoBehaviour
{
    // Esta función la llamará el botón
    public void VolverAlMenu()
    {
        // Asegúrate de que tu escena del menú se llame exactamente "Menu"
        // Si tienes problemas, usa el índice: SceneManager.LoadScene(0);
        SceneManager.LoadScene("Menu");
        
        // IMPORTANTE: Como venimos de una derrota, aseguramos que el tiempo corra
        Time.timeScale = 1f; 
    }
}