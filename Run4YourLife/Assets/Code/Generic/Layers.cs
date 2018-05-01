using UnityEngine;

namespace Run4YourLife
{
    public class Layers
    {

        #region Unity Defaults

        public static readonly LayerMask Default = LayerMask.GetMask("Default");
        public static readonly LayerMask TransparentFX = LayerMask.GetMask("TransparentFX");
        public static readonly LayerMask IgnoreRaycast = LayerMask.GetMask("Ignore Raycast");
        public static readonly LayerMask Water = LayerMask.GetMask("Water");
        public static readonly LayerMask UI = LayerMask.GetMask("UI");

        #endregion

        #region Customs

        public static readonly LayerMask Runner = LayerMask.GetMask("Runner");
        public static readonly LayerMask RunnerInteractable = LayerMask.GetMask("RunnerInteractable");
        public static readonly LayerMask Stage = LayerMask.GetMask("Stage");
        public static readonly LayerMask Trap = LayerMask.GetMask("Trap");
        public static readonly LayerMask Boss = LayerMask.GetMask("Boss");

        #endregion
    }
}
