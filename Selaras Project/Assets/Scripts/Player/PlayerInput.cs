using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

[System.Serializable]
public class PlayerInput
{
    [Tooltip("Batas waktu minimal untuk player dikatakan sedang menahan mousenya")]
    [Range(0, 1)] public float buttonHoldMin = 0.3f;

    #region Hidden Variable

    /// <summary>
    /// Berapa lama player menekan mousennya
    /// </summary>
    [HideInInspector] public float buttonHoldTime;

    #endregion
}
