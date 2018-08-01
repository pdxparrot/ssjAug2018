using System.Collections.Generic;

using JetBrains.Annotations;

using pdxpartyparrot.Core.DebugMenu;
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

        [SerializeField]
        private MailboxData _mailboxData;

        [SerializeField]
        private LayerMask _mailboxLayer;

        [SerializeField]
        [ReadOnly]
        private Vector3 _seedPosition;

        [SerializeField]
        [ReadOnly]
        [CanBeNull]
        private Mailbox _seedBox;

        [SerializeField]
        [ReadOnly]
        [SyncVar]
        private int _currentSetSize;

        public int CurrentSetSize => _currentSetSize;

        private readonly List<Mailbox> _mailboxes = new List<Mailbox>();

        private readonly HashSet<Mailbox> _activeMailboxes = new HashSet<Mailbox>();

        private readonly List<Mailbox> _previousActiveMailboxes = new List<Mailbox>();

        private readonly List<Mailbox> _suitableMailboxes = new List<Mailbox>();

        public int CompletedMailboxes => CurrentSetSize - _activeMailboxes.Count;

        private readonly System.Random _random = new System.Random();

        private DebugMenuNode _debugMenuNode;

#region Unity Lifecycle
        private void Awake()
        {
            if(HasInstance) {
                Debug.LogError($"[NetworkSingleton] Instance already created: {Instance.gameObject.name}");
                return;
            }

            Instance = this;

            InitDebugMenu();
        }

        private void OnDestroy()
        {
            DestroyDebugMenu();

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

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(_seedPosition, _mailboxData.DistanceMinRange);

            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(_seedPosition, _mailboxData.DistanceMaxRange);
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
            Mailbox mailbox = _random.GetRandomEntry(_mailboxes);
            ActivateMailboxGroup(mailbox?.transform.position ?? Vector3.zero);
        }

        [Server]
        public void ActivateMailboxGroup(Vector3 seedPosition)
        {
            _seedPosition = seedPosition;
            Debug.Log($"Seeding mailboxes at {_seedPosition}");

            Profiler.BeginSample("MailboxManager.ActivateMailboxGroup");
            try {
                _previousActiveMailboxes.ForEach(x => x.Reset());
                _previousActiveMailboxes.Clear();

                // get the seed mailbox
                _seedBox = GetSeedMailbox(_seedPosition);
                if(null == _seedBox) {
                    Debug.LogWarning("No seed mailbox found!");
                    return;
                }

                // Activate the seed box
                SpawnMailbox(_seedBox);

                // Get boxes in range of the seed for the set
                List<Mailbox> foundBoxes = GetValidMailboxesInRange(_seedBox.transform.position, _mailboxData.SetMinRange, _mailboxData.SetMaxRange);

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

        private Mailbox GetSeedMailbox(Vector3 seedPosition)
        {
            List<Mailbox> foundBoxes = GetValidMailboxesInRange(seedPosition, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange);

            if(foundBoxes.Count < 1) {
                Debug.LogWarning("Using random mailbox seed, consider resizing the seed range!");
                return _random.GetRandomEntry(_mailboxes);
            }
            return _random.GetRandomEntry(foundBoxes);
        }

        private List<Mailbox> GetValidMailboxesInRange(Vector3 origin, float minimum, float maximum)
        {
            float minSquared = minimum * minimum;
            float maxSquared = maximum * maximum;

            _suitableMailboxes.Clear();

            foreach(Mailbox mailbox in _mailboxes) {
                float distanceSquared = (mailbox.transform.position - origin).sqrMagnitude;
                if(distanceSquared < minSquared || distanceSquared > maxSquared) {
                    continue;
                }
                _suitableMailboxes.Add(mailbox);
            }

            return _suitableMailboxes;
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
                ActivateMailboxGroup(mailbox.transform.position);
            }
        }

        [Command]
        private void CmdCompleteAllMailboxes()
        {
            List<Mailbox> temp = new List<Mailbox>();
            temp.AddRange(_activeMailboxes);

            foreach(Mailbox mailbox in temp) {
                mailbox.ForceComplete();
            }
        }

        private void InitDebugMenu()
        {
            _debugMenuNode = DebugMenuManager.Instance.AddNode(() => "ssjAug2018.MailboxManager");
            _debugMenuNode.RenderContentsAction = () => {
                if(GUILayout.Button("Force Complete")) {
                    CmdCompleteAllMailboxes();
                }

                GUILayout.Label($"Current seed position: {_seedPosition}");
                if(null != _seedBox) {
                    GUILayout.Label($"Current seed mailbox: {_seedBox.name} {_seedBox.transform.position}");
                }
                GUILayout.Label($"Current set size: {CurrentSetSize}");
                GUILayout.BeginVertical("Active mailboxes", GUI.skin.box);
                    foreach(Mailbox mailbox in _activeMailboxes) {
                        GUILayout.Label($"{mailbox.name} {mailbox.transform.position}");
                    }
                GUILayout.EndVertical();
                GUILayout.Label($"Completed mailboxes: {CompletedMailboxes}");
            };
        }

        private void DestroyDebugMenu()
        {
            if(DebugMenuManager.HasInstance) {
                DebugMenuManager.Instance.RemoveNode(_debugMenuNode);
            }
            _debugMenuNode = null;
        }
    }
}
