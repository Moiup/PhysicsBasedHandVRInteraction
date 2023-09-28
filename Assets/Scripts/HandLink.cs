using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandLink : MonoBehaviour
{
    private GameObject _target = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Orientation
        transform.LookAt(_target.transform);
        // Distance
        Vector3 target_pos = _target.transform.localPosition;
        float dist = target_pos.magnitude;

        Vector3 dir = Vector3.Normalize(target_pos) * 0.5f;

        Vector3 scale = transform.localScale;
        scale.z = dist - 1.0f;
        Vector3 local_position = dir;
        transform.localPosition = local_position;
        transform.localScale = scale;
    }

    public void SetTarget(GameObject target){
        _target = target;
    }

    // void OnCollisionStay(Collision collision){
    //     HandJoint hj = _target.GetComponent<HandJoint>();
    //     hj.SetCollided();
    //     foreach(ContactPoint contact in collision.contacts){
    //         hj.normal_dir = contact.normal;
    //     }
    // }
}