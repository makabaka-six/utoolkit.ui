using UnityEngine;

namespace UToolkit.UI
{
    public abstract class View
    {
        protected UIReference _reference;

        public virtual void InitViewComponents(UIReference reference)
        {
            _reference = reference;
        }
    }
}