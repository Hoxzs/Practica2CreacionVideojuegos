using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fabricaController : MonoBehaviour
{
    [Header("Configuración de la Oleada")]
    // AQUÍ ES DONDE TÚ DEFINES LA CANTIDAD EN EL INSPECTOR
    // No tiene valor fijo en el código, depende de lo que pongas en Unity
    public int cantidadTotalEnemigos;      // Ej: 10, 20, 50...
    public int maxEnemigosSimultaneos;     // Ej: 3 activos a la vez
    public float tiempoEntreIntentos = 2.0f; // Cada cuánto intenta sacar uno

    [Header("Referencias")]
    public GameObject[] prefabsEnemigos; // Arrastra tus prefabs aquí

    // Variables internas (privadas) para control
    private int enemigosGeneradosHastaAhora = 0;

    void Start()
    {
        // PASO 1: COMUNICAR LA META AL JUEGO
        // La fábrica lee el número que tú pusiste en el Inspector (cantidadTotalEnemigos)
        // y se lo impone al GameController como la meta para ganar.
        
        GameController gameController = FindFirstObjectByType<GameController>();
        if (gameController != null)
        {
            // Sobrescribimos la meta del juego con EL NÚMERO DE LA FÁBRICA
            gameController.metaParaGanar = cantidadTotalEnemigos;
            
            Debug.Log("Fábrica configurada: Se generarán " + cantidadTotalEnemigos + " enemigos en total.");
        }
        else
        {
            Debug.LogError("¡No se encontró el GameController! La victoria no funcionará bien.");
        }

        // Iniciamos el ciclo de generación
        InvokeRepeating("GestionarSpawn", 2.0f, tiempoEntreIntentos);
    }

    void GestionarSpawn()
    {
        // 1. Si el juego terminó (victoria o derrota), paramos la máquina
        if (GameController.juegoTerminado)
        {
            CancelInvoke();
            return;
        }

        // 2. LÍMITE TOTAL: ¿Ya generamos todos los que pediste en el Inspector?
        if (enemigosGeneradosHastaAhora >= cantidadTotalEnemigos)
        {
            // Ya cumplimos la cuota de producción. No cancelamos el Invoke
            // porque la fábrica se queda "en espera" por si acaso, 
            // pero ya no entra al método Generar().
            return;
        }

        // 3. LÍMITE EN PANTALLA: ¿Hay demasiados enemigos vivos ahora mismo?
        // Usamos la variable que lleva el GameController
        if (GameController.enemigosVivosActualmente >= maxEnemigosSimultaneos)
        {
            // Hay mucho tráfico, esperamos al siguiente ciclo (2 segs)
            return;
        }

        // Si pasamos los filtros, creamos el enemigo
        GenerarEnemigo();
    }

    void GenerarEnemigo()
    {
        if (prefabsEnemigos.Length == 0) return;

        // Elegir aleatorio
        int indice = Random.Range(0, prefabsEnemigos.Length);
        
        // Crear
        Instantiate(prefabsEnemigos[indice], transform.position, prefabsEnemigos[indice].transform.rotation);

        // Aumentar contador interno
        enemigosGeneradosHastaAhora++;
    }
}