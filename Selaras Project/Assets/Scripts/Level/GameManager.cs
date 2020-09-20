using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [System.Serializable]
    public class effect
    {
        public Volume volume = null;

        [HideInInspector] public Vignette vignette;
        [HideInInspector] public DepthOfField depthOfField;
        [HideInInspector] public PaniniProjection paniniProjection;
    }

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


    Dictionary<UIPos, GameObject> UI = new Dictionary<UIPos, GameObject>();

    //Post processing effect
    [HideInInspector] public PlayerController player;

    private UIPos lastUiPos;
    private UIPos uiPos = UIPos.Menu;

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
        PostProcessingEffect.volume.sharedProfile.TryGet<Vignette>(out PostProcessingEffect.vignette);
        PostProcessingEffect.volume.sharedProfile.TryGet<DepthOfField>(out PostProcessingEffect.depthOfField);
        PostProcessingEffect.volume.sharedProfile.TryGet<PaniniProjection>(out PostProcessingEffect.paniniProjection);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        spawnPoint = player.transform.position.x;
        StartMagnitude = baseWater.flowMagnitude;
        baseWater.flowMagnitude = 0;
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 1;

        isDeath = false;
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
    }

    // Update is called once per frame
    void Update()
    {
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
    }
    
    public void Home()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public void ChangeUI()
    {
        UI[lastUiPos].SetActive(false);
        UI[uiPos].SetActive(true);

        lastUiPos = uiPos;
    }
}
