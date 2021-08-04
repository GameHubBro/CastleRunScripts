using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryTemplate;
    private Transform entryContainer;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    public static HighscoreTable instance;

    private Highscores highscores;

    private void Awake()
    {
        if (HighscoreTable.instance != null)                       //если есть другой объект класс WorldController, то удаляем его
        {
            Destroy(gameObject);
            return;
        }
        HighscoreTable.instance = this;

        entryContainer = transform.Find("HighscoreEntryContainer");
        entryTemplate = entryContainer.Find("HighscoreEntryTemplate");

        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(10, "Sts");

        //highscoreEntryList = new List<HighscoreEntry>()
        //{
        //    new HighscoreEntry{ score = 1, name = "Stas"},
        //    new HighscoreEntry{ score = 2, name = "Arnold"},
        //    new HighscoreEntry{ score = 3, name = "Maya"},
        //    new HighscoreEntry{ score = 4, name = "Vik"},
        //    new HighscoreEntry{ score = 5, name = "Olya"},
        //    new HighscoreEntry{ score = 6, name = "Serg"},
        //    new HighscoreEntry{ score = 7, name = "Max"},
        //    new HighscoreEntry{ score = 8, name = "Artem"},
        //};


        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //Debug.Log(message: "jsonString: " + jsonString);

        //var highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //Debug.Log(highscores);

        var highscores = GetScores();

        //if (highscores.highscoreEntryList.Count != 0)
        //{
        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
            {
                for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
                {
                    if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                    {
                        HighscoreEntry tmp = highscores.highscoreEntryList[i];
                        highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                        highscores.highscoreEntryList[j] = tmp;
                    }
                }
            }

            highscoreEntryTransformList = new List<Transform>();

        //foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        //{
        //    CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        //}
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
        //}


        //Highscores highscores = new Highscores { highscoreEntryList = highscoreEntryList };

        //string json = JsonUtility.ToJson(highscores);
        //PlayerPrefs.SetString("highscoreTable", json);
        //PlayerPrefs.Save();
        //Debug.Log(PlayerPrefs.GetString("highscoreTable"));

    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 30f;

        
            Transform entryTransform = Instantiate(entryTemplate, container);
            RectTransform rectEntryTransform = entryTransform.GetComponent<RectTransform>();
            rectEntryTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
            entryTransform.gameObject.SetActive(true);

            int rank = transformList.Count + 1;
            string rankString;
            switch (rank)
            {
                default: rankString = rank + "TH"; break;
                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }
            entryTransform.Find("posText").GetComponent<Text>().text = rankString;

            int score = highscoreEntry.score;
            entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

            string name = highscoreEntry.name;
            entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
        
    }

    public void AddHighscoreEntry (int score, string name)
    {
        //Создаем HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        //загружаем сохраненные Highscores
        //string jsonString = PlayerPrefs.GetString("highscoreTable");
        //Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores = GetScores();

        //Добавляем новый ввод в Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        //Сохраняем обновленный Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    public void CreateHighscoreTable()
    {
        highscores = GetScores();
        //highscores = JsonUtility.FromJson<Highscores>(PlayerPrefs.GetString("highscoreTable"));


        for (int i = 0; i < highscores.highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoreEntryList.Count; j++)
            {
                if (highscores.highscoreEntryList[j].score > highscores.highscoreEntryList[i].score)
                {
                    HighscoreEntry tmp = highscores.highscoreEntryList[i];
                    highscores.highscoreEntryList[i] = highscores.highscoreEntryList[j];
                    highscores.highscoreEntryList[j] = tmp;
                }
            }
        }

        if (entryContainer.childCount < 6)
        {
            for (int i = 0; i < 5; i++)
            {
                CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            }
        }
        else
        {
            highscoreEntryTransformList.Clear();
            for (int i = 1; i < entryContainer.childCount; i++)
            {
                entryContainer.GetChild(i).transform.DetachChildren();
                entryContainer.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < 5; i++)
            {
                CreateHighscoreEntryTransform(highscores.highscoreEntryList[i], entryContainer, highscoreEntryTransformList);
            }
        }
    }

    public class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;

        public Highscores()
        {
            highscoreEntryList = new List<HighscoreEntry>();
        }
    }

    [System.Serializable]
    public class HighscoreEntry
    {
        public int score;
        public string name;
    }

    public void DeleteHighscoreHistory()
    {
        PlayerPrefs.DeleteAll();
    }

    public Highscores GetScores()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Debug.Log(message: "jsonString: " + jsonString);
        if (PlayerPrefs.GetString("highscoreTable") != null)
        {
            var highscores = JsonUtility.FromJson<Highscores>(jsonString);
            Debug.Log(highscores);
            return highscores;
        }
        else
        {
            highscoreEntryList = new List<HighscoreEntry>()
            {
                new HighscoreEntry{ score = 1, name = "Stas"},
                new HighscoreEntry{ score = 2, name = "Arnold"},
                new HighscoreEntry{ score = 3, name = "Maya"},
                new HighscoreEntry{ score = 4, name = "Vik"},
                new HighscoreEntry{ score = 5, name = "Olya"},
                new HighscoreEntry{ score = 6, name = "Serg"},
                new HighscoreEntry{ score = 7, name = "Max"},
                new HighscoreEntry{ score = 8, name = "Artem"},
            };

            Highscores highscores = new Highscores { highscoreEntryList = highscoreEntryList };

            string json = JsonUtility.ToJson(highscores);
            PlayerPrefs.SetString("highscoreTable", json);
            PlayerPrefs.Save();
            string stringFromJson = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(stringFromJson);

            return highscores;    
        }

    }
}
