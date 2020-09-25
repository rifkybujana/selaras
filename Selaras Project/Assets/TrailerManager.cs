using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class TrailerManager : MonoBehaviour
{
    public TMP_Text title;

    public Volume volume;

    private DepthOfField dof;
    private Vignette vignette;

    // Start is called before the first frame update
    void Start()
    {
        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<DepthOfField>(out dof);
    }

    // Update is called once per frame
    void Update()
    {
        if(dof.focusDistance.value > 0)
        {
            dof.focusDistance.value -= Time.deltaTime;
        }

        if(title.color.a > 0)
        {
            Color color = title.color;
            color.a -= Time.deltaTime;
            title.color = color;
        }
    }
}
