using UnityEngine;

public class LastTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform teleportTarget;       // �÷��̾ �̵���ų ��ǥ ��ġ
    public GameObject warningPanel;        // ��� �г� (UI)

    [Header("Audio Settings")]
    public AudioClip lackPieces;
    public float soundVolume;

    AudioSource audioSource;

    PieceCollector pieceCollector; // PieceCollector ��ũ��Ʈ ����

    void Start()
    {
        // PieceCollector ��ũ��Ʈ ã�ƿ���
        pieceCollector = FindObjectOfType<PieceCollector>();

        // ��� �г��� ó������ ��Ȱ��ȭ
        warningPanel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();


    }

    void OnTriggerEnter(Collider other)
    {
        // �÷��̾ �浹���� ���� ����
        if (other.CompareTag("Player") && teleportTarget != null)
        {
            if (pieceCollector != null && pieceCollector.HasAllPieces())
            {
                // ��� ������ ������� �����̵�
                other.transform.position = teleportTarget.position;
                Debug.Log("�ڷ���Ʈ Ȱ��ȭ");
            }
            else
            {
                // ���� ������ �� ������ �ʾҴٸ� �г� ǥ��
                if (warningPanel != null)
                    warningPanel.SetActive(true);

                // �� ������ ���ϸ� ���� ���
                audioSource.PlayOneShot(lackPieces, soundVolume);

                Debug.Log("������ �� ������ ���߽��ϴ�!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // �÷��̾ Ʈ���ſ��� ������ ��� �г� ��Ȱ��ȭ
        if (other.CompareTag("Player"))
        {
            if (warningPanel != null)
                warningPanel.SetActive(false);
        }
    }
}
