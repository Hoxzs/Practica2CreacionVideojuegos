using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator personajeAnimator;
    public Transform eje;

    public float velocidad;
    public float distanciaSuelo;
    public Vector3 offsetRaycast = Vector3.zero;
    public bool inGround;
    private RaycastHit hit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out hit, distanciaSuelo))
        {
            inGround = true;
        }
        else
        {
            inGround = false;
        }
    }
    void FixedUpdate()
    {
        Movimiento();
    }

    public void Movimiento()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(horizontal, 0, vertical).normalized;

        if(input.magnitude > 0)
        {
            Vector3 direccion = eje.TransformDirection(input);
            direccion.y = 0;

            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Lerp(transform.rotation,rotacionObjetivo, 0.3f);
            Vector3 nuevaVelocidad = direccion * velocidad * Time.fixedDeltaTime;
            nuevaVelocidad.y = rb.velocity.y;
            rb.velocity = nuevaVelocidad;
            personajeAnimator.SetBool("correr", true);

        }
        else
        {
            if(inGround)
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }
            personajeAnimator.SetBool("correr", false);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position + offsetRaycast, Vector3.down * distanciaSuelo);
    }
}
