using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScoreTable : MonoBehaviour
{
    public static HighScoreTable instance { get; private set; }

    [SerializeField] private Transform entryContainer;
    [SerializeField] private Transform entryTemplate;
    private List<Transform> highscoreEntryTransformList;

    [SerializeField] private string highScoreDataKey = "highscoreTable";

    private HighScores highScores;

    void Awake()
    {
        instance = this;

        entryTemplate.gameObject.SetActive(false);

        // Reset the leaderboard save
        // HighScores highScores1 = new HighScores { highScoreEntries = new List<HighScoreEntry>() };
        // string json = JsonUtility.ToJson(highScores1);
        // PlayerPrefs.SetString(highScoreDataKey, json);

        // Load Score Save
        string jsonString = PlayerPrefs.GetString(highScoreDataKey);
        highScores = JsonUtility.FromJson<HighScores>(jsonString);

        Debug.Log(jsonString);
    }

    private void SortAndCut(HighScores highScores) {
        // Sort the score list
        for (int i = 0; i < highScores.highScoreEntries.Count; i++) {
            for (int j = i + 1; j < highScores.highScoreEntries.Count; j++) {
                if (highScores.highScoreEntries[j].score > highScores.highScoreEntries[i].score) {
                    // Swap position
                    HighScoreEntry temp = highScores.highScoreEntries[i];
                    highScores.highScoreEntries[i] = highScores.highScoreEntries[j];
                    highScores.highScoreEntries[j] = temp;
                }
            }
        }

        // Make sure there is only top 4 entries
        if (highScores.highScoreEntries.Count > 4)
        {
            for (int h = highScores.highScoreEntries.Count; h > 4 ; h--)
        {
                highScores.highScoreEntries.RemoveAt(4);
            }
        }
    }

    private void CreateHighscoreEntryTransform(HighScoreEntry highScoreEntry, Transform container, List<Transform> transformList) {
        float templateHeight = 40f;

        // Instantiate the entry from the template
        GameObject newEntry = Instantiate(entryTemplate.gameObject);

        // Set the parent of the new entry to be the entry container
        newEntry.transform.SetParent(container, false);

        // Get the RectTransform of the new entry
        RectTransform entryRectTransform = newEntry.GetComponent<RectTransform>();

        // Set the anchored position of the new entry
        entryRectTransform.anchoredPosition = new Vector2(0, - templateHeight * transformList.Count);

        // Make sure the new entry is active
        newEntry.SetActive(true);

        // int rank = transformList.Count + 1;
        newEntry.transform.Find("SavedName").GetComponent<TMP_Text>().text = highScoreEntry.name;
        newEntry.transform.Find("SavedScore").GetComponent<TMP_Text>().text = highScoreEntry.score.ToString();
        
        transformList.Add(newEntry.transform);
    }

    public void spawnScoreRecords() {
        string jsonString = PlayerPrefs.GetString(highScoreDataKey);
        highScores = JsonUtility.FromJson<HighScores>(jsonString);

        highscoreEntryTransformList = new List<Transform>{};

        foreach (HighScoreEntry highScoreEntry in highScores.highScoreEntries) {
            CreateHighscoreEntryTransform(highScoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    public void AddHighScoreEntry (int score, string name) {
        // Create new score entry
        HighScoreEntry highScoreEntry= new HighScoreEntry { score = score, name = name };

        // Load saved score list
        string jsonString = PlayerPrefs.GetString(highScoreDataKey);
        HighScores highScores = JsonUtility.FromJson<HighScores>(jsonString);

        // Add new entry to score list
        highScores.highScoreEntries.Add(highScoreEntry);

        SortAndCut(highScores);

        // Save updated list
        string json = JsonUtility.ToJson(highScores);
        PlayerPrefs.SetString(highScoreDataKey, json);
        PlayerPrefs.Save();
    }

    private class HighScores {
        public List<HighScoreEntry> highScoreEntries;
    }

    [System.Serializable]
    private class HighScoreEntry {
        public int score;
        public string name;
    }
}
