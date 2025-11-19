using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectilController : MonoBehaviour
{
    public float velocidad;
    private Transform jugador;
    private GameObject jugadorObj;
    public float distancia;

    // Start is called before the first frame update
    void Start()
    {
        jugadorObj = GameObject.Find("Personaje");
        jugador = jugadorObj.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        movimiento();
        destuir();
    }

    public void movimiento()
    {
        transform.Translate(Vector3.forward * velocidad * Time.deltaTime);
    }

    public void destuir()
    {
        if (Vector3.Distance(jugador.transform.position, transform.position) > distancia)
            Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

        }
    }
}