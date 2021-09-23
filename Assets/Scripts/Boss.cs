using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    public Missile missile;
    public Award awardBox;
    public GameObject gameWinQuad;

    private Animator bossAnimator;
    private int bossHealth;
    private int bossCurHealth;

    private int bossAttackType = -1;
    private float lastAttackTime = 0;
    private float attackType0Time = 0;
    private float attackType1Time = 0;
    private float attackType2Time = 0;
    private float attackType3Time = 0;
    private float winGameTime = 0;

    private void Awake()
    {
        this.bossAnimator = this.GetComponent<Animator>();
        //Debug.Log(this.bossAnimator);
        this.bossHealth =500;
        this.bossCurHealth = this.bossHealth;
    }

    private void Start()
    {
        this.bossAttackType = 10;
        BossAttack();
    }

    private void Update()
    {
        if(this.winGameTime > 0 && Time.time >= this.winGameTime + 5)
        {
            SceneManager.LoadScene("GameIntro");
        }

        //backdoor for testing
        if (Input.GetKeyDown(KeyCode.L))
        {
            this.bossCurHealth = 0;
        }
        
        if (this.bossCurHealth > 0)
        {
            if (Time.time >= this.lastAttackTime + 2.5f)
            {
                LimitUpadte();
            }

            if (this.attackType0Time > 0 && Time.time > this.attackType0Time + 1.2)
            {
                AttachMethod(0);
                this.attackType0Time = 0;
            }

            if (this.attackType1Time > 0 && Time.time > this.attackType1Time + 1.2)
            {
                AttachMethod(1);
                this.attackType1Time = 0;
            }

            if (this.attackType2Time > 0 && Time.time > this.attackType2Time + 1.5)
            {
                AttachMethod(2);
                this.attackType2Time = 0;
            }

            if (this.attackType3Time > 0 && Time.time > this.attackType3Time + 1.2)
            {
                AttachMethod(3);
                this.attackType3Time = 0;
            }
        } 
        else {
            this.bossAnimator.SetBool("WinBoss", true);
            Vector3 position = this.transform.position;
            position.y += 40;
            Instantiate(this.gameWinQuad, position, Quaternion.identity);
            if (Mathf.Abs(this.winGameTime) < 1e-4)
            {
                this.winGameTime = Time.time;
            }
        }
    }

    private void LimitUpadte()
    {
        this.bossAttackType = (int)Random.Range(0, 6);
        //Debug.Log(bossAttackType);
        BossAttack();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ShipMissile"))
        {
            this.bossCurHealth--;
            //Debug.Log(this.bossCurHealth);
            bossAnimator.SetTrigger("TakeDamage");
            //Debug.Log("DamageTaken");
            if (Random.Range(0,10) >= 7)
            {
                Instantiate(this.awardBox, this.transform.position, Quaternion.Euler(0, 90, 0));
            }
        }
    }

    private void BossAttack()
    {
        this.lastAttackTime = Time.time;
        if (this.bossAttackType >= 4)
        {
            this.bossAnimator.SetInteger("Attack", 0);
        } else
        {
            switch (this.bossAttackType)
            {
                case (0):
                    this.bossAnimator.SetInteger("Attack", 1);
                    this.attackType0Time = Time.time;
                    break;
                case (1):
                    this.bossAnimator.SetInteger("Attack", 2);
                    this.attackType1Time = Time.time;
                    break;
                case (2):
                    this.bossAnimator.SetInteger("Attack", 3);
                    this.attackType2Time = Time.time;
                    break;
                case (3):
                    this.bossAnimator.SetInteger("Attack", 4);
                    this.attackType3Time = Time.time;
                    break;
            }
        }
    }

    private void AttachMethod(int method)
    {
        switch (method)
        {
            case (0):
                for (int i = 0; i < 40; i += 4)
                {
                    Vector3 position_0 = this.transform.position;
                    position_0.x += i;
                    position_0.y += 10;
                    Instantiate(this.missile, position_0, Quaternion.AngleAxis(180, Vector3.right));
                }
                break;
            case (1):
                for (int i = -40; i < 40; i += 4)
                {
                    Vector3 position_1 = this.transform.position;
                    position_1.x += i;
                    position_1.y += 10;
                    Instantiate(this.missile, position_1, Quaternion.AngleAxis(180, Vector3.right));
                }
                break;
            case (2):
                Vector3 position_2 = this.transform.position;
                position_2.x -= 10;
                position_2.y += 10;
                Missile curMissile = Instantiate(this.missile, position_2, Quaternion.AngleAxis(180, Vector3.right));
                curMissile.transform.localScale = new Vector3(5, 10, 5);
                break;
            case (3):
                for (int i = -30; i < 30; i += 6)
                {
                    Vector3 position_3 = this.transform.position;
                    position_3.x += i;
                    position_3.y += 10;
                    Instantiate(this.missile, position_3, Quaternion.AngleAxis(180, Vector3.right)).transform.localScale = new Vector3(5,10,5);
                }
                break;
        }
    }
}
