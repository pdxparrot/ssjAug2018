using UnityEngine;

namespace pdxpartyparrot.Core.Actors
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class ActorController : MonoBehaviour
    {
        [SerializeField]
        private ActorDriver _driver;

        public ActorDriver Driver => _driver;

        public Rigidbody Rigidbody { get; private set; }

        protected IActor Owner { get; private set; }

#region Unity Lifecycle
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
#endregion

        public virtual void Initialize(IActor owner)
        {
            Owner = owner;

            _driver.Initialize(owner, this);
        }

        public void MoveTo(Vector3 position)
        {
            Debug.Log($"Teleporting actor {Owner.Id} to {position}");
            Rigidbody.position = position;
        }
    }
}
