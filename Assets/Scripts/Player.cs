using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

private Transform cam;
private CharacterController controller;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1f;

    private float currentVelocity;
    private Vector3 playerVelocity;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundSensor;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float sensorRadius = 0.2f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (movement != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref currentVelocity, smoothTime);

            transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward; 
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        isGrounded = Physics.CheckSphere(groundSensor.position, sensorRadius, groundLayer);

        if (playerVelocity.y < 0 && isGrounded)
        {
            playerVelocity.y = 0;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
}


