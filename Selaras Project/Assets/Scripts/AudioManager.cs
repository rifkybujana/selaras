using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public class sound
    {
        public string name;

        public AudioClip clip;
        public float volume;
        public float pitch;

        [HideInInspector]
        public AudioSource source;
    }

    [Space(10)]
    public sound[] sounds;

    // Start is called before the first frame update
    private void Awake()
    {
        foreach (sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void PlaySound(string name)
    {
        sound s = sounds.Where(x => x.name == name).First();
        s.source.Play();
    }
}
