using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    
    public Animator ani; // Arrastra el Animator del Padre aquí

    void OnTriggerEnter(Collider other)
    {
        // Verifica si colisiona con el Jugador
        if (other.CompareTag("Jugador") || other.CompareTag("Player")) 
        {
            // Activa animación de ataque
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true);

            // Variable estática del script Enemigo corregido
            Enemigo.atacando = true;

            // Desactiva este collider para no atacar infinitamente al instante
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}