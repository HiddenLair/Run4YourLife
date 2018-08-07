using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Utils
{
    public class ParticleScaler : MonoBehaviour
    {

        public class ShapeContainer
        {
            public ParticleSystem.ShapeModule shape;
            public ParticlesInfo info;

            public ShapeContainer(ParticleSystem.ShapeModule shape, ParticlesInfo info)
            {
                this.shape = shape;
                this.info = info;
            }
        }

        List<Transform> particlesToScale = new List<Transform>();
        List<ShapeContainer> particlesShapeToScale = new List<ShapeContainer>();

        // Use this for initialization
        void Awake()
        {
            ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem p in particles)
            {
                ParticlesInfo info = p.GetComponent<ParticlesInfo>();
                if (info != null && info.setScaleAsShape)
                {
                    particlesShapeToScale.Add(new ShapeContainer(p.shape,info));
                }
                else
                {
                    particlesToScale.Add(p.transform);
                }
            }
        }

        public void SetScale(Vector3 scale)
        {
            foreach (Transform transform in particlesToScale)
            {
                transform.localScale = scale;
            }
            for (int i = 0; i < particlesShapeToScale.Count; ++i)
            {
                ShapeContainer container = particlesShapeToScale[i];
                if (container.info.adjustPositionOnScaling)
                {
                    Vector3 temp = container.shape.scale;
                    container.shape.scale = scale;
                    container.shape.position += (scale -temp)/2;
                }
                else
                {
                    container.shape.scale = scale;
                }
            }
        }

        public void SetXScale(float x)
        {
            foreach (Transform transform in particlesToScale)
            {
                Vector3 temp = transform.localScale;
                temp.x = x;
                transform.localScale = temp;
            }
            for (int i = 0; i < particlesShapeToScale.Count; ++i)
            {
                ShapeContainer container = particlesShapeToScale[i];
                Vector3 tempScale = container.shape.scale;
                if (container.info.adjustPositionOnScaling)
                {
                    Vector3 tempPos = container.shape.position;
                    tempPos.x += (x - tempScale.x) / 2;
                    tempScale.x = x;
                    container.shape.scale = tempScale;
                    container.shape.position = tempPos;
                    
                }
                else
                {
                    tempScale.x = x;
                    container.shape.scale = tempScale;
                }
            }
        }

        public void SetYScale(float y)
        {
            foreach (Transform transform in particlesToScale)
            {
                Vector3 temp = transform.localScale;
                temp.y = y;
                transform.localScale = temp;
            }
            for (int i = 0; i < particlesShapeToScale.Count; ++i)
            {
                ShapeContainer container = particlesShapeToScale[i];
                Vector3 tempScale = container.shape.scale;
                if (container.info.adjustPositionOnScaling)
                {
                    Vector3 tempPos = container.shape.position;
                    tempPos.y += (y - tempScale.y) / 2;
                    tempScale.y = y;
                    container.shape.scale = tempScale;
                    container.shape.position = tempPos;

                }
                else
                {
                    tempScale.y = y;
                    container.shape.scale = tempScale;
                }
            }
        }

        public void SetZScale(float z)
        {
            foreach (Transform transform in particlesToScale)
            {
                Vector3 temp = transform.localScale;
                temp.z = z;
                transform.localScale = temp;
            }
            for (int i = 0; i < particlesShapeToScale.Count; ++i)
            {
                ShapeContainer container = particlesShapeToScale[i];
                Vector3 tempScale = container.shape.scale;
                if (container.info.adjustPositionOnScaling)
                {
                    Vector3 tempPos = container.shape.position;
                    tempPos.z += (z - tempScale.z) / 2;
                    tempScale.z = z;
                    container.shape.scale = tempScale;
                    container.shape.position = tempPos;

                }
                else
                {
                    tempScale.z = z;
                    container.shape.scale = tempScale;
                }
            }
        }

        public void AddToScale(Vector3 variation)
        {
            foreach (Transform transform in particlesToScale)
            {
                transform.localScale += variation;
            }
            for (int i = 0; i < particlesShapeToScale.Count; ++i)
            {
                ShapeContainer container = particlesShapeToScale[i];
                if (container.info.adjustPositionOnScaling)
                {                    
                    container.shape.scale += variation;
                    container.shape.position += variation / 2;
                }
                else
                {
                    container.shape.scale += variation;
                }
            }
        }
    }
}