using System;
using UnityEngine;

namespace AllInOne
{
    public interface ISceneManager : IService
    {
        event Action<AsyncOperation> onSceneLoad;
        
        void SetScene(Scenes scene);
        void OnSceneLoad(AsyncOperation operation);
    }
}