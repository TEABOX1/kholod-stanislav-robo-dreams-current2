using UnityEngine;

namespace AllInOne
{
    public interface ICameraService : IService
    {
        Camera Camera { get; }
    }
}