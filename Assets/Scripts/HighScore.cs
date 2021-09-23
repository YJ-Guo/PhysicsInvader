using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;

public class HighScore : MonoBehaviour
{
    public ScoreUI currentScore;
    public Text highScore;
    private int hisHighScore;

    private void Start()
    {
        this.currentScore = GameObject.Find("CurScore").GetComponent<ScoreUI>();
        this.highScore = GetComponent<Text>();
        
        LoadFile();
        this.highScore.text = "HighScore " + this.hisHighScore.ToString();
    }

    private void Update()
    {
        if (currentScore.GetCurrentScore() > this.hisHighScore)
        {
            this.highScore.text = "HighScore " + currentScore.GetCurrentScore().ToString();
            this.hisHighScore = currentScore.GetCurrentScore();
            SaveFile();
        }
    }

    // Use a file to store the history high score
    private void SaveFile()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(hisHighScore);

        FileStream fileStream = new FileStream(Application.dataPath + "/record.txt", FileMode.Create);
        byte[] bytes = new UTF8Encoding().GetBytes(stringBuilder.ToString());
        fileStream.Write(bytes, 0, bytes.Length);

        fileStream.Close();
    }

    private void LoadFile()
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/record.txt", FileMode.Open);
        byte[] bytes = new byte[100];
        fileStream.Read(bytes, 0, bytes.Length);
        string s = new UTF8Encoding().GetString(bytes);

        if (!int.TryParse(s, out this.hisHighScore))
        {
            Debug.Log("High Score Loading Error! Default Zero.");
        }

        fileStream.Close();
    }

}
