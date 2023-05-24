using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NPCAI
{
    public class NPCWeaponController : MonoBehaviour
    {
        [SerializeField] private GameObject _bullet;

        //if grenade
        [SerializeField] private bool _isGrenade;
        //bullet force
        [SerializeField] private float _shootForce, _upwardForce;

        //Gun stats
        [SerializeField] private float _timeBetweenShooting, _spread, _reloadTime, _timeBetweenShots;
        [SerializeField] private int _magazineSize, _bulletsPerTap;
        [SerializeField] private bool _allowButtonHold;
        private int _bulletsLeft, _bulletsShot;
        [SerializeField] private float _zoomRatio;

        //bools 
        private bool _shooting, _readyToShoot, _reloading, _aiming, _dropGrenade;

        //References
        [SerializeField] private GameObject _fpsCam;
        [SerializeField] private Transform _attackPoint;
        [SerializeField] private GameObject _weaponSway;
        [SerializeField] private Transform playerPosition;

        [SerializeField] private float _sightOffset;
        [SerializeField] private float _sightTime;
        [SerializeField] private Vector3 _weaponSwayPosition;

        private Recoil _recoilScript;

        //Hidden stats;
        private Vector3 _weaponSwayPositionVelocity;

        //Graphics
        [SerializeField] private GameObject _muzzleFlash;
        [SerializeField] private TextMeshProUGUI _ammunitionDisplay;

        [SerializeField] private bool _allowInvoke = true;

        private WeaponInput _input;

        //public Vector3 directionToPlayer;
        private bool inSight;
        private void Awake()
        {
            _input = new WeaponInput();
            //make sure magazine is full
            _bulletsLeft = _magazineSize;
            _readyToShoot = true;
            _recoilScript = GameObject.Find("Camera Holder/RecoilCam").GetComponent<Recoil>();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        // Update is called once per frame
        void Update()
        {
            //MyInput();
            _shooting = inSight;
            if (!_isGrenade)
            {
                if (_shooting)
                {
                    if (_readyToShoot && !_reloading && _bulletsLeft > 0)
                    {
                        //set bullets shot to 0
                        _bulletsShot = 0;
                        //shoot main
                        
                    }
                }
            }
            else if (_dropGrenade)
            {
                if (_readyToShoot && !_reloading && _bulletsLeft > 0)
                {
                    //set bullets shot to 0
                    _bulletsShot = 0;
                    //shoot main
                    
                }
            }
            if (_bulletsLeft <= 0 && !_reloading && !_isGrenade)
            {
                Reload();
            }
            if (_ammunitionDisplay != null)
            {
                _ammunitionDisplay.SetText(_bulletsLeft / _bulletsPerTap + "/" + _magazineSize / _bulletsPerTap);
            }
        }

        public void Shoot(Vector3 directionToPlayer)
        {
            if (_readyToShoot && !_reloading && _bulletsLeft > 0)
            {
                _readyToShoot = false;

                //find the exact hit positon using a raycast
                //Ray ray = _fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

                RaycastHit hit;

                // check if ray is hits smth
                Vector3 targetPoint;
                if (Physics.Raycast(_fpsCam.transform.position, directionToPlayer.normalized, out hit))
                    targetPoint = hit.point;
                else
                    targetPoint = hit.point;
                Debug.DrawRay(transform.position, directionToPlayer);
                //calculate direction from attackPoint to targetPoint
                Vector3 directionWithoutSpread = targetPoint - _attackPoint.position;

                //calculate spread
                float x = Random.Range(-_spread, +_spread);
                float y = Random.Range(-_spread, +_spread);

                //calculate direction or no
                Vector3 directionWithSpread;
                if (_aiming)
                {
                    directionWithSpread = directionWithoutSpread;
                }
                else
                {
                    directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
                }

                // Instantiate bullet
                GameObject currentBullet = Instantiate(_bullet, _attackPoint.position, Quaternion.identity);
                //Rotate bullet to shoot direction
                currentBullet.transform.forward = directionWithoutSpread.normalized;

                //add forces to bullet
                currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * _shootForce, ForceMode.Impulse);

                //Instantiate muzzleFlash
                if (_muzzleFlash != null)
                {
                    Instantiate(_muzzleFlash, _attackPoint.position, Quaternion.identity);
                }
                _recoilScript.RecoilFire();
                _bulletsLeft--;
                _bulletsShot++;

                //Invoke resetShot function
                if (_allowInvoke)
                {
                    Invoke("ResetShot", _timeBetweenShooting);
                    _allowInvoke = false;
                }
                if (_bulletsShot < _bulletsPerTap && _bulletsLeft > 0)
                {
                    Invoke("Shoot", _timeBetweenShots);
                }
            }
        }
        private void ResetShot()
        {
            _readyToShoot = true;
            _allowInvoke = true;
        }

        private void Reload()
        {
            if (!_reloading && _bulletsLeft < _magazineSize)
            {
                _reloading = true;
                Invoke("ReloadFinished", _reloadTime);
            }
        }
        private void ReloadFinished()
        {
            _bulletsLeft = _magazineSize;
            _reloading = false;
        }

    }
}
