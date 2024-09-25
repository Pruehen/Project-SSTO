using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �����ϴ� �װ��� �����鿡 ���� �����͸� �����ϴ� Ŭ����
public class FlightController : MonoBehaviour
{
    [SerializeField] AircraftControl aircraftControl;

    // Update is called once per frame
    void Update()
    {
        aircraftControl.SetAxisValue(PlayerInputCustom.Instance.pitchAxis, PlayerInputCustom.Instance.rollAxis, PlayerInputCustom.Instance.yawAxis, PlayerInputCustom.Instance.throttleAxis);//�׽�Ʈ �ڵ�
    }
}