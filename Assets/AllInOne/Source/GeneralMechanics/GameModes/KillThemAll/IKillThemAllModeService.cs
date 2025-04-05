using UnityEngine;

namespace AllInOne
{
    public interface IKillThemAllModeService : IService
    {
        Vector3 MapCentre { get; }
    }
}