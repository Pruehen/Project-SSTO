using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//참조용 클래스. 하위 컴포넌트에 접근할 때 사용. 웬만하면 여기서 하위 컴포넌트를 수정하지 말 것
//전투 기능을 우선 여기에 붙여봤음.
public class AircraftMaster : SceneSingleton<AircraftMaster>
{
    public AircraftControl AircraftControl { get; private set; }
    public AircraftData AircraftData { get; private set; }
    public AircraftFM AircraftFM { get; private set; }


    /// <summary>
    /// 현재 항공기의 속도(km/h)를 반환하는 메서드 
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
