using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerControls : MonoBehaviour
{
    private Rigidbody2D rb;
    private AudioSource src;
    private bool isControlLock = false;

    private const float ROTATION_SPEED = 1.0f;
    private const float JET_FORCE = 10.0f;

    private const float WIN_SPEED_THRESHOLD = 3.0f;
    private const float WIN_ANGLE_THRESHOLD = 0.3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        src = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (!isControlLock)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(ROTATION_SPEED);
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow))
            {
                rb.AddTorque(-ROTATION_SPEED);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Space))
            {
                rb.AddForce(transform.up * JET_FORCE);
            }

            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private bool IsWinningAngle()
    {
        return transform.rotation.z < WIN_ANGLE_THRESHOLD && transform.rotation.z > -WIN_ANGLE_THRESHOLD;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Map") || (collision.gameObject.CompareTag("Finish") && !isControlLock))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            //Debug.Log(rb.velocity.magnitude);
            //Debug.Log(transform.rotation.z);
            if (rb.velocity.magnitude < WIN_SPEED_THRESHOLD && IsWinningAngle())
            {
                Debug.Log("You Win");
                isControlLock = true;
                src.Play();
            }
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
