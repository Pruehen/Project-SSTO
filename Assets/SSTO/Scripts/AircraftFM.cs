using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������� �װ����� FM �����͸� �޾Ƽ� ���� ���� ������ ����
public class AircraftFM : MonoBehaviour
{
    AircraftData aircraftData;
    Rigidbody rb;    

    public Vector3 Velocity
    {
        get
        {
            return rb.velocity;
        }
    }
    public void Init(AircraftData aircraftData)
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.velocity = this.transform.forward * 200;

        this.aircraftData = aircraftData;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity = rb.velocity;
        float velocitySpeed = velocity.magnitude;
        //Debug.Log(velocity);

        //���� �߷� ����
        rb.AddForce(this.transform.forward * aircraftData.EnginePower(velocitySpeed, this.transform.position.y), ForceMode.Acceleration);
        //��ġ ��ũ ����
        rb.AddTorque(this.transform.right * -aircraftData.PitchTorque(velocitySpeed), ForceMode.Acceleration);
        //�� ��ũ ����
        rb.AddTorque(this.transform.forward * -aircraftData.RollTorque(velocitySpeed), ForceMode.Acceleration);
        //�� ��ũ ����
        rb.AddTorque(this.transform.up * aircraftData.YawTorque(velocitySpeed), ForceMode.Acceleration);
        //���� ��ũ ����
        Vector3 stallAxis = Vector3.Cross(this.transform.forward, new Vector3(0, -1, 0));
        rb.AddTorque(stallAxis * aircraftData.StallTorque(velocitySpeed), ForceMode.Acceleration);

        Vector3 localVelocityDir = this.transform.InverseTransformDirection(rb.velocity).normalized;
        //���� ������
        float aoa = -Mathf.Asin(localVelocityDir.y) * Mathf.Rad2Deg;
        //���� ������
        float aoaSide = -Mathf.Asin(localVelocityDir.x) * Mathf.Rad2Deg;

        //�ӵ� ������ ���� �������� ��� ����
        rb.AddForce(Vector3.Cross(velocity, this.transform.right).normalized * aircraftData.GetLiftPower(aoa, velocitySpeed), ForceMode.Acceleration);
        rb.AddForce(this.transform.right * velocitySpeed * velocitySpeed * aoaSide * 0.0001f, ForceMode.Acceleration);
        //�ӵ� ������ �ݴ� �������� �����׷� �� �����׷� ����
        rb.AddForce(-velocity.normalized * (aircraftData.GetInducedDrag(aoa, velocitySpeed) + aircraftData.GetParasiteDrag(aoa, velocitySpeed)), ForceMode.Acceleration);

        //�׷� ����
        rb.drag = Atmosphere.Drag(this.transform.position.y, aircraftData.GetDC(), velocitySpeed);
        //Debug.Log(velocitySpeed);

        //effect?.SetEffect(velocitySpeed, aoa);
    }
}
