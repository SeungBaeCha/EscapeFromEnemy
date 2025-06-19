using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshPro textMeshPro;           // �ؽ�Ʈ ���
    public float countdownTime = 10f;         // ���� �ð�

    public PlayerMove player;                 // �÷��̾� ��ũ��Ʈ ����
    public GameObject targetPiece;            // �ð��� ������ Ȱ��ȭ�Ǵ� ���� ������Ʈ

    bool timerStarted = false;


    float originalMoveSpeed;
    float originalJumpForce;

    void Start()
    {
        targetPiece.SetActive(false); // ���� ��Ȱ��ȭ
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

        // �÷��̾� �ɷ��� �� ������ ����
        originalMoveSpeed = player.moveSpeed;       // playerMove ��ũ��Ʈ �� moveSpeed������ originalMoveSpeed�� ����
        originalJumpForce = player.jumpForce;       // playerMove ��ũ��Ʈ �� jumpForce������ originalJumpForce�� ����

        player.moveSpeed *= 0.1f;
        player.jumpForce *= 0.2f;

        while (timeRemaining > 0)
        {
            textMeshPro.text = $"����� {Mathf.CeilToInt(timeRemaining)} �� ��\n���� �� �ֽ��ϴ�.\n\n�� ���ߺ�����.";
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }

        // �÷��̾� �ɷ� ����
        player.moveSpeed = originalMoveSpeed;       
        player.jumpForce = originalJumpForce;

        // ���� Ȱ��ȭ
        targetPiece.SetActive(true);

        textMeshPro.text = "���� ���� �� �ֽ��ϴ�!\n\n�ȳ��� ���ʽÿ�.";
    }
}
