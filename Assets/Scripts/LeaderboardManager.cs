using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private Transform entriesParent;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private Button backButton;

    private void Start()
    {
        backButton.onClick.AddListener(ReturnToMainMenu);
        LoadLeaderboard();
    }

    private void LoadLeaderboard()
    {
        foreach (Transform child in entriesParent)
        {
            Destroy(child.gameObject);
        }

        List<LeaderboardEntry> scores = Leaderboard.LoadScores();
        for (int i = 0; i < scores.Count; i++)
        {
            CreateLeaderboardEntry(i + 1, scores[i]);
        }
    }

    private void CreateLeaderboardEntry(int rank, LeaderboardEntry entry)
    {
        GameObject entryGO = Instantiate(entryPrefab, entriesParent);
        Text[] texts = entryGO.GetComponentsInChildren<Text>();
        
        if (texts.Length >= 3)
        {
            texts[0].text = $"{rank}."; 
            texts[1].text = entry.playerName;     
            texts[2].text = entry.score.ToString();
        }
        else
        {
            Debug.LogError("Entry prefab doesn't have enough Text components!");
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}