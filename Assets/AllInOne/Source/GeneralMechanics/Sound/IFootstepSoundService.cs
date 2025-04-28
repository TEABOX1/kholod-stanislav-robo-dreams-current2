using UnityEngine;

namespace AllInOne
{
    public interface IFootstepSoundService : IService
    {
        AudioClip GetFootstepSound(PhysicMaterial physicMaterial, Vector3 worldPosition);
    }
}