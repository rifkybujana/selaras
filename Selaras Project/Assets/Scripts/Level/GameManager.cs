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

        [HideInInspector] public Vignette vignette;
        [HideInInspector] public DepthOfField depthOfField;
        [HideInInspector] public PaniniProjection paniniProjection;
    }

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

            case UIPos.Menu:
                break;

            case UIPos.Pause:
                pauseUI.UI.SetActive(true);
                break;

            case UIPos.Play:
                playUI.UI.SetActive(true);
                break;

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
    }
}
