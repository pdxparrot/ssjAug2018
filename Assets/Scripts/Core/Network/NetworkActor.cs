using System;

using JetBrains.Annotations;

using pdxpartyparrot.Core.Actors;
using pdxpartyparrot.Core.Camera;
using pdxpartyparrot.Core.Util;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.Core.Network
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(NetworkAnimator))]
    public abstract class NetworkActor : NetworkBehaviour, IActor
    {
#region IActor
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

        [CanBeNull]
        public abstract Viewer Viewer { get; }
#endregion

        protected NetworkIdentity NetworkIdentity { get; private set; }

        protected NetworkTransform NetworkTransform { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            NetworkIdentity = GetComponent<NetworkIdentity>();
            NetworkTransform = GetComponent<NetworkTransform>();

            NetworkAnimator networkAnimator = GetComponent<NetworkAnimator>();
            if(null == networkAnimator.animator) {
                networkAnimator.animator = Animator;
            }

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

            _controller.Initialize(this);
        }

        public abstract void OnSpawn();

#region Event Handlers
        private void PauseEventHandler(object sender, EventArgs args)
        {
            Animator.enabled = !PartyParrotManager.Instance.IsPaused;
        }
#endregion
    }
}
