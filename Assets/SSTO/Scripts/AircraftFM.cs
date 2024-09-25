using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용중인 항공기의 FM 데이터를 받아서 실제 비행 물리를 적용
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

        //엔진 추력 적용
        rb.AddForce(this.transform.forward * aircraftData.EnginePower(velocitySpeed, this.transform.position.y), ForceMode.Acceleration);
        //피치 토크 적용
        rb.AddTorque(this.transform.right * -aircraftData.PitchTorque(velocitySpeed), ForceMode.Acceleration);
        //롤 토크 적용
        rb.AddTorque(this.transform.forward * -aircraftData.RollTorque(velocitySpeed), ForceMode.Acceleration);
        //요 토크 적용
        rb.AddTorque(this.transform.up * aircraftData.YawTorque(velocitySpeed), ForceMode.Acceleration);
        //스톨 토크 적용
        Vector3 stallAxis = Vector3.Cross(this.transform.forward, new Vector3(0, -1, 0));
        rb.AddTorque(stallAxis * aircraftData.StallTorque(velocitySpeed), ForceMode.Acceleration);

        Vector3 localVelocityDir = this.transform.InverseTransformDirection(rb.velocity).normalized;
        //정면 받음각
        float aoa = -Mathf.Asin(localVelocityDir.y) * Mathf.Rad2Deg;
        //측면 받음각
        float aoaSide = -Mathf.Asin(localVelocityDir.x) * Mathf.Rad2Deg;

        //속도 벡터의 수직 방향으로 양력 생성
        rb.AddForce(Vector3.Cross(velocity, this.transform.right).normalized * aircraftData.GetLiftPower(aoa, velocitySpeed), ForceMode.Acceleration);
        rb.AddForce(this.transform.right * velocitySpeed * velocitySpeed * aoaSide * 0.0001f, ForceMode.Acceleration);
        //속도 벡터의 반대 방향으로 유도항력 및 유해항력 생성
        rb.AddForce(-velocity.normalized * (aircraftData.GetInducedDrag(aoa, velocitySpeed) + aircraftData.GetParasiteDrag(aoa, velocitySpeed)), ForceMode.Acceleration);

        //항력 적용
        rb.drag = Atmosphere.Drag(this.transform.position.y, aircraftData.GetDC(), velocitySpeed);
        //Debug.Log(velocitySpeed);

        //effect?.SetEffect(velocitySpeed, aoa);
    }
}
