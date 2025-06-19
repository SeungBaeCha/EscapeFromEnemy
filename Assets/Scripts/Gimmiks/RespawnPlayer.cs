using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public float fallThreshold; // ���� ���� ����
    public Vector3 backupRespawnPosition; // �ʱ� ������ ��ġ (�⺻��)
    public LayerMask groundLayer;      // �ٴڸ� �����ϱ� ���� ���̾� ����ũ

    private Vector3 lastSafePosition;

    void Start()
    {
        lastSafePosition = transform.position; // ���� �� ��ġ�� ������������ ����
    }

    void Update()
    {
        if (IsGrounded())
        {
            lastSafePosition = transform.position;
        }

        if (transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    bool IsGrounded()
    {
        float rayDistance = 2f;
        Vector3 rayOrigin = transform.position;

        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red); // ������ Ray

        return Physics.Raycast(rayOrigin, Vector3.down, rayDistance, groundLayer);
    }

    void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero; // ���� �ӵ� ����

        // ������ ��ġ�� ������ ��� ��ġ�� ����
        if (lastSafePosition == Vector3.zero)
        {
            transform.position = backupRespawnPosition;
        }
        else
        {
            transform.position = lastSafePosition;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(lastSafePosition, 0.3f); // ���� ���� ǥ�� (Scene �信�� Ȯ�� ����)
    }
}
