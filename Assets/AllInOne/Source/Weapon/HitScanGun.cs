using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

namespace AllInOne
{
    public class HitScanGun : MonoBehaviour
    {
        public event Action<Collider, float> OnHit;
        public event Action OnShot;
        
        [SerializeField] protected GunAimer _aimer;
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

        protected InputController _inputController;

        private float _time;
        private bool _isShootPrepearing = false;

        private float _distanceToTarget;

        public float HoldTime => _time;

        protected virtual void Start()
        {
            _tilingId = Shader.PropertyToID(_tilingName);
            _time = 1f;
        }

        protected virtual void FixedUpdate()
        {
            if (_isShootPrepearing && _time <= 5)
                _time += Time.deltaTime * 2;
        }

        protected void OnEnable()
        {
            if (_inputController == null)
                _inputController = ServiceLocator.Instance.GetService<InputController>();
            _inputController.OnPrimaryInput += PrimaryInputHandler;
        }

        protected void OnDisable()
        {
            _inputController.OnPrimaryInput -= PrimaryInputHandler;
        }

        protected virtual void PrimaryInputHandler(bool isHold)
        {
            _isShootPrepearing = isHold;
            if (isHold)
                return;
            Vector3 muzzlePosition = _muzzleTransform.position;
            Vector3 muzzleForward = _muzzleTransform.forward;
            Ray ray = new Ray(muzzlePosition, muzzleForward);
            Vector3 hitPoint = muzzlePosition + muzzleForward * _range;
            if (Physics.SphereCast(ray, _shotRadius, out RaycastHit hitInfo, _range, _layerMask))
            {
                Vector3 directVector = hitInfo.point - _muzzleTransform.position;
                Vector3 rayVector = Vector3.Project(directVector, ray.direction);
                hitPoint = muzzlePosition + rayVector;
                
                OnHit?.Invoke(hitInfo.collider, _time);
            }

            ParticleSystem shot = Instantiate(_shotPrefab, _muzzleTransform.position, Quaternion.LookRotation(hitPoint - _muzzleTransform.position));
            if (shot != null)
                shot.Play();
            _distanceToTarget = (hitPoint - _muzzleTransform.position).magnitude;
            //shot.distance = (hitPoint - _muzzleTransform.position).magnitude;
            //shot.outerPropertyBlock = new MaterialPropertyBlock();
            StartCoroutine(ShotRoutine(shot, hitPoint));
            
            OnShot?.Invoke();
            _time = 1f;
        }

        protected IEnumerator ShotRoutine(ParticleSystem shot, Vector3 targetPosition)
        {
            float interval = _decaySpeed * Time.deltaTime;

            Vector3 scale = shot.transform.localScale;
            scale.x *= _time;
            scale.y *= _time;

            //shot.transform.localScale = scale;
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