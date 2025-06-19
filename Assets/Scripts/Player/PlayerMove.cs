using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Teleport Settings")]
    public Vector3 initialPosition;
    public float lastTeleportTime;

    [Header("Sound Settings")]
    public AudioClip jumpSound;
    [Range(0f, 1f)] public float jumpVolume = 0.7f;

    public AudioClip interactSound;
    [Range(0f, 1f)] public float interactVolume = 0.7f;

    private AudioSource audioSource;

    Vector2 inputVector;
    Vector3 moveVector;

    bool isGrounded;
    bool isGetted;

    PlayerInputActions playerInput;
    Rigidbody rb;
    ToolTipTutorial toolTip;
    Piece currentPiece;
    PieceCollector collector;

    private void Awake()
    {
        playerInput = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
        collector = GetComponent<PieceCollector>();
        initialPosition = transform.position;

        // ✅ 오디오 소스 추가
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void FixedUpdate()
    {
        Vector3 moveDir = transform.TransformDirection(moveVector.normalized);
        Vector3 targetPosition = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        inputVector = value.ReadValue<Vector2>();
        moveVector = new Vector3(inputVector.x, 0f, inputVector.y);
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.performed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // 점프 사운드 재생
            if (jumpSound != null)
                audioSource.PlayOneShot(jumpSound, jumpVolume);
        }
    }

    public void OnInteraction(InputAction.CallbackContext value)
    {
        if (value.performed && isGetted && toolTip != null)
        {
            // 이미 수집된 조각이면 아무것도 하지 않음
            if (currentPiece != null && currentPiece.isCollected)
                return;

            // 조각 상호작용 수행
            toolTip.TryCollectPiece(collector);
            currentPiece = null;
            isGetted = false;

            // 상호작용 사운드 재생 (수집되지 않은 경우에만)
            if (interactSound != null)
                audioSource.PlayOneShot(interactSound, interactVolume);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fragment"))
        {
            isGetted = true;
            toolTip = other.GetComponent<ToolTipTutorial>();
            currentPiece = other.GetComponent<Piece>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Fragment"))
        {
            isGetted = false;
            if (toolTip != null)
            {
                toolTip.HideTooltip();
                toolTip = null;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
