using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowRotation : MonoBehaviour
{
    public Transform followObj;
    public Transform ObjToFollow;
    public float smooth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        followObj.rotation = Quaternion.Lerp(followObj.rotation, ObjToFollow.localRotation, smooth * Time.deltaTime);
    }
}
