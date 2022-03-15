using System.Collections;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other) { 
        if(other.gameObject.name == "NextLevel"){
            if(Input.GetKey("F")){
                Application.LoadLevel("Ares");
            }
        }
    }
}
