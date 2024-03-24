using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameJamEntry.Gameplay {
    public class Player : MonoBehaviour {
        [SerializeField] float Speed = 0.5f;
        
        
        protected void Update() {
            // movement
            var direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * Speed * Time.deltaTime;
            transform.Translate(direction);
            // rotation to the mouse
            var mousePosition = Input.mousePosition;
            mousePosition.z = 0;
            var point         = Camera.main.ScreenToWorldPoint(mousePosition);
            point.z      = 0;
            transform.up = point - transform.position;
            
        }

        protected void OnDrawGizmos() {
            var reflectionsInfo = DoReflectionCalculation(transform.position, transform.up);
            // draw finished points
            var points = reflectionsInfo.Points;
            for ( var i = 0; i < (points.Count - 1); i++ ) {
                Gizmos.DrawLine(points[i], points[i+1]);
            }
            // draw final ray
            Gizmos.DrawRay(points[^1], reflectionsInfo.LastDirection * 10000);
            Debug.Log($"points count {reflectionsInfo.Points.Count}");
        }

        (List<Vector2> Points, Vector3 LastDirection) DoReflectionCalculation(Vector2 origin, Vector2 direction) {
            var res                    = new List<Vector2> { origin };
            var maxNumberOfReflections = 100;

            var rayOrigin    = origin;
            var rayDirection = direction;

            for ( var i = 0; i < maxNumberOfReflections; i++ ) {
                var incomingVec = rayDirection;
                var raycastHit  = Physics2D.Raycast(rayOrigin, rayDirection);
                if ( raycastHit.collider == null ) {
                    break;
                }
                res.Add(raycastHit.point);
                rayOrigin    = raycastHit.point;
                rayDirection = Vector2.Reflect(incomingVec.normalized, raycastHit.normal);
            }
            // disable ray direction if we hit max amount of collisions
            return (res, res.Count == (maxNumberOfReflections + 1) ? Vector2.zero : rayDirection);
        }
    }
}