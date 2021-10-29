using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float moveSpeed = 12f;
    public float gravity = -9.81f;
    Vector3 velocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    private AudioSource stepSound;
    private float stepLen = 0.1f, x, z;

    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        stepSound = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (x < -stepLen || x > stepLen || z < -stepLen || z > stepLen)
        {
            if (!stepSound.isPlaying)
            {
                stepSound.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    public void TakeDamage(int amountDamage)
    {
        currentHealth -= amountDamage;
        healthBar.SetHealth(currentHealth);
    
        if (currentHealth <= 0)
        {
            // Player DIE
        }
    }
}
