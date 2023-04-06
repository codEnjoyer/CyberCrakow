using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myStateMachine;
public class CheckIfGrounded : MonoBehaviour
{
    LayerMask groundMask = Character.whatIsGround;
    //[SerializeField] LayerMask groundMask;
    public static List<Collider> groundTouchPoints = new List<Collider>();
    [SerializeField] Rigidbody rb;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && groundTouchPoints.Count >0)
        {
            rb.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
            Debug.Log("jump");
        }
        if (groundTouchPoints.Count > 0)
            Debug.Log("groundTouchPoint");
    }
    void OnCollisionStay(Collision collision)
    {
        List<ContactPoint> contactPoints = new List<ContactPoint>();
        int numOfContacts = collision.GetContacts(contactPoints);
        for(int i =0; i < numOfContacts;i++)
        {
            Collider collider = collision.collider;
            if (RoundedNormalVectorAngle(contactPoints[i].normal, 3) <= 45 && CompareLayerIndex(collision.transform,groundMask) && !groundTouchPoints.Contains(collider))
            {
                groundTouchPoints.Add(collider);
            }
            else if(IsStillTouchingGround(contactPoints,numOfContacts) && groundTouchPoints.Contains(collider))
            {
                groundTouchPoints.Remove(collider);
            }
        }
    }
    void OnCollisionExit(Collision collision)
    {
        Collider collider = collision.collider;
        if (groundTouchPoints.Contains(collider))
        {
            groundTouchPoints.Remove(collider);
        }
    }
    float RoundedNormalVectorAngle(Vector3 normal,uint decimalAccuracy)
    {
        int accuracy = (int)Mathf.Pow(10, decimalAccuracy);
        return Mathf.RoundToInt(Vector3.Angle(normal,Vector3.up)* accuracy)/ accuracy;
    }
    bool CompareLayerIndex(Transform transform, LayerMask layer)
    {
        return Mathf.Pow(2, transform.gameObject.layer) == layer;
    }
    bool IsStillTouchingGround(List<ContactPoint> contactPoints,int numOfContacts)
    {
        for (int i = 0; i < numOfContacts; i++)
        {
            if (RoundedNormalVectorAngle(contactPoints[i].normal, 3) <= 45)
            {
                return true;
            }
        }
        return false;
    }
}
