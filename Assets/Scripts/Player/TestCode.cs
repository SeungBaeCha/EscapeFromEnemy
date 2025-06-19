using UnityEngine;

public class TestCode : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A, D (�¿�)
        float vertical = Input.GetAxis("Vertical"); // W, S (�յ�)

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection.magnitude > 0)
        {
            // ��ü�� �̵� ���⿡ ���� ȸ�� ���� �������� ���
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection.normalized);

            // �������� ���� ����
            rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
            rb.AddTorque(rotationAxis * rotationSpeed, ForceMode.Acceleration);
        }
    }
}