using System;
using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    [CreateAssetMenu(fileName = "FootstepSoundLibrary", menuName = "Data/Audio/Footstep Library", order = 0)]
    public class FootSoundLibrary : ScriptableObject
    {
        [Serializable]
        public struct FootstepData
        {
            public PhysicMaterial material;
            public AudioClip[] clips;
        }

        [Serializable]
        public struct TerrainLayerData
        {
            public AudioClip[] clips;
        }
        
        [SerializeField] private AudioClip _defaultFootstepSound;
        [SerializeField] private PhysicMaterial _terrainMaterial;
        [SerializeField] private TerrainLayerData[] _terrainSounds;
        [SerializeField] private FootstepData[] _footstepData;

        private Dictionary<PhysicMaterial, IFootstepResolver> _footstepResolvers = new();

        public void Init(Terrain terrain)
        {
            IFootstepResolver terrainResolver = new TerrainFootstepResolver(_terrainSounds, terrain);
            _footstepResolvers.Add(_terrainMaterial, terrainResolver);

            for (int i = 0; i < _footstepData.Length; ++i)
            {
                FootstepData footstepData = _footstepData[i];
                IFootstepResolver resolver = new ColliderFootstepResolver(footstepData.clips);
                _footstepResolvers.Add(footstepData.material, resolver);
            }
        }
        
        public AudioClip GetFootstepSound(PhysicMaterial material, Vector3 worldPosition)
        {
            // When footstep sound is needed, physics material serves as key to lookup
            // Corresponding resolver then returns an AudioClip
            // In case of missing physics material in lookup, default sound will be returned
            if (_footstepResolvers.TryGetValue(material, out IFootstepResolver footstepResolver))
            {
                return footstepResolver.Resolve(worldPosition);
            }

            return _defaultFootstepSound;
        }
    }
}