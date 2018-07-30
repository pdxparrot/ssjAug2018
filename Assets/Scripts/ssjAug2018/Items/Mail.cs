﻿using System;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.Core.Util.ObjectPool;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.World;

using UnityEngine;
using UnityEngine.Networking;

namespace pdxpartyparrot.ssjAug2018.Items
{
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(PooledObject))]
    public sealed class Mail : NetworkBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private long _despawnTime;

        private Rigidbody _rigidbody;
        private PooledObject _pooledObject;

        private Player _owner;

#region Unity Lifecycle
        private void Awake()
        {
            NetworkTransform networkTransform = GetComponent<NetworkTransform>();
            networkTransform.transformSyncMode = NetworkTransform.TransformSyncMode.SyncRigidbody3D;
            networkTransform.syncRotationAxis = NetworkTransform.AxisSyncMode.AxisXYZ;

            _rigidbody = GetComponent<Rigidbody>();
            _pooledObject = GetComponent<PooledObject>();

            InitRigidbody();

            _pooledObject.RecycleEvent += RecycleEventHandler;
        }

        private void Update()
        {
            if(_despawnTime > 0 && TimeManager.Instance.CurrentUnixMs >= _despawnTime) {
                Miss();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            Miss();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(null == other.GetComponent<NetworkIdentity>()) {
                return;
            }

            Hit(other.gameObject);
        }
#endregion

        private void InitRigidbody()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.useGravity = ItemManager.Instance.ItemData.ThrownItemsUseGravity;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.detectCollisions = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            _rigidbody.interpolation = RigidbodyInterpolation.None;
        }

        public void Throw(Player owner, Vector3 origin, Vector3 direction, float speed)
        {
            _owner = owner;

            _rigidbody.position = origin;
            _rigidbody.velocity = direction * speed;
            _despawnTime = TimeManager.Instance.CurrentUnixMs + ItemManager.Instance.ItemData.MailDespawnMs;
        }

        [Server]
        private void Hit(GameObject go)
        {
            Mailbox b = go.GetComponent<Mailbox>();
            b?.MailHit();

            GameManager.Instance.Score(_owner);

            _pooledObject.Recycle();
        }

        [Server]
        private void Miss()
        {
            Debug.Log("Mailed missed!");

            _pooledObject.Recycle();
        }

#region Event Handlers
        private void RecycleEventHandler(object sender, EventArgs args)
        {
            _owner = null;
        }
#endregion
    }
}
