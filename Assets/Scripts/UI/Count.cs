using TMPro;
using UnityEngine;

public class Count : MonoBehaviour
{
    public TextMeshProUGUI Text; // UI 텍스트 참조
    public int Counts = -1;      // 현재 모은 개수
    public int totalCount;       // 전체 개수

    void OnEnable()
    {
        UpdateText();
    }

    // 조각을 수집할 때 호출
    public void Collection()
    {
        if (Counts < totalCount)
        {
            Counts++;
            UpdateText();
        }
    }

    // UI 텍스트 업데이트
    void UpdateText()
    {
        if (Text != null)
            Text.text = $"{Counts} / {totalCount}";
    }

    // 씬 로드 후 새로운 Text 참조를 전달받을 때 호출
    public void SetTextUI(TextMeshProUGUI newText)
    {
        Text = newText;
        UpdateText();
    }
}