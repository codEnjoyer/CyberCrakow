using UnityEngine;
using TMPro;

public class WeaponController : MonoBehaviour
{

    //bullet
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

    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoilZ;

    [SerializeField] private float _aimRecoilX;
    [SerializeField] private float _aimRecoilY;
    [SerializeField] private float _aimRecoilZ;

    //bools 
    private bool _shooting, _readyToShoot, _reloading, _aiming, _dropGrenade;

    //References
    [SerializeField] private Camera _fpsCam;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _sightTarget;
    [SerializeField] private GameObject _weaponSway;
    [SerializeField] private Transform _recoilCam;

    [SerializeField] private float _sightOffset;
    [SerializeField] private float _sightTime;
    [SerializeField] private Vector3 _weaponSwayPosition;

    private CameraMovement _recoilScript;

    //Hidden stats;
    private Vector3 _weaponSwayPositionVelocity;

    //Graphics
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private TextMeshProUGUI _ammunitionDisplay;

    [SerializeField] private bool _allowInvoke = true;

    private WeaponInput _input;
    private void Awake()
    {
        _input = new WeaponInput();
        //make sure magazine is full
        _bulletsLeft = _magazineSize;
        _readyToShoot = true;
        _recoilScript = GameObject.Find("Camera Holder/Main Camera").GetComponent<CameraMovement>();
        _input.Weapon.Reload.performed += context => Reload();
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
        _shooting = _input.Weapon.Shoot.inProgress;
        _dropGrenade = _input.Weapon.Grenade.IsPressed();
        Aiming();
        if (!_isGrenade)
        { 
            if (_shooting)
            {
                if (_readyToShoot && !_reloading && _bulletsLeft > 0)
                {
                    //set bullets shot to 0
                    _bulletsShot = 0;
                    //shoot main
                    Shoot();
                }
            }
        }   
        else if(_dropGrenade)
        {
            if (_readyToShoot && !_reloading && _bulletsLeft > 0)
            {
                   //set bullets shot to 0
                   _bulletsShot = 0;
                   //shoot main
                   Shoot();
            }
        }
        if (_bulletsLeft <= 0 && !_reloading && !_isGrenade)
        {
            Reload();
        }
        if (_ammunitionDisplay != null)
        {
            _ammunitionDisplay.text = ( _bulletsLeft / _bulletsPerTap + "/" + _magazineSize / _bulletsPerTap);
        }
    }

    private void Shoot()
    {
        _readyToShoot = false;

        //find the exact hit positon using a raycast
        Ray ray = _fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // check if ray is hits smth
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(75);

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
        if (!_isGrenade)
        {
            _recoilScript.RecoilFire(_aiming, _recoilX, _recoilY, _recoilZ, _aimRecoilX, _aimRecoilY, _aimRecoilZ);
        }
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
    private void ResetShot()
    {
        _readyToShoot = true;
        _allowInvoke = true;
    }

    private void Reload()
    {
        if (!_reloading && _bulletsLeft< _magazineSize) 
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

    private void Aiming()
    {
        _aiming = _input.Weapon.Aiming.inProgress;
        var targetPosition = transform.position;
        if(_aiming)
        {
            targetPosition = _fpsCam.transform.position + (transform.position - _sightTarget.transform.position) + (_fpsCam.transform.forward * _sightOffset);
            _fpsCam.fieldOfView = Mathf.MoveTowards(_fpsCam.fieldOfView, 60f / _zoomRatio, _sightTime * 7 * Time.deltaTime);
        }
        else
        {
            _fpsCam.fieldOfView = Mathf.MoveTowards(_fpsCam.fieldOfView, 60f, _sightTime * 7 * Time.deltaTime);
        }
        _weaponSwayPosition = _weaponSway.transform.position;
        _weaponSwayPosition = Vector3.SmoothDamp(_weaponSwayPosition, targetPosition, ref _weaponSwayPositionVelocity, _sightTime);
        _weaponSway.transform.position = _weaponSwayPosition;
    }
}