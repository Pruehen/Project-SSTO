using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����鿡 ���� �����͸� ���� �� �������� ���¸� �˷��ִ� Ŭ����
public class AircraftControl : MonoBehaviour
{
    [SerializeField] Transform _la; //���� ���Ϸ���
    [SerializeField] Transform _ra; //���� ���Ϸ���
    [SerializeField] Transform _le; //���� ����������
    [SerializeField] Transform _re; //���� ����������
    [SerializeField] Transform _lr; //���� ����
    [SerializeField] Transform _rr; //���� ����
    [SerializeField] List<JetEngineController> jetEngineControllers;//����

    [Header("���ʼ� �Ǻ�")]
    [SerializeField] Transform _lc;
    [SerializeField] Transform _rc;
    [SerializeField] Transform _lEngine;
    [SerializeField] Transform _rEngine;

    void JetEngineControl()
    {
        foreach (JetEngineController jet in jetEngineControllers)
        {
            jet.InputValue = Mathf.Clamp(throttle, 0, 1);
        }
    }

    Quaternion _laAxis;
    Quaternion _raAxis;
    Quaternion _leAxis;
    Quaternion _reAxis;
    Quaternion _lrAxis;
    Quaternion _rrAxis;

    Quaternion _lcAxis;
    Quaternion _rcAxis;
    Quaternion _lEngineAxis;
    Quaternion _rEngineAxis;

    [Header("������ ���� ����")]
    [SerializeField] float _a_Range;
    [SerializeField] float _e_Range;
    [SerializeField] float _r_Range;

    [Header("������ ������")]
    [SerializeField] float _controlPower;

    [Header("���� �߷� ������")]
    [SerializeField] float _engineDelay;

    float _pitchTarget;
    float _rollTarget;
    float _yawTarget;
    float _throttleTarget;

    public float pitch { get; private set; }//-1~1
    public float roll { get; private set; }//-1~1
    public float yaw { get; private set; }//-1~1
    public float throttle { get; private set; }//-1~1

    /// <summary>
    /// �װ����� �����鿡 �� ȸ����(float)�� �Է��ϴ� �޼���. �� ���� -1~1�� ������ ������ ��
    /// </summary>
    /// <param name="pitch"></param>
    /// <param name="roll"></param>
    /// <param name="yaw"></param>
    public void SetAxisValue(float pitch, float roll, float yaw, float throttle)
    {
        this._pitchTarget = pitch;
        this._rollTarget = roll;
        this._yawTarget = yaw;
        this._throttleTarget = throttle;
    }
        private void Start()
    {
        _laAxis = _la.localRotation;
        _raAxis = _ra.localRotation;
        _leAxis = _le.localRotation;
        _reAxis = _re.localRotation;
        _lrAxis = _lr.localRotation;
        _rrAxis = _rr.localRotation;

        if (_lc != null && _rc != null)
        {
            _lcAxis = _lc.localRotation;
            _rcAxis = _rc.localRotation;
        }
        if (_lEngine != null && _rEngine != null)
        {
            _lEngineAxis = _lEngine.localRotation;
            _rEngineAxis = _rEngine.localRotation;
        }
    }
    // Update is called once per frame
    void Update()
    {      
        pitch = Mathf.Lerp(pitch, _pitchTarget, Time.deltaTime * _controlPower);
        roll = Mathf.Lerp(roll, _rollTarget, Time.deltaTime * _controlPower);
        yaw = Mathf.Lerp(yaw, _yawTarget, Time.deltaTime * _controlPower);
        throttle = Mathf.Lerp(throttle, _throttleTarget, Time.deltaTime * _engineDelay);

        _la.localRotation = _laAxis * Quaternion.Euler(roll * _a_Range, 0, 0);
        _ra.localRotation = _raAxis * Quaternion.Euler(-roll * _a_Range, 0, 0);

        _le.localRotation = _leAxis * Quaternion.Euler((roll * _e_Range + -pitch * _e_Range) * 0.5f, 0, 0);
        _re.localRotation = _reAxis * Quaternion.Euler((-roll * _e_Range + -pitch * _e_Range) * 0.5f, 0, 0);

        _lr.localRotation = _lrAxis * Quaternion.Euler(0, -yaw * _r_Range, 0);
        _rr.localRotation = _rrAxis * Quaternion.Euler(0, -yaw * _r_Range, 0);

        if (_lc != null && _rc != null)
        {
            _lc.localRotation = _lcAxis * Quaternion.Euler((roll * _e_Range + pitch * _e_Range) * 0.5f, 0, 0);
            _rc.localRotation = _rcAxis * Quaternion.Euler((-roll * _e_Range + pitch * _e_Range) * 0.5f, 0, 0);
        }
        if (_lEngine != null && _rEngine != null)
        {
            _lEngine.localRotation = _lEngineAxis * Quaternion.Euler((roll * _e_Range + -pitch * _e_Range) * 0.3f, -yaw * _r_Range * 0.5f, 0);
            _rEngine.localRotation = _rEngineAxis * Quaternion.Euler((-roll * _e_Range + -pitch * _e_Range) * 0.3f, -yaw * _r_Range * 0.5f, 0);
        }

        JetEngineControl();
    }
}
