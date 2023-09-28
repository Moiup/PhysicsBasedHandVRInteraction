using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
using ViveHandTracking;

public class mainHeadset : MonoBehaviour
{
    public GameObject joint;
    public GameObject joint_trigger;
    public GameObject link;
    public GameObject position_joint;
    private HandManagerHeadset hand_manager = null;

    public Int32 sliding_avg = 2;

    public GameObject ref_world = null;

    public List<Material> hand_materials = new List<Material>(2);

    // Start is called before the first frame update
    void Start()
    {
        hand_manager = new HandManagerHeadset(
            ref_world,
            joint,
            joint_trigger,
            link,
            position_joint,
            sliding_avg
        );
        hand_manager.createHands(hand_materials);
    }

    // Update is called once per frame
    void Update()
    {
        hand_manager.updateHands();
    }
}
