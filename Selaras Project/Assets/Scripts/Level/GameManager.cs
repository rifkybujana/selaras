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
        Pause,
        Death,
        Exit
    }

    public effect PostProcessingEffect = new effect();
    public GameObject playUI;
    public GameObject pauseUI;
    public GameObject deathUI;
    public GameObject menuUI;
    public GameObject exitUI;

    public TMP_Text speedUI;
    public TMP_Text distanceUI;

    public CinemachineVirtualCamera vCamera;
    public BuoyancyEffector2D baseWater;
=======
    public GameObject uPause;
    public GameObject uDeath;

>>>>>>> parent of 94f23c4... UI

=======
    public GameObject uPause;
    public GameObject uDeath;

>>>>>>> parent of 94f23c4... UI

    Dictionary<UIPos, GameObject> UI = new Dictionary<UIPos, GameObject>();

    //Post processing effect
    [HideInInspector] public Vignette vignette;
    [HideInInspector] public DepthOfField depthOfField;
    [HideInInspector] public PaniniProjection paniniProjection;

<<<<<<< HEAD
<<<<<<< HEAD
    private UIPos lastUiPos;
    private UIPos uiPos = UIPos.Menu;
=======
    [HideInInspector] public PlayerController player;
>>>>>>> parent of 94f23c4... UI
=======
    [HideInInspector] public PlayerController player;
>>>>>>> parent of 94f23c4... UI

    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isStart;

    private float spawnPoint;
    private float StartMagnitude;
    private float distance()
    {
        return Mathf.Abs((int)(spawnPoint - player.transform.position.x));
    }
    private float speed()
    {
        return Mathf.Abs((int)player.GetComponent<Rigidbody2D>().velocity.x);
    }

    private void Awake()
    {
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<DepthOfField>(out depthOfField);
        volume.sharedProfile.TryGet<PaniniProjection>(out paniniProjection);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

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
        Time.timeScale = 1;
        spawnPoint = player.transform.position.x;

        isDeath = false;
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
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
<<<<<<< HEAD
        /*if(uiPos != lastUiPos)
        {
            ChangeUI();
        }*/

        if (isDeath)
        {
            Death();
        }
    }

    public void StartGame()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        baseWater.flowMagnitude = StartMagnitude;
        isStart = true;

        uiPos = UIPos.Play;
    }

    public void SetSpeedAndDistance()
    {
        if (isDeath) return;

        distanceUI.text = distance().ToString() + " m";
        speedUI.text = speed().ToString() + " m / s";
    }

    public void PauseGame()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0;

        uiPos = UIPos.Pause;
    }

    public void Death()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0.5f;

        uiPos = UIPos.Death;
=======
        uDistance.text = distance().ToString();

        if (isDeath)
        {
            depthOfField.focusDistance.value = 0.1f;
            //Time.timeScale = 0;
        }
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
    }

    public void Respawn()
    {

<<<<<<< HEAD
>>>>>>> parent of 94f23c4... UI
    }
    
    public void Home()
    {
<<<<<<< HEAD
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
=======
>>>>>>> parent of 94f23c4... UI
    }

    public void ChangeUI()
    {
        UI[lastUiPos].SetActive(false);
        UI[uiPos].SetActive(true);

        lastUiPos = uiPos;
=======

>>>>>>> parent of 94f23c4... UI
    }
}
