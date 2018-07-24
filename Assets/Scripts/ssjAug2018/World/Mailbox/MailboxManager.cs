using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;

using System.Collections.Generic;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    public class MailboxManager : SingletonBehavior<MailboxManager>
    {
        private readonly HashSet<Mailbox> _mailboxes = new HashSet<Mailbox>();

        [SerializeField]
        private int _maxMailboxes;

#region Unity Lifecycle

        private void Awake()
        {
            // TODO: Uncomment when mail holding value is added to player data
            //_maxMailboxes = PlayerManager.Instance.PlayerData.;
            // Also might not be a bad idea to figure out a better way to do this. I'm not sure Awake is the proper
            // call to make. Probably something that might not run into a race condition if PlayerManager hasn't been
            // initialized before this one is.
        }

#endregion

#region Registration
        public virtual void RegisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Add(mailbox);
        }

        public virtual void UnregisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Remove(mailbox);
        }
#endregion

        public void ActivateMailboxGroup()
        {
            
            // TODO: This should be called when all active objectives have been met, but more are needed to satisfy game win state
            // Find seed mailbox, get aplicable number in range, distrubute mail total between them?
        }
    }
}