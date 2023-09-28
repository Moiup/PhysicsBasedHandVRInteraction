using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{
    public Transform cube;
    private Vector3 start_pos;

    private bool is_pushed = false;
    private float pushed_time = 0.0f;
    private float push_delta = 3.0f;
    private bool is_locked = false;

    // Start is called before the first frame update
    void Start()
    {
        start_pos = new Vector3(
            cube.localPosition.x,
            cube.localPosition.y,
            cube.localPosition.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        if(!is_locked && is_pushed && ((Time.time - pushed_time) > push_delta)){
            InstantiateObject();
            pushed_time = Time.time;
        }

        if(Input.GetKeyDown("l")){
            is_locked = !is_locked;
        }

        if(Input.GetKeyDown("g")){
            InstantiateObject();
        }

        is_pushed = false;
    }

    void OnTriggerEnter(Collider other){
        is_pushed = true;
    }

    void InstantiateObject(){
        Transform t = Instantiate(cube, transform.parent);
        t.localPosition = start_pos;
    }
}
