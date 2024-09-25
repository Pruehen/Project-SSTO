using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

//�ش� �װ����� FM ������
public class AircraftData : MonoBehaviour
{
    [SerializeField] float enginePower;//���� �߷�(���ӵ�)
    [SerializeField] float pitchTorque;
    [SerializeField] float rollTorque;
    [SerializeField] float yawTorque;

    [SerializeField] AnimationCurve enginePowerCurve; //���� �߷� Ŀ��
    [SerializeField] AnimationCurve torqueCurve; //��ũ Ŀ��
    [SerializeField] AnimationCurve clCurve;//�������� ���� ��� ��� Ŀ��
    [SerializeField] AnimationCurve cdCurve;//�������� ���� �����׷� ��� Ŀ��
    [SerializeField][Range(0, 1)] float e;//����ȿ����� (0~1������ ���� ����)
    [SerializeField] float liftPower;//����͸���.

    [SerializeField] float dragCoefficient;//�׷� ���

    AircraftControl aircraftControl;
    private void Awake()
    {
        aircraftControl = this.gameObject.GetComponent<AircraftControl>();
    }
    /// <summary>
    /// ���� �ӵ��� ���� ���� �߷��� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public float EnginePower(float speed, float altitude)
    {
        float axis = aircraftControl.throttle;
        if (axis >= 0)
        {
            return enginePower * enginePowerCurve.Evaluate(speed) * axis * Atmosphere.AtmosphericPressure(altitude * 0.5f);
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// ���� �ӵ��� ���� ��ġ �� ��ũ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public float PitchTorque(float speed)
    {
        float pitch;
        if(aircraftControl.pitch > 0)
        {
            pitch = aircraftControl.pitch;
        }
        else
        {
            pitch = aircraftControl.pitch * 0.5f;
        }
        return pitchTorque * torqueCurve.Evaluate(speed) * pitch * Atmosphere.AtmosphericPressure(this.transform.position.y * 0.5f);
    }
    /// <summary>
    /// �װ��Ⱑ ������ �� ���� ��ũ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public float StallTorque(float speed)
    {
        if (speed > 300)
            return 0;

        float value = torqueCurve.Evaluate(speed);
        if (value > 0.45f)
        {
            return 0;
        }
        else
        {
            return (0.45f - value) * 20;
        }    
    }
    /// <summary>
    /// ���� �ӵ��� ���� �� �� ��ũ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public float RollTorque(float speed)
    {
        return rollTorque * torqueCurve.Evaluate(speed) * aircraftControl.roll * Atmosphere.AtmosphericPressure(this.transform.position.y * 0.5f);
    }
    /// <summary>
    /// ���� �ӵ��� ���� �� �� ��ũ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    public float YawTorque(float speed)
    {
        return yawTorque * torqueCurve.Evaluate(speed) * aircraftControl.yaw * Atmosphere.AtmosphericPressure(this.transform.position.y * 0.5f);
    }
    /// <summary>
    /// �׷� ����� ��ȯ�ϴ� �޼���. ����극��ũ ��� �߰���
    /// </summary>
    /// <returns></returns>
    public float GetDC()
    {
        float axis = aircraftControl.throttle;
        if(axis >= 0)
        {
            return dragCoefficient;
        }
        else
        {
            return (1 + (-axis * 5)) * dragCoefficient;
        }
    }
    /// <summary>
    /// �������� ���� ����� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="aoa"></param>
    /// <returns></returns>
    public float GetLiftPower(float aoa, float speed)
    {        
        return clCurve.Evaluate(aoa) * liftPower * e * Atmosphere.AtmosphericPressure(transform.position.y) * speed * speed * 0.0001f;
    }
    /// <summary>
    /// �������� �ӵ��� ���� ���� �׷��� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="aoa"></param>
    /// <returns></returns>
    public float GetInducedDrag(float aoa, float speed)
    {
        return Mathf.Abs(clCurve.Evaluate(aoa) * liftPower * (1 - e) * Atmosphere.AtmosphericPressure(transform.position.y) * speed * speed * 0.00005f);
    }
    /// <summary>
    /// �������� �ӵ��� ���� ���� �׷��� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns></returns>
    public float GetParasiteDrag(float aoa, float speed)
    {
        return cdCurve.Evaluate(aoa) * liftPower * Atmosphere.AtmosphericPressure(transform.position.y) * speed * speed * 0.00005f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
