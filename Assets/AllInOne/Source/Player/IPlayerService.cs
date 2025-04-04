using UnityEngine;

namespace AllInOne
{
    public interface IPlayerService : IService
    {
        PlayerController Player { get; }
        bool IsPlayer(Collider collider);
    }
}