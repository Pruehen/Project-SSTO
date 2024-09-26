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
    [SerializeField] bool hasControlWing = false;

    [Header("���ʼ� �Ǻ�")]
    [SerializeField] Transform _lc;
    [SerializeField] Transform _rc;
    [SerializeField] Transform _lEngine;
    [SerializeField] Transform _rEngine;

    [Header("���� ���")]
    public float motorTorque = 2000;
    public float brakeTorque = 2000;
    public float maxSpeed = 20;
    public float steeringRange = 30;
    public float steeringRangeAtMaxSpeed = 10;
    public float centreOfGravityOffset = -1f;

    WheelControl[] wheels;
    Rigidbody rigidBody;

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
        if (hasControlWing)
        {
            _laAxis = _la.localRotation;
            _raAxis = _ra.localRotation;
            _leAxis = _le.localRotation;
            _reAxis = _re.localRotation;
            _lrAxis = _lr.localRotation;
            _rrAxis = _rr.localRotation;
        }

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

        rigidBody = GetComponent<Rigidbody>();

        // Adjust center of mass vertically, to help prevent the car from rolling
        rigidBody.centerOfMass += Vector3.up * centreOfGravityOffset;
        wheels = GetComponentsInChildren<WheelControl>();
    }
    // Update is called once per frame
    void Update()
    {
        SetAxisValue(PlayerInputCustom.Instance.pitchAxis, PlayerInputCustom.Instance.rollAxis, PlayerInputCustom.Instance.yawAxis, PlayerInputCustom.Instance.throttleAxis);//�׽�Ʈ �ڵ�

        pitch = Mathf.Lerp(pitch, _pitchTarget, Time.deltaTime * _controlPower);
        roll = Mathf.Lerp(roll, _rollTarget, Time.deltaTime * _controlPower);
        yaw = Mathf.Lerp(yaw, _yawTarget, Time.deltaTime * _controlPower);
        throttle = Mathf.Lerp(throttle, _throttleTarget, Time.deltaTime * _engineDelay);

        if (hasControlWing)
        {
            _la.localRotation = _laAxis * Quaternion.Euler(roll * _a_Range, 0, 0);
            _ra.localRotation = _raAxis * Quaternion.Euler(-roll * _a_Range, 0, 0);

            _le.localRotation = _leAxis * Quaternion.Euler((roll * _e_Range + -pitch * _e_Range) * 0.5f, 0, 0);
            _re.localRotation = _reAxis * Quaternion.Euler((-roll * _e_Range + -pitch * _e_Range) * 0.5f, 0, 0);

            _lr.localRotation = _lrAxis * Quaternion.Euler(0, -yaw * _r_Range, 0);
            _rr.localRotation = _rrAxis * Quaternion.Euler(0, -yaw * _r_Range, 0);
        }

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
        WheelControl();
    }

    void WheelControl()
    {
        float vInput = throttle;
        float hInput = yaw;

        // Calculate current speed in relation to the forward direction of the car
        // (this returns a negative number when traveling backwards)
        float forwardSpeed = Vector3.Dot(transform.forward, rigidBody.velocity);


        // Calculate how close the car is to top speed
        // as a number from zero to one
        float speedFactor = Mathf.InverseLerp(0, maxSpeed, forwardSpeed);

        // Use that to calculate how much torque is available 
        // (zero torque at top speed)
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, speedFactor);

        // ��and to calculate how much to steer 
        // (the car steers more gently at top speed)
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, speedFactor);

        // Check whether the user input is in the same direction 
        // as the car's velocity
        bool isAccelerating = Mathf.Sign(vInput) == Mathf.Sign(forwardSpeed);

        foreach (var wheel in wheels)
        {
            // Apply steering to Wheel colliders that have "Steerable" enabled
            if (wheel.steerable)
            {
                wheel.WheelCollider.steerAngle = hInput * currentSteerRange;
            }

            if (isAccelerating)
            {
                // Apply torque to Wheel colliders that have "Motorized" enabled
                if (wheel.motorized)
                {
                    wheel.WheelCollider.motorTorque = vInput * currentMotorTorque;
                }
                wheel.WheelCollider.brakeTorque = 0;
            }
            else
            {
                // If the user is trying to go in the opposite direction
                // apply brakes to all wheels
                wheel.WheelCollider.brakeTorque = Mathf.Abs(vInput) * brakeTorque;
                wheel.WheelCollider.motorTorque = 0;
            }
        }
    }
}
