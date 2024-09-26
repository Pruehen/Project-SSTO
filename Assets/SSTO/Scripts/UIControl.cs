using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    // Center
    [Header("Common Center UI")]
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI altitudeText;    

    [Header("1st View Center UI")]
    [SerializeField]
    UVController speedUV;
    [SerializeField]
    UVController altitudeUV;
    [SerializeField] RectTransform crosshair;

    [SerializeField]
    Image throttleGauge;
    [SerializeField]
    HeadingUIController headingUIController;


    // Center
    void SetSpeed(int speed)
    {
        string text = string.Format("<mspace=18>{0}</mspace>", speed);
        speedText.text = text;

        speedUV.SetUV(speed);
    }
    void SetCrosshair(Vector3 originPos, Vector3 velocity)
    {
        if(velocity.sqrMagnitude > 100)
        {
            crosshair.gameObject.SetActive(true);
            crosshair.SetUIPos_WorldToScreenPos(originPos + velocity);
        }        
        else
        {
            crosshair.gameObject.SetActive(false);
        }
    }

    void SetAltitude(int altitude)
    {
        string text = string.Format("<mspace=18>{0}</mspace>", altitude);
        altitudeText.text = text;

        altitudeUV.SetUV(altitude);
    }

    public void SetThrottle(float throttle)
    {
        throttleGauge.fillAmount = (1 + throttle) * 0.5f; ;
    }

    public void SetHeading(float heading)
    {
        headingUIController.SetHeading(heading);
        //minimapCompass.SetCompass(heading);
    }



    //public void SwitchUI(CameraController.CameraIndex index)
    //{
    //    bool isFirstView = (index == CameraController.CameraIndex.FirstView ||
    //                        index == CameraController.CameraIndex.FirstViewWithCockpit);

    //    firstCenterViewTransform.gameObject.SetActive(isFirstView);

    //    RectTransform parentTransform = (isFirstView == true) ? firstCenterViewTransform : thirdCenterViewTransform;
    //    commonCenterUI.SetParent(parentTransform);

    //    commonCenterUI.anchoredPosition = Vector2.zero;
    //}


    void Start()
    {
        //aircraftControl = aircraftMaster.AircraftSelecter().aircraftControl;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (enableCount == true && remainTime > 0 && GameManager.Instance.IsGameOver == false) SetTime();

        UpdateUI();
    }

    void UpdateUI()
    {
        SetSpeed((int)(AircraftMaster.Instance.GetVelocity().magnitude * 3.6f));
        SetCrosshair(AircraftMaster.Instance.transform.position, AircraftMaster.Instance.GetVelocity());
        SetAltitude((int)AircraftMaster.Instance.transform.position.y);
        SetThrottle(AircraftMaster.Instance.AircraftControl.throttle);
        SetHeading(AircraftMaster.Instance.transform.eulerAngles.y);        
    }
}
