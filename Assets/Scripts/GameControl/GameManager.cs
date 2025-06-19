using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Count Count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var newText = GameObject.Find("Count")?.GetComponent<TextMeshProUGUI>();
        if (newText != null)
        {
            Count.SetTextUI(newText);
        }

        var playerHeart = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerHeart>();
        if (playerHeart != null)
        {
            playerHeart.InitHpUI();
        }
    }

    public void StartDelayedCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
