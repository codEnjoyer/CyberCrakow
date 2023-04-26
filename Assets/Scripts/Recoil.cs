using UnityEngine;

public class Recoil : MonoBehaviour
{

    private Vector3 _currentRotation;
    private Vector3 _targetRotation;

    [SerializeField] private float _recoilX;
    [SerializeField] private float _recoilY;
    [SerializeField] private float _recoilZ;
    [SerializeField] private float _snappiness;
    [SerializeField] private float _returnSpeed;

    void Update()
    {
        _targetRotation = Vector3.Lerp(_targetRotation, Vector3.zero, _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }

    public void RecoilFire()
    {
        _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}
