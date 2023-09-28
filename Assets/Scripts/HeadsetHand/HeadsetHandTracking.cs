using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ViveHandTracking {
public class HeadsetHandTracking : MonoBehaviour
{
    public int nb_joints = 21;
    public int nb_hands = 2;
    public Transform joint;

    public GameObject left_go;
    public GameObject right_go;

    private List<List<Transform>> joints;
    private List<List<Transform>> joints_for_pos;

    private static int[] parenting = new int[]{
        0, 0, 1, 2, 3,
        0, 5, 6, 7,
        0, 9, 10, 11,
        0, 13, 14, 15,
        0, 17, 18, 19
    };

    // Start is called before the first frame update
    IEnumerator Start()
    {
        // wait until detection is started, so we know what mode we are using
        while (GestureProvider.Status == GestureStatus.NotStarted) yield return null;

        createJoints();
    }


    // Update is called once per frame
    void Update()
    {
        GestureResult left_hand = GestureProvider.LeftHand;
        GestureResult right_hand = GestureProvider.RightHand;
        
        if(left_hand != null){
            positionHand(
                left_hand,
                left_go,
                0
            );
        }
        if(right_hand != null){
            positionHand(
                right_hand,
                right_go,
                1
            );
        }
    }

    void positionHand(GestureResult hand, GameObject parent, int hand_i){
        Vector3[] points = hand.points;

        parent.transform.position = hand.position;
        parent.transform.rotation = hand.rotation;

        Matrix4x4 handWorld2LocalMatrix =
        Matrix4x4.TRS(hand.position, hand.rotation, Vector3.one).inverse;

        for(int i = 0; i < nb_joints; i++){
            joints_for_pos[hand_i][i].localPosition = handWorld2LocalMatrix.MultiplyPoint(hand.points[i]);
            joints[hand_i][i].position = joints_for_pos[hand_i][i].position;
        }
    }

    void createJoints(){
        joints = new List<List<Transform>>(nb_hands);
        joints_for_pos = new List<List<Transform>>(nb_hands);
        for(int i = 0; i < nb_hands; i++){
            // For each hand
            joints.Add(new List<Transform>(nb_joints));
            joints_for_pos.Add(new List<Transform>(nb_joints));
            for(int j = 0; j < nb_joints; j++){
                // For each joint
                joints[i].Add(Instantiate(joint));
                joints_for_pos[i].Add(Instantiate(joint));
                Destroy(joints_for_pos[i][j].GetComponent<MeshRenderer>());
            }

            // Building the skeleton
            for(int j = 1; j < parenting.Length; j++){
                joints[i][j].SetParent(joints[i][parenting[j]]);
            }
        }
        joints[0][0].SetParent(left_go.transform);
        joints[1][0].SetParent(right_go.transform);

        joints_for_pos[0][0].SetParent(left_go.transform);
        joints_for_pos[1][0].SetParent(right_go.transform);
        for(int j = 0; j < nb_joints; j++){
            joints_for_pos[0][j].SetParent(left_go.transform);
        }

        for(int j = 0; j < nb_joints; j++){
            joints_for_pos[1][j].SetParent(right_go.transform);
        }
    }
}
}