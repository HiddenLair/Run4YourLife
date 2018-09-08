using UnityEngine;

namespace Run4YourLife.Utils
{
    public class HideCursorOnAwake : MonoBehaviour
    {
        void Awake()
        {
            Cursor.visible = false;
        }
    }
}