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
    private float fall_force = 10.0f;
    [SerializeField]
    private float step_force = 3.0f;
    [SerializeField]
    private float decelerate_val = 0.5f;
    [SerializeField]
    private float ray_distance = 10.0f;
    [SerializeField]
    private Text distance_text;

    private float ray_y_offset = 0.5f;
    private float ray_x_offset = 0.2f;

    private Quaternion LEFT = Quaternion.Euler(0, -90, 0);
    private Quaternion RIGHT = Quaternion.Euler(0, 90, 0);

    Rigidbody _rigidBody;
    RaycastHit hit_info_r;
    RaycastHit hit_info_l;

    //地面との接触判定
    private bool is_onground = true;
    private bool is_step = false;

    private void Awake()
    {
    }

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (distance_text != null)
        {
            distance_text.text = "distance: " + hit_info_r.distance.ToString("f3");
        }
    }

    public void Jump()
    {
        _rigidBody.AddForce(transform.up * jump_force, ForceMode.VelocityChange);
        is_onground = false;
    }

    public void Fall()
    {
        _rigidBody.AddForce(-transform.up * fall_force, ForceMode.VelocityChange);
    }

    public void MoveFalling(float axis_val)
    {
        if (axis_val <= -0.5f)
        {
            transform.rotation = LEFT;
            transform.position += transform.forward * move_speed * 0.5f;
        }
        if (axis_val >= 0.5f)
        {
            transform.rotation = RIGHT;
            transform.position += transform.forward * move_speed * 0.5f;
        }
    }

    public void Direction(float axis_val)
    {
        if (axis_val <= -0.5f)
        {
            transform.rotation = LEFT;
        }
        if (axis_val >= 0.5f)
        {
            transform.rotation = RIGHT;
        }
    }

    public void MoveFoward()
    {
        transform.position += transform.forward * move_speed;
    }

    public void Step()
    {
        _rigidBody.AddForce(transform.forward * step_force, ForceMode.VelocityChange);
        is_step = true;
        //_rigidBody.AddForce(transform.forward * step_force);

    }

    public bool GetIsStep()
    {
        return is_step;
    }

    public void Decelerate()
    {
        _rigidBody.velocity -= transform.forward * decelerate_val;
    }

    public void Stop()
    {
        _rigidBody.velocity = Vector3.zero;
        is_step = false;
    }

    public void RayCastToGround()
    {
        Vector3 pos = transform.position;

        pos.y += ray_y_offset;
        pos.x += ray_x_offset;
        Physics.Raycast(pos, -transform.up, out RaycastHit info_r, ray_distance);
        Debug.DrawRay(pos, -transform.up * ray_distance, Color.red);
        hit_info_r = info_r;

        pos.x -= ray_x_offset*2;
        Physics.Raycast(pos, -transform.up, out RaycastHit info_l, ray_distance);
        Debug.DrawRay(pos, -transform.up * ray_distance, Color.red);
        hit_info_l = info_l;
       
    }

    public bool GetIsOnground()
    {
        return is_onground;
    }

    public bool IsOnground()
    {
        if (hit_info_r.collider != null && hit_info_l.collider != null)
        {
            if(!is_onground)
            {
                if (hit_info_r.collider.gameObject.CompareTag("Ground") && hit_info_r.distance <= ray_y_offset
                 || hit_info_l.collider.gameObject.CompareTag("Ground") && hit_info_l.distance <= ray_y_offset)
                {
                    is_onground = true;
                }
            }
            else if(is_onground)
            {
               if (hit_info_r.collider.gameObject.CompareTag("Ground") && hit_info_r.distance > ray_y_offset
                && hit_info_l.collider.gameObject.CompareTag("Ground") && hit_info_l.distance > ray_y_offset)
               {
                    is_onground = false;
               }
            }
        }
        return is_onground;
    }

}

