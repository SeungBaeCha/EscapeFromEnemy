using UnityEngine;

public class Piece : MonoBehaviour
{
    public GameObject panel;
    public bool isCollected = false;

    bool panelActive = false;

    [Header("Activate Sound Settings")]
    public AudioClip activateSound;
    public float portalSoundVolume;

    private AudioSource audioSource;

    void Start()
    {
        panel.SetActive(false);

        // AudioSource 컴포넌트 자동 연결 또는 동적으로 생성
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void ActivatePortal()
    {
        panel.SetActive(true);
        panelActive = true;

        // 게임 일시정지
        Time.timeScale = 0f;

        // 마우스 커서 보이게
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 효과음 재생
        if (activateSound != null)
        {
            audioSource.PlayOneShot(activateSound, portalSoundVolume);
        }
    }

    void Update()
    {
        if (panelActive && Input.GetMouseButtonDown(0))
        {
            panel.SetActive(false);
            panelActive = false;

            // 게임 재개
            Time.timeScale = 1f;

            // 마우스 커서 다시 잠금 + 숨김
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
