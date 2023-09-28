using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;

public class HandJointTrigger : MonoBehaviour
{
    private Int32 id;
    public Int32 nb_step = 10;
    private float step;
    private float distance;

    private GameObject _bro;
    private Rigidbody _bro_rb;
    private Collider collider;
    private bool is_trigger = false;

    private Vector3 p_prec_fantome;
    private Vector3 destination;
    private Vector3 direction;
    private Vector3 target;
    private Int32 i = 0;

    public Transform touched_tr = null;
    public Vector3 direction_bis;
    private Vector3 prev_pos;

    public Material collision_material;
    private Material initial_material;

    private Vector3 force_to_apply_dir = new Vector3();

    public float raideur = 10000.0f;
    public float amorti = 20.0f;
    public float multiply_force = 1.0f;

    void Start(){
        p_prec_fantome = transform.position;
        destination = transform.position;
        direction = new Vector3(0.0f, 0.0f, 0.0f);
        collider = transform.GetComponent<Collider>();
        i = nb_step;
        step = 1 / (float)nb_step;
        is_trigger = false;

        prev_pos = transform.position;
        _bro_rb = _bro.GetComponent<Rigidbody>();

        Physics.queriesHitBackfaces = true;

        initial_material = _bro.transform.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = destination;
        MoveBro();
    }

    void MoveBro(){
        ComputeForce();
    }

    void ComputeForce(){
        Vector3 v_decalage = _bro_rb.position - transform.position;
        Vector3 f_elastique = - raideur * v_decalage;

        Vector3 vitesse_rb = _bro_rb.velocity;
        Vector3 vitesse_fantome = (transform.position - p_prec_fantome) / Time.fixedDeltaTime;
        Vector3 f_amorti = -amorti * (vitesse_rb - vitesse_fantome); 

        _bro_rb.AddForce((f_elastique + f_amorti) * multiply_force);

        p_prec_fantome = transform.position;
    }

    void OnTriggerEnter(Collider other){
        if(ingnoreTrigger(other)){
            return;
        }

        is_trigger = true;

        touched_tr = other.transform;

        _bro.GetComponent<Renderer>().material = collision_material;
    }


    void OnTriggerExit(Collider other){
        if(ingnoreTrigger(other)){
            return;
        }
        is_trigger = false;

        _bro.GetComponent<Renderer>().material = initial_material;
    }

    public void SetPosition(Vector3 n_destination){
        destination = n_destination;
        direction = destination - p_prec_fantome;
    }

    public Vector3 GetDestination(){
        return destination;
    }

    public void Print(){
        Debug.Log("This is a HandJointTrigger");
    }

    public void SetId(Int32 n_id){
        id = n_id;
    }

    public Int32 GetId(){
        return id;
    }//

    public void SetRigidbodyBrother(GameObject bro){
        _bro = bro;
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

    bool isFingerTips(){
        return id == 4 || id == 8 || id == 12 || id == 16 || id == 20;
    }
}//
