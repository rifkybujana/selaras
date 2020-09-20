using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    [System.Serializable]
    public class PlayUI
    {
        public GameObject UI;
        public TMP_Text distance;
    }

    [System.Serializable]
    public class PauseUI
    {
        public GameObject UI;
    }

    [System.Serializable]
    public class DeathUI
    {
        public GameObject UI;

        public TMP_Text pictureTaken;
        public TMP_Text coinTaken;
        public TMP_Text totalPoint;
    }

    [System.Serializable]
    public class PlayUI
    {
        public GameObject UI;
        public TMP_Text distance;
    }

    [System.Serializable]
    public class PauseUI
    {
        public GameObject UI;
    }

    [System.Serializable]
    public class DeathUI
    {
        public GameObject UI;

        public TMP_Text pictureTaken;
        public TMP_Text coinTaken;
        public TMP_Text totalPoint;
    }

    [System.Serializable]
    public class PlayUI
    {
        public GameObject UI;
        public TMP_Text distance;
    }

    [System.Serializable]
    public class PauseUI
    {
        public GameObject UI;
    }

    [System.Serializable]
    public class DeathUI
    {
        public GameObject UI;

        public TMP_Text pictureTaken;
        public TMP_Text coinTaken;
        public TMP_Text totalPoint;
    }

    [System.Serializable]
    public class effect
    {
        public Volume volume = null;
=======
    [Header("PostProcessing Effect")]

    [Tooltip("Post Processing Volume")]
    [SerializeField] private Volume volume = null;

    [Space(10)]
    [Header("Cinemachine Camera")]
    public CinemachineVirtualCamera vCamera;
>>>>>>> parent of 94f23c4... UI

=======
    [Header("PostProcessing Effect")]

    [Tooltip("Post Processing Volume")]
    [SerializeField] private Volume volume = null;

    [Space(10)]
    [Header("Cinemachine Camera")]
    public CinemachineVirtualCamera vCamera;

>>>>>>> parent of 94f23c4... UI
    [Space(10)]
    [Header("UI")]
    public TMP_Text uDistance;

<<<<<<< HEAD
<<<<<<< HEAD
    public enum UIPos
    {
        Play,
        Menu,
        Setting,
        Pause,
        Death
    }

    public effect PostProcessingEffect = new effect();
    public PlayUI playUI = new PlayUI();
    public PauseUI pauseUI = new PauseUI();
    public DeathUI deathUI = new DeathUI();

    public CinemachineVirtualCamera vCamera;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    public BuoyancyEffector2D baseWater;
=======
    public GameObject uPause;
    public GameObject uDeath;

>>>>>>> parent of 94f23c4... UI

=======
    public GameObject uPause;
    public GameObject uDeath;
=======
>>>>>>> parent of 4c7e4ec... FUCK
=======
>>>>>>> parent of 4c7e4ec... FUCK

>>>>>>> parent of 94f23c4... UI
=======
    [Header("PostProcessing Effect")]

<<<<<<< HEAD
    [Tooltip("Post Processing Volume")]
    [SerializeField] private Volume volume = null;

    [Space(10)]
    [Header("Cinemachine Camera")]
    public CinemachineVirtualCamera vCamera;

    [Space(10)]
    [Header("UI")]
    public TMP_Text uDistance;
=======
>>>>>>> parent of 4c7e4ec... FUCK

    public GameObject uPause;
    public GameObject uDeath;

<<<<<<< HEAD
>>>>>>> parent of 94f23c4... UI

=======
>>>>>>> parent of 4c7e4ec... FUCK
=======
>>>>>>> parent of 4c7e4ec... FUCK
    //Post processing effect
    [HideInInspector] public Vignette vignette;
    [HideInInspector] public DepthOfField depthOfField;
    [HideInInspector] public PaniniProjection paniniProjection;

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    private UIPos lastUiPos;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
    private UIPos uiPos = UIPos.Menu;
=======
    [HideInInspector] public PlayerController player;
>>>>>>> parent of 94f23c4... UI
=======
    [HideInInspector] public PlayerController player;
>>>>>>> parent of 94f23c4... UI
=======
    [HideInInspector] public PlayerController player;
>>>>>>> parent of 94f23c4... UI
=======
    [HideInInspector] public UIPos uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK
=======
    [HideInInspector] public UIPos uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK
=======
    [HideInInspector] public UIPos uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK

    [HideInInspector] public bool isDeath;

    [HideInInspector] public float spawnPoint;
    [HideInInspector] public float distance()
    {
        return Mathf.Abs((int)(spawnPoint - player.transform.position.x));
    }

    private void Awake()
    {
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<DepthOfField>(out depthOfField);
        volume.sharedProfile.TryGet<PaniniProjection>(out paniniProjection);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        spawnPoint = player.transform.position.x;
        StartMagnitude = baseWater.flowMagnitude;
        baseWater.flowMagnitude = 0;
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
=======
        depthOfField.focusDistance.value = 0.5f;
>>>>>>> parent of 94f23c4... UI
=======
        depthOfField.focusDistance.value = 0.5f;
>>>>>>> parent of 94f23c4... UI
=======
        depthOfField.focusDistance.value = 0.5f;
>>>>>>> parent of 94f23c4... UI
=======
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        spawnPoint = player.transform.position.x;
>>>>>>> parent of 4c7e4ec... FUCK
=======
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        spawnPoint = player.transform.position.x;
>>>>>>> parent of 4c7e4ec... FUCK
=======
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        spawnPoint = player.transform.position.x;
>>>>>>> parent of 4c7e4ec... FUCK
        Time.timeScale = 1;
        spawnPoint = player.transform.position.x;

        isDeath = false;
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        isStart = false;
        lastUiPos = uiPos;

        //UISetup();
        Home();

        InvokeRepeating("SetSpeedAndDistance", 0, 0.5f);
    }

    void UISetup()
    {
        UI.Add(UIPos.Death, deathUI);
        UI.Add(UIPos.Exit, exitUI);
        UI.Add(UIPos.Menu, menuUI);
        UI.Add(UIPos.Pause, pauseUI);
        UI.Add(UIPos.Play, playUI);
=======
>>>>>>> parent of 94f23c4... UI
=======
>>>>>>> parent of 94f23c4... UI
=======
>>>>>>> parent of 94f23c4... UI
=======

        lastUiPos = uiPos;
>>>>>>> parent of 4c7e4ec... FUCK
=======

        lastUiPos = uiPos;
>>>>>>> parent of 4c7e4ec... FUCK
=======

        lastUiPos = uiPos;
>>>>>>> parent of 4c7e4ec... FUCK
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        /*if(uiPos != lastUiPos)
=======
        if (isDeath)
>>>>>>> parent of 4c7e4ec... FUCK
=======
        if (isDeath)
>>>>>>> parent of 4c7e4ec... FUCK
=======
        if (isDeath)
>>>>>>> parent of 4c7e4ec... FUCK
        {
            PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
            Time.timeScale = 0.5f;

            uiPos = UIPos.Death;
        }
        else
        {
            playUI.distance.text = distance().ToString();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isDeath)
        {
            if(uiPos == UIPos.Pause)
            {
                Resume();
            }
            else if(uiPos == UIPos.Play)
            {
                Pause();
            }
        }

        if(uiPos != lastUiPos)
        {
            SetUI();
        }
    }

    public void SetUI()
    {
        switch (lastUiPos)
        {
            case UIPos.Death:
                deathUI.UI.SetActive(false);
                break;

            case UIPos.Menu:
                break;

            case UIPos.Pause:
                pauseUI.UI.SetActive(false);
                break;

            case UIPos.Play:
                playUI.UI.SetActive(false);
                break;

            case UIPos.Setting:
                break;

            default:
                Debug.LogError("Unknown UI Type");
                break;
        }

        switch (uiPos)
        {
            case UIPos.Death:
                deathUI.UI.SetActive(true);
                break;
<<<<<<< HEAD
<<<<<<< HEAD

            case UIPos.Menu:
                break;

            case UIPos.Pause:
                pauseUI.UI.SetActive(true);
                break;

            case UIPos.Play:
                playUI.UI.SetActive(true);
                break;

=======

            case UIPos.Menu:
                break;

            case UIPos.Pause:
                pauseUI.UI.SetActive(true);
                break;

            case UIPos.Play:
                playUI.UI.SetActive(true);
                break;

>>>>>>> parent of 4c7e4ec... FUCK
=======

            case UIPos.Menu:
                break;

            case UIPos.Pause:
                pauseUI.UI.SetActive(true);
                break;

            case UIPos.Play:
                playUI.UI.SetActive(true);
                break;

>>>>>>> parent of 4c7e4ec... FUCK
            case UIPos.Setting:
                break;

            default:
                Debug.LogError("Unknown UI Type");
                break;
        }

        lastUiPos = uiPos;
    }

    public void Pause()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0;

        uiPos = UIPos.Pause;
    }

    public void Resume()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        Time.timeScale = 1;

<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        uiPos = UIPos.Death;
=======
        uDistance.text = distance().ToString();

=======
        uDistance.text = distance().ToString();

>>>>>>> parent of 94f23c4... UI
        if (isDeath)
        {
            depthOfField.focusDistance.value = 0.1f;
            //Time.timeScale = 0;
        }
<<<<<<< HEAD
=======
        uDistance.text = distance().ToString();

        if (isDeath)
        {
            depthOfField.focusDistance.value = 0.1f;
            //Time.timeScale = 0;
        }
    }

    public void Respawn()
    {

>>>>>>> parent of 94f23c4... UI
=======
>>>>>>> parent of 94f23c4... UI
    }

    public void Respawn()
    {

<<<<<<< HEAD
<<<<<<< HEAD
>>>>>>> parent of 94f23c4... UI
=======
>>>>>>> parent of 94f23c4... UI
=======
        uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK
    }

    public void Play()
    {
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
=======
>>>>>>> parent of 94f23c4... UI
=======
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;

        uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK
=======
        uiPos = UIPos.Play;
    }

    public void Play()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;

        uiPos = UIPos.Play;
>>>>>>> parent of 4c7e4ec... FUCK
    }

    public void Restart()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        UI[lastUiPos].SetActive(false);
        UI[uiPos].SetActive(true);

        lastUiPos = uiPos;
=======

>>>>>>> parent of 94f23c4... UI
=======

>>>>>>> parent of 94f23c4... UI
=======
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
>>>>>>> parent of 4c7e4ec... FUCK
=======
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
>>>>>>> parent of 4c7e4ec... FUCK
=======
        uiPos = UIPos.Play;
    }

    public void Play()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;

        uiPos = UIPos.Play;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
>>>>>>> parent of 4c7e4ec... FUCK
    }
}
