using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject self;
    public GameObject my2dObject;
    public GameObject my1dObject;

    public void Activate2D(){
        my2dObject.SetActive(true);
        self.SetActive(false);
    }
    public void Activate1D(){
        my1dObject.SetActive(true);
        self.SetActive(false);
    }
}
