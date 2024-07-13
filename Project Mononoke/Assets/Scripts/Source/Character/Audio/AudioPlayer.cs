using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Source.Character.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource = null;
        [SerializeField] private List<AudioClipMetadata> _clipsMeta = null;
        private Dictionary<AudioTypes, AudioClip> _sounds = null;
        

        private void OnValidate()
        {
            if(Application.isPlaying) return;
            
            _audioSource = GetComponent<AudioSource>();
            
            if (_clipsMeta == null) return;
            
            InitializeSounds();
        }

        private void InitializeSounds()
        {
            _sounds ??= new Dictionary<AudioTypes, AudioClip>();
            foreach (var clipMeta in _clipsMeta.Where(clipMeta => clipMeta != null && clipMeta.Sound != null))
            {
                if (_sounds.ContainsKey(clipMeta.SoundType)) _sounds[clipMeta.SoundType] = clipMeta.Sound;
                _sounds.Add(clipMeta.SoundType, clipMeta.Sound);
            }
        }

        public void PlaySound(AudioTypes type, float volume = 1f, float minimumPitch = 0.85f, float maximumPitch = 1.25f)
        {
            if(_sounds == null) InitializeSounds();
            
            if(!_sounds.ContainsKey(type)) return;
            var pitch = Random.Range(minimumPitch, maximumPitch);
            _audioSource.pitch = pitch;
            _audioSource.PlayOneShot(_sounds[type], volume);
        }
    }

    [Serializable]
    public class AudioClipMetadata
    {
        [field: SerializeField] public AudioTypes SoundType { get; private set; } = AudioTypes.Footstep;
        [field: SerializeField] public AudioClip Sound { get; private set; } = null;

        public AudioClipMetadata(AudioTypes soundType, AudioClip clip)
        {
            SoundType = soundType;
            Sound = clip;
        }
    }
}