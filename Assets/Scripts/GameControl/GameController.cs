using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject panel;  // 시작 시 잠깐 보이는 패널
    public GameObject count;  // 조각 UI
    public GameObject heart;  // 체력 UI

    bool hasTriggered = false;

    void Start()
    {
        panel.SetActive(false);
        count.SetActive(false);
        heart.SetActive(false);  // 시작 시 숨겨놓음
    }

    void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "GameEnding")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Tutorial");
        AudioListener.pause = false;

    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Stage2");
    }



    public void ExitGame()
    {
        Application.Quit();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(HidePanel());
        }
    }

    IEnumerator HidePanel()
    {
        panel.SetActive(true);           // 시작 패널 활성화
        Time.timeScale = 0f;             // 게임 일시정지

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;             // 다시 시간 진행
        panel.SetActive(false);          // 패널 비활성화

        count.SetActive(true);           // 조각 UI 표시
        heart.SetActive(true);           // 체력 UI 먼저 켜기

        var playerHeart = GameObject.FindWithTag("Player")?.GetComponent<PlayerHeart>();
        if (playerHeart != null)
        {
            playerHeart.InitHpUI();      // 활성화된 상태에서 UI 초기화
        }


        GameManager.Instance.Count.Collection();  // 조각 UI 초기화

        AudioListener.pause = false;
    }
}
