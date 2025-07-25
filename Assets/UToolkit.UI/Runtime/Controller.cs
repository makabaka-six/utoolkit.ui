using UnityEngine;

namespace UToolkit.UI
{
    public enum LunchMode
    {
        Normal,
        SingleInstance,
        SingleInstanceWithStack,
        SingleTop,
    }

    public abstract class Controller
    {
        protected LunchMode _lunchMode = LunchMode.Normal;
        protected GameObject _target = null;
        protected Intent _intent = null;

        public virtual void OnCreate(Intent intent)
        {
            _intent = intent;
        }

        public virtual void OnShow() { }

        public virtual void OnHide() { }

        public virtual void OnDestroy() { }

        protected T GetView<T>(ref T view) where T : View, new()
        {
            if (_target.TryGetComponent<UIReference>(out UIReference reference))
            {
                T t = new T();
                t.InitViewComponents(reference);
                return t;
            }
            else
            {
                return null;
            }
        }
    }
}