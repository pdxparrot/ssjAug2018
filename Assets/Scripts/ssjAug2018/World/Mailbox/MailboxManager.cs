using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Data;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Profiling;

namespace pdxpartyparrot.ssjAug2018.World
{
    [RequireComponent(typeof(NetworkIdentity))]
    public sealed class MailboxManager : NetworkSingletonBehavior
    {
#region NetworkSingleton
        public static MailboxManager Instance { get; private set; }

        public static bool HasInstance => null != Instance;
#endregion

        private static List<Mailbox> GetValidMailboxesInRange(Vector3 origin, float minimum, float maximum, LayerMask layer)
        {
            float minsquared = minimum * minimum;

            List<Mailbox> found = new List<Mailbox>();

            var hits = Physics.OverlapSphere(origin, maximum, layer);
            foreach(Collider hit in hits) {
                Mailbox box = hit.gameObject.GetComponent<Mailbox>();
                if((box.transform.position - origin).sqrMagnitude >= minsquared && !box.HasActivated) {
                    found.Add(box);
                }
            }

            return found;
        }

        [SerializeField]
        private MailboxData _mailboxData;

        [SerializeField]
        private LayerMask _mailboxLayer;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentSetSize;

        public int CurrentSetSize => _currentSetSize;

        private readonly HashSet<Mailbox> _mailboxes = new HashSet<Mailbox>();

        private readonly HashSet<Mailbox> _activeMailboxes = new HashSet<Mailbox>();

        private readonly List<Mailbox> _previousActiveMailboxes = new List<Mailbox>();

        private Mailbox _seedBox;

        public int CompletedMailboxes => CurrentSetSize - _activeMailboxes.Count;

        private readonly System.Random _random = new System.Random();

#region Unity Lifecycle
        private void Awake()
        {
            if(HasInstance) {
                Debug.LogError($"[NetworkSingleton] Instance already created: {Instance.gameObject.name}");
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private void OnDrawGizmos()
        {
            if(null != _seedBox) {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(_seedBox.transform.position, _mailboxData.SetMinRange);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_seedBox.transform.position, _mailboxData.SetMaxRange);
            }
        }
#endregion

#region Registration
        public void RegisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Add(mailbox);
        }

        public void UnregisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Remove(mailbox);
        }
#endregion

        public void Initialize()
        {
            ActivateMailboxGroup();
        }

        [Server]
        public void ActivateMailboxGroup()
        {
            if(_mailboxes.Count < 1) {
                Debug.LogWarning("No mailboxes found!");
                return;
            }

            Profiler.BeginSample("MailboxManager.ActivateMailboxGroup");
            try {
                _previousActiveMailboxes.ForEach(x => x.Reset());
                _previousActiveMailboxes.Clear();

                // pick a random seed if we don't have one yet
                _seedBox = _random.GetRandomEntry(_mailboxes);

                // Activate the seed box and decrement the required box count
                SpawnMailbox(_seedBox);

                // Get boxes in range of the seet for the set
                List<Mailbox> foundBoxes = GetValidMailboxesInRange(_seedBox.transform.position, _mailboxData.SetMinRange, _mailboxData.SetMaxRange, _mailboxLayer);

                // Select & activate the rest of the required boxes
                int setSize = _random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax);     // NOTE: not SetCountMax - 1 because we spawend the seed already
                while(setSize > 0 && foundBoxes.Count > 0) {
                    Mailbox box = _random.RemoveRandomEntry(foundBoxes);
                    SpawnMailbox(box);
                    setSize--;
                }

                _previousActiveMailboxes.AddRange(_activeMailboxes);
                _currentSetSize = _activeMailboxes.Count;
            } finally {
                Profiler.EndSample();
            }
        }

        [Server]
        private void SpawnMailbox(Mailbox mailbox)
        {
            int letterCount = _random.Next(1, _mailboxData.MaxLettersPerBox + 1);

            mailbox.ActivateMailbox(letterCount);
            NetworkServer.Spawn(mailbox.gameObject);

            _activeMailboxes.Add(mailbox);
        }

        [Server]
        public void MailboxCompleted(Mailbox mailbox)
        {
            NetworkServer.UnSpawn(mailbox.gameObject);

            _activeMailboxes.Remove(mailbox);

            if(_activeMailboxes.Count <= 0) {
                ActivateMailboxGroup();
            }
        }
    }
}
