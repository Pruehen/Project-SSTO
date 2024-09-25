using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInputCustom : SceneSingleton<PlayerInputCustom>
{
    public bool isControlable { get; set; } = true;
    public float pitchAxis {get; private set;}
    public float rollAxis {get; private set;}
    public float yawAxis {get; private set;}
    public float throttleAxis { get; private set; }
    public bool isLeftClick {get; private set;}
    Vector2 mouseDeltaPos;
    float mouseControllGain = 0.1f;

    public System.Action OnFireEvent;

    // Start is called before the first frame update
    void Start()
    {

    }

    public UnityEvent onClick_X;
    public UnityEvent onClick_R;
    public UnityEvent onClick_Fdown;
    public UnityEvent onClick_Fup;
    public UnityEvent onClick_RightMouse;
    public UnityEvent onClick_LeftMouseDown;
    public UnityEvent onClick_LeftMouseUp;
    public UnityEvent onClick_MidMouseDown;
    public UnityEvent onClick_MidMouseUp;

    // Update is called once per frame
    void Update()
    {
        if(!isControlable)
        { return; } 
        ControlSurface();

        if (Input.GetMouseButtonDown(0))
        {
            onClick_LeftMouseDown.Invoke();
        }
        if (Input.GetMouseButtonUp(0))
        {
            onClick_LeftMouseUp.Invoke();
        }
        if (Input.GetMouseButtonDown(1))
        {
            missileFireTrigger = true;
        }
        if (Input.GetMouseButtonDown(2))
        {
            onClick_MidMouseDown.Invoke();
        }
        if (Input.GetMouseButtonUp(2))
        {
            onClick_MidMouseUp.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            onClick_X.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            onClick_R.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            onClick_Fdown.Invoke();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            onClick_Fup.Invoke();
        }
    }
    bool missileFireTrigger = false;
    private void FixedUpdate()
    {
        if (missileFireTrigger)
        {
            onClick_RightMouse.Invoke();
            missileFireTrigger = false;
        }
    }
    void OnMouseDeltaPos(InputValue inputValue)
    {
        if (!isControlable)
        { return; }
        mouseDeltaPos = inputValue.Get<Vector2>();//��ǲ ���� �޾ƿ�                
    }

    void ControlSurface()//������ ���� ��ǲ
    {
        pitchAxis = 0;
        rollAxis = 0;
        yawAxis = 0;
        throttleAxis = 0;

        if (Input.GetKey(KeyCode.A))//�� ����
        {
            yawAxis -= 1;
        }
        if (Input.GetKey(KeyCode.D))//�� ����
        {
            yawAxis += 1;
        }
        if (Input.GetKey(KeyCode.W))//��ġ �ٿ�
        {
            pitchAxis -= 1;
        }
        if (Input.GetKey(KeyCode.S))//��ġ ��
        {
            pitchAxis += 1;
        }
        if (Input.GetKey(KeyCode.Q))//�� ����
        {
            rollAxis -= 1;
        }
        if (Input.GetKey(KeyCode.E))//�� ����
        {
            rollAxis += 1;
        }
        if (Input.GetKey(KeyCode.LeftControl))//����Ʋ �ٿ�
        {
            throttleAxis -= 1;
        }
        if (Input.GetKey(KeyCode.LeftShift))//����Ʋ ��
        {
            throttleAxis += 1;
        }


        pitchAxis -= mouseDeltaPos.y * mouseControllGain;
        rollAxis += mouseDeltaPos.x * mouseControllGain;

        pitchAxis = Mathf.Clamp(pitchAxis, -1, 1);
        rollAxis = Mathf.Clamp(rollAxis, -1, 1);
    }
    void OnFire(InputValue inputValue)
    {

        if (!isControlable)
        { return; }
        if (inputValue.isPressed)
        {
            OnFireEvent?.Invoke();
        }
    }
}
