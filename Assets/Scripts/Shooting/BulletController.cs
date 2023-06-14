using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //assignables
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private LayerMask _whatIsEnemies;

    //Stats
    [Range(0f,1f)]
    [SerializeField] private float _bounciness;

    //Damage
    [SerializeField] private int _explosionDamage;
    [SerializeField] private float _explosionRange;

    //lifetime
    [SerializeField] private int _MaxCollisions;
    [SerializeField] private float _maxLifeTime;
    [SerializeField] private bool _explodeOnTouch;

    //Sounds
    [SerializeField] private AudioSource _explosionSound;

    //References
    [SerializeField] private GameObject _bulletHole;
    [SerializeField] private GameObject _gun;
    GameObject ex;

    private int _collisions;
    private Vector3 _lastCollision;
    private bool _exploaded = false;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        if (!_exploaded)
        {
            //When to explode:
            if (_collisions > _MaxCollisions)
            {
                Explode();
            }

            //Count down lifetime
            _maxLifeTime -= Time.deltaTime;
            if (_maxLifeTime < 0f)
            {
                Explode();
            }
        }
    }

    private void Explode()
    {
        _exploaded = true;
        //Instantiate explosion
        if(_explosion!=null)
        {
            ex = Instantiate(_explosion, transform.position, Quaternion.identity);
        }
        if(_bulletHole!=null)
        {
            RaycastHit hit;
            Physics.Raycast(_lastCollision, _gun.transform.position, out hit);
            GameObject bulletHole = Instantiate(_bulletHole, _lastCollision, Quaternion.LookRotation(hit.normal));
            //Debug.Log("Hit" + hit.normal);
            //Debug.Log("Rotation" + Quaternion.LookRotation(hit.normal));
            bulletHole.transform.position += bulletHole.transform.forward / 1000;
            bulletHole.transform.SetParent(hit.transform);
            Destroy(bulletHole, 10f);
        }
        Collider[] enemies = Physics.OverlapSphere(transform.position, _explosionRange, _whatIsEnemies);
        for(int i=0; i<enemies.Length; i++)
        {

        }
        if (_explosionSound != null)
        {
            _explosionSound.Play();
            Invoke("Delay", _explosionSound.clip.length);
            Invoke("DestroyExplosion", 1f);
        }
        else
        {
            //add Delay, to debug
            Invoke("Delay", 0.05f);
        }
    }

    private void Delay()
    {
        Debug.Log("Delay");
        Destroy(gameObject);
    }
    private void DestroyExplosion()
    {
        Destroy(ex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _lastCollision = transform.position;
        //don't compare with other bullets
        if(collision.collider.CompareTag("Bullet"))
        {
            return;
        }

        //count up collisions
        _collisions++;

        //Explode if bullet has an enemy Directly and explodeOnTouch is activated
        if(collision.collider.CompareTag("Enemy") && _explodeOnTouch)
        {
            Explode();
        }
    }

    private void Setup()
    {
        //Assign material to coolider
        GetComponent<SphereCollider>().material.bounciness = _bounciness;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRange);
    }
}