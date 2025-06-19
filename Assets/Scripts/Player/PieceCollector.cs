using UnityEngine;

public class PieceCollector : MonoBehaviour
{
    public int totalPieces;       // 모아야 할 개수 
    int currentPieces = 0;

    public PortalControl portalControl; // 포탈 스크립트 연결

    public void CollectPiece()
    {
        currentPieces++;

        if (currentPieces >= totalPieces)
        {
            portalControl.ActivatePortal(); // 포탈 생성
        }
    }

    // 조각을 모두 모았는지 확인하는 함수
    public bool HasAllPieces()
    {
        return currentPieces >= totalPieces;
    }
}
