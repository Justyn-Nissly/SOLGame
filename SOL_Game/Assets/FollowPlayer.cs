using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

   public GameObject tPlayer;
   public Transform tFollowTarget;
   private CinemachineVirtualCamera vcam;

   // Use this for initialization
   void Start()
   {
        vcam = GetComponent<CinemachineVirtualCamera>();
        tPlayer = GameObject.FindWithTag("Player");
    }

   // Update is called once per frame
   void Update()
   {
           if (tPlayer != null)
           {
               tFollowTarget = tPlayer.transform;
               vcam.LookAt = tFollowTarget;
               vcam.Follow = tFollowTarget;
           }
   }
}
