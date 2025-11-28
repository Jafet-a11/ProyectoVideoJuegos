using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float moveSpeed = 5.0f;
    private Vector2 moveInput;
    private Rigidbody rb;

    [Header("Configuración de Mirada")]
    public Transform playerCamera;
    public float lookSensitivity = 2.0f;
    private Vector2 lookInput;
    private float xRotation = 0f;

    // ¡NUEVO! Variable para almacenar la rotación Y del cuerpo
    private float yBodyRotation;
    private PlayerInventory inventarioScript;

    [HideInInspector] public ItemPickup objetoCercano;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inventarioScript = GetComponent<PlayerInventory>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // ¡NUEVO! Almacenar la rotación inicial del cuerpo
        yBodyRotation = transform.eulerAngles.y;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
    public void OnInventory(InputValue value)
    {
        // Solo actuamos cuando se presiona el botón (isPressed), no al soltarlo
        if (value.isPressed)
        {
            if (inventarioScript != null)
            {
                inventarioScript.ToggleInventory();
            }
        }
    }
    void LateUpdate()
    {
        // --- Lógica de Mirada (Cámara Vertical) ---
        // Sigue en Update para máxima responsividad vertical

        // ¡OJO! Multiplicamos por Time.deltaTime para que sea independiente del framerate
        float lookY = lookInput.y * lookSensitivity * Time.deltaTime;

        // Rotación Vertical (Pitch)
        xRotation -= lookY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // ¡NUEVO! Acumulamos la rotación horizontal
        // También multiplicamos por Time.deltaTime
        float lookX = lookInput.x * lookSensitivity * Time.deltaTime;
        yBodyRotation += lookX;
    }

    void FixedUpdate()
    {
        // --- Lógica de Rotación Horizontal (Física) ---
        // Aplicamos la rotación acumulada al Rigidbody usando MoveRotation

        // Convertimos el ángulo a un Quaternion
        Quaternion targetRotation = Quaternion.Euler(0f, yBodyRotation, 0f);
        rb.MoveRotation(targetRotation); // Esto rota el cuerpo de forma segura

        // --- Lógica de Movimiento (Física) ---
        // Esto ya estaba bien, pero ahora usará la rotación actualizada de FixedUpdate
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        
        // ¡IMPORTANTE! Ya no usamos transform.TransformDirection.
        // En su lugar, rotamos la dirección de movimiento por nuestra rotación objetivo.
        moveDirection = targetRotation * moveDirection;

        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }
}