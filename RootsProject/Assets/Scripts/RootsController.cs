using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsController : MonoBehaviour
{
    public Transform root;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            root.Translate(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            root.Translate(-Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            root.Translate(Vector3.right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            root.Translate(-Vector3.right);
        }
    }
}
