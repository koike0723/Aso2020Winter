using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


enum CharaState
{
    Idel,
    Run,
}

public class MoveUnitychan : MonoBehaviour
{

    [SerializeField]
    private float move_speed = 0.01f;

    private Quaternion LEFT = Quaternion.Euler(0, -90, 0);
    private Quaternion RIGHT = Quaternion.Euler(0, 90, 0);

    private CharaState state;

    private UnitychanControls _input;
    Animator _animator;

    private void Awake()
    {
        // アセット名と同名のクラスを生成する
        _input = new UnitychanControls();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        // 使用する前に有効化する必要がある
        _input.Enable();
    }

    void Update()
    {
        state = CharaState.Idel;
        MoveLeft();
        MoveRight();

        ChangeAnim();
    }

    private void OnDisable()
    {
        // 使用が終わったら無効化する
        _input.Disable();
    }

    private void OnDestroy()
    {
        _input.Dispose();
    }

    private void MoveLeft()
    {
        if (_input.Player.MoveLeft.ReadValue<float>() > 0)
        {
            transform.rotation = LEFT;
            transform.position += transform.forward * move_speed;
            state = CharaState.Run;
        }
    }

    private void MoveRight()
    {
        if(_input.Player.MoveRight.ReadValue<float>() > 0)
        {
            transform.rotation = RIGHT;
            transform.position += transform.forward * move_speed;
            state = CharaState.Run;
        }
    }

    private void ChangeAnim()
    {
        
        switch(state)
        {
            case CharaState.Idel:
                _animator.SetBool("is_stand", true);
                _animator.SetBool("is_running", false);
                break;

            case CharaState.Run:
                _animator.SetBool("is_stand", false);
                _animator.SetBool("is_running", true);
                break;

            default:
                _animator.SetBool("is_stand", false);
                _animator.SetBool("is_running", false);
                break;
        }
    }
   
}
