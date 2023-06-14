using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    //bullet
    [SerializeField] private GameObject _bullet;

    //bullet force
    [SerializeField] private float _shootForce;

    //Gun stats
    [SerializeField] private float _timeBetweenShooting, _spread;
    [SerializeField] private int _magazineSize;
    [SerializeField] private bool _allowButtonHold;
    private int _bulletsLeft;

    //bools 
    private bool _readyToShoot, _dropGrenade;

    //References
    [SerializeField] private Camera _fpsCam;
    [SerializeField] private Transform _attackPoint;

    //Graphics
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private TextMeshProUGUI _ammunitionDisplay;

    //Sounds
    [SerializeField] private AudioSource _shootingSound;

    [SerializeField] private bool _allowInvoke = true;

    private PlayerInput _input;
    private void Awake()
    {
        _input = new PlayerInput();
        //make sure magazine is full
        _bulletsLeft = _magazineSize;
        _readyToShoot = true;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    void Update()
    {
        _dropGrenade = _input.Player.Grenade.IsPressed();
        if (_dropGrenade)
        {
            if (_readyToShoot && _bulletsLeft > 0)
            {
                //shoot main
                Shoot();
            }
        }
        if (_ammunitionDisplay != null)
        {
            _ammunitionDisplay.text = _bulletsLeft + "/" + _magazineSize;
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

        directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet
        GameObject currentBullet = Instantiate(_bullet, _attackPoint.position, Quaternion.identity);
        //Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithoutSpread.normalized;

        //add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * _shootForce, ForceMode.Impulse);
        _bulletsLeft--;

        //Invoke resetShot function
        if (_allowInvoke)
        {
            Invoke("ResetShot", _timeBetweenShooting);
            _allowInvoke = false;
        }
    }
    private void ResetShot()
    {
        _readyToShoot = true;
        _allowInvoke = true;
    }
}
