using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public UFOs allUFO;
    public Text scoreUI;
    public MysteryShip msShip;
    private int mysteryScore = 0;
    private int currentScore = 0;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    private void Start()
    {
        this.allUFO = GameObject.Find("UFOs").GetComponent<UFOs>();
        this.msShip = GameObject.Find("MysteryShip").GetComponent<MysteryShip>();
        this.msShip.mysteryHit += addMysteryScore;
        this.scoreUI = GetComponent<Text>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Debug.Log(this.allUFO.totalScore);
        this.currentScore = this.allUFO.totalScore + this.mysteryScore;
        this.scoreUI.text = "SCORE " + (this.currentScore).ToString();
    }

    private void addMysteryScore()
    {
        this.mysteryScore += 100;
    }

    public int GetCurrentScore()
    {
        return this.currentScore;
    }
}
