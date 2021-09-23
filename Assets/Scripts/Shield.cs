using UnityEngine;

public class Shield : MonoBehaviour
{
    // Set the shield life to 10
    private int shieldLife = 10;
    
    // Adjust the mesh to simulate shield been hit
    private MeshRenderer shieldMesh;
    private Quaternion initRotation;

    // Add visual effect to shield destruction
    public GameObject shieldDestroyVEF;

    private void Start()
    {
        this.shieldMesh = GetComponent<MeshRenderer>();
        initRotation = shieldMesh.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the shield is directly hit by the UFO
        // it gets destroyed
        if (other.gameObject.layer == LayerMask.NameToLayer("UFO"))
        {
            this.gameObject.SetActive(false);
            Instantiate(this.shieldDestroyVEF, this.gameObject.transform.position, Quaternion.identity);
        }

        // It the shield is hit by some missile either
        // from the player or the UFO
        // count to 15 to destroy
        if (other.gameObject.layer == LayerMask.NameToLayer("UFOMissile") || 
            other.gameObject.layer == LayerMask.NameToLayer("ShipMissile"))
        {
            this.shieldLife--;

            if (this.shieldLife <= 0)
            {
                this.gameObject.SetActive(false);
                Instantiate(this.shieldDestroyVEF, this.gameObject.transform.position, Quaternion.identity);
            }

            // Use tilt to represent the state of shield being shot at
            if (this.shieldLife % 2 == 0)
            {
                shieldMesh.transform.rotation = this.initRotation;
                shieldMesh.transform.Rotate(0.0f, 0.0f, 10.0f, Space.Self);
            } else
            {
                shieldMesh.transform.rotation = this.initRotation;
                shieldMesh.transform.Rotate(0.0f, 0.0f, -10.0f, Space.Self);
            }
        }
    }
}
