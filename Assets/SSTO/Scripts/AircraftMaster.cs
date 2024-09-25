using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ Ŭ����. ���� ������Ʈ�� ������ �� ���. �����ϸ� ���⼭ ���� ������Ʈ�� �������� �� ��
//���� ����� �켱 ���⿡ �ٿ�����.
public class AircraftMaster : MonoBehaviour
{
    public AircraftControl aircraftControl;
    //public VehicleCombat vehicleCombat;

    Rigidbody rigidbody;

    /// <summary>
    /// ���� �װ����� �ӵ�(km/h)�� ��ȯ�ϴ� �޼��� 
    /// </summary>
    /// <returns></returns>
    public float GetSpeed()
    {
        return rigidbody.velocity.magnitude * 3.6f;
    }
    
    //public AircraftControl aircraftControl;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        //aircraftSelecter = GetComponent<AircraftSelecter>();
        //aircraftControl = aircraftSelecter.aircraftControl;
        ////vehicleCombat = GetComponent<VehicleCombat>();

        //if (aiControl)
        //{
        //    GetComponent<FlightController>().enabled = false;            
        //}
        //else
        //{
        //    GetComponent<FlightController_AI>().enabled = false;
        //    GetComponent<WeaponController_AI>().enabled = false;
        //    GetComponent<CustomAI>().enabled = false;
        //}
    }
}
