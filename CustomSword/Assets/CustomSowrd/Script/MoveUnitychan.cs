using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MoveUnitychan : MonoBehaviour
{

    //移動スピード
    [SerializeField]
    private float move_speed = 0.01f;
    //落下時の移動スピード減算率
    [SerializeField]
    private float speed_rate = 0;
    //ジャンプ時に与えられる力
    [SerializeField]
    private float jump_force = 10.0f;
    //空中攻撃時に与えられる力
    [SerializeField]
    private float fall_force = 10.0f;
    //ステップ時に与えられる力
    [SerializeField]
    private float step_force = 3.0f;
    //ステップ時の減速量
    [SerializeField]
    private float decelerate_val = 0.5f;
    //着地判定用レイの長さ
    [SerializeField]
    private float ray_distance = 10.0f;
    //自キャラから地面までの距離表示用テキスト
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
    //ステップ中判定
    private bool is_step = false;
	//ヒット判定
	private bool is_hit = false;
	//ダメージ判定
	private bool is_dmg = false;

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

    //ジャンプ
    public void Jump()
    {
        _rigidBody.AddForce(transform.up * jump_force, ForceMode.VelocityChange);
        //_rigidBody.velocity = transform.up * jump_force;
        is_onground = false;
    }

    //空中攻撃時落下加速
    public void FallDown()
    {
        _rigidBody.AddForce(-transform.up * fall_force, ForceMode.VelocityChange);
        //_rigidBody.velocity = -transform.up * fall_force;
    }

    //落下
    public void Gravity()
    {
        _rigidBody.velocity -= new Vector3(0, 9.8f / 60.0f, 0);
    }

    //空中時移動
    public void MoveFalling(float axis_val)
    {
        Direction(axis_val);
        var fallVec = new Vector3(0, _rigidBody.velocity.y, 0);
        var moveVec = transform.forward * move_speed * speed_rate;
        var vec = new Vector3(moveVec.x, fallVec.y, 0);
        if (axis_val <= -0.5f)
        {
            _rigidBody.velocity = vec;
        }
        else if (axis_val >= 0.5f)
        {
            _rigidBody.velocity = vec;
        }
        else
        {
            _rigidBody.velocity = fallVec;
        }
    }

    //向き判定
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

    //移動
    public void MoveFoward()
    {
        _rigidBody.velocity = transform.forward * move_speed;
    }

    //ステップ
    public void Step()
    {
        if(!is_step)
        {
            _rigidBody.AddForce(transform.forward * step_force, ForceMode.VelocityChange);
            //_rigidBody.velocity = transform.forward * step_force;
            is_step = true;
        }
    }

    //移動攻撃用ステップ
    public void StepOn(float force)
    {
        _rigidBody.AddForce(transform.forward * force, ForceMode.VelocityChange);
    }

    //ステップ判定取得
    public bool GetIsStep()
    {
        return is_step;
    }

    //ステップ時減速
    public void Decelerate()
    {
        _rigidBody.velocity -= transform.forward * decelerate_val;
    }

    //停止
    public void Stop()
    {
        _rigidBody.velocity = Vector3.zero;
        is_step = false;
    }

    //地面方向レイ
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

    //着地判定取得
    public bool GetIsOnground()
    {
        return is_onground;
    }

    //着地判定処理
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

	public bool HitAtack()
	{
		bool hit = false;
		if(is_hit && !is_dmg)
		{
			hit = true;
			is_dmg = true;
		}

		return hit;
	}

	public void OnTriggerEnter(Collider other)
	{
		GameObject _hitBox = transform.Find("HitBox").gameObject;
		if( _hitBox.tag == "HitBox2P" && other.tag == "SwordHitBox1P" 
		 || _hitBox.tag == "HitBox1P" && other.tag == "SwordHitBox2P")
		{
			is_hit = true;
		}
	}

}

