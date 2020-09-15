using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    [Header("PostProcessing Effect")]

    [Tooltip("Post Processing Volume")]
    [SerializeField] private Volume volume;

    [Space(10)]
    [Header("Cinemachine Camera")]

    public CinemachineVirtualCamera vCamera;

    //Post processing effect
    [HideInInspector] public Vignette vignette;
    [HideInInspector] public DepthOfField depthOfField;
    [HideInInspector] public PaniniProjection paniniProjection;

    [HideInInspector] public bool isDeath;

    private void Awake()
    {
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<DepthOfField>(out depthOfField);
        volume.sharedProfile.TryGet<PaniniProjection>(out paniniProjection);

        depthOfField.focusDistance.value = 0.5f;

        isDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {

    }

    public void Play()
    {

    }
}
