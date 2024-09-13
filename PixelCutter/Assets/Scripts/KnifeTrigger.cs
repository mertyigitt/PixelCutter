using UnityEngine;

public class KnifeTrigger : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private Material changeMaterial;
    [SerializeField] private Material[] materials;

    #endregion
    
    #region Private Variables
    
    private Rigidbody2D _rb;
    private Renderer _materialRenderer;
    
    #endregion

    #endregion
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pixel"))
        {
            _rb = collision.GetComponent<Rigidbody2D>();
            _materialRenderer = collision.GetComponent<Renderer>();
            collision.GetComponent<BoxCollider2D>().isTrigger = false;
            
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.mass = 5;
            _rb.gravityScale = 2;
            _rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
            
            materials = _materialRenderer.sharedMaterials;
            materials[1] = changeMaterial;
            _materialRenderer.sharedMaterials = materials;
            
            if(UIManager.Instance.Vibrate)
            {
                Vibration.Vibrate(150);
            }
        }
    }
}
