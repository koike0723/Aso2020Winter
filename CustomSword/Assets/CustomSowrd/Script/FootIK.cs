using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
   // private CharacterController _characterController;
    private Animator _animator;

    //テスト用のIKのON,OFFスイッチ
    [SerializeField]
    private bool use_IK = true;
    //IKの角度を有効にするかどうか
    [SerializeField]
    private bool use_IK_rot = true;
    //右足のウエイト
    private float right_foot_weight = 0f;
    //左足のウエイト
    private float left_foot_weight = 0f;
    //右足の位置
    private Vector3 right_foot_pos;
    //左足の位置
    private Vector3 left_foot_pos;
    //右足の角度
    private Quaternion right_foot_rot;
    //左足の角度
    private Quaternion left_foot_rot;
    //右足と左足の距離
    private float distance;
    //足を付く位置のオフセット値
    [SerializeField]
    private float offset = 0.1f;
    //コライダの中心位置
   // private Vector3 default_center;
    //レイを飛ばす距離
    [SerializeField]
    private float ray_range = 1f;

    //コライダの位置を調整するときのスピード
    [SerializeField]
    private float smothing = 100f;

    //レイを飛ばす位置の調整値
    [SerializeField]
    private Vector3 ray_position_offset = Vector3.up * 0.3f;

    private void Start()
    {
       // _characterController = GetComponent<CharacterController>();
        //default_center = _characterController.center;
        _animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        //IKを使わない場合はこれ以降なにもしない
        if(!use_IK)
        {
            return;
        }

        //アニメーションパラメータからIKのウエイトを取得
        right_foot_weight = _animator.GetFloat("RightFootWeight");
        left_foot_weight = _animator.GetFloat("LeftFootWeight");

        //右足用のレイの視覚化
        Debug.DrawRay(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + ray_position_offset,
                        -transform.up * ray_range, Color.red);
        //右足用のレイを飛ばす処理
        var ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.RightFoot) + ray_position_offset,
                        -transform.up);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, ray_range, LayerMask.GetMask("Field")))
        {
            right_foot_pos = hit.point;

            //右足Ikの設定
            _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, right_foot_weight);
            _animator.SetIKPosition(AvatarIKGoal.RightFoot, right_foot_pos + new Vector3(0f, offset, 0f));
            if(use_IK_rot)
            {
                right_foot_rot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, right_foot_weight);
                _animator.SetIKRotation(AvatarIKGoal.RightFoot, right_foot_rot);
            }
        }

        //左足用のレイの視覚化
        Debug.DrawRay(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + ray_position_offset,
                        -transform.up * ray_range, Color.red);
        //右足用のレイを飛ばす処理
        ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + ray_position_offset,
                        -transform.up);

        if (Physics.Raycast(ray, out hit, ray_range, LayerMask.GetMask("Field")))
        {
            left_foot_pos = hit.point;

            //左足Ikの設定
            _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, left_foot_weight);
            _animator.SetIKPosition(AvatarIKGoal.LeftFoot, left_foot_pos + new Vector3(0f, offset, 0f));
            if (use_IK_rot)
            {
                left_foot_rot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, left_foot_weight);
                _animator.SetIKRotation(AvatarIKGoal.LeftFoot, left_foot_rot);
            }
        }

    }
}
