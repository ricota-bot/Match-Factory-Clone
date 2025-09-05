using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private ItemNameEnum itemName;
    // Criamos essa Propriedade public para acessar esse Dado "Podemos criar um metodo tambem.."
    public ItemNameEnum ItemName => itemName; // Utilizando a Lambda apenas criamos um Get.. Não tem Set aqui "É o que queremos"

    [Header("Elements")]
    [SerializeField] private Renderer rend;
    [SerializeField] private Collider coll;

    private Material baseMaterial;

    private void Awake()
    {
        baseMaterial = rend.material;
    }

    public void DisableShadows()
    {
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void DisablePhysics()
    {
        GetComponent<Rigidbody>().isKinematic = true;
        coll.enabled = false;
    }


    public void Select(Material outlineMaterial)
    {
        // We create a new array of materials with the baseMaterial and the outlineMaterial
        rend.materials = new Material[] { baseMaterial, outlineMaterial };
    }

    public void Deselect()
    {
        // We set the materials array to only the baseMaterial
        rend.materials = new Material[] { baseMaterial };
    }
}
