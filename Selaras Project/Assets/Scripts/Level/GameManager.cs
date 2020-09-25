using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
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

    public TMP_Text maxDistanceText;
    public TMP_Text maxSpeedText;
    public TMP_Text photoCapturedText;

    public Button CapturePhoto;

    public Vector2 ScreenShotResolution = new Vector2(2550, 3300);

    public CinemachineVirtualCamera vCamera;
    public BuoyancyEffector2D BaseWaterBuoyancy;
    public LevelGenerator levelGenerator;

    public Dictionary<UIPos, GameObject> UI = new Dictionary<UIPos, GameObject>();


    private float startWaterFlowMagnitude;

    //Post processing effect
    [HideInInspector] public PlayerController player;
    [HideInInspector] public ProceduralGenerator pGen;

    private UIPos lastUiPos;
    [HideInInspector] public UIPos uiPos = UIPos.Menu;

    [HideInInspector] public bool isDeath;
    [HideInInspector] public bool isStart;

    [HideInInspector] public Vector3 spawnPoint;

    [HideInInspector] public int distance()
    {
        return Mathf.Abs((int)(spawnPoint.x - player.transform.position.x));
    }
    [HideInInspector] public int speed()
    {
        return Mathf.Abs((int)player.GetComponent<Rigidbody2D>().velocity.x);
    }

    [HideInInspector] public int PhotoCaptured;
    [HideInInspector] public int maxDistance;
    [HideInInspector] public int maxSpeed;

    private void Awake()
    {
        vCamera.Follow = null;
        vCamera.LookAt = null;

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
        if (uiPos != lastUiPos)
        {
            Debug.Log(uiPos + " " + lastUiPos);
            SetUI();
        }

        if (!isStart)
        {
            if (isDeath) isDeath = false;

            return;
        }

        if(vCamera.Follow == null)
        {
            vCamera.Follow = player.transform;
            vCamera.LookAt = player.transform;
        }

        if (isDeath)
        {
            Death();
        }
        else
        {
            distanceText.text = distance().ToString() + " m";
            speedText.text = ((int)player.GetComponent<Rigidbody2D>().velocity.x).ToString() + " m/s";

            GetMaxDistance();
            GetMaxSpeed();
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

        if(pGen != null && pGen.meshType == ProceduralGenerator.MeshType.Flat && pGen.flatType == ProceduralGenerator.FlatType.PhotoSpot)
        {
            CapturePhoto.gameObject.SetActive(true);
        }
        else
        {
            CapturePhoto.gameObject.SetActive(false);
        }

        if (!CapturePhoto.enabled)
        {
            StartCoroutine(EnableCapturePhoto());
        }
    }

    IEnumerator EnableCapturePhoto()
    {
        yield return new WaitForSeconds(3);

        CapturePhoto.enabled = true;
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

        maxDistanceText.text = maxDistance + " m";
        maxSpeedText.text = maxSpeed + " m/s";
        photoCapturedText.text = PhotoCaptured.ToString();

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
        player.transform.rotation = Quaternion.identity;
        levelGenerator.ResetLevel();
        Time.timeScale = 1;

        maxDistance = 0;
        maxSpeed = 0;

        uiPos = UIPos.Play;
    }

    public void Home()
    {
        isDeath = false;
        isStart = false;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = spawnPoint;
        player.transform.rotation = Quaternion.identity;

        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        levelGenerator.ResetLevel();

        BaseWaterBuoyancy.flowMagnitude = 0;
        Time.timeScale = 1;
        maxDistance = 0;
        maxSpeed = 0;

        uiPos = UIPos.Menu;

        vCamera.Follow = null;
        vCamera.LookAt = null;

        //SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                              Application.dataPath,
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void TakePhoto()
    {
        StartCoroutine(CaptureScreen());

        PhotoCaptured++;
        CapturePhoto.enabled = false;
    }

    public IEnumerator CaptureScreen()
    {
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        ScreenCapture.CaptureScreenshot(ScreenShotName((int)ScreenShotResolution.x, (int)ScreenShotResolution.y));

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
    }

    public void GetMaxSpeed()
    {
        if (speed() > maxSpeed) maxSpeed = speed();
    }

    public void GetMaxDistance()
    {
        if (distance() > maxDistance) maxDistance = distance();
    }
}
