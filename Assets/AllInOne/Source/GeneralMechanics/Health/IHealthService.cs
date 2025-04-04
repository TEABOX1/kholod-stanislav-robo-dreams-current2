using UnityEngine;

namespace AllInOne
{
    public interface IHealthService : IService
    {
        IHealth this[Collider collider] { get; }

        void AddCharacter(IHealth character);

        void RemoveCharacter(IHealth character);

        bool GetHealth(Collider characterController, out Health health);
    }
}