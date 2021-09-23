using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject camera_1;
    private GameObject camera_2;
    private float cameraSpeed = 10.0f;

    private void Start()
    {
        this.camera_1 = GameObject.Find("MainCamera");
        this.camera_2 = GameObject.Find("FirstCamera");
        this.camera_1.SetActive(true);
        this.camera_2.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            this.camera_1.SetActive(false);
            this.camera_2.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            this.camera_1.SetActive(true);
            this.camera_2.SetActive(false);
        }

  
    if (Input.GetKey(KeyCode.LeftArrow))
    {
            Vector3 position = this.camera_2.transform.position;
            position.x -= this.cameraSpeed * Time.deltaTime;
            this.camera_2.transform.position = position;
    }

     if (Input.GetKey(KeyCode.RightArrow))
    {
            Vector3 position = this.camera_2.transform.position;
            position.x += this.cameraSpeed * Time.deltaTime;
            this.camera_2.transform.position = position;
    }

    }
}
