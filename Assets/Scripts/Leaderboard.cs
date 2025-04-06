using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
}

public static class Leaderboard
{
    private const string PrefKey = "Leaderboard";
    private const int MaxEntries = 10;

    public static void SaveScore(string name, int score)
    {
        List<LeaderboardEntry> entries = LoadScores();
        entries.Add(new LeaderboardEntry { playerName = name, score = score });
        
        entries.Sort((a, b) => b.score.CompareTo(a.score));
        
        if(entries.Count > MaxEntries)
        {
            entries = entries.GetRange(0, MaxEntries);
        }

        string saveString = "";
        foreach(LeaderboardEntry entry in entries)
        {
            saveString += $"{entry.playerName}:{entry.score}|";
        }
        PlayerPrefs.SetString(PrefKey, saveString.TrimEnd('|'));
    }

    public static List<LeaderboardEntry> LoadScores()
    {
        List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
        string savedData = PlayerPrefs.GetString(PrefKey, "");
        
        if(!string.IsNullOrEmpty(savedData))
        {
            string[] entryStrings = savedData.Split('|');
            foreach(string entry in entryStrings)
            {
                string[] parts = entry.Split(':');
                if(parts.Length == 2 && int.TryParse(parts[1], out int score))
                {
                    entries.Add(new LeaderboardEntry {
                        playerName = parts[0],
                        score = score
                    });
                }
            }
        }
        
        return entries;
    }
}