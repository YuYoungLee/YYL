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
        //TODO InputAction 행동별로 저장할것 move skill, look은 제외
        inputAsset.Enable();
        SetKey();
    }

    private void SetKey()
    {
        //TODO
        //인풋시스템 키 바인드를 여러번 할 경우의 문제 + 키가 변경됬을때 Add할때의 문제
        //1. 계속 세로 만들기, 2 기존 바인드 삭제 후 다시 할당
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

        //키 바인드 설정
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

        mPlayerInput.SwitchCurrentActionMap("Player");   //플레이어 인풋 바인딩
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
    }   //인풋 액션


    public void InputSwitchB(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.actionMap.name + " InputSwitchB UI");
        mPlayerInput.SwitchCurrentActionMap("UI");
    }   //인풋 액션


    /// <summary>
    /// eKeyType : 활성화 할 키 타입
    /// bStatu : 활성화 비활성화
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
    }   //키 설정
}
