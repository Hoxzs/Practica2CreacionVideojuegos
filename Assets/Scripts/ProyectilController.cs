using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilController : MonoBehaviour
{
    public float velocidad = 10f;
    public float distancia = 20f;
    public int daño = 1; // Daño del proyectil

    private Transform jugador;

    void Start()
    {
        jugador = GameObject.Find("Personaje").transform;
    }

    void Update()
    {
        Movimiento();
        DestruirPorDistancia();
    }

    void Movimiento()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    void DestruirPorDistancia()
    {
        if (Vector3.Distance(jugador.position, transform.position) > distancia)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemigo"))
        {
            // Detectar el script Enemigo en el objeto o en sus padres
            Enemigo enemigo = other.GetComponentInParent<Enemigo>();

            if (enemigo != null)
            {
                enemigo.TakeDamage(daño);
            }
            else
            {
                Debug.LogWarning("Proyectil tocó un objeto con TAG 'Enemigo', pero no tiene script Enemigo en el mismo objeto ni en sus padres.");
            }

            Destroy(gameObject); // destruir solo el proyectil
        }
    }
}
