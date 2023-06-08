using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushButton : MonoBehaviour
{
    // Start is called before the first frame update
    HordeStart actionScript;
    public void Action()
    {
        actionScript.Activate();
        Debug.Log("Action");
        Deactivate();
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    public void Recover()
    {
        gameObject.SetActive(true);
    }
    void Start()
    {
        actionScript = GetComponent<HordeStart>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
