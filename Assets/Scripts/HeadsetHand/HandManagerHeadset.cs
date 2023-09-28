using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace ViveHandTracking {
public class HandManagerHeadset : MonoBehaviour
{
    private GameObject _camera;

    private GameObject _joint;
    private GameObject _joint_trigger;
    private GameObject _link;
    private GameObject _position_joint;

    private List<List<GameObject>> _hands;
    private List<List<GameObject>> _hands_trigger;
    private List<List<GameObject>> _links;
    private List<List<Int32>> _look_at_index;
    private List<List<GameObject>> _joints_for_pos;

    public GameObject _left_go;
    public GameObject _right_go;

    private List<List<List<Vector3>>> _previous_points;
    private List<List<List<Vector3>>> _previous_points_info;
    private List<List<Vector3>> _avg_points;
    private List<List<Vector3>> _avg_points_info;

    private Int32 _sliding_avg;

    // private const float MP_TO_M_WORLD = 0.92f; // Joint computed from wrist
    private const float MP_TO_M_WORLD = 2.05f;
    // private const float MP_TO_M_WORLD = 2.14f;
    // private const float MP_TO_M_WORLD = 0.214f;
    private const float MP_TO_M = 0.214f; // joint positionned on the image
    private const float SCALE_M = 5.0f;

    private Vector3 _prev_offset_head;

    private static int[] parenting = new int[]{
        0, 1, 2, 3, 4, // thumb
        0, 5, 6, 7, 8, // index
        0, 9, 10, 11, 12, // middle
        0, 13, 14, 15, 16, //ring
        0, 17, 18, 19, 20 // pinky
    };

    private int nb_joints = 21;

    public HandManagerHeadset(
        GameObject camera,
        GameObject joint,
        GameObject joint_trigger,
        GameObject link,
        GameObject position_joint,
        Int32 sliding_avg
        ){
        _camera = camera;
        _joint = joint;
        _joint_trigger = joint_trigger;
        _link = link;
        _sliding_avg = sliding_avg;
        _position_joint = position_joint;

        _prev_offset_head = new Vector3(0, 0, 0);
    }

    public void createHands(List<Material> materials){
        Int32 nb_rep = 2;
        Int32 nb_points = nb_joints;
        
        _hands = new List<List<GameObject>>(nb_rep);
        _hands_trigger = new List<List<GameObject>>(nb_rep);
        _links = new List<List<GameObject>>(nb_rep);
        _look_at_index = new List<List<Int32>>(nb_rep);
        _joints_for_pos = new List<List<GameObject>>(nb_rep);

        _avg_points = new List<List<Vector3>>(nb_rep);
        _avg_points_info = new List<List<Vector3>>(nb_rep);

        _previous_points = new List<List<List<Vector3>>>(_sliding_avg);
        _previous_points_info = new List<List<List<Vector3>>>(_sliding_avg);

        // Creating the sliding average positions array
        for(Int32 i = 0; i < _sliding_avg; i++){
            _previous_points.Add(new List<List<Vector3>>(nb_rep));
            _previous_points_info.Add(new List<List<Vector3>>(nb_rep));
            for(Int32 j = 0; j < nb_rep; j++){
                _previous_points[i].Add(new List<Vector3>(nb_points));
                _previous_points_info[i].Add(new List<Vector3>(nb_points));
                for(Int32 k = 0; k < nb_points; k++){
                    Vector3 tmp = new Vector3(0, 0, 0);
                    _previous_points[i][j].Add(tmp);
                    _previous_points_info[i][j].Add(tmp);
                }
            }
        }

        // Instantiating the joints
        for(Int32 i = 0; i < nb_rep; i++){
            _hands.Add(new List<GameObject>(nb_points));
            _hands_trigger.Add(new List<GameObject>(nb_points));
            _joints_for_pos.Add(new List<GameObject>(nb_points));

            _avg_points.Add(new List<Vector3>(nb_points));
            _avg_points_info.Add(new List<Vector3>(nb_points));

            for(Int32 j = 0; j < nb_points; j++){
                GameObject tmp_joint = Instantiate(_joint);
                tmp_joint.transform.position = new Vector3(0, 0, 0);
                HandJoint hj = tmp_joint.GetComponent<HandJoint>();
                hj.SetId(j);
                tmp_joint.GetComponent<Renderer>().material = materials[i];
                _hands[i].Add(tmp_joint);
                GameObject tmp_joint_trigger = Instantiate(_joint_trigger);
                tmp_joint_trigger.transform.position = new Vector3(0, 0, 0);
                HandJointTrigger hjt = tmp_joint_trigger.GetComponent<HandJointTrigger>();

                // hjt.SetPosition(new Vector3(0, 0, 0));
                hjt.SetId(j);
                hjt.SetRigidbodyBrother(tmp_joint);
                _hands_trigger[i].Add(tmp_joint_trigger);
                _avg_points[i].Add(new Vector3(0, 0, 0));
                _avg_points_info[i].Add(new Vector3(0, 0, 0));
                // Creating the joints for pos
                _joints_for_pos[i].Add(Instantiate(_position_joint));
            }
        }

        // Instantiating the links
        for(Int32 i = 0; i < nb_rep; i++){
            _links.Add(new List<GameObject>());
            
            for(Int32 j = 0; j < nb_joints; j += 5){
                for(Int32 k = 0; k < 4; k++){
                    GameObject link = Instantiate(_link);
                    link.GetComponent<linkHand>().setFather(_hands[i][parenting[j + k]]);
                    link.GetComponent<linkHand>().setSon(_hands[i][parenting[j + k + 1]]);

                    _links[i].Add(link);
                }
            }
        }

        // Assigning the joints_for_pos
        _left_go = new GameObject("left");
        _right_go = new GameObject("right");
        for(int j = 0; j < nb_points; j++){
            _joints_for_pos[0][j].transform.SetParent(_left_go.transform);
        }

        for(int j = 0; j < nb_points; j++){
            _joints_for_pos[1][j].transform.SetParent(_right_go.transform);
        }
    }

    private void updatePositions(GestureResult hand, GameObject parent, int hand_i){
        Int32 i = hand_i;
        Int32 nb_point = hand.points.Length;

        parent.transform.position = hand.position;
        parent.transform.rotation = hand.rotation;

        Matrix4x4 handWorld2LocalMatrix =
        Matrix4x4.TRS(hand.position, hand.rotation, Vector3.one).inverse;

        for(Int32 j = 0; j < nb_point; j++){
            for(Int32 k = 0; k < _sliding_avg - 1; k++){
                _previous_points[k][hand_i][j] = _previous_points[k + 1][hand_i][j];
            }
            _joints_for_pos[hand_i][j].transform.localPosition = handWorld2LocalMatrix.MultiplyPoint(hand.points[j]);

            Vector3 tmp_point = new Vector3(
                _joints_for_pos[hand_i][j].transform.position.x,
                _joints_for_pos[hand_i][j].transform.position.y,
                _joints_for_pos[hand_i][j].transform.position.z
            );

            _previous_points[_sliding_avg - 1][hand_i][j] = tmp_point;

            _avg_points[hand_i][j] = _previous_points[0][hand_i][j];
            for(Int32 k = 1; k < _sliding_avg; k++){
                _avg_points[hand_i][j] += _previous_points[k][hand_i][j];
            }
            _avg_points[hand_i][j] /= _sliding_avg;

            HandJointTrigger hjt = _hands_trigger[hand_i][j].GetComponent<HandJointTrigger>();

        }
        

        const float scale_val = 10.0f;;
        Vector3 diff = _avg_points[hand_i][0] * scale_val  - _avg_points[hand_i][0];
        for(Int32 j = 0; j < nb_point; j++){
            HandJointTrigger hjt = _hands_trigger[hand_i][j].GetComponent<HandJointTrigger>();
            hjt.SetPosition(_avg_points[hand_i][j] * scale_val);
        }
    }

    public void updateHands(){
        GestureResult left_hand = GestureProvider.LeftHand;
        GestureResult right_hand = GestureProvider.RightHand;

        if(left_hand != null){
            updatePositions(
                left_hand,
                _left_go,
                0
            );
        }
        if(right_hand != null){
            updatePositions(
                right_hand,
                _right_go,
                1
            );
        }
    }

    private float computeTriangleArea(Vector3 point1, Vector3 point2, Vector3 point3){
        Vector3 vec_u = point2 - point1;
        Vector3 vec_v = point3 - point1;

        Vector3 n = Vector3.Cross(vec_u, vec_v);
        return n.magnitude * 0.5f;
    }

    private Vector3 imageToWorld(Vector3 p_i, float z){
        const float f_x_inv = 0.0018534856651418657928f;
        const float f_y_inv = 0.0019207129686539643515f;
        const float c_x_inv = -0.55067615157064375264f;
        const float c_y_inv = -0.4274297019053472649f;

        Vector3 d = new Vector3();

        d.x = f_x_inv * p_i.x + c_x_inv * p_i.z;
        d.y = f_y_inv * p_i.y + c_y_inv * p_i.z;
        d.z = 1.0f;
        d = Vector3.Normalize(d);

        return z * d;
    }
}
}