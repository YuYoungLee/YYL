using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Input : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    [SerializeField] InputActionAsset inputAsset;
    [SerializeField] PlayerInput mPlayerInput;
    public Test_UI mUI;

    public void Initialize()
    {
        //TODO InputAction �ൿ���� �����Ұ� move skill, look�� ����
        inputAsset.Enable();
        SetKey();
    }

    private void SetKey()
    {
        //TODO
        //��ǲ�ý��� Ű ���ε带 ������ �� ����� ���� + Ű�� ��������� Add�Ҷ��� ����
        //1. ��� ���� �����, 2 ���� ���ε� ���� �� �ٽ� �Ҵ�
        //InputActionMap actionMap = inputAsset.FindActionMap("Player");
        //if (actionMap == null)
        //{
        //    Debug.LogError("actionMap is NULL : InputManager");
        //    return;
        //}

        //moveAction = actionMap.FindAction("Move");
        //if (moveAction == null)
        //{
        //    Debug.LogError("moveAction is NULL : InputManager");
        //    return;
        //}

        //Player player = GameManager.Instance.GetPlayer;
        //enable

        //Ű ���ε� ����
        //moveAction.AddCompositeBinding("2DVector")
        //.With("Up", "<Keyboard>/w")
        //.With("Down", "<Keyboard>/s")
        //.With("Left", "<Keyboard>/a")
        //.With("Right", "<Keyboard>/d");
        //Player Move
        //UI
        mPlayerInput.SwitchCurrentActionMap("UI");
        mPlayerInput.actions["I"].started += mUI.InputGUIA;
        mPlayerInput.actions["O"].started += mUI.InputGUIB;
        mPlayerInput.actions["E"].started += InputSwitchA;
        mPlayerInput.actions["R"].started += InputSwitchB;

        mPlayerInput.SwitchCurrentActionMap("Player");   //�÷��̾� ��ǲ ���ε�
        mPlayerInput.actions["I"].started += mUI.InputGUIA;
        mPlayerInput.actions["O"].started += mUI.InputGUIB;
        mPlayerInput.actions["E"].started += InputSwitchA;
        mPlayerInput.actions["R"].started += InputSwitchB;

        //mPlayerInput.SwitchCurrentActionMap("UI");
        //mPlayerInput.SwitchCurrentActionMap("Player");
        //inputAsset.Enable();
    }

    public void InputSwitchA(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.actionMap.name + " InputSwitchA Player");
        mPlayerInput.SwitchCurrentActionMap("Player");
    }   //��ǲ �׼�


    public void InputSwitchB(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.actionMap.name + " InputSwitchB UI");
        mPlayerInput.SwitchCurrentActionMap("UI");
    }   //��ǲ �׼�


    /// <summary>
    /// eKeyType : Ȱ��ȭ �� Ű Ÿ��
    /// bStatu : Ȱ��ȭ ��Ȱ��ȭ
    /// </summary>
    public void KeySetting()
    {
        if (mUI.GetActiveGUICount() == 0)
        {
            Debug.Log("Player");
            mPlayerInput.SwitchCurrentActionMap("Player");
        }
        else
        {
            Debug.Log("UI");
            mPlayerInput.SwitchCurrentActionMap("UI");
        }
    }   //Ű ����
}
