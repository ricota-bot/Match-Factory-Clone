using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] private Renderer rend;

    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = rend.material;
    }

    public void DisableShadows()
    {

    }

    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }


    public void Select(Material outlineMaterial)
    {
        baseMaterial = rend.material; // The baseMaterial is the material that is in the Renderer
        // We create a new array of materials with the baseMaterial and the outlineMaterial
        rend.materials = new Material[] { baseMaterial, outlineMaterial };


    }

    public void Deselect()
    {
        // We set the materials array to only the baseMaterial
        rend.materials = new Material[] { baseMaterial };
    }
}
