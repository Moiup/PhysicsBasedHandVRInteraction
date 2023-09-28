using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class HandJoint : MonoBehaviour
{
    private Int32 id;
    private float distance;

    private Rigidbody rb;
    private Collider collider;

    private Vector3 normal_dir;

    private Vector3 origin;
    private Vector3 destination;
    private Vector3 target;
    private Vector3 _move_target;
    private Vector3 direction;
    private Vector3 zero_dir;

    public Material collision_material;
    private Material initial_material;

    private Transform _look_at_target;

    void Start(){
        origin = transform.position;
        destination = transform.position;
        direction = new Vector3(0.0f, 0.0f, 0.0f);

        zero_dir = new Vector3(0.0f, 0.0f, 0.0f);

        rb = transform.GetComponent<Rigidbody>();

        initial_material = GetComponent<Renderer>().material;
    }

    public void SetPosition(Vector3 n_destination){
        origin = transform.position;
        destination = n_destination;
        direction = destination - origin;

        rb.MovePosition(destination);
    }

    public Vector3 GetDestination(){
        return destination;
    }

    public void SetMoveTarget(Vector3 move_target){
        _move_target = move_target;
    }

    public void Print(){
        Debug.Log("This is a HandJoint");
    }


    public void SetId(Int32 n_id){
        id = n_id;
    }

    public Int32 GetId(){
        return id;
    }

    bool ingnoreTrigger(Collider other){
        return other.transform.name == "hand_joint(Clone)"
            || other.transform.name == "hand_joint_cube(Clone)"
            || other.transform.name == "hand_joint_trigger(Clone)"
            || other.transform.name == "link(Clone)"
            || other.transform.name == "Terrain"
            || other.transform.name == "body_joint(Clone)"
            || other.transform.name == "shelf";
    }

    bool ignoreCollision(Collision collider){
        return collider.transform.name == "hand_joint(Clone)"
            || collider.transform.name == "hand_joint_cube(Clone)"
            || collider.transform.name == "hand_joint_trigger(Clone)"
            || collider.transform.name == "link(Clone)"
            || collider.transform.name == "Terrain"
            || collider.transform.name == "body_joint(Clone)"
            || collider.transform.name == "shelf";
    }

    bool isFingerTips(){
        return id == 4 || id == 8 || id == 12 || id == 16 || id == 20;
    }
}
