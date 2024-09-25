using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputCustom : SceneSingleton<PlayerInputCustom>
{
    public float pitchAxis {get; private set;}
    public float rollAxis {get; private set;}
    public float yawAxis {get; private set;}
    public float throttleAxis { get; private set; }
    public bool isLeftClick {get; private set;}
    Vector2 mouseDeltaPos;
    float mouseControllGain = 0.1f;


    // Update is called once per frame
    void Update()
    {
        ControlSurface();
    }

    void OnMouseDeltaPos(InputValue inputValue)
    {
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
}
