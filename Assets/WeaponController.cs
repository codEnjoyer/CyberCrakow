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

    //bools 
    private bool _shooting, _readyToShoot, _reloading, _aiming;

    //References
    [SerializeField] private Camera _fpsCam;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private Transform _sightTarget;
    [SerializeField] private GameObject _weaponSway;

    [SerializeField] private float _sightOffset;
    [SerializeField] private float _sightSpeed;
    [SerializeField] private Vector3 _weaponSwayPosition;
    private Vector3 _weaponSwayPositionVelocity;

    //Graphics
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private TextMeshProUGUI _ammunitionDisplay;

    [SerializeField] private bool _allowInvoke = true;

    private void Awake()
    {
        //make sure magazine is full
        _bulletsLeft = _magazineSize;
        _readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();

        if (_ammunitionDisplay != null)
        {
            _ammunitionDisplay.SetText(_bulletsLeft / _bulletsPerTap + "/" + _magazineSize / _bulletsPerTap);
        }
    }
    private void MyInput()
    {
        //check if you allowed to hold down fire button
        if (!_isGrenade)
        {
            if (_allowButtonHold)
            {
                _shooting = Input.GetKey(KeyCode.Mouse0);
            }
            else
            {
                _shooting = Input.GetKeyDown(KeyCode.Mouse0);
            }
        }
        else
        {
            if (_allowButtonHold)
            {
                _shooting = Input.GetKey(KeyCode.G);
            }
            else
            {
                _shooting = Input.GetKeyDown(KeyCode.G);
            }
        }

        //aiming
        Aiming();
        //shooting
        if (_readyToShoot && _shooting && !_reloading && _bulletsLeft > 0)
        {
            //set bullets shot to 0
            _bulletsShot = 0;
            //shoot main
            Shoot();
        }

        if (!_isGrenade)
        {
            if (Input.GetKeyDown(KeyCode.R) && _bulletsLeft < _magazineSize && !_reloading)
                Reload();
            if (_readyToShoot && _shooting && !_reloading && _bulletsLeft <= 0)
                Reload();
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
        _reloading = true;
        Invoke("ReloadFinished", _reloadTime);
    }
    private void ReloadFinished()
    {
        _bulletsLeft = _magazineSize;
        _reloading = false;
    }

    private void Aiming()
    {
        _aiming = Input.GetKey(KeyCode.Mouse1);
        var targetPosition = transform.position;
        if(_aiming)
        {
            targetPosition = _fpsCam.transform.position + (transform.position - _sightTarget.transform.position) + (_fpsCam.transform.forward * _sightOffset);
        }
        _weaponSwayPosition = _weaponSway.transform.position;
        _weaponSwayPosition = Vector3.SmoothDamp(_weaponSwayPosition, targetPosition, ref _weaponSwayPositionVelocity, _sightSpeed);
        _weaponSway.transform.position = _weaponSwayPosition;
    }
}