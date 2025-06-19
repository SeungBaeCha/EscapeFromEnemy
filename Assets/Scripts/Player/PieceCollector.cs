using UnityEngine;

public class PieceCollector : MonoBehaviour
{
    public int totalPieces;       // ��ƾ� �� ���� 
    int currentPieces = 0;

    public PortalControl portalControl; // ��Ż ��ũ��Ʈ ����

    public void CollectPiece()
    {
        currentPieces++;

        if (currentPieces >= totalPieces)
        {
            portalControl.ActivatePortal(); // ��Ż ����
        }
    }

    // ������ ��� ��Ҵ��� Ȯ���ϴ� �Լ�
    public bool HasAllPieces()
    {
        return currentPieces >= totalPieces;
    }
}
