using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : InteractableObject
{
    // Start is called before the first frame update
    ActionScript actionScript;
    public override void Action()
    {
        actionScript.Activate();
        //Debug.Log("Action");
        Deactivate();
    }

    public override void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public override void Recover()
    {
        gameObject.SetActive(true);
    }
    void Start()
    {
        actionScript = GetComponent<ActionScript>();
    }
}
