using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveUnitychan : MonoBehaviour
{

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
        // PhaseがPerformedのときにログ出力
        if (_input.Player.MoveLeft.ReadValue<float>() >= 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            transform.position += transform.forward * 0.1f;
            _animator.SetBool("is_running", true);
            Debug.Log("left");
        }
        else
        {
            _animator.SetBool("is_running", false);
        }

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

    
   
}
