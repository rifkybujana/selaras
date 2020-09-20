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

    public Dictionary<UIPos, GameObject> UI = new Dictionary<UIPos, GameObject>();


    //Post processing effect
    [HideInInspector] public PlayerController player;

    private UIPos lastUiPos;
    [HideInInspector] public UIPos uiPos = UIPos.Play;

    [HideInInspector] public bool isDeath;

    [HideInInspector] public float spawnPoint;
    [HideInInspector] public float distance()
    {
        return Mathf.Abs((int)(spawnPoint - player.transform.position.x));
    }

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

        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        spawnPoint = player.transform.position.x;
        Time.timeScale = 1;

        isDeath = false;

        lastUiPos = uiPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            Death();
        }
        else
        {
            distanceText.text = distance().ToString();
            speedText.text = player.GetComponent<Rigidbody2D>().velocity.x.ToString();
        }

        if(uiPos != lastUiPos)
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

    public void RestartGame()
    {

    }

    public void Home()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
