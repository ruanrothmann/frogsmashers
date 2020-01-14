using UnityEngine;
using System.Collections;

namespace FreeLives
{
    [System.Serializable]
    public class SoundInfo
    {
        public string name;
        public AudioClip[] clips;
        public float volumeMod = 1f;
        public float pitchVariance = 0.1f;
    }


    public class SoundHolder : MonoBehaviour
    {
        public SoundInfo[] sounds;
    }

}