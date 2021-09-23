using UnityEngine;

public class Award : MonoBehaviour
{
    private float speed;
    private float curTime;

    private void Start()
    {
        this.speed = 5.0f;
        this.curTime = Time.time;
    }

    private void Update()
    {
        
        BoxMovement(this.curTime);
        BoxDisappear();
    }

    private void BoxMovement(float curTime)
    {
        Vector3 position = this.transform.position;
        position.y -= this.speed * Time.deltaTime;
        position.x += this.speed * (curTime % 10) * Mathf.Sin(Time.time) * Time.deltaTime;
        this.transform.position = position;
    }

    private void BoxDisappear()
    {
        if (this.transform.position.y <= -150.0f)
        {
            this.gameObject.SetActive(false);
        }
    }

}
