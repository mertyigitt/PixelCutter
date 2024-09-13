using System.Collections;
using UnityEngine;

public class SlowPixel : MonoBehaviour
{
    private bool _isSlow;
    private Rigidbody2D _rigidbody2D;
    

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _isSlow = true;
    }
    private void Update()
    {
        if (_isSlow)
        {
            _rigidbody2D.velocity /= (1 + 0.8f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isSlow = false;
            StartCoroutine(Wait(0.75f));
        }
    }

    private IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _isSlow = true;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
