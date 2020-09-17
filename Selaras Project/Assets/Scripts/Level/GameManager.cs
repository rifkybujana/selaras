using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("PostProcessing Effect")]

    [Tooltip("Post Processing Volume")]
    [SerializeField] private Volume volume = null;

    [Space(10)]
    [Header("Cinemachine Camera")]
    public CinemachineVirtualCamera vCamera;

    [Space(10)]
    [Header("UI")]
    public TMP_Text uDistance;

    public GameObject uPause;
    public GameObject uDeath;



    //Post processing effect
    [HideInInspector] public Vignette vignette;
    [HideInInspector] public DepthOfField depthOfField;
    [HideInInspector] public PaniniProjection paniniProjection;

    [HideInInspector] public PlayerController player;

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

        depthOfField.focusDistance.value = 0.5f;
        Time.timeScale = 1;
        spawnPoint = player.transform.position.x;

        isDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        uDistance.text = distance().ToString();

        if (isDeath)
        {
            depthOfField.focusDistance.value = 0.1f;
            //Time.timeScale = 0;
        }
    }

    public void Respawn()
    {

    }

    public void Play()
    {

    }
}
