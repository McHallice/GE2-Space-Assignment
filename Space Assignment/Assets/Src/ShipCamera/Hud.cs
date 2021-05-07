using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Src.Interfaces;
using Assets.Src.Targeting;

namespace Assets.Src.ShipCamera
{
    public class Hud : MonoBehaviour
    {
       
        public List<string> MainTags = new List<string> { "SpaceShip" };
        public List<string> SecondaryTags = new List<string> { "Projectile" };
       
        public Camera Camera;
       
        public Texture FollowedTargetReticleTexture;
        public Texture WatchedTargetReticleTexture;

       
        public float MinShowDistanceDistance = 20;

        private ITargetDetector _detector;
        
        private ShipCam _shipCam;
        public bool OnlyDrawReticlesForTopParent = true;

        void Start()
        {
            
            _shipCam = GetComponent<ShipCam>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        

        

        private void DrawSingleLable(PotentialTarget target)
        {
            
            Vector3 boxPosition = Camera.main.WorldToScreenPoint(target.Transform.position);
            if (boxPosition.z > 0)
            {
                Vector3 baseLocation;
                if(_shipCam != null && _shipCam.FollowedTarget != null)
                {
                    baseLocation = _shipCam.FollowedTarget.transform.position;
                } else
                {
                    baseLocation = Camera.transform.position;
                }
                var distance = Vector3.Distance(baseLocation, target.Transform.position);

                
                boxPosition.y = Screen.height - boxPosition.y;

                if (target.Transform.parent == null)
                { 
                   
                    GUI.Label(new Rect(boxPosition.x - 20, boxPosition.y - 35, 50, 20), target.Transform.tag);
                    if (distance > MinShowDistanceDistance)
                    {
                        //Draw the distance from the followed object to this object - only if it's suitably distant.
                        GUI.Label(new Rect(boxPosition.x - 20, boxPosition.y + 25, 40, 40), Math.Round(distance).ToString());
                    }
                }

                var rect = new Rect(boxPosition.x - 50, boxPosition.y - 50, 100, 100);
              

                var healthControler = target.Transform.GetComponent<HealthControler>();
                
            }
        }
    
       

       
    }
}