using UnityEngine;
public class CandyScripts : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Candy"))
        {
            other.GetComponent<CircleCollider2D>().isTrigger = false;
            Destroy(other.gameObject, 1f);
        }
    }
}
