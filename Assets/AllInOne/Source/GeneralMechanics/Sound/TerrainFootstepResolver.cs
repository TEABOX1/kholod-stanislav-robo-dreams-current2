using UnityEngine;

namespace AllInOne
{
    public class TerrainFootstepResolver : IFootstepResolver
    {
        private readonly AudioClip[][] _audioClips;
        private readonly Color[][] _alphaMaps;
        private readonly Vector3 _position;
        private readonly Vector2 _size;
        private readonly int _resolution;
        private readonly Vector2 _leftSouthCorner;

        public TerrainFootstepResolver(FootSoundLibrary.TerrainLayerData[] layerData, Terrain terrain)
        {
            _audioClips = new AudioClip[layerData.Length][];

            for (int i = 0; i < layerData.Length; ++i)
            {
                _audioClips[i] = layerData[i].clips;
            }

            Texture2D[] alphamapTextures = terrain.terrainData.alphamapTextures;
            _alphaMaps = new Color[alphamapTextures.Length][];
            for (int i = 0; i < alphamapTextures.Length; ++i)
            {
                _alphaMaps[i] = alphamapTextures[i].GetPixels();
            }
            _position = terrain.transform.position;
            _size = new Vector2(terrain.terrainData.size.x, terrain.terrainData.size.z);
            _leftSouthCorner = new Vector2(_position.x, _position.z);
            _resolution = terrain.terrainData.alphamapResolution;
        }

        public AudioClip Resolve(Vector3 position)
        {
            Vector2 normalizedPosition = new Vector2((position.x - _leftSouthCorner.x) / _size.x, (position.z - _leftSouthCorner.y) / _size.y);
            Vector2Int uv = new Vector2Int((int)(normalizedPosition.x * _resolution), (int)(normalizedPosition.y * _resolution));

            int layerIndex = 0;
            float layerWeight = -1;
            for (int i = 0; i < _alphaMaps.Length; ++i)
            {
                Color[] alphaMap = _alphaMaps[i];
                Color weight = alphaMap[uv.y * _resolution + uv.x];
                if (weight.r > layerWeight)
                {
                    layerIndex = i * 4;
                    layerWeight = weight.r;
                }
                if (weight.g > layerWeight)
                {
                    layerIndex = i * 4 + 1;
                    layerWeight = weight.g;
                }
                if (weight.b > layerWeight)
                {
                    layerIndex = i * 4 + 2;
                    layerWeight = weight.b;
                }
                if (weight.a > layerWeight)
                {
                    layerIndex = i * 4 + 3;
                    layerWeight = weight.a;
                }
            }

            if (layerIndex >= _audioClips.Length)
                return _audioClips[0][Random.Range(0, _audioClips[0].Length)];
            return _audioClips[layerIndex][Random.Range(0, _audioClips[layerIndex].Length)];
        }
    }
}