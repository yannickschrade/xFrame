namespace xFrame.WPF.Controls.WindowShell
{

    public sealed class ChangeScope : DisposableObject
    {
        private readonly GlowWindowBehavior behavior;

        public ChangeScope(GlowWindowBehavior behavior)
        {
            this.behavior = behavior;
            this.behavior.DeferGlowChangesCount++;
        }

        protected override void DisposeManagedResources()
        {
            behavior.DeferGlowChangesCount--;
            if (behavior.DeferGlowChangesCount == 0)
            {
                behavior.EndDeferGlowChanges();
            }
        }
    }
}