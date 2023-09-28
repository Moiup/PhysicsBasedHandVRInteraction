using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach(GameObject go in allObjects){
            go.transform.localPosition = go.transform.localPosition * 10.0f;
            go.transform.localScale = go.transform.localScale * 10.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
