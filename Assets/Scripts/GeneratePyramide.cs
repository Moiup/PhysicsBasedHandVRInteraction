using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePyramide : MonoBehaviour
{
    public GameObject cube;
    public Transform parent;

    public int pyramide_size = 5;

    // Start is called before the first frame update
    void Start()
    {
        int line_size = pyramide_size;
        int line_end = line_size;

        Vector3 shelf_offset = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y + (transform.localScale.y / 2) + (cube.transform.localScale.y / 2),
            transform.localPosition.z - (transform.localScale.z / 2) + cube.transform.localScale.z
        );
        
        float offset_cube_z = cube.transform.localScale.z * 0.75f;


        for(int col = 0; col < line_size; col++){
            for(int line = col; line < line_end; line++){
                GameObject go = Instantiate(cube, parent);
                go.transform.localPosition = new Vector3(
                    shelf_offset.x,
                    shelf_offset.y + col * cube.transform.localScale.y,
                    shelf_offset.z + line * (cube.transform.localScale.z + cube.transform.localScale.z / 2) - col * offset_cube_z
                );
            }
            // line_end--;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
