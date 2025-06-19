using UnityEngine;

public class LastTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;       // 플레이어를 이동시킬 목표 위치
    public GameObject warningPanel;        // 경고 패널 (UI)

    [Header("Audio Settings")]
    public AudioClip lackPieces;
    public float soundVolume;

    AudioSource audioSource;

    PieceCollector pieceCollector; // PieceCollector 스크립트 참조

    void Start()
    {
        // PieceCollector 스크립트 찾아오기
        pieceCollector = FindObjectOfType<PieceCollector>();

        // 경고 패널을 처음에는 비활성화
        warningPanel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();


    }

    void OnTriggerEnter(Collider other)
    {
        // 플레이어가 충돌했을 때만 동작
        if (other.CompareTag("Player") && teleportTarget != null)
        {
            if (pieceCollector != null && pieceCollector.HasAllPieces())
            {
                // 모든 조각을 모았으면 순간이동
                other.transform.position = teleportTarget.position;
                Debug.Log("텔레포트 활성화");
            }
            else
            {
                // 아직 조각을 다 모으지 않았다면 패널 표시
                if (warningPanel != null)
                    warningPanel.SetActive(true);

                // 다 모으지 못하면 사운드 재생
                audioSource.PlayOneShot(lackPieces, soundVolume);

                Debug.Log("조각을 다 모으지 못했습니다!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // 플레이어가 트리거에서 나가면 경고 패널 비활성화
        if (other.CompareTag("Player"))
        {
            if (warningPanel != null)
                warningPanel.SetActive(false);
        }
    }
}
