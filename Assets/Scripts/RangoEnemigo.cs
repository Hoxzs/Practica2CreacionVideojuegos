using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangoEnemigo : MonoBehaviour
{
    public Animator ani;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jugador"))
        {
            ani.SetBool("walk", false);
            ani.SetBool("run", false);
            ani.SetBool("attack", true);
            Enemigo.atacando = true;
            GetComponent<CapsuleCollider>().enabled = false;
        }
    }
}
