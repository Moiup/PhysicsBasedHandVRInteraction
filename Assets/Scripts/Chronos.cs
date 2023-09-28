using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

public class Chronos : MonoBehaviour
{
    public static string id;
    public static bool is_id = false;

    private string[] scenes = {
        "Break",
        "Pile",
        "Placement",
        "Sphere",
        "Throw"
    };

    private float start;
    private float end;
    private float total;
    private bool is_launched = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!is_id){
            id = System.DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
            is_id = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1")){
            SceneManager.LoadScene(scenes[0]);
        }
        if(Input.GetKeyDown("2")){
            SceneManager.LoadScene(scenes[1]);
        }
        if(Input.GetKeyDown("3")){
            SceneManager.LoadScene(scenes[2]);
        }
        if(Input.GetKeyDown("4")){
            SceneManager.LoadScene(scenes[3]);
        }
        if(Input.GetKeyDown("5")){
            SceneManager.LoadScene(scenes[4]);
        }

        if(Input.GetKeyDown("c") && !is_launched){
            Debug.Log("START");
            start = Time.time;
            is_launched = true;
        }
        else if(Input.GetKeyDown("c") && is_launched){
            end = Time.time;
            total = end - start;
            string scene_name = SceneManager.GetActiveScene().name;
            using(StreamWriter s = File.AppendText("Assets/Results/" + id + ".txt")){
                s.WriteLine(scene_name + " " + start.ToString() + " " + end.ToString() + " " + total);
            }
            Debug.Log(scene_name + " " + start.ToString() + " " + end.ToString() + " " + total);
            is_launched = false;
            Debug.Log("END");
        }
    }
}
