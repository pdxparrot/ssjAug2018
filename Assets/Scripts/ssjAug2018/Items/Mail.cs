using System;

using pdxpartyparrot.Core.Audio;
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
        private Timer _despawnTimer;

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
            if(!NetworkServer.active) {
                return;
            }

            float dt = Time.deltaTime;

            _despawnTimer.Update(dt);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log($"Mail collision: {collision.gameObject.name}");

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

        [Server]
        public void Throw(Player owner, Vector3 origin, Vector3 velocity)
        {
            _owner = owner;

            _rigidbody.position = origin;
            _rigidbody.velocity = velocity;

            _despawnTimer.Start(ItemManager.Instance.ItemData.MailDespawnSeconds, Miss);

            AudioManager.Instance.PlayOneShot(ItemManager.Instance.ItemData.ThrowMailAudio);
        }

        [Server]
        private void Hit(GameObject go)
        {
            Mailbox b = go.GetComponent<Mailbox>();
            if(null != b && b.MailHit(_owner)) {
                _despawnTimer.Stop();
                _pooledObject.Recycle();
            }
        }

        [Server]
        private void Miss()
        {
            Debug.Log("Mailed missed!");

            _despawnTimer.Stop();
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
