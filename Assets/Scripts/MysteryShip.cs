using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public GameObject destroyVEF;

    public float speed = 10.0f;

    private float leftEdge = -140.0f;

    private Vector3 direction = Vector3.right;

    // When Mysteryship gets hit, trigger an action for score
    public System.Action mysteryHit;

    private void Start()
    {
        Vector3 position = this.gameObject.transform.position;
        position.x = leftEdge + 2.0f;
        this.gameObject.transform.position = position;
        this.gameObject.transform.Rotate(0.0f, 90.0f, -15.0f, Space.Self);
    }

    private void Update()
    {
        Vector3 position = this.gameObject.transform.position;
        // suppose the mystery ship travels from somewhere outside the screen
        position.x += direction.x * this.speed * Time.deltaTime;
        this.gameObject.transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ShipMissile"))
        {
            // Invoke score adding
            this.mysteryHit.Invoke();
            
            // reset the mystery ship
            Transform curTransform = this.gameObject.transform;
            Instantiate(destroyVEF, curTransform.position, Quaternion.identity);
            // reset the mystery's ship's position to initial spot
            Vector3 position = this.gameObject.transform.position;
            position.x = this.leftEdge + 2.0f;
            this.gameObject.transform.position = position;
            // reset the rotation of the ship
            Quaternion rotation = Quaternion.Euler(0.0f, 90.0f, -15.0f);
            this.gameObject.transform.rotation = rotation;
        }

        // Change direction when reaches left or right Boundary
        if (other.gameObject.layer == LayerMask.NameToLayer("Boundary"))
        {
            // Change direction when hit boundary
            this.direction *= -1.0f;
            this.gameObject.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
            //Debug.Log("Boundary Reached");
        }
    }

}
