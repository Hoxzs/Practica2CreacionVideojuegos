using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    // Variables para rutina
    private int rutina;
    private float cronometro;
    private Animator aniEnemigo;
    public float velWalk;
    public float velRun;

    // Variables para la detección del jugador
    public LayerMask capaDelJugador;
    private bool estarAlerta;
    public float rangoDeAlerta;
    private bool atacar;
    public float rangoAtaque;
    private Transform jugador;

    // VFX
    public GameObject efectoVFX;  // Efecto de explosión asignado

    // SFX
    private AudioSource fuente;
    public AudioClip clipDie;  // Este sonido ya no lo usaremos, pero lo dejamos para referencia

    public static bool atacando;

    public GameObject rango;

    private Rigidbody rb;

    // Salud del enemigo
    public int vidas;

    // Variables necesarias para el enemigo
    public float dañoAlJugador = 10f; // Daño que el enemigo inflige al jugador
    public float tiempoEntreAtaques = 1f; // Tiempo entre ataques para evitar ataques continuos

    private float cronometroAtaque = 0f;  // Cronómetro para controlar los ataques

    void Start()
    {
        fuente = GameObject.Find("SFX").GetComponent<AudioSource>();
        aniEnemigo = GetComponent<Animator>();
        jugador = GameObject.Find("Personaje").transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ComportamientoEnemigo();
        cronometroAtaque += Time.deltaTime;
    }

    void FixedUpdate()
    {
        MovimientoFisico();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("Proyectil"))
        {
            TakeDamage(1); // Llamar a TakeDamage con el valor del daño
            Destroy(other.gameObject); // Destruir el proyectil
        }
    }

    // Método para reducir la vida del enemigo
// Método para reducir la vida del enemigo
public void TakeDamage(int damage)
{
    // Reducir las vidas primero
    vidas -= damage;

    // Mostrar el mensaje de vida restante después de restar el daño
    Debug.Log("Enemigo ha recibido daño. Vida restante: " + vidas);

    // Verificar si el enemigo ha muerto
    if (vidas <= 0)
    {
        Debug.Log("Enemigo muerto");
        Die(); // Llamar a Die para destruir el enemigo
    }
}


    // Matar al enemigo (sin animación ni sonido)
    private void Die()
    {
        // Efecto de partículas (explosión, etc.)
        efectoParticulas();

        // Destruir al enemigo inmediatamente
        Destroy(gameObject); // El enemigo se destruye inmediatamente

        // Log de destrucción
        Debug.Log("Enemigo destruido sin animación.");
    }

    // Efectos de partículas cuando muere
    void efectoParticulas()
    {
        // Verifica si el VFX está asignado
        if (efectoVFX != null)
        {
            // Instancia el efecto en la posición y rotación del enemigo
            Instantiate(efectoVFX, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No se ha asignado un efecto de partículas en el campo 'efectoVFX'.");
        }
    }

    public void ComportamientoEnemigo()
    {
        estarAlerta = Physics.CheckSphere(transform.position, rangoDeAlerta, capaDelJugador);

        if (!estarAlerta)
        {
            atacar = false;
            aniEnemigo.SetBool("run", false);
            cronometro += Time.deltaTime;

            if (cronometro >= 4)
            {
                rutina = Random.Range(0, 2);
                cronometro = 0;
            }

            switch (rutina)
            {
                case 0:
                    aniEnemigo.SetBool("walk", false);
                    break;

                case 1:
                    transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    rutina++;
                    break;

                case 2:
                    aniEnemigo.SetBool("walk", true);
                    break;
            }
        }
        else
        {
            Vector3 posJugador = new Vector3(jugador.position.x, transform.position.y, jugador.position.z);
            transform.rotation = Quaternion.LookRotation(posJugador - transform.position);

            atacar = Physics.CheckSphere(transform.position, rangoAtaque, capaDelJugador);

            if (!atacar && !atacando)
            {
                aniEnemigo.SetBool("walk", false);
                aniEnemigo.SetBool("run", true);
                aniEnemigo.SetBool("attack", false);
            }
            else
            {
                aniEnemigo.SetBool("walk", false);
                aniEnemigo.SetBool("run", false);
            }
        }
    }

    void MovimientoFisico()
    {
        if (atacando)
        {
            rb.velocity = Vector3.zero;
            return;
        }

        if (!estarAlerta)
        {
            if (rutina == 2)
            {
                rb.velocity = transform.forward * velWalk;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            if (!atacar)
            {
                Vector3 dir = (jugador.position - transform.position).normalized;
                dir.y = 0;
                rb.velocity = dir * velRun;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
    }

    // Método que se llama cuando termina la animación de ataque
    public void finAni()
    {
        aniEnemigo.SetBool("attack", false);
        atacando = false;
        rango.GetComponent<CapsuleCollider>().enabled = true;
    }

    // Este método será llamado desde el evento de la animación de ataque
    public void InflicarDaño()
    {
        Debug.Log("Evento ejecutado correctamente");

        float distancia = Vector3.Distance(transform.position, jugador.position);
        Debug.Log("Distancia:" + distancia);

        if (distancia < 2.5f)
        {
            Debug.Log("Aplicando daño…");
            jugador.GetComponent<PersonajeController>().TomarDaño(dañoAlJugador);
        }
        else
        {
            Debug.Log("Jugador fuera de rango");
        }
    }
}
