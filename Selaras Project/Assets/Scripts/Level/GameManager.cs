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
        Exit,
        Pause,
        Death
    }

    public effect PostProcessingEffect = new effect();
    public GameObject playUI;
    public GameObject menuUI;
    public GameObject exitUI;
    public GameObject pauseUI;
    public GameObject deathUI;

    public TMP_Text distanceText;
    public TMP_Text speedText;

    public CinemachineVirtualCamera vCamera;
    public BuoyancyEffector2D BaseWaterBuoyancy;
    public LevelGenerator levelGenerator;

    public Dictionary<UIPos, GameObject> UI = new Dictionary<UIPos, GameObject>();


    private float startWaterFlowMagnitude;

    //Post processing effect
    [HideInInspector] public PlayerController player;

    private UIPos lastUiPos;
    [HideInInspector] public UIPos uiPos = UIPos.Menu;

    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isStart;

    [HideInInspector] public Vector3 spawnPoint;

    [HideInInspector] public int distance()
    {
        return Mathf.Abs((int)(spawnPoint.x - player.transform.position.x));
    }
    [HideInInspector] public int PhotoCaptured;
    [HideInInspector] public int maxDistance;
    [HideInInspector] public int maxSpeed;

    private void Awake()
    {
        PostProcessingEffect.volume.sharedProfile.TryGet<Vignette>(out PostProcessingEffect.vignette);
        PostProcessingEffect.volume.sharedProfile.TryGet<DepthOfField>(out PostProcessingEffect.depthOfField);
        PostProcessingEffect.volume.sharedProfile.TryGet<PaniniProjection>(out PostProcessingEffect.paniniProjection);

        UI.Add(UIPos.Death, deathUI);
        UI.Add(UIPos.Exit, exitUI);
        UI.Add(UIPos.Menu, menuUI);
        UI.Add(UIPos.Pause, pauseUI);
        UI.Add(UIPos.Play, playUI);


        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;

        startWaterFlowMagnitude = BaseWaterBuoyancy.flowMagnitude;
        BaseWaterBuoyancy.flowMagnitude = 0;

        spawnPoint = player.transform.position;
        Time.timeScale = 1;

        isDeath = false;

        uiPos = UIPos.Menu;
        lastUiPos = uiPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            if (isDeath) isDeath = false;

            return;
        }

        if (isDeath)
        {
            Death();
        }
        else
        {
            distanceText.text = distance().ToString() + " m";
            speedText.text = ((int)player.GetComponent<Rigidbody2D>().velocity.x).ToString() + " m/s";
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(uiPos == UIPos.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (uiPos != lastUiPos)
        {
            SetUI();
        }
    }

    public void SetUI()
    {
        UI[lastUiPos].SetActive(false);
        UI[uiPos].SetActive(true);

        lastUiPos = uiPos;
    }

    public void Death()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0.5f;

        uiPos = UIPos.Death;
    }

    public void PauseGame()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0f;

        uiPos = UIPos.Pause;
    }

    public void ResumeGame()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        Time.timeScale = 1f;

        uiPos = UIPos.Play;
    }

    public void TryExit()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0f;

        uiPos = UIPos.Exit;
    }

    public void ConfirmExit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {

    }

    public void StartGame()
    {
        isStart = true;
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        BaseWaterBuoyancy.flowMagnitude = startWaterFlowMagnitude;

        uiPos = UIPos.Play;
    }

    public void RestartGame()
    {
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        isDeath = false;
        player.transform.position = spawnPoint;
        levelGenerator.ResetLevel();
        Time.timeScale = 1;

        maxDistance = 0;
        maxSpeed = 0;

        uiPos = UIPos.Play;
    }

    public void Home()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
