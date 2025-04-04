using System;
using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class EnemyHitScanGun : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        public event Action<Collider> OnMeeleHit;
        public event Action OnShot;

        [SerializeField] protected HitscanShotAspect _shotPrefab;
        [SerializeField] protected Transform _muzzleTransform;
        [SerializeField] protected float _decaySpeed;
        [SerializeField] protected Vector3 _shotScale;
        [SerializeField] protected float _shotRadius;
        [SerializeField] protected float _shotVisualDiameter;
        [SerializeField] protected string _tilingName;
        [SerializeField] protected float _range;
        [SerializeField] protected LayerMask _layerMask;

        protected int _tilingId;

        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }

        public virtual void Hit(Collider enemy)
        {
            OnMeeleHit?.Invoke(enemy);
            StartCoroutine(AttackAnimation());
        }

        private IEnumerator AttackAnimation()
        {
            Quaternion startRotation = transform.localRotation;
            Quaternion attackRotation = startRotation * Quaternion.Euler(-90, 0, 0);
            float attackSpeed = 5f;
            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime * attackSpeed;
                transform.localRotation = Quaternion.Lerp(startRotation, attackRotation, t);
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);

            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * attackSpeed;
                transform.localRotation = Quaternion.Lerp(attackRotation, startRotation, t);
                yield return null;
            }
        }

        public virtual void Shoot()
        {
            Vector3 muzzlePosition = _muzzleTransform.position;
            Vector3 muzzleForward = _muzzleTransform.forward;
            Ray ray = new Ray(muzzlePosition, muzzleForward);
            Vector3 hitPoint = muzzlePosition + muzzleForward * _range;

            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
            {
                Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                hitPoint = muzzlePosition + rayVector;

                OnHit?.Invoke(hitInfo.collider);
            }

             HitscanShotAspect shot = Instantiate(_shotPrefab, hitPoint, _muzzleTransform.rotation);
             shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
             shot.outerPropertyBlock = new MaterialPropertyBlock();
             StartCoroutine(ShotRoutine(shot));
             OnShot?.Invoke();
            
        }

        protected IEnumerator ShotRoutine(HitscanShotAspect shot)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (shot.distance >= interval)
            {
                EvaluateShot(shot);
                yield return null;
                shot.distance -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        protected void EvaluateShot(HitscanShotAspect shot)
        {
            shot.Transform.localScale = new Vector3(_shotScale.x, _shotScale.y, shot.distance * 0.5f);
            Vector4 tiling = Vector4.one;
            tiling.y = shot.distance * 0.5f / _shotVisualDiameter;
            shot.outerPropertyBlock.SetVector(_tilingId, tiling);
            shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }
    }
}