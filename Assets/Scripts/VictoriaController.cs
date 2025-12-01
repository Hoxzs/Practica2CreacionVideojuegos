using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;  // Necesario para trabajar con TextMeshPro

public class VictoriaController : MonoBehaviour
{
    public TMP_Text buttonText; // Referencia al componente TMP_Text

    void Start()
{
    // Actualizamos el texto del botón cuando se carga la escena de Victoria
}

    // Método para regresar al menú
    public void RegresarAlMenu()
    {
        SceneManager.LoadScene("Menu"); // Cargar la escena del menú
    }
}
