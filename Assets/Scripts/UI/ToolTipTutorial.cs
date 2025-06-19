using UnityEngine;
using TMPro;
using System.Collections;

public class ToolTipTutorial : MonoBehaviour
{
    public GameObject tooltipPanel;         // 툴팁 패널
    public GameObject alreadyCollectedPanel; // 이미 수집된 알림 패널
    public TMP_Text tooltipText;            // 설명 텍스트
    public string itemDescription;          // 아이템 설명

    private Piece piece;

    void Start()
    {
        piece = GetComponent<Piece>();      // 자신의 Piece 참조
        tooltipPanel.SetActive(false);      // 시작 시 비활성화
        if (alreadyCollectedPanel != null)
            alreadyCollectedPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (piece != null && piece.isCollected)
            {
                // 이미 수집된 경우: "이미 상호작용 되었습니다" 패널 보여주기
                if (alreadyCollectedPanel != null)
                {
                    alreadyCollectedPanel.SetActive(true);
                    StartCoroutine(HideAlreadyCollectedPanel());
                }
            }
            else
            {
                // 수집 전이라면 기존 툴팁 보여주기
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

    // 상호작용 시 호출되는 함수
    public void TryCollectPiece(PieceCollector collector)
    {
        if (piece != null && !piece.isCollected)
        {
            // 조각 수집 처리
            GameManager.Instance.Count.Collection();
            collector.CollectPiece();
            piece.isCollected = true;

            // 모든 조각 수집 완료 시 포탈 활성화
            if (collector.HasAllPieces())
            {
                Debug.Log("전부 활성화");
                piece.ActivatePortal();
            }


            Debug.Log("조각 수집 완료");
        }
        else
        {
            // 이미 수집된 경우: "이미 수집됨" 패널 보여주기
            if (alreadyCollectedPanel != null)
            {
                alreadyCollectedPanel.SetActive(true);
                StartCoroutine(HideAlreadyCollectedPanel());
            }
            Debug.Log("이미 수집된 조각입니다.");
        }

        // 툴팁 숨기기
        HideTooltip();
    }

    private IEnumerator HideAlreadyCollectedPanel()
    {
        yield return new WaitForSeconds(0.5f);
        alreadyCollectedPanel.SetActive(false);
    }
}
