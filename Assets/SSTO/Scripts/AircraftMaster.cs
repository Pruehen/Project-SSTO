using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//������ Ŭ����. ���� ������Ʈ�� ������ �� ���. �����ϸ� ���⼭ ���� ������Ʈ�� �������� �� ��
//���� ����� �켱 ���⿡ �ٿ�����.
public class AircraftMaster : SceneSingleton<AircraftMaster>
{
    public AircraftControl AircraftControl { get; private set; }
    public AircraftData AircraftData { get; private set; }
    public AircraftFM AircraftFM { get; private set; }


    /// <summary>
    /// ���� �װ����� �ӵ�(km/h)�� ��ȯ�ϴ� �޼��� 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetVelocity()
    {
        return AircraftFM.Velocity;
    }   

    private void Awake()
    {
        AircraftControl = GetComponent<AircraftControl>();
        AircraftData = GetComponent<AircraftData>();
        AircraftFM = GetComponent<AircraftFM>();

        AircraftFM.Init(AircraftData);
    }
}
