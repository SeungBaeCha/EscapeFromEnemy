using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshPro textMeshPro;           // 텍스트 출력
    public float countdownTime = 10f;         // 제한 시간

    public PlayerMove player;                 // 플레이어 스크립트 참조
    public GameObject targetPiece;            // 시간이 지나야 활성화되는 조각 오브젝트

    bool timerStarted = false;


    float originalMoveSpeed;
    float originalJumpForce;

    void Start()
    {
        targetPiece.SetActive(false); // 조각 비활성화
    }

    void OnTriggerEnter(Collider other)
    {
        if (!timerStarted && other.CompareTag("Player"))
        {
            timerStarted = true;
            StartCoroutine(StartCountdown());
        }
    }

    IEnumerator StartCountdown()
    {
        float timeRemaining = countdownTime;

        // 플레이어 능력을 새 변수로 저장
        originalMoveSpeed = player.moveSpeed;       // playerMove 스크립트 안 moveSpeed변수를 originalMoveSpeed로 지정
        originalJumpForce = player.jumpForce;       // playerMove 스크립트 안 jumpForce변수를 originalJumpForce로 지정

        player.moveSpeed *= 0.1f;
        player.jumpForce *= 0.2f;

        while (timeRemaining > 0)
        {
            textMeshPro.text = $"당신은 {Mathf.CeilToInt(timeRemaining)} 초 후\n나갈 수 있습니다.\n\n잘 버텨보세요.";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        // 플레이어 능력 복구
        player.moveSpeed = originalMoveSpeed;       
        player.jumpForce = originalJumpForce;

        // 조각 활성화
        targetPiece.SetActive(true);

        textMeshPro.text = "이제 나갈 수 있습니다!\n\n안녕히 가십시오.";
    }
}
