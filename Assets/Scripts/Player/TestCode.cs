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
        float horizontal = Input.GetAxis("Horizontal"); // A, D (좌우)
        float vertical = Input.GetAxis("Vertical"); // W, S (앞뒤)

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical);

        if (moveDirection.magnitude > 0)
        {
            // 구체의 이동 방향에 따라 회전 축을 동적으로 계산
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, moveDirection.normalized);

            // 물리적인 굴림 적용
            rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
            rb.AddTorque(rotationAxis * rotationSpeed, ForceMode.Acceleration);
        }
    }
}