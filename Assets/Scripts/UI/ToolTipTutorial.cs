using UnityEngine;
using TMPro;
using System.Collections;

public class ToolTipTutorial : MonoBehaviour
{
    public GameObject tooltipPanel;         // ���� �г�
    public GameObject alreadyCollectedPanel; // �̹� ������ �˸� �г�
    public TMP_Text tooltipText;            // ���� �ؽ�Ʈ
    public string itemDescription;          // ������ ����

    private Piece piece;

    void Start()
    {
        piece = GetComponent<Piece>();      // �ڽ��� Piece ����
        tooltipPanel.SetActive(false);      // ���� �� ��Ȱ��ȭ
        if (alreadyCollectedPanel != null)
            alreadyCollectedPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (piece != null && piece.isCollected)
            {
                // �̹� ������ ���: "�̹� ��ȣ�ۿ� �Ǿ����ϴ�" �г� �����ֱ�
                if (alreadyCollectedPanel != null)
                {
                    alreadyCollectedPanel.SetActive(true);
                    StartCoroutine(HideAlreadyCollectedPanel());
                }
            }
            else
            {
                // ���� ���̶�� ���� ���� �����ֱ�
                ShowTooltip();
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideTooltip();
        }
    }

    public void ShowTooltip()
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = itemDescription;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    // ��ȣ�ۿ� �� ȣ��Ǵ� �Լ�
    public void TryCollectPiece(PieceCollector collector)
    {
        if (piece != null && !piece.isCollected)
        {
            // ���� ���� ó��
            GameManager.Instance.Count.Collection();
            collector.CollectPiece();
            piece.isCollected = true;

            // ��� ���� ���� �Ϸ� �� ��Ż Ȱ��ȭ
            if (collector.HasAllPieces())
            {
                Debug.Log("���� Ȱ��ȭ");
                piece.ActivatePortal();
            }


            Debug.Log("���� ���� �Ϸ�");
        }
        else
        {
            // �̹� ������ ���: "�̹� ������" �г� �����ֱ�
            if (alreadyCollectedPanel != null)
            {
                alreadyCollectedPanel.SetActive(true);
                StartCoroutine(HideAlreadyCollectedPanel());
            }
            Debug.Log("�̹� ������ �����Դϴ�.");
        }

        // ���� �����
        HideTooltip();
    }

    private IEnumerator HideAlreadyCollectedPanel()
    {
        yield return new WaitForSeconds(0.5f);
        alreadyCollectedPanel.SetActive(false);
    }
}
