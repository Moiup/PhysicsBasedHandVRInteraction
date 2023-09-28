using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeTransparent : MonoBehaviour
{
    public Material transparent;
    private Material initial;

    private int nb_contact;

    // Start is called before the first frame update
    void Start()
    {
        initial = gameObject.GetComponent<Renderer>().material;   
        nb_contact = 0;    
    }

    void OnTriggerEnter(Collider other){
        if(ingnoreTrigger(other)){
            return;
        }
        nb_contact++;
        gameObject.GetComponent<Renderer>().material = transparent;
    }

    void OnTriggerExit(Collider other){
        if(ingnoreTrigger(other)){
            return;
        }
        nb_contact--;
        if(nb_contact == 0){
            gameObject.GetComponent<Renderer>().material = initial;
        }
    }

    bool ingnoreTrigger(Collider other){
        return other.transform.name == "Terrain"
            || other.transform.name == "body_joint(Clone)"
            || other.transform.name == "shelf";
    }
}
