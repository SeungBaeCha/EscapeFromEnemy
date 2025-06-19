using UnityEngine;

public class RespawnPlayer : MonoBehaviour
{
    public float fallThreshold; // 낙사 기준 높이
    public Vector3 backupRespawnPosition; // 초기 리스폰 위치 (기본값)
    public LayerMask groundLayer;      // 바닥만 감지하기 위한 레이어 마스크

    private Vector3 lastSafePosition;

    void Start()
    {
        lastSafePosition = transform.position; // 시작 시 위치를 안전지점으로 저장
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

        Debug.DrawRay(rayOrigin, Vector3.down * rayDistance, Color.red); // 디버깅용 Ray

        return Physics.Raycast(rayOrigin, Vector3.down, rayDistance, groundLayer);
    }

    void Respawn()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero; // 낙하 속도 제거

        // 안전한 위치가 없으면 백업 위치로 복귀
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
        Gizmos.DrawSphere(lastSafePosition, 0.3f); // 안전 지점 표시 (Scene 뷰에서 확인 가능)
    }
}
