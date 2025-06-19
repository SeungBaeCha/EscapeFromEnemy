using TMPro;
using UnityEngine;

public class Count : MonoBehaviour
{
    public TextMeshProUGUI Text; // UI �ؽ�Ʈ ����
    public int Counts = -1;      // ���� ���� ����
    public int totalCount;       // ��ü ����

    void OnEnable()
    {
        UpdateText();
    }

    // ������ ������ �� ȣ��
    public void Collection()
    {
        if (Counts < totalCount)
        {
            Counts++;
            UpdateText();
        }
    }

    // UI �ؽ�Ʈ ������Ʈ
    void UpdateText()
    {
        if (Text != null)
            Text.text = $"{Counts} / {totalCount}";
    }

    // �� �ε� �� ���ο� Text ������ ���޹��� �� ȣ��
    public void SetTextUI(TextMeshProUGUI newText)
    {
        Text = newText;
        UpdateText();
    }
}