using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;
    private Vector3 _currentRotation;
    private Vector3 _targetRotation;


    [SerializeField] private float _snappiness;
    [SerializeField] private float _returnSpeed;

    private float _xRotation;
    private float _yRotation;

    public Transform orientation;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        var mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _targetRotation = Vector3.Lerp(_targetRotation, new Vector3(_xRotation,_yRotation,0), _returnSpeed * Time.deltaTime);
        _currentRotation = Vector3.Slerp(_currentRotation, _targetRotation, _snappiness * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(_currentRotation);
    }
    public void RecoilFire(bool _isAiming,float _recoilX, float _recoilY, float _recoilZ, float _aimRecoilX, float _aimRecoilY, float _aimRecoilZ)
    {
        if (_isAiming)
            _targetRotation += new Vector3(_aimRecoilX, Random.Range(-_aimRecoilY, _aimRecoilY), Random.Range(-_aimRecoilZ, _aimRecoilZ));
        else
            _targetRotation += new Vector3(_recoilX, Random.Range(-_recoilY, _recoilY), Random.Range(-_recoilZ, _recoilZ));
    }
}