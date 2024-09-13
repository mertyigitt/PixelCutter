using UnityEngine;
using UnityEngine.UI;

public class DestroyScript : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] private GameObject candyPrefab;

    #endregion

    #region Private Variables

    private Slider _slider;
    private int _pixel;
    private int _totalCoin;

    #endregion
    

    #endregion
    
    
    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Pixel");
        _slider = GameObject.Find("Progress").GetComponent<Slider>();
        _slider.maxValue = (gameObjects.Length * 80) / 100;
        _slider.value = 0;
    }

    private void FixedUpdate()
    {
        if(Mathf.Approximately(_slider.value, _slider.maxValue))
        {
            _totalCoin += _pixel + (PlayerPrefs.GetInt("incomeLevel") * 20);
            UIManager.Instance.Coin += _totalCoin;
            PlayerPrefs.SetInt("coin", UIManager.Instance.Coin);
            UIManager.Instance.OnIsOver?.Invoke(_totalCoin);
            
            _slider.value = 0;
            _pixel = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pixel"))
        {
            _pixel++;
            _slider.value = _pixel;
            if(_pixel % 4 == 0)
            {
                Instantiate(candyPrefab, collision.gameObject.transform.position, Quaternion.identity);
            }
            if(UIManager.Instance.Sound && UIManager.Instance.Success == false)
            {
                UIManager.Instance.CoinSound.Play();
            }
            Destroy(collision.gameObject);
        }
    }
}
