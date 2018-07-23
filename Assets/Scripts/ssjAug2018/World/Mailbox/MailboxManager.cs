using pdxpartyparrot.Core.Actors;

using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    public class MailboxManager : ActorManager<Mailbox>
    {
        public new static MailboxManager Instance => (MailboxManager)ActorManager<Mailbox>.Instance;
        

    }
}