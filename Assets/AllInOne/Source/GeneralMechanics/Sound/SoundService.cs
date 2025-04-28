using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AllInOne
{
    public class SoundService : GlobalMonoServiceBase, ISoundService
    {
        [Serializable]
        public struct VolumeData
        {
            public SoundType type;
            public string parameter;
        }
        
        public static float VolumeToDecibels(float volume)
        {
            return volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        }

        public static float DecibelsToVolume(float dB)
        {
            return Mathf.Pow(10f, dB / 20f);
        }

        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private VolumeData[] _volumeData;

        private Dictionary<SoundType, string> _soundTypes = new();
        private ISaveService _saveService;
        
        public override Type Type { get; } = typeof(ISoundService);

        protected override void Awake()
        {
            base.Awake();
            
            for (int i = 0; i < _volumeData.Length; ++i)
                _soundTypes.Add(_volumeData[i].type, _volumeData[i].parameter);
        }

        private void Start()
        {
            _saveService = ServiceLocator.Instance.GetService<ISaveService>();

            SoundSaveData saveData = _saveService.SaveData.soundData;
            
            SetVolume(SoundType.Master, saveData.masterVolume);
            SetVolume(SoundType.Sfx, saveData.sfxVolume);
            SetVolume(SoundType.Ambience, saveData.ambienceVolume);
        }

        public void SetVolume(SoundType type, float volume)
        {
            _audioMixer.SetFloat(_soundTypes[type], VolumeToDecibels(volume));
            switch (type)
            {
                case SoundType.Master:
                    _saveService.SaveData.soundData.masterVolume = volume;
                    break;
                case SoundType.Sfx:
                    _saveService.SaveData.soundData.sfxVolume = volume;
                    break;
                case SoundType.Ambience:
                    _saveService.SaveData.soundData.ambienceVolume = volume;
                    break;
            }
        }

        public float GetVolume(SoundType type)
        {
            float volume = 0;
            _audioMixer.GetFloat(_soundTypes[type], out volume);
            return DecibelsToVolume(volume);
        }
    }
}