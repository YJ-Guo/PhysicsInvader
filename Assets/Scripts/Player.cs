using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // The missile the player shoots
    public Missile missilePrefab;
    // The speed of the user's ship
    public float speed = 40.0f;

    private Vector3 initPosition;

    // The missile launching sound 
    public AudioClip missileLaunch;
    public AudioClip awardGet;

    // The number of lives the player have in a game 
    public int lifeRemain = 3;
    public GameObject shipDestroyVEF;

    // Use a continue button to resume game after a death
    public Button contButton;
    public Button gameOverButton;

    // Add some buttons for mobile control
    public Button shootFireButton;

    private enum AwardType{
        Tribullet,
        OneLife,
        MoveQuick,
        Nothing,
    }

    private bool hasTribullet = false;
    private bool hasLargebullet = false;
    private bool hasMoveQuick = false;

    private float tribulletTime = 0.0f;
    private float largebulletTime = 0.0f;
    private float quickTime = 0.0f;
    private float lastLaunchTime = 0;

    // When player respawn, have some invincible time
    private bool playerInvincible = false;
    private float playerInvinsibleTime = 0;


    private void Start()
    {
        this.initPosition = this.gameObject.transform.position;
        this.contButton.onClick.AddListener(delegate { ContGame(); });
        this.gameOverButton.onClick.AddListener(delegate { EndGame(); });
        this.shootFireButton.onClick.AddListener(delegate { ShootFireOnClick(); });
        
    }

    private void FixedUpdate()
    {
        if (this.quickTime > 0.0f && Time.time >= this.quickTime + 5.0f)
        {
            this.hasMoveQuick = false;
            this.quickTime = 0.0f;
            this.speed = 40.0f;
        }
        
        Rigidbody playerRigidbody = this.GetComponent<Rigidbody>();
        // Apply Award Bouns
        if (hasMoveQuick)
        {
            this.speed = 80.0f;
        }

        if (this.gameObject.transform.position.x <= 45.0f)
        {
            // Use the left and right arrow to control the ship
            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerRigidbody.MovePosition(this.transform.position + Vector3.right * this.speed * Time.deltaTime);
            }
        }

        if (-50.0f <= this.gameObject.transform.position.x)
        {
            // Use the left and right arrow to control the ship
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerRigidbody.MovePosition(this.transform.position + Vector3.left * this.speed * Time.deltaTime);
            }
        }
    }

    private void Update()
    {
        if (this.tribulletTime > 0.0f && Time.time >= this.tribulletTime + 5.0f)
        {
           //Debug.Log("Tribullet expired");
            this.hasTribullet = false;
            this.tribulletTime = 0.0f;
        }

        if (this.largebulletTime > 0.0f && Time.time >= this.largebulletTime + 5.0f)
        {
            //Debug.Log("Large bullet expired");
            this.hasLargebullet = false;
            this.largebulletTime = 0.0f;
        }

        if (this.playerInvinsibleTime > 0 && Time.time >= this.playerInvinsibleTime + 1.5f)
        {
            this.playerInvincible = false;
            this.playerInvinsibleTime = 0;
        }

        // Use the space key to shoot missile
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= this.lastLaunchTime + 0.5)
            {
                Launch();
                Instantiate(shipDestroyVEF, this.gameObject.transform.position, Quaternion.identity);
            }
        }

        if (this.transform.position.x >= 50.0f || this.transform.position.x <= -55.0f)
        {
            LoseOneLife();
        }
    }

    // The function used to launch a missle when space is pressed
    private void Launch()
    {
        this.lastLaunchTime = Time.time;
        if (hasTribullet)
        {
            // generate 3 bullets with different directions
            Missile missile_1 = Instantiate(this.missilePrefab, this.transform.position, Quaternion.identity);
            
            Missile missile_2 = Instantiate(this.missilePrefab, this.transform.position, Quaternion.Euler(0, 0, 10));
            missile_2.direction = Quaternion.Euler(0, 0, 10) * missile_2.direction;
            
            Missile missile_3 = Instantiate(this.missilePrefab, this.transform.position, Quaternion.Euler(0, 0, -10));
            missile_3.direction = Quaternion.Euler(0, 0, -10) * missile_3.direction;

            if(hasLargebullet)
            {
                EnlargeBullet(missile_1);
                EnlargeBullet(missile_2);
                EnlargeBullet(missile_3);
            }

        } else
        {
            Missile missile = Instantiate(this.missilePrefab, this.transform.position, Quaternion.identity);
            if (hasLargebullet)
            {
                EnlargeBullet(missile);
            }
        } 
        ShootSound();
    }

    private void EnlargeBullet(Missile missile)
    {
        missile.transform.localScale *= 1.5f;
        Collider missileCollider = missile.gameObject.GetComponent<Collider>();
        missileCollider.transform.localScale *= 1.5f;
    }

    // Use a fucntion to play sound when player launch a missile
    private void ShootSound()
    {
        AudioSource.PlayClipAtPoint(missileLaunch, new Vector3(0, 0, -100));
        //Debug.Log("Missile Sound Played!");
    }


    // When player's ship gets hit, check remaining life to 
    // prompt gameover or something
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("UFOMissile") && !this.playerInvincible)
        {
            lifeRemain--;
            // Game over trigger
            if (lifeRemain <= 0)
            {
                Debug.Log("Game Over!");
                Time.timeScale = 0;
                this.gameOverButton.gameObject.SetActive(true);
                // Go back to the Game Intro Scene
            } else
            {
                LoseOneLife();
            }
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Bonus"))
        {
            AudioSource.PlayClipAtPoint(this.awardGet, new Vector3(0, 0, -100));
            other.gameObject.SetActive(false);
            AwardDetermine();
        }
    }

    private void AwardDetermine()
    {
        int awardType = (int)Random.Range(1.0f, 5.0f);
        switch (awardType)
        {
            case(1):
                this.hasTribullet = true;
                this.tribulletTime = Time.time;
                break;
            case (2):
                this.lifeRemain++;
                break;
            case (3):
                this.hasMoveQuick = true;
                this.quickTime = Time.time;
                break;
            case (4):
                this.hasLargebullet = true;
                this.largebulletTime = Time.time;
                break;
        }
    }

    private void LoseOneLife()
    {
        Instantiate(shipDestroyVEF, this.gameObject.transform.position, Quaternion.identity);
        this.playerInvincible = true;
        this.playerInvinsibleTime = Time.time;

        this.gameObject.SetActive(false);
        this.contButton.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    private void ShootFireOnClick()
    {
        if (Time.time >= this.lastLaunchTime + 0.5)
        {
            Launch();
            Instantiate(shipDestroyVEF, this.gameObject.transform.position, Quaternion.identity);
        }
    }


    private void ContGame()
    {
        // Resume the game and hide the continue button
        Time.timeScale = 1;
        this.gameObject.SetActive(true);
        this.gameObject.transform.position = this.initPosition;
        this.contButton.gameObject.SetActive(false);
    }

    private void EndGame()
    {
        SceneManager.LoadScene("GameIntro");
        Time.timeScale = 1;
    }
}
