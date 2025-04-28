using System;
using System.Collections;
using UnityEngine;

namespace AllInOne
{
    public class EnemyHitScanGun : MonoBehaviour
    {
        public event Action<Collider> OnHit;
        public event Action<Collider> OnMeeleHit;
        public event Action<RaycastHit> OnHitPrecise;
        public event Action OnShot;

        [SerializeField] protected ParticleSystem _shotPrefab;
        [SerializeField] protected Transform _muzzleTransform;
        [SerializeField] protected float _decaySpeed;
        [SerializeField] protected Vector3 _shotScale;
        [SerializeField] protected float _shotRadius;
        [SerializeField] protected float _shotVisualDiameter;
        [SerializeField] protected string _tilingName;
        [SerializeField] protected float _range;
        [SerializeField] protected LayerMask _layerMask;

        protected int _tilingId;

        private float _distanceToTarget;

        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
        }

        public virtual void Hit(Collider enemy)
        {
            OnMeeleHit?.Invoke(enemy);
            //StartCoroutine(AttackAnimation());
        }

        //private IEnumerator AttackAnimation()
        //{
        //Quaternion startRotation = transform.localRotation;
        //Quaternion attackRotation = startRotation * Quaternion.Euler(-90, 0, 0);
        //float attackSpeed = 5f;
        //float t = 0f;

        //while (t < 1f)
        //{
        //    t += Time.deltaTime * attackSpeed;
        //    transform.localRotation = Quaternion.Lerp(startRotation, attackRotation, t);
        //    yield return null;
        //}

        //yield return new WaitForSeconds(0.1f);

        //t = 0f;
        //while (t < 1f)
        //{
        //    t += Time.deltaTime * attackSpeed;
        //    transform.localRotation = Quaternion.Lerp(attackRotation, startRotation, t);
        //    yield return null;
        //}
        //}

        protected void InvokeHitPrecise(RaycastHit hit)
        {
            OnHitPrecise?.Invoke(hit);
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
                InvokeHitPrecise(hitInfo);
            }

            ParticleSystem shot = Instantiate(_shotPrefab, _muzzleTransform.position, Quaternion.LookRotation(hitPoint - _muzzleTransform.position));
            if (shot != null)
                shot.Play();
            _distanceToTarget = (hitPoint - _muzzleTransform.position).magnitude;
            //shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
            //shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot, hitPoint));
            OnShot?.Invoke();
            
        }

        protected IEnumerator ShotRoutine(ParticleSystem shot, Vector3 targetPosition)
        {
            float interval = _decaySpeed * Time.deltaTime;
            while (_distanceToTarget >= interval)
            {
                EvaluateShot(shot, targetPosition);
                yield return null;
                _distanceToTarget -= interval;
                interval = _decaySpeed * Time.deltaTime;
            }

            Destroy(shot.gameObject);
        }

        protected void EvaluateShot(ParticleSystem shot, Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - shot.transform.position).normalized;
            shot.transform.position += direction * _decaySpeed * Time.deltaTime;
            //shot.Outer.SetPropertyBlock(shot.outerPropertyBlock);
        }
    }
}