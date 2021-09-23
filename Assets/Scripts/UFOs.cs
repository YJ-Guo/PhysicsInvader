using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UFOs : MonoBehaviour
{
    // An array to hold all the UFOs in the scene
    public UFO[] prefabs;
    private List<UFO> allUFOsList = new List<UFO>();

    // The rows and columns to place UFOs
    public int rows = 5;
    public int columns = 11;

    // Set the direction of the UFOs' movement
    // To the left and right and to the bottom
    private Vector3 moveDir = Vector2.right;

    // The speed of the movement is related to the number of UFOs remaining
    public AnimationCurve speed;
    //public float speed = 10.0f;

    // The frequency of UFO attacts
    public float attackFrequency = 1.0f;
    // The missile prefab for the UFO
    public Missile misslePrefab;

    public Button gameOverButton;

    // The sound file for UFO destroy
    public AudioClip UFODestroyedSound;

    // The sound for boss showing
    public AudioClip bossSpawn;
    public AudioClip bossFightBGM;

    // Adjust bgm for change to boss fight
    public Camera bgmCamera;

    // Use to keep track of the UFOs killed
    public int totalScore;
    public int amountHit;
    public int totalUFONumber => this.rows * this.columns;
    public float percentHit => (float)this.amountHit / (float) this.totalUFONumber;

    // Check the win condition
    public Text winUI;
    private bool hasWin = false;

    // Set a wait method to auto scene change
    private float timeRecord = 0.0f;
    private bool isTimeRecorded = false;

    // Add the level Boss to the scene
    public GameObject wraithPrefab;
    private bool bossGenerated = false;

    private void Awake()
    {
        //LoadBossScene();

        for (int row = 0; row < this.rows; row++)
        {
            // Find the center of the scene
            float width = 10.0f * (this.columns - 1);
            float height = 10.0f * (this.rows - 1);
            // Find the center position of the center UFO
            Vector3 centerPos = new Vector2(-width, -height) / 2.0f;
            // The y position for the ufo
            Vector3 rowPosition = new Vector3(centerPos.x, centerPos.y + row * 10.0f, 0.0f);

            for (int col = 0; col < this.columns; col++)
            {
                UFO ufo = Instantiate(this.prefabs[row], this.transform);
                ufo.ufoHit += UFOHit;
                // Find the x,y position for the ufo
                Vector3 position = rowPosition;
                // Add spacing to the UFOs in x direction
                position.x += col * 10.0f;
                // set the ufo's position to the calculated (x,y)
                ufo.transform.localPosition = position;
                this.allUFOsList.Add(ufo);
            }
        }
    }

    // A function to call when the UFO is hit
    private void UFOHit (int UFOScore)
    {
        this.amountHit++;
        this.totalScore += UFOScore;
        AudioSource.PlayClipAtPoint(UFODestroyedSound, new Vector3(0, 0, -100));
    }


    private void Start()
    {     
        InvokeRepeating(nameof(UFOAttack), this.attackFrequency, this.attackFrequency);
        this.gameOverButton.onClick.AddListener(delegate { EndGame(); });
    }
    private void EndGame()
    {
        SceneManager.LoadScene("GameIntro");
        Time.timeScale = 1;
    }

    private void UFOAttack()
    {
        foreach (UFO curUFO in this.allUFOsList)
        {
            // When a UFO is destroyed, it should not bother the edge test
            if (curUFO.GetHasHit())
            {
                continue;
            }
            // Use a random value to control the possibility of UFO attack
            if (Random.value < percentHit)
            {
                Instantiate(this.misslePrefab, curUFO.transform.position, Quaternion.AngleAxis(180, Vector3.right));
                // Suppose there is only one attack at a frame
                break;
            }
        }
    }

    private void Update()
    {
        //Debug.Log("Current Score:" + totalScore.ToString());
        if (!hasWin)
        {
            WinCheck();
        }

        // Support the time wait logic
        if (isTimeRecorded)
        {
            // wait 5 seconds
            if (Time.time >= timeRecord + 5.0f && !this.bossGenerated)
            {
                LoadBossScene();
                this.isTimeRecorded = false;
                //ReturnIntroScene();
            }
        }

        // Use the update function to move the UFOs as a whole
        //this.transform.position += moveDir * this.speed.Evaluate(this.percentHit) * Time.deltaTime;
        UFOMoving();
        //Debug.Log(percentHit);

        // When the UFO touches the edge, then should change direction(horizontally)
        foreach (UFO curUFO in this.allUFOsList)
        {
            // When a UFO is destroyed, it should not bother the edge test
            if (curUFO.GetHasHit())
            {
                continue;
            }

            if (curUFO.transform.localPosition.y <= -105.0f)
            {
                //Debug.Log("Game Over!");
                AudioSource cameraAudioSource = this.bgmCamera.GetComponent<AudioSource>();
                cameraAudioSource.Pause();
                Time.timeScale = 0;
                this.gameOverButton.gameObject.SetActive(true);
            }

            // If hit edge, change direction and go down
            if (moveDir == Vector3.right && curUFO.transform.position.x >= (70.0f))
            {
                UFOMarching();
            } else if (moveDir == Vector3.left && curUFO.transform.position.x <= (-70.0f))
            {
                UFOMarching();
            }
        }
    }

    private void UFOMoving ()
    {
        foreach(UFO curUFO in this.allUFOsList)
        {
            if(!curUFO.GetHasHit())
            {
                curUFO.transform.position += moveDir * this.speed.Evaluate(this.percentHit) * Time.deltaTime;
            }
        }
    }

    // This function aims to lead the UFOs go down toward the player 
    // and change the marching direction from left/right to right/left respectively
    private void UFOMarching()
    {
        // Change the left/right marching direction
        moveDir.x *= -1.0f;

        // March toward the player
        foreach (UFO curUFO in this.allUFOsList)
        {
            if (!curUFO.GetHasHit())
            {
                Vector3 position = curUFO.transform.position;
                position.y -= 2.0f;
                curUFO.transform.position = position;
            }
        }
    }

    private void WinCheck()
    {
        if (this.amountHit > 0 && this.amountHit == this.totalUFONumber)
        {
            //Debug.Log("Win Game!");
            this.winUI.gameObject.SetActive(true);
            AudioSource cameraAudioSource = this.bgmCamera.GetComponent<AudioSource>();
            cameraAudioSource.Pause();
            AudioSource.PlayClipAtPoint(bossSpawn, new Vector3(0, 0, -140));
            this.hasWin = true;
            ToRecordTime();
        }
    }

    private void ToRecordTime()
    {
        this.timeRecord = Time.time;
        this.isTimeRecorded = true;
    }

    private void ReturnIntroScene()
    {
        this.isTimeRecorded = false;
        SceneManager.LoadScene("GameIntro");
    }

    private void LoadBossScene()
    {
        Instantiate(this.wraithPrefab, new Vector3(0, -20, 0), Quaternion.Euler(0, 180, 0));
        this.bossGenerated = true;
        this.winUI.gameObject.SetActive(false);
       
        AudioSource cameraAudioSource = this.bgmCamera.GetComponent<AudioSource>();
        cameraAudioSource.clip = this.bossFightBGM;
        cameraAudioSource.volume = 0.25f;
        cameraAudioSource.Play();
    }
}
