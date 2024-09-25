using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    [SerializeField] AircraftMaster aircraftMaster;
    AircraftControl aircraftControl;

    // Center
    [Header("Common Center UI")]
    [SerializeField]
    TextMeshProUGUI speedText;
    [SerializeField]
    TextMeshProUGUI altitudeText;
    //[SerializeField]
    //AlertUIController alertUIController;

    [Header("1st-3rd View Control")]
    [SerializeField]
    RectTransform commonCenterUI;
    [SerializeField]
    RectTransform firstCenterViewTransform;
    [SerializeField]
    RectTransform thirdCenterViewTransform;

    [SerializeField]
    Canvas firstViewCanvas;
    [SerializeField]
    Vector2 firstViewAdjustAngle;

    [Header("1st View Center UI")]
    [SerializeField]
    UVController speedUV;
    [SerializeField]
    UVController altitudeUV;

    [SerializeField]
    Image throttleGauge;
    [SerializeField]
    HeadingUIController headingUIController;

    // Upper Right
    [Header("Upper Left UI")]
    [SerializeField]
    GameObject checkpointReachedUI;

    // Upper Left
    [Header("Upper Left UI")]
    [SerializeField]
    TextMeshProUGUI timeText;
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    TextMeshProUGUI targetText;

    // Lower Right
    [Header("Lower Right UI : Armament")]
    [SerializeField]
    TextMeshProUGUI gunText;
    [SerializeField]
    TextMeshProUGUI mslText;
    [SerializeField]
    TextMeshProUGUI spwText;

    [SerializeField]
    TextMeshProUGUI dmgText;

    [SerializeField]
    GameObject mslIndicator;
    [SerializeField]
    GameObject spwIndicator;

    // Status
    [Header("Lower Right UI : Aircraft/Weapon Status")]
    [SerializeField]
    Image aircraftImage;
    //[SerializeField]
    //CooldownImage leftMslCooldownImage;
    //[SerializeField]
    //CooldownImage rightMslCooldownImage;

    [Header("Minimap Misc.")]
    //[SerializeField]
    //MinimapCompass minimapCompass;
    //[SerializeField]
    //MinimapController minimapController;

    [Header("Materials")]
    [SerializeField]
    Material spriteMaterial;
    [SerializeField]
    Material fontMaterial;

    [Header("Colors")]
    [SerializeField]
    Color cautionColor;

    //List<MaskColorChange> maskColorChanges = new List<MaskColorChange>();

    [Header("Sounds")]
    [SerializeField]
    AudioClip spwChangeAudioClip;
    [SerializeField]
    AudioClip mslChangeAudioClip;
    [SerializeField]
    AudioClip timeLowAudioClip;

    AudioSource audioSource;

    float remainTime;
    int score = 0;
    float damage = 0;
    bool isTimeLow = false;
    bool isRedTimerActive = false;
    bool enableCount = true;

    float elapsedTime = 0;

    public float StopCountAndGetElapsedTime()
    {
        enableCount = false;
        return elapsedTime;
    }

    public void StartCountAndResetElapsedTime()
    {
        enableCount = true;
        elapsedTime = 0;
    }

    // Center
    void SetSpeed(int speed)
    {
        string text = string.Format("<mspace=18>{0}</mspace>", speed);
        speedText.text = text;

        speedUV.SetUV(speed);
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

    //public void AddMaskColorChange(MaskColorChange mask)
    //{
    //    maskColorChanges.Add(mask);
    //}

    public void SetWarningUIColor(bool isWarning)
    {
        Color color;
        if (isWarning == true)
        {
            color = Color.red;
            //aircraftImage.color = GameManager.WarningColor;
        }
        else
        {
            color = new Color32(0xAA, 0xFF, 0xAA, 0xFF);
        }

        spriteMaterial.color = color;
        fontMaterial.SetColor("_FaceColor", color);

        //foreach (MaskColorChange maskColorChange in maskColorChanges)
        //{
        //    maskColorChange.ChangeComponentColor(color);
        //}
    }

    void Start()
    {
        firstViewAdjustAngle = new Vector2(1 / firstViewAdjustAngle.x, 1 / firstViewAdjustAngle.y);

        //mslIndicator.SetActive(true);
        //spwIndicator.SetActive(false);

        SetWarningUIColor(false);

        //aircraftControl = aircraftMaster.AircraftSelecter().aircraftControl;
    }

    // Update is called once per frame
    void Update()
    {
        //if (enableCount == true && remainTime > 0 && GameManager.Instance.IsGameOver == false) SetTime();

        UpdateUI();
    }

    void UpdateUI()
    {
        SetSpeed((int)aircraftMaster.GetSpeed());
        SetAltitude((int)aircraftMaster.transform.position.y);
        SetThrottle(aircraftControl.throttle);
        SetHeading(aircraftMaster.transform.eulerAngles.y);
    }
}
