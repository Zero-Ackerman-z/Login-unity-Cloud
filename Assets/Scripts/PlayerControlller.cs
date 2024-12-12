using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb2;
    private bool facingRight = true; // Variable para saber si el jugador está mirando a la derecha

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        rb2.velocity = new Vector2(move * speed, rb2.velocity.y);

        // Lógica para voltear el sprite si el jugador cambia de dirección
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    // Método para voltear el sprite
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1; // Invertir la escala en el eje X para voltear el sprite
        transform.localScale = theScale;
    }
}
