using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    public CharacterController controller;

    [Header("Variable")]
    public float speed = 12f;
    public float gravity = -10f;
    public float jumpHeight = 3f;
    public float mouseSensitivity = 60f;

    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;

    Vector3 velocity;
    Vector3 move;
    public bool isGrounded;

    public Camera playerCamera;

    float xRotation = 0f;

    bool isWalking;

    public event WalkingEvent walkEvent;
    public delegate void WalkingEvent();

    void Start()
    {
        playerCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void CameraUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, GroundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (x > 0 || z > 0) {
            walkEvent?.Invoke();
        }

        move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(!isWalking) StartCoroutine(WalkSound());
    }

    void Update()
    {
        CameraUpdate();

        PlayerMovement();
    }

    IEnumerator WalkSound()
    {
        if (isWalking) { yield return null; }
        while(move != Vector3.zero && isGrounded)
        {
            isWalking = true;
            float wait = 0.4f;
            FindObjectOfType<AudioManager>().PlayFootstep();
            yield return new WaitForSeconds(wait);
            
        }
        if(move == Vector3.zero || !isGrounded)
        {
            isWalking = false;
        }
    }
}
