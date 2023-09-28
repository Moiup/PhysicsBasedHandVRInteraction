using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class linkHand : MonoBehaviour
{
    public GameObject father;
    public GameObject son;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // gameObject.GetComponent<Rigidbody>().MovePosition(father.transform.position);
        transform.position = father.transform.position;
        transform.LookAt(son.transform);

        float dist = (son.transform.position - father.transform.position).magnitude;

        Vector3 current_scale = transform.localScale;
        transform.localScale = new Vector3(
            current_scale.x,
            current_scale.y,
            dist
            );
    }

    public void setFather(GameObject f){
        father = f;
    }

    public void setSon(GameObject s){
        son = s;
    }
}
