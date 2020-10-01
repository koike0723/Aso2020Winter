using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveUnitychan : MonoBehaviour
{

    [SerializeField]
    private float move_speed = 0.01f;
    [SerializeField]
    private float jump_force = 10.0f;

    private Quaternion LEFT = Quaternion.Euler(0, -90, 0);
    private Quaternion RIGHT = Quaternion.Euler(0, 90, 0);

    private UnitychanControls _input;

    Rigidbody _rb;
    Animator _animator;

    //地面との接触判定
    private bool is_onground = true;

    private void Awake()
    {
        // アセット名と同名のクラスを生成する
        _input = new UnitychanControls();
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // 使用する前に有効化する必要がある
        _input.Enable();
    }

    void Update()
    {
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

    public void MoveLeft()
    {
        transform.rotation = LEFT;
        transform.position += transform.forward * move_speed;
    }

    public void MoveRight()
    {
        transform.rotation = RIGHT;
        transform.position += transform.forward * move_speed;
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * jump_force);
        is_onground = false;
    }
}

