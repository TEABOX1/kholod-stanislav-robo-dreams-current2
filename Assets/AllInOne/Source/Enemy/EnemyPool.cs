using System.Collections.Generic;
using UnityEngine;

namespace AllInOne
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyPrefab;
        [SerializeField] private int _initialSize = 10;

        private Queue<EnemyController> _pool = new Queue<EnemyController>();

        public static EnemyPool Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            FillPool();
        }

        private void FillPool()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                EnemyController enemy = Instantiate(_enemyPrefab, transform);
                enemy.gameObject.SetActive(false);
                _pool.Enqueue(enemy);
            }
        }

        public EnemyController Get()
        {
            if (_pool.Count == 0)
                FillPool();

            EnemyController enemy = _pool.Dequeue();
            enemy.gameObject.SetActive(true);
            return enemy;
        }

        public void ReturnToPool(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
            _pool.Enqueue(enemy);
        }
    }
}