using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveUnitychan : MonoBehaviour
{

    [SerializeField]
    private float move_speed = 0.01f;
    [SerializeField]
    private float jump_force = 10.0f;
    [SerializeField]
    private float step_force = 3.0f;
    [SerializeField]
    private float ray_distance = 10.0f;
    [SerializeField]
    private float ray_y_offset = 0.5f;
    [SerializeField]
    private Text distance_text;

    private Quaternion LEFT = Quaternion.Euler(0, -90, 0);
    private Quaternion RIGHT = Quaternion.Euler(0, 90, 0);

    private UnitychanControls _input;

    Rigidbody _rb;
    Animator _animator;
    RaycastHit hit_info;

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
        distance_text.text = "distance: " + hit_info.distance.ToString("f3");
    }

    private void FixedUpdate()
    {
        Vector3 pos = transform.position;
        pos.y += ray_y_offset;
        Physics.Raycast(pos, -transform.up, out RaycastHit info, ray_distance);
        Debug.DrawRay(pos, -transform.up * ray_distance, Color.red);
        hit_info = info;
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

    public void MoveFalling(bool inputleft, bool inputright)
    {
        if(inputleft)
        {
            transform.rotation = LEFT;
            transform.position += transform.forward * move_speed;
        }
        if(inputright)
        {
            transform.rotation = RIGHT;
            transform.position += transform.forward * move_speed;
        }
    }

    public void MoveFalling(float axis_val)
    {
        if (axis_val <= -0.5f)
        {
            transform.rotation = LEFT;
            transform.position += transform.forward * move_speed * 0.75f;
        }
        if (axis_val >= 0.5f)
        {
            transform.rotation = RIGHT;
            transform.position += transform.forward * move_speed * 0.75f;
        }
    }

    public void Jump()
    {
        _rb.AddForce(transform.up * jump_force);
        is_onground = false;
    }

    public void Step()
    {
        _rb.AddForce(transform.forward * step_force, ForceMode.Impulse);
       
    }

    public void Stop()
    {
        _rb.velocity = Vector3.zero;
    }

    public bool CharacterOnGround()
    {
        if(!is_onground && hit_info.collider != null)
        {
            if (hit_info.collider.gameObject.CompareTag("Ground") && hit_info.distance < ray_y_offset)
            {
                is_onground = true;
                return true;
            }
        }
        return false;
    }

}

