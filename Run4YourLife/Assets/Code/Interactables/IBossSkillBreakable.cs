namespace Run4YourLife.Interactables
{
    public interface IBossSkillBreakable
    {
        //<summary>
        // Breaks instantly
        // Pre: this.gameObject.activeSelf == true
        //</summary>
        void Break();
    }
}