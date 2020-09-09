using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [Tooltip("Batas waktu minimal untuk player dikatakan sedang menahan mousenya")]
    [Range(0, 1)] public float buttonHoldMin = 0.3f;

    [Range(0, 2)] public float MaxInputTime = 0.5f;

    #region Hidden Variable

    /// <summary>
    /// Berapa lama player menekan mousennya
    /// </summary>
    [HideInInspector] public float buttonHoldTime;
    [HideInInspector] public float inputTimer;

    #endregion
}
