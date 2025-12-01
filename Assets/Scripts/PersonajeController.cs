using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    public Rigidbody rb;
    public Animator personajeAnimator;
    public Transform eje; // Eje o cámara que define la dirección de movimiento

    public float velocidad = 5f;
    public float distanciaSuelo = 1.2f;
    public Vector3 offsetRaycast = Vector3.zero;
    public bool inGround;
    private RaycastHit hit;
    public static bool cooldown;
    //Roll
    private bool rotando;
    public float fuerzaRoll;  // qué tan fuerte empuja el roll
    private Vector3 direccionRoll;
    //Golpes
    public float tiempoComboMax = 1.5f; // tiempo máximo entre golpes
    private float tiempoComboActual = 0f;
    public int faseCombo;
    //private bool conteo;
    //Lanzar Poder
    public GameObject proyectil;
    public GameObject posProyectil;
    //salud del jugador
    public float salud = 100f;  // Salud del jugador

    void Update()
    {
       if (Physics.Raycast(transform.position, Vector3.down, out hit, distanciaSuelo))
        {
            inGround = true; // Tocó cualquier collider
        }
        else
        {
            inGround = false; // No tocó nada
        }
        gestorInput();
    }

    void FixedUpdate()
    {
        if(!cooldown)
        {
            Movimiento();
        }   

         if (rotando)
        {
            //Direccion A Rotar
            direccionRoll = transform.forward;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            // Mantiene el impulso solo mientras dura el roll
            rb.AddForce(direccionRoll * fuerzaRoll, ForceMode.VelocityChange);
        }
       
    }

    void Movimiento()
    {
        //Entrada del jugador (WASD o joystick)
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0f, vertical).normalized;

        //Si hay movimiento
        if (input.magnitude > 0)
        {
            // Direccion del movimiento relativa a la cámara
            Vector3 direccion = eje.TransformDirection(input);
            direccion.y = 0f; // evita rotaciones hacia arriba/abajo

            //Rotación suave
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotacionObjetivo, 0.3f);

            //Movimiento físico
            Vector3 nuevaVelocidad = direccion * velocidad * Time.fixedDeltaTime;
            nuevaVelocidad.y = rb.velocity.y; // mantiene gravedad
            rb.velocity = nuevaVelocidad;

            //Activar animación de correr
            personajeAnimator.SetBool("correr", true);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);    
            personajeAnimator.SetBool("correr", false);
        }
    }

    // Dibuja el raycast en el editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + offsetRaycast, Vector3.down * distanciaSuelo);
    }

    public void gestorInput()
    {
        if(!rotando)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                // Comenzamos el roll
                rotando = true;
                cooldown = true;    
                personajeAnimator.SetBool("correr", false);
                personajeAnimator.SetTrigger("roll");
            }
        }   
        // Si está dentro de la ventana de combo, cuenta el tiempo
        if (faseCombo > 0)
        {
            tiempoComboActual += Time.deltaTime;

            if (tiempoComboActual > tiempoComboMax)
            {
                // Se acabó el tiempo para encadenar golpes
                faseCombo = 0;
                tiempoComboActual = 0;
            }
        }

        if (!cooldown && Input.GetKeyDown(KeyCode.Space))
        {
            personajeAnimator.SetBool("correr", false);
            personajeAnimator.SetTrigger("roll");

            cooldown = true;
            tiempoComboActual = 0f; // reinicia el temporizador del combo

            if (faseCombo == 0)
            {
                personajeAnimator.SetFloat("ataque", 0);
            }
            else if (faseCombo == 1)
            {
                personajeAnimator.SetFloat("ataque", 1);
            }
            else if (faseCombo == 2)
            {
                personajeAnimator.SetFloat("ataque", 2);
            }

            faseCombo++;

            // Reinicia a 0 si pasa del último ataque
            if (faseCombo > 2)
            {
                faseCombo = 0;
            }  
        }   
        if(!cooldown && Input.GetKeyDown(KeyCode.Q))
        {
            cooldown = true;
            personajeAnimator.SetTrigger("lanzar");
        }
    }

    public void finEstados()
    {
        rotando = false;
        rb.velocity = Vector3.zero;
    }

    public void cooldownMoves()
    {
        Debug.Log("Congelado");
        cooldown = false;
    }

    public void crearProyectil()
    {
        GameObject instancia = Instantiate(proyectil, posProyectil.transform.position,
                                            transform.rotation);
    }
    public void TomarDaño(float cantidad)
    {
        salud -= cantidad;  // Reducir la salud del jugador

        if (salud <= 0)
        {
            MuerteJugador();  // Si la salud llega a 0 o menos, el jugador muere
        }
    }
    void MuerteJugador()
    {
        // Aquí puedes agregar la lógica para la muerte del jugador, como mostrar una pantalla de Game Over
        Debug.Log("¡El jugador ha muerto!");
        // Por ejemplo, puedes desactivar el jugador o cambiar la escena:
        // gameObject.SetActive(false);
        // o cargar una escena de Game Over
    }
    public void TomarDaño(int cantidad)
{
    // 1. Restamos la vida en el marcador global
    GameController.vidas -= cantidad;
    
    // 2. (Opcional) Mensaje en consola para verificar
    Debug.Log("¡Auch! Me golpearon. Vidas restantes: " + GameController.vidas);

    // 3. Verificamos si morimos (aunque GameController ya lo hace, es bueno tenerlo aquí para animaciones)
    if (GameController.vidas <= 0)
    {
        // Aquí podrías poner tu animación de muerte del personaje
        // GetComponent<Animator>().SetTrigger("Muerte");
        Debug.Log("El personaje ha muerto.");
    }
}
}
