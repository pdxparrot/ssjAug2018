using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    public abstract class Actor : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private int _id = -1;

        public int Id => _id;

        public GameObject GameObject => gameObject;

        public string Name => name;

        [SerializeField]
        private Collider _collider;

        public Collider Collider => _collider;

        public abstract float Height { get; }

        public abstract float Radius { get; }

        [SerializeField]
        private GameObject _model;

        public GameObject Model => _model;

        [SerializeField]
        private ActorController _controller;

        public ActorController Controller => _controller;

        [SerializeField]
        private Animator _animator;

        public Animator Animator => _animator;

        public abstract bool IsLocalActor { get; }

        [CanBeNull]
        public abstract Viewer Viewer { get; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            PartyParrotManager.Instance.PauseEvent += PauseEventHandler;
        }

        protected virtual void OnDestroy()
        {
            if(PartyParrotManager.HasInstance) {
                PartyParrotManager.Instance.PauseEvent -= PauseEventHandler;
            }
        }
#endregion

        public virtual void Initialize(int id)
        {
            _id = id;
        }

#region Callbacks
        public abstract void OnSpawn();

        public abstract void OnReSpawn();
#endregion

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            Animator.enabled = !PartyParrotManager.Instance.IsPaused;
        }
#endregion
    }
}
