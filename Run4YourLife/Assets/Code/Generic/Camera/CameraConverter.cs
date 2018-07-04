using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.CameraUtils
{
    public class CameraConverter {
        
        /// <summary>
        /// Converts normalizedViewport (x,y) in range [0,1] to world position at z 0
        /// for more information look at Camera.ScreenToWorldPoint
        /// </summary>
        /// <returns>World position at the specified normalized viewport position</returns>
        public static Vector3 ViewportToGamePlaneWorldPosition(Camera camera, Vector2 viewport)
        {
            Vector3 position = new Vector3()
            {
                x = viewport.x,
                y = viewport.y,
                z = Math.Abs(camera.transform.position.z)
            };

            return camera.ViewportToWorldPoint(position);
        }
    }
}
