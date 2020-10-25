using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Windows;
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

        public void Setup()
        {
            volume.sharedProfile.TryGet<Vignette>(out vignette);
            volume.sharedProfile.TryGet<DepthOfField>(out depthOfField);
            volume.sharedProfile.TryGet<PaniniProjection>(out paniniProjection);
        }
    }

    public enum UIPos
    {
        Play,
        Menu,
        Exit,
        Pause,
        Death,
        Character,
        Boat,
        Camera
    }

    public GameData data;

    public AudioSource BaseSFX;
    public AudioSource GameMusic;

    public Transform BGParent;

    public effect PostProcessingEffect = new effect();

    public Image PictureShow;

    public GameObject playUI;
    public GameObject menuUI;
    public GameObject exitUI;
    public GameObject pauseUI;
    public GameObject deathUI;
    public GameObject CharacterUI;
    public GameObject BoatUI;
    public GameObject CameraUI;

    public TMP_Text Tutorial;

    public TMP_Text distanceText;
    public TMP_Text speedText;

    public TMP_Text maxDistanceText;
    public TMP_Text maxSpeedText;
    public TMP_Text photoCapturedText;
    public TMP_Text scoreText;
    public TMP_Text yourBest;

    public Button CapturePhoto;

    public Vector2 ScreenShotResolution = new Vector2(2550, 3300);

    public CinemachineVirtualCamera vCamera;
    public CinemachineBasicMultiChannelPerlin Noise;

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

    [HideInInspector] public AudioManager audioManager;

    [HideInInspector] public bool firstTimePlay;

    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        Noise = vCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        Noise.m_AmplitudeGain = 0;
        Noise.m_FrequencyGain = 0;

        StartCoroutine(SoundFadeIn(BaseSFX, 0.5f));

        //set default
        isDeath = false;
        Time.timeScale = 1;

        //if there's no saved file, make new
        if (PlayerPrefs.GetInt("Char Index", -1) == -1)
        {
            Tutorial.gameObject.SetActive(true);
            firstTimePlay = true;
            SavedData.SaveData(true);

            Debug.Log("Making New Saving Data...");
        }

        //get data from the saved file
        SavedData.GetData();

        //reset the virtual camera
        vCamera.Follow = null;
        vCamera.LookAt = null;

        //setting up the post processing
        PostProcessingEffect.Setup();
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        PostProcessingEffect.vignette.center.value = new Vector2(0.2f, 0f);
        PostProcessingEffect.vignette.intensity.value = 0.05f;

        //setting up the UI
        AddUI();
        uiPos = UIPos.Menu;
        lastUiPos = uiPos;

        //setup the water bouyancy physics
        startWaterFlowMagnitude = BaseWaterBuoyancy.flowMagnitude;
        BaseWaterBuoyancy.flowMagnitude = 0;

        //setting up the player
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        spawnPoint = player.transform.position;
        player.character = data.Character[SavedData.CharIndex];
        player.boat = data.Boats[SavedData.BoatIndex];
    }

    /// <summary>
    /// Add every UI windows to the UI dictionary
    /// </summary>
    public void AddUI()
    {
        UI.Add(UIPos.Death, deathUI);
        UI.Add(UIPos.Exit, exitUI);
        UI.Add(UIPos.Menu, menuUI);
        UI.Add(UIPos.Pause, pauseUI);
        UI.Add(UIPos.Play, playUI);
        UI.Add(UIPos.Character, CharacterUI);
        UI.Add(UIPos.Boat, BoatUI);
        UI.Add(UIPos.Camera, CameraUI);
    }

    // Update is called once per frame
    void Update()
    {
        if (!firstTimePlay)
        {
            Tutorial.gameObject.SetActive(false);
        }

        //change the UI if the uiPos was changed
        SetUI();

        //if not yet started or restarted, setup and return
        if (!isStart) return;

        if(distance() > 300 && firstTimePlay)
        {
            firstTimePlay = false;
        }

        if (isDeath)
        {
            Death();
        }
        else
        {
            //show the distance and the speed
            distanceText.text = distance().ToString() + " m";
            speedText.text = ((int)player.GetComponent<Rigidbody2D>().velocity.x).ToString() + " m/s";

            //get the maximum distance and maximum speed
            GetMaxDistance();
            GetMaxSpeed();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && uiPos != UIPos.Camera)
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


        //re-enabled the capture photo button
        if (!CapturePhoto.interactable) StartCoroutine(EnableCapturePhoto());

        CapturePhoto.gameObject.SetActive(pGen != null && pGen.meshType == ProceduralGenerator.MeshType.Flat && pGen.flatType == ProceduralGenerator.FlatType.PhotoSpot);
    }

    public static IEnumerator SoundFadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public static IEnumerator SoundFadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = 0.2f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < 1.0f)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = 1f;
    }

    IEnumerator EnableCapturePhoto()
    {
        yield return new WaitForSeconds(3);

        CapturePhoto.interactable = true;
    }

    public void SetUI()
    {
        if (uiPos != lastUiPos)
        {
            UI[lastUiPos].SetActive(false);
            UI[uiPos].SetActive(true);

            lastUiPos = uiPos;
        }
    }

    public void Death()
    {
        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        Time.timeScale = 0.5f;

        int score = maxDistance + (50 * maxSpeed) + (200 * PhotoCaptured);

        maxDistanceText.text = maxDistance + " m";
        maxSpeedText.text = string.Format("50 x {0} m/s", maxSpeed);
        photoCapturedText.text = string.Format("200 x {0}", PhotoCaptured.ToString());
        scoreText.text = score.ToString();

        yourBest.text = score >= SavedData.bestScore ? "Best Score!" : string.Format("Your Best: {0}", SavedData.bestScore);

        uiPos = UIPos.Death;

        SavedData.SetBestSpeed(maxSpeed);
        SavedData.SetBestDistance(maxDistance);
        SavedData.SetBestScore(score);
        SavedData.SaveData();
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

        StartCoroutine(SoundFadeIn(GameMusic, 0.5f));

        vCamera.Follow = player.transform;
        vCamera.LookAt = player.transform;

        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
        BaseWaterBuoyancy.flowMagnitude = startWaterFlowMagnitude;

        uiPos = UIPos.Play;
    }

    public void RestartGame()
    {
        reset();

        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;

        foreach(GameObject g in GameObject.FindGameObjectsWithTag("BG"))
        {
            Destroy(g);
        }

        foreach(Background bg in BGParent.GetComponentsInChildren<Background>())
        {
            bg.isPlaced = false;
        }

        uiPos = UIPos.Play;
    }

    public void Home()
    {
        reset();

        isStart = false;

        StartCoroutine(SoundFadeOut(GameMusic, 0.5f));

        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;
        BaseWaterBuoyancy.flowMagnitude = 0;

        uiPos = UIPos.Menu;
    }

    public void GoToCharacterSelect()
    {
        uiPos = UIPos.Character;
    }

    public void GoToBoatSelect()
    {
        uiPos = UIPos.Boat;
    }

    public void GoBackToMenu()
    {
        uiPos = UIPos.Menu;
    }

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                              Application.dataPath,
                              width, height,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void OpenPic(Texture2D tx)
    {
        uiPos = UIPos.Camera;

        Time.timeScale = 0;

        PostProcessingEffect.vignette.center.value = new Vector2(0.5f, 0.5f);
        PostProcessingEffect.vignette.intensity.value = 0.255f;

        PostProcessingEffect.depthOfField.focusDistance.value = 0.1f;

        PictureShow.sprite = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), new Vector2(0.5f, 0.5f));
    }

    public void CloseCamera()
    {
        uiPos = UIPos.Play;

        Time.timeScale = 1;

        PostProcessingEffect.vignette.center.value = new Vector2(0.2f, 0f);
        PostProcessingEffect.vignette.intensity.value = 0.05f;

        PostProcessingEffect.depthOfField.focusDistance.value = 0.5f;
    }

    public void OpenPictureFolder()
    {
        System.Diagnostics.Process.Start("explorer.exe", string.Format("{0}/screenshots", Application.dataPath).Replace(@"/", @"\"));
    }

    public void TakePhoto()
    {
        StartCoroutine(CaptureScreen());

        PhotoCaptured++;
        CapturePhoto.interactable = false;
    }

    public IEnumerator CaptureScreen()
    {
        yield return null;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();

        string path = ScreenShotName((int)ScreenShotResolution.x, (int)ScreenShotResolution.y);

        ScreenCapture.CaptureScreenshot(path);
        Texture2D tx = ScreenCapture.CaptureScreenshotAsTexture();

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        OpenPic(tx);
    }

    public void GetMaxSpeed()
    {
        if (speed() > maxSpeed) maxSpeed = speed();
    }

    public void GetMaxDistance()
    {
        if (distance() > maxDistance) maxDistance = distance();
    }

    public void Shake()
    {
        StartCoroutine(CameraShake());
    }

    IEnumerator CameraShake()
    {
        Noise.m_AmplitudeGain = 1;
        Noise.m_FrequencyGain = 1;

        yield return new WaitForSeconds(0.2f);

        Noise.m_AmplitudeGain = 0;
        Noise.m_FrequencyGain = 0;
    }

    void reset()
    {
        isDeath = false;

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.position = spawnPoint;
        player.transform.rotation = Quaternion.identity;

        Time.timeScale = 1;
        maxDistance = 0;
        maxSpeed = 0;

        levelGenerator.ResetLevel();
    }
}
