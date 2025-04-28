using UnityEngine;

namespace AllInOne
{
    public interface IFootstepResolver
    {
        AudioClip Resolve(Vector3 position);
    }
}