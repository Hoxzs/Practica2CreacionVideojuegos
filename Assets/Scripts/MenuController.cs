using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Iniciando juego...");
        // Usamos el nombre exacto. ¡Ojo con las mayúsculas!
        SceneManager.LoadScene("Juego"); 
    }
}