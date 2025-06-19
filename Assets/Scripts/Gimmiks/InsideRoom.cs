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
    public float cooldownTime; // ���⿡ �Է��� 20�ʸ� �״�� ���






    private Coroutine countdownCoroutine;
    private bool isCounting = false;
    private int currentTime; // ���� ī��Ʈ�ٿ� �ð�

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
            PlayerMove playerMove = other.GetComponent<PlayerMove>();   //player�� PlayerMove ��ũ��Ʈ ��������
            if (playerMove != null)
            {
                //timeSinceLastTeleport�� ���� �ð� �׸��� trigger�� �ε��� lastTeleportTime�� �� �ð��̴�.
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

                        // �г� ���ڸ��� ��� �ؽ�Ʈ ����
                        countdownText.text = $"�ڷ���Ʈ���� {currentTime}�� ���ҽ��ϴ�.";
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
                    Debug.LogWarning("InsideRoom: destination�� �������� �ʾҽ��ϴ�. (" + gameObject.name + ")");
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
            // �гθ� ��Ȱ��ȭ
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
                countdownText.text = $"�ڷ���Ʈ���� {currentTime}�� ���ҽ��ϴ�.";
            }

            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        if (countdownPanel != null && countdownPanel.activeSelf)
        {
            countdownText.text = "�ڷ���Ʈ Ȱ��ȭ";
        }

        isCounting = false;
    }
}
