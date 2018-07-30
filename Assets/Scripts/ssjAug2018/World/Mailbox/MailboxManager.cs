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

        private readonly HashSet<Mailbox> _mailboxes = new HashSet<Mailbox>();

        private readonly System.Random _random = new System.Random();

        [SerializeField]
        [ReadOnly]
        private int _activeMailboxes;

        // Used for finding valid mailboxes
        private readonly List<Mailbox> _foundBoxes = new List<Mailbox>();

        private Mailbox _seedBox;

        [SerializeField]
        private MailboxData _mailboxData;

        [SerializeField]
        private LayerMask _mailboxLayer;

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
            // Find list of valid seed boxes          
            GetMailboxesInRange(origin, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange);

            // Remove all that have been previously activated unless all found boxes have been previously activated
            if(!_foundBoxes.TrueForAll(x => x.HasActivated)) _foundBoxes.RemoveAll(x => x.HasActivated);

            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            _seedBox = (_foundBoxes.Count == 0) 
                ? _random.GetRandomEntry(_mailboxes) 
                : _random.GetRandomEntry(_foundBoxes);
            if(null == _seedBox) {
                Debug.LogWarning("No seed mailbox found!");
                return;
            }

            // Determine how many boxes we need for the set but don't go over remaining
            int setSize = Mathf.Clamp(_random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax), 0, PlayerManager.Instance.PlayerData.MaxLetters);

            // Activate the seed box and decrement the required box count
            setSize -= SpawnMailbox(_seedBox, setSize);

            // Get boxes in range of the seet for the set
            GetMailboxesInRange(_seedBox.transform.position, _mailboxData.SetMinRange, _mailboxData.SetMaxRange);

            // Select & activate the rest of the required boxes
            Mailbox box = null;
            while(_foundBoxes.Count > 0 && setSize > 0)
            {
                box = _random.GetRandomEntry(_foundBoxes);
                setSize -= SpawnMailbox(box, setSize);
                _foundBoxes.Remove(box);
            }

            // If we still need letters, just shove them all onto the last box.
            if(setSize > 0)
            {
                box?.AddLetters(setSize);
            }
        }

        [Server]
        private int SpawnMailbox(Mailbox mailbox, int maxLetters)
        {
            int letterCount = Mathf.Clamp(_random.Next(1, _mailboxData.MaxLettersPerBox), 1, maxLetters);

            mailbox.ActivateMailbox(letterCount);
            NetworkServer.Spawn(mailbox.gameObject);

            _activeMailboxes++;

            return letterCount;
        }

        private void GetMailboxesInRange(Vector3 origin, float minimum, float maximum)
        {
            float minsquared = minimum * minimum;

            _foundBoxes.Clear();
            Collider[] hits = Physics.OverlapSphere(origin, maximum, _mailboxLayer);
            foreach(Collider hit in hits)
            {
                Mailbox box = hit.gameObject.GetComponent<Mailbox>();
                if((box.transform.position - origin).sqrMagnitude > minsquared) 
                {
                    _foundBoxes.Add(box); 
                }
            }
        }

        [Server]
        public void MailboxCompleted(Mailbox mailbox)
        {
            NetworkServer.UnSpawn(mailbox.gameObject);

            _activeMailboxes--;
            if(_activeMailboxes <= 0)
            {
                ActivateMailboxGroup(_seedBox.transform.position);
            }
        }
    }
}
