using UnityEngine;
using System.Collections;

namespace FreeLives
{
    public class SoundController : MonoBehaviour
    {
        SoundHolder globalSounds;
        private static SoundController instance;

        protected void Awake()
        {   
            globalSounds = GetComponent<SoundHolder>();
            if (instance != null)
            {
                GameObject.DestroyImmediate(instance);
            }
            instance = this;
        }


        public static void PlaySoundEffect(string soundEffectName)
        {
            PlaySoundEffect(instance.globalSounds, soundEffectName);
        }

        public static void PlaySoundEffect(SoundHolder soundHolder, string soundEffectName)
        {
            var audio = CreateAudioSource();
            audio.spatialize = false;
            var info = GetSoundInfo(soundHolder, soundEffectName);
            var clip = info.clips[Random.Range(0, info.clips.Length)];

            audio.clip = clip;
            audio.volume = info.volumeMod;
            audio.pitch = GetPitch(info.pitchVariance);
            audio.Play();

            Destroy(audio.gameObject, clip.length * audio.pitch + 1f);
        }


        public static void StopMusic()
        {
            if (instance.GetComponent<AudioSource>() != null)
            {
                instance.GetComponent<AudioSource>().Stop();
            }
        }

        public static void PlaySoundEffect(string soundEffectName, float volume)
        {
            PlaySoundEffect(instance.globalSounds, soundEffectName, volume);
        }

        public static void PlaySoundEffect(SoundHolder soundHolder, string soundEffectName, float volume)
        {
            
            var info = GetSoundInfo(soundHolder, soundEffectName);
            if (info != null && info.clips.Length > 0)
            {
                var audio = CreateAudioSource();
                audio.spatialize = false;
                var clip = info.clips[Random.Range(0, info.clips.Length)];

                audio.clip = clip;
                audio.volume = info.volumeMod * volume;
                audio.pitch = GetPitch(info.pitchVariance);
                audio.Play();

                Destroy(audio.gameObject, clip.length * audio.pitch + 1f);
            }
        }

        public static void PlaySoundEffect(string soundEffectName, float volume, Vector3 pos)
        {
            PlaySoundEffect(instance.globalSounds, soundEffectName, volume, pos);
        }


        public static void PlaySoundEffect(SoundHolder soundHolder, string soundEffectName, float volume, Vector3 pos)
        {
            
            var info = GetSoundInfo(soundHolder, soundEffectName);
            if (info == null || info.clips.Length == 0)
            {
                Debug.LogError("No sound effects for " + soundEffectName);
                return;
            }

            var audio = CreateAudioSource();
            audio.transform.position = pos;
            var clip = info.clips[Random.Range(0, info.clips.Length)];
            
            audio.spatialBlend = 1f;
            audio.clip = clip;
            audio.volume = info.volumeMod * volume;
            audio.pitch = GetPitch(info.pitchVariance);
            audio.Play();

            Destroy(audio.gameObject, clip.length * audio.pitch + 1f);
        }

        public static void PlaySoundEffect(string soundEffectName, float volume, Vector3 pos, float pitch)
        {
            PlaySoundEffect(instance.globalSounds, soundEffectName, volume, pos, pitch);
        }

        public static void PlaySoundEffect(SoundHolder soundHolder, string soundEffectName, float volume, Vector3 pos, float pitch)
        {
            var audio = CreateAudioSource();
            audio.transform.position = pos;
            var info = GetSoundInfo(soundHolder, soundEffectName);
            var clip = info.clips[Random.Range(0, info.clips.Length)];
            
            audio.spatialBlend = 1f;
            audio.clip = clip;
            audio.volume = info.volumeMod * volume;
            audio.pitch = pitch * GetPitch(info.pitchVariance);
            audio.Play();

            Destroy(audio.gameObject, clip.length * audio.pitch + 1f);
        }


        private static SoundInfo GetSoundInfo (SoundHolder soundHolder, string name)
        {
            name = name.ToUpper();
            for (int i = 0; i < soundHolder.sounds.Length; i++)
            {
                if (name.Equals(soundHolder.sounds[i].name.ToUpper()))
                {
                    return soundHolder.sounds[i];
                }
            }
            Debug.LogError("Could not find sound: " + name);
            return null;
        }

        private static AudioSource CreateAudioSource()
        {
            var go = new GameObject();
            go.name = "Audio Oneshot";
            var audio = go.AddComponent<AudioSource>();
            audio.rolloffMode = AudioRolloffMode.Linear;
            audio.dopplerLevel = 0f;
            return audio;
        }

        private static float GetPitch(float variance)
        {
            float amount = 1f + variance;
            float min = 1f / amount;
            float max = 1f + variance;
            return Random.Range(min, max);
        }
    }
}