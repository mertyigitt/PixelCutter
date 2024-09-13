using UnityEngine;

public class DontDestroyObjectsScript : MonoBehaviour
{
    public static DontDestroyObjectsScript Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
