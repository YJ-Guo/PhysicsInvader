using UnityEngine;
using UnityEngine.UI;

public class UFO : MonoBehaviour
{
    // Create an action when the UFO is hit
    public System.Action<int> ufoHit;
    // The particle visual effect for UFO destroy
    public GameObject UFODestroyVEF;

    // Attach the award to the UFO
    public GameObject randomAward;

    // set score for each UFO
    public int UFOScore;
    private bool hasHit;

    private enum RenderingMode
    {
        Opaque,
        Cutout,
        Fade,
        Transparent,
    }

    private void Start()
    {
        InitializeColor();
        this.hasHit = false;
        Rigidbody ufoRigidbofy = this.gameObject.GetComponent<Rigidbody>();
        ufoRigidbofy.constraints = RigidbodyConstraints.FreezePosition;
    }

    public bool GetHasHit()
    {
        return hasHit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ShipMissile") && this.hasHit == false)
        {
            this.ufoHit.Invoke(UFOScore);
            Instantiate(UFODestroyVEF,other.transform.position, Quaternion.identity);
            GenerateAward(other);

            //this.gameObject.SetActive(false);
            this.hasHit = true;
            TurnGrey();
            HitFall();
        }
    }

    private void GenerateAward (Collider other)
    {
        // Once a UFO is hit, there is 20% change to generate award        
        if (Random.Range(0.0f, 10.0f ) <= 2.0f)
        {
            Instantiate(randomAward, other.transform.position, Quaternion.Euler(0, 90, 0));
        }
    }

    private void TurnGrey()
    {
        Material[] allMaterials = this.gameObject.GetComponent<Renderer>().materials;
        foreach(Material ufoMaterial in allMaterials)
        {
            SetMaterialRenderingMode(ufoMaterial, RenderingMode.Transparent);
            Color curColor = ufoMaterial.color;
            float grey = (float)(0.299 * curColor.r + 0.587 * curColor.g + 0.114 * curColor.b);
            //Debug.Log(curColor);
            ufoMaterial.color = new Color(grey, grey, grey, 0.1f);
        }

    }

    private void HitFall()
    {
        Rigidbody ufoRigidbofy = this.gameObject.GetComponent<Rigidbody>();
        ufoRigidbofy.useGravity = true;
        ufoRigidbofy.constraints = RigidbodyConstraints.None;
        ufoRigidbofy.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    private void InitializeColor ()
    {
        Material[] allMaterials = this.gameObject.GetComponent<Renderer>().materials;
        foreach (Material ufoMaterial in allMaterials)
        {
            ufoMaterial.color = RandonColorGenerator();
        }
    }

    private Color RandonColorGenerator()
    {
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        float a = Random.Range(0f, 0.5f);
        return new Color(r,g,b,a);
    }

    private void SetMaterialRenderingMode(Material material, RenderingMode renderingMode)
    {
        switch(renderingMode)
        {
            case RenderingMode.Opaque:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = -1;
                break;
            case RenderingMode.Cutout:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                material.SetInt("_ZWrite", 1);
                material.EnableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 2450;
                break;
            case RenderingMode.Fade:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.EnableKeyword("_ALPHABLEND_ON");
                material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
            case RenderingMode.Transparent:
                material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetInt("_ZWrite", 0);
                material.DisableKeyword("_ALPHATEST_ON");
                material.DisableKeyword("_ALPHABLEND_ON");
                material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                material.renderQueue = 3000;
                break;
        }
    }

}
