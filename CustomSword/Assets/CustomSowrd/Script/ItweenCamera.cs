using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItweenCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private float move_speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iTween.MoveUpdate(this.gameObject, iTween.Hash(
            "position", Target.position,
            "time", move_speed));
        iTween.RotateUpdate(this.gameObject, iTween.Hash(
            "rotation", Target.rotation.eulerAngles,
            "time", move_speed));
    }
}
