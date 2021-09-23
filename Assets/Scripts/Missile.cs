using UnityEngine;

public class Missile : MonoBehaviour
{
    // These defines the direction and speed of the missile
    public Vector3 direction;
    public float speed;

    // Add an action to tell the player class that the 
    // missile has shot something
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.destroyed != null) 
        { 
        this.destroyed.Invoke();
        }
        Destroy(this.gameObject);
    }
}
