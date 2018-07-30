using System.Collections.Generic;

using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Data;
using pdxpartyparrot.ssjAug2018.Players;

using UnityEngine;
using UnityEngine.Networking;

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
                if((box.transform.position - origin).sqrMagnitude > minsquared && !box.HasActivated) {
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

        public void Initialize(Vector3 origin)
        {
            ActivateMailboxGroup(origin);
        }

        [Server]
        public void ActivateMailboxGroup(Vector3 origin)
        {
            _previousActiveMailboxes.ForEach(x => x.Reset());
            _previousActiveMailboxes.Clear();

            // Find list of valid seed boxes
            List<Mailbox> foundBoxes = GetValidMailboxesInRange(origin, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange, _mailboxLayer);

            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            Mailbox seedBox = foundBoxes.Count == 0 ? _random.GetRandomEntry(_mailboxes) : _random.GetRandomEntry(foundBoxes);
            if(null == seedBox) {
                Debug.LogWarning("No seed mailbox found!");
                return;
            }

            // Determine how many boxes we need for the set but don't go over remaining
            int lettersRemaining = Mathf.Clamp(_random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax), 0, PlayerManager.Instance.PlayerData.MaxLetters);

            // Activate the seed box and decrement the required box count
            lettersRemaining -= SpawnMailbox(seedBox, lettersRemaining);

            // Get boxes in range of the seet for the set
            foundBoxes = GetValidMailboxesInRange(seedBox.transform.position, _mailboxData.SetMinRange, _mailboxData.SetMaxRange, _mailboxLayer);

            // Select & activate the rest of the required boxes
            Mailbox box = null;
            while(foundBoxes.Count > 0 && lettersRemaining > 0) {
                box = _random.GetRandomEntry(foundBoxes);
                lettersRemaining -= SpawnMailbox(box, lettersRemaining);
                foundBoxes.Remove(box);
            }

            // If we still need letters, just shove them all onto the last box.
            if(lettersRemaining > 0) {
                box?.AddLetters(lettersRemaining);
            }

            _previousActiveMailboxes.AddRange(_activeMailboxes);
            _currentSetSize = _activeMailboxes.Count;
        }

        [Server]
        private int SpawnMailbox(Mailbox mailbox, int maxLetters)
        {
            int letterCount = Mathf.Clamp(_random.Next(1, _mailboxData.MaxLettersPerBox), 1, maxLetters);

            mailbox.ActivateMailbox(letterCount);
            NetworkServer.Spawn(mailbox.gameObject);

            _activeMailboxes.Add(mailbox);

            return letterCount;
        }

        [Server]
        public void MailboxCompleted(Mailbox mailbox)
        {
            NetworkServer.UnSpawn(mailbox.gameObject);

            _activeMailboxes.Remove(mailbox);

            if(_activeMailboxes.Count <= 0) {
                ActivateMailboxGroup(mailbox.transform.position);
            }
        }
    }
}
