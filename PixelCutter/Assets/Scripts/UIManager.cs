using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    #region Self Variables

    #region Serialized Variables

    [SerializeField] GameObject[] knifes;
    [SerializeField] private bool sound;
    [SerializeField] private bool vibrate;
    [SerializeField] private bool success;
    [SerializeField] private bool play;
    [SerializeField] private bool isActive;
    [SerializeField] private int coin;
    [SerializeField] private AudioSource knifeSound, coinSound,menuClick, gameSuccess;

    [SerializeField] private GameObject canvasStart,
        canvasSuccess,
        soundOn,
        soundOff,
        vibrateOn,
        vibrateOff,
        storeKnifeTextUsed1,
        storeKnifeTextCoin1,
        storeKnifeTextCoinImage1,
        storeKnifeTwoButton,
        storeKnifeTextUsed2,
        storeKnifeTextCoin2,
        storeKnifeTextCoinImage2,
        storeKnifeThreeButton,
        storeImageFrame1,
        storeImageFrame2,
        storeImageFrame3;
    
    [SerializeField] private TextMeshProUGUI startCanvasLevelText, playCanvasLevelText;

    #endregion

    #region Private Variables

    private int _level;
    private int _knifeLevel;
    private Transform _knifeSpawnPoint;
    private Button[] _buttons;
    private GameObject _knife;

    #endregion

    #region Public Variables

    public GameObject canvasPlay;
    public TextMeshProUGUI startCoinText, playCoinText, successCoinText, upgradeCoinText, successEarnedText, storeCoinText;
    public bool Sound => sound;
    public bool Vibrate => vibrate;
    private int _incomeLevel;
    public int Coin
    {
        get => coin;
        set => coin = value;
    }
    public AudioSource KnifeSound => knifeSound;
    public AudioSource CoinSound => coinSound;
    public bool Success => success;
    public bool Play => play;
    public bool IsActive => isActive;
    public UnityAction<int> OnIsOver = delegate { };

    #endregion
    
    #endregion
    
    
    public static UIManager Instance;
    

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


    private void Start()
    {
        _buttons = FindObjectsOfType<Button>();

        foreach (Button btn in _buttons)
        {
            btn.onClick.AddListener(UISoundandVibration);
        }

        canvasStart.SetActive(true);
        canvasPlay.SetActive(true);
        canvasSuccess.SetActive(false);
        sound = true;
        vibrate = true;

        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("coin"))
            PlayerPrefs.SetInt("coin", 100);
        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 0);
        if (!PlayerPrefs.HasKey("incomeLevel"))
            PlayerPrefs.SetInt("incomeLevel", 0);
        if (!PlayerPrefs.HasKey("knifeLevel"))
            PlayerPrefs.SetInt("knifeLevel", 1);
        if (!PlayerPrefs.HasKey("vibrate"))
            PlayerPrefs.SetInt("vibrate", 1);
        if (!PlayerPrefs.HasKey("sound"))
            PlayerPrefs.SetInt("sound", 1);
        _level = PlayerPrefs.GetInt("level");
        coin = PlayerPrefs.GetInt("coin");
        _incomeLevel = PlayerPrefs.GetInt("incomeLevel");
        _knifeLevel = PlayerPrefs.GetInt("knifeLevel");
        vibrate = PlayerPrefsGetBool("vibrate");
        sound = PlayerPrefsGetBool("sound");

        if (sound)
        {
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }
        else
        {
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
        if (vibrate)
        {
            vibrateOn.SetActive(true);
            vibrateOff.SetActive(false);
        }
        else
        {
            vibrateOn.SetActive(false);
            vibrateOff.SetActive(true);
        }
        
        startCoinText.text = PlayerPrefs.GetInt("coin").ToString();
        
        _knifeSpawnPoint = GameObject.Find("KnifeSpawnPoint").transform;
        _knife =  Instantiate(knifes[_knifeLevel - 1], _knifeSpawnPoint.position, Quaternion.identity);
        _knife.GetComponent<KnifeController>().enabled = false;
        
        startCanvasLevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
        playCanvasLevelText.text = "Level " + (SceneManager.GetActiveScene().buildIndex + 1);
        
        success = false;
        play = false;


        if(_knifeLevel >= 2)
        {
            storeKnifeTextCoin1.SetActive(false);
            storeKnifeTextUsed1.SetActive(true);
            storeKnifeTextCoinImage1.GetComponent<Image>().enabled = false;
            storeKnifeTwoButton.GetComponent<Button>().enabled = false;
        }
        if(_knifeLevel >= 3)
        {
            storeKnifeTextCoin2.SetActive(false);
            storeKnifeTextUsed2.SetActive(true);
            storeKnifeTextCoinImage2.GetComponent<Image>().enabled = false;
            storeKnifeThreeButton.GetComponent<Button>().enabled = false;
        }
    }

    private void OnEnable()
    {
        OnIsOver += OnIsOverFunc;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(SceneManager.GetActiveScene().buildIndex != 0)
            Start();
    }

    private void OnIsOverFunc(int totalCoin)
    {
        if(sound && success == false)
        {
            gameSuccess.Play();
        }
        canvasSuccess.SetActive(true);
        success = true;
        successCoinText.text = PlayerPrefs.GetInt("coin").ToString();
        successEarnedText.text = totalCoin.ToString();
    }

    private void OnDisable()
    {
        OnIsOver -= OnIsOverFunc;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Update()
    {
        if(canvasStart.activeSelf)
        {
            if(_knife != null)
                _knife.GetComponent<KnifeController>().enabled = false;
        }
        else
        {
            if(_knife != null)
                _knife.GetComponent<KnifeController>().enabled = true;
        }
    }

    public void StartTrigger()
    {
        play = true;
        isActive = true;
        playCoinText.text = PlayerPrefs.GetInt("coin").ToString();
    }
    

    public void UISoundandVibration()
    {
        if (vibrate)
        {
            Vibration.Vibrate(150);
        }
        if (sound)
        {
            menuClick.Play();
        }
    }

    public void SuccessStart()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _level++;
        PlayerPrefs.SetInt("level", _level);
    }

    public void LoadScene(int sceneIndex)
    {
        if (sceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            sceneIndex = 0;
        }
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return SceneManager.LoadSceneAsync(sceneIndex);
    }

    //====== STORE ======

    public void Store()
    {
        storeCoinText.text = PlayerPrefs.GetInt("coin").ToString();

        switch (PlayerPrefs.GetInt("knifeLevel"))
        {
            case 1:
                storeImageFrame1.SetActive(true);
                storeImageFrame2.SetActive(false);
                storeImageFrame3.SetActive(false);
                break;
            case 2:
                storeImageFrame1.SetActive(false);
                storeImageFrame2.SetActive(true);
                storeImageFrame3.SetActive(false);
                break;
            case 3:
                storeImageFrame1.SetActive(false);
                storeImageFrame2.SetActive(false);
                storeImageFrame3.SetActive(true);
                break;
        }
    }

    public void StoreExit()
    {
        startCoinText.text = PlayerPrefs.GetInt("coin").ToString();
    }
    
    public void KnifeTwo()
    {
        if (coin >= 700)
        {
            coin -= 700;
            PlayerPrefs.SetInt("coin", coin);
            storeCoinText.text = PlayerPrefs.GetInt("coin").ToString();
            _knifeLevel = 2;
            PlayerPrefs.SetInt("knifeLevel", _knifeLevel);
            storeImageFrame2.SetActive(true);
            storeImageFrame1.SetActive(false);
            storeKnifeTextCoin1.SetActive(false);
            storeKnifeTextUsed1.SetActive(true);
            storeKnifeTextCoinImage1.GetComponent<Image>().enabled = false;
            storeKnifeTwoButton.GetComponent<Button>().enabled = false;
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void KnifeThree()
    {
        if (coin >= 2900)
        {
            coin -= 2900;
            PlayerPrefs.SetInt("coin", coin);
            storeCoinText.text = PlayerPrefs.GetInt("coin").ToString();
            _knifeLevel = 3;
            PlayerPrefs.SetInt("knifeLevel", _knifeLevel);
            storeImageFrame2.SetActive(false);
            storeImageFrame1.SetActive(false);
            storeImageFrame3.SetActive(true);
            storeKnifeTextCoin2.SetActive(false);
            storeKnifeTextUsed2.SetActive(true);
            storeKnifeTextCoinImage2.GetComponent<Image>().enabled = false;
            storeKnifeThreeButton.GetComponent<Button>().enabled = false;
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    //====== UPGRADE ======
    public void Upgrade()
    {
        upgradeCoinText.text = PlayerPrefs.GetInt("coin").ToString();
    }

    public void UpgradeExit()
    {
        startCoinText.text = PlayerPrefs.GetInt("coin").ToString();
    }

    public void IncomeUpgrade()
    {
        if(coin >= 500)
        {
            _incomeLevel++;
            PlayerPrefs.SetInt("incomeLevel", _incomeLevel);
            coin -= 500;
            PlayerPrefs.SetInt("coin", coin);
            upgradeCoinText.text = PlayerPrefs.GetInt("coin").ToString();
        }
    }

    //====== SETTINGS ======

    public void SettingsSoundOn()
    {
        sound = false;
        PlayerPrefsSetBool("sound", sound);
    }

    public void SettingsSoundOff()
    {
        sound = true;
        PlayerPrefsSetBool("sound", sound);
    }

    public void SettingsVibrateOn()
    {
        vibrate = false;
        PlayerPrefsSetBool("vibrate", vibrate);
    }

    public void SettingsVibrateOff()
    {
        vibrate = true;
        PlayerPrefsSetBool("vibrate", vibrate);
    }

    void PlayerPrefsSetBool(string hash, bool value)
    {
        int deger = (value == false) ? 0 : 1;
        PlayerPrefs.SetInt(hash, deger);
    }

    bool PlayerPrefsGetBool(string hash)
    {
        bool durum = (PlayerPrefs.GetInt(hash) == 0) ? false : true;
        return durum;
    }
}
