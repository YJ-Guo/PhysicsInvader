using UnityEngine;
using UnityEngine.UI;

public class LifeUI : MonoBehaviour
{
    public Player playerObj;
    public Text lifeText;

    private void Awake()
    {
        this.gameObject.SetActive(true);
    }

    private void Start()
    {
        GameObject g = GameObject.Find("Player");
        this.playerObj = g.GetComponent<Player>();
        this.lifeText = gameObject.GetComponent<Text>();
    }

    private void Update()
    {
        this.lifeText.text = "LIFE  " + this.playerObj.lifeRemain.ToString();
    }
}
