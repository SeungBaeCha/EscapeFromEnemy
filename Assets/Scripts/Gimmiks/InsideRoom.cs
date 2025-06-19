using UnityEngine;
using System.Collections;
using TMPro;

public class InsideRoom : MonoBehaviour
{
    [Header("Teleport Settings")]
    public GameObject countdownPanel;
    public TMP_Text countdownText;

    
    [Header("CoolTime Setting")]
    public bool isExitTrigger = false;
    public Transform destination;
    public float cooldownTime; // 여기에 입력한 20초를 그대로 사용






    private Coroutine countdownCoroutine;
    private bool isCounting = false;
    private int currentTime; // 현재 카운트다운 시간

    private void Start()
    {
        if (countdownPanel != null)
        {
            countdownPanel.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove playerMove = other.GetComponent<PlayerMove>();   //player의 PlayerMove 스크립트 가져오기
            if (playerMove != null)
            {
                //timeSinceLastTeleport는 실제 시간 그리고 trigger의 부딪힌 lastTeleportTime를 뺀 시간이다.
                float timeSinceLastTeleport = Time.time - playerMove.lastTeleportTime; 
                if (timeSinceLastTeleport < cooldownTime)
                {
                    if (!isCounting)
                    {
                        currentTime = Mathf.CeilToInt(cooldownTime - timeSinceLastTeleport);
                        countdownCoroutine = StartCoroutine(StartCountdown());
                    }

                    if (countdownPanel != null)
                    {
                        countdownPanel.SetActive(true);

                        // 패널 켜자마자 즉시 텍스트 갱신
                        countdownText.text = $"텔레포트까지 {currentTime}초 남았습니다.";
                    }

                    return;
                }
            }

            isCounting = false;

            if (isExitTrigger)
            {
                if (playerMove != null)
                {
                    other.transform.position = playerMove.initialPosition;
                }
            }
            else
            {
                if (destination != null)
                {
                    if (playerMove != null)
                    {
                        playerMove.initialPosition = other.transform.position;
                    }
                    other.transform.position = destination.position;
                }
                else
                {
                    Debug.LogWarning("InsideRoom: destination이 지정되지 않았습니다. (" + gameObject.name + ")");
                    return;
                }
            }

            if (playerMove != null)
            {
                playerMove.lastTeleportTime = Time.time;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 패널만 비활성화
            if (countdownPanel != null)
                countdownPanel.SetActive(false);
        }
    }

    IEnumerator StartCountdown()
    {
        isCounting = true;

        while (currentTime > 0)
        {
            if (countdownPanel != null && countdownPanel.activeSelf)
            {
                countdownText.text = $"텔레포트까지 {currentTime}초 남았습니다.";
            }

            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        if (countdownPanel != null && countdownPanel.activeSelf)
        {
            countdownText.text = "텔레포트 활성화";
        }

        isCounting = false;
    }
}
