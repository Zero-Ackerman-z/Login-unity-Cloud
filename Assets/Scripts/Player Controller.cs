using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 9f;
    private Rigidbody2D rb;
    private bool isGrounded;

    public Transform groundCheck; 
    public LayerMask groundLayer;
    public Timer timer; 
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Evitar rotación en el aire
        rb.freezeRotation = true; // Evita que el Rigidbody2D rote al saltar o al moverse
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // A/D o flechas izquierda/derecha
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Cambiar la dirección del sprite sin girar al moverse
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
    }

    private void Jump()
    {
        // Comprobar si el jugador está en el suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded && Input.GetButtonDown("Jump")) // Espacio o tecla de salto
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Abismo"))
        {
            GameManager.Instance.OnTimeExpired();

        }
    }
}

