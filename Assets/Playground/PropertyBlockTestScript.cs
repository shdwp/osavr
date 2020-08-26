using UnityEngine;

namespace Playground
{
    [RequireComponent(typeof(Renderer))]
    public class PropertyBlockTestScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var r = GetComponent<Renderer>();
            var p = new MaterialPropertyBlock();
            p.SetColor("_BaseColor", Color.cyan);
            r.SetPropertyBlock(p);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
