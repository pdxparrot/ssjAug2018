using System;

using UnityEngine;

namespace pdxpartyparrot.Core.Util.ObjectPool
{
    public sealed class PooledObject : MonoBehaviour
    {
#region Events
        public event EventHandler<EventArgs> RecycleEvent;
#endregion

        public string Tag { get; set; }

        public void Recycle()
        {
            ObjectPoolManager.Instance.Recycle(this);

            RecycleEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
