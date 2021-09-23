using UnityEngine;
using UnityEngine.EventSystems;

public class MoveRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Player player;
    private bool isPressed;

    public void OnPointerDown(PointerEventData eventData)
    {
        this.isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.isPressed = false;
    }

    private void Update()
    {
        Rigidbody playerRigidBody = this.player.GetComponent<Rigidbody>();
        if (isPressed && (45.0f >= this.player.transform.position.x))
        {
            playerRigidBody.MovePosition(this.player.transform.position + Vector3.right * this.player.speed * Time.deltaTime);
        }
    }
}
