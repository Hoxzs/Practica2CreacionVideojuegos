using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // --- VARIABLE ESTÁTICA ---
    // (La mantenemos para que tu script RangoEnemigo no de error, 
    // aunque la lógica principal ahora la maneja este script por distancia)
    public static bool atacando; 

    // --- REFERENCIAS ---
    private Animator aniEnemigo;
    private Transform jugador;
    private Rigidbody rb;
    public GameObject efectoVFX;

    // --- CONFIGURACIÓN ---
    public int vidas = 3;
    public float rangoDeAlerta = 15f;
    public float rangoAtaque = 2.5f; // Distancia para empezar a golpear
    public float velRun = 4f;
    
    // --- DAÑO ---
    public int dañoAlJugador = 10;
    
    // Control de tiempo para el daño (Cooldown)
    private float ultimoAtaqueTime = 0;
    public float cooldownAtaque = 1.0f; // Tiempo mínimo entre daños

    void Start()
    {
        aniEnemigo = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        
        // Buscar al jugador de forma segura
        GameObject playerObj = GameObject.Find("Personaje");
        if (playerObj != null) 
            jugador = playerObj.transform;

        // Registrar este enemigo en el conteo de la fábrica
        GameController.enemigosVivosActualmente++;
    }

    void Update()
    {
        // Si no hay jugador, no hacemos nada
        if (jugador == null) return;
        
        float distancia = Vector3.Distance(transform.position, jugador.position);

        // --- LÓGICA DE PERSECUCIÓN ---
        if (distancia < rangoDeAlerta)
        {
            // Girar para mirar al jugador
            Vector3 targetPostition = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
            transform.LookAt(targetPostition);

            if (distancia <= rangoAtaque)
            {
                // --- ESTÁ EN RANGO DE ATAQUE ---
                // 1. Frenar
                rb.velocity = Vector3.zero;
                
                // 2. Activar animación
                aniEnemigo.SetBool("walk", false);
                aniEnemigo.SetBool("run", false);
                aniEnemigo.SetBool("attack", true);
                
                // Nota: NO llamamos a hacer daño aquí. 
                // Esperamos a que la Animación llame a 'InflicarDaño' mediante el Evento.
            }
            else
            {
                // --- ESTÁ LEJOS: PERSEGUIR ---
                aniEnemigo.SetBool("attack", false); 
                aniEnemigo.SetBool("run", true);
                aniEnemigo.SetBool("walk", false);
                
                // Moverse hacia el jugador
                Vector3 dir = (jugador.position - transform.position).normalized;
                rb.velocity = dir * velRun;
            }
        }
        else
        {
            // --- IDLE (JUGADOR MUY LEJOS) ---
            aniEnemigo.SetBool("run", false);
            aniEnemigo.SetBool("attack", false);
            rb.velocity = Vector3.zero;
        }
    }

    // ---------------------------------------------------------
    // ESTA FUNCIÓN SE EJECUTA SOLO CUANDO LA ANIMACIÓN LO ORDENA
    // (Debes configurar el Evento en la ventana Animation)
    // ---------------------------------------------------------
    public void InflicarDaño()
    {
        // 1. Revisar si ya pasó el tiempo de espera (Cooldown)
        if (Time.time < ultimoAtaqueTime + cooldownAtaque) return;

        // 2. Revisar si el jugador sigue cerca (por si esquivó)
        if (jugador == null) return;
        
        float distancia = Vector3.Distance(transform.position, jugador.position);
        
        // Damos un pequeño margen extra (0.5f) para que el golpe conecte si está casi en rango
        if (distancia <= rangoAtaque + 0.5f) 
        {
            var pj = jugador.GetComponent<PersonajeController>();
            if (pj != null)
            {
                pj.TomarDaño(dañoAlJugador);
                Debug.Log("¡GOLPE CONECTADO!");
                
                // Reiniciamos el reloj del cooldown
                ultimoAtaqueTime = Time.time; 
            }
        }
    }

    // --- RECIBIR DAÑO Y MORIR ---
    public void TakeDamage(int damage)
    {
        vidas -= damage;
        if (vidas <= 0) Die();
    }

    private void Die()
    {
        // Actualizar el GameController para la victoria
        GameController.enemigosDerrotados++;
        GameController.enemigosVivosActualmente--;

        // Efectos visuales
        if (efectoVFX != null) 
            Instantiate(efectoVFX, transform.position, Quaternion.identity);
            
        Destroy(gameObject);
    }
}