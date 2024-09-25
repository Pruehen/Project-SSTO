using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atmosphere
{
    /// <summary>
    /// ������ٵ� ������ �׷��� Ȯ���ϴ� �޼���. ���� ���� ��, �׷� ���, �ӵ����� �Է��ϸ� �ȴ�.
    /// </summary>
    /// <param name="yPos"></param>
    /// <param name="dc"></param>
    /// <param name="speed"></param>
    /// <returns></returns>
    public static float Drag(float yPos, float cd, float speed)
    {        
        //Debug.Log(ap);

        //������ ����� �׷�.
        float dragGain = 1 + (Mathf.Clamp(speed - 300, 0, 60)) / 60;

        return AtmosphericPressure(yPos) * cd * speed * 0.001f * dragGain;
    }
    /// <summary>
    /// ���� ���� ������ Ȯ���ϴ� �޼���
    /// </summary>
    /// <param name="yPos"></param>
    /// <returns></returns>
    public static float AtmosphericPressure(float yPos)
    {
        //�ٻ�ġ. 5500���� �ö󰥶����� �������� ������.
        return 1 / Mathf.Exp(-Mathf.Log(0.5f) / 5500 * yPos);
    }
}
