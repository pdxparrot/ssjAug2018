using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Players;
using pdxpartyparrot.ssjAug2018.Data;

using System.Collections.Generic;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    public class MailboxManager : SingletonBehavior<MailboxManager>
    {
        private readonly HashSet<Mailbox> _mailboxes = new HashSet<Mailbox>();
        protected static readonly System.Random Random = new System.Random();

        // TODO: Fix this. Everything's coming up null when I try to find the player.
        private PlayerData _playerData = PlayerManager.Instance.PlayerData;

        private int _maxMailboxes;
        private int _activeMailboxes;

        [SerializeField]
        private MailboxData _mailboxData;

#region Unity Lifecycle

        private void Awake()
        {
            // TODO: Uncomment when mail holding value is added to player data
            _maxMailboxes = /*_playerData.MailHoldCount*/ 10;

            // TODO: Determine if we just want objectives spawned at the start of game or delayed
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

        
        // TODO: Add weight for seed box based on previous activation count
            
        public void ActivateMailboxGroup(Player player)
        {
            // Find list of valid seed boxes          
            HashSet<Mailbox> validSeeds = new HashSet<Mailbox>();
            Collider[] allSeeds = Physics.OverlapSphere(player.transform.position, _mailboxData.PlayerMaxRange, LayerMask.GetMask("Mailboxes"));
            Collider[] ignoreSeeds = Physics.OverlapSphere(player.transform.position, _mailboxData.PlayerMinRange, LayerMask.GetMask("Mailboxes"));
            foreach(Collider box in allSeeds)
            {
                validSeeds.Add(box.gameObject.GetComponent<Mailbox>());
            }
            foreach(Collider box in ignoreSeeds)
            {
                validSeeds.Remove(box.gameObject.GetComponent<Mailbox>());
            }
            

            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            Mailbox seedBox = (validSeeds.Count == 0) 
                ? Random.GetRandomEntry<Mailbox>(_mailboxes) 
                : Random.GetRandomEntry<Mailbox>(validSeeds);

            // Get valid boxes for the set
            HashSet<Mailbox> validBoxes = new HashSet<Mailbox>();
            Collider[] allBoxes = Physics.OverlapSphere(player.transform.position, _mailboxData.PlayerMaxRange, LayerMask.GetMask("Mailboxes"));
            Collider[] ignoreBoxes = Physics.OverlapSphere(player.transform.position, _mailboxData.PlayerMinRange, LayerMask.GetMask("Mailboxes"));
            foreach(Collider box in allBoxes)
            {
                validBoxes.Add(box.gameObject.GetComponent<Mailbox>());
            }
            foreach(Collider box in ignoreBoxes)
            {
                validBoxes.Remove(box.gameObject.GetComponent<Mailbox>());
            }

            // Determine how many boxes we need for the set
            int setSize = Random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax);
            if(setSize > _maxMailboxes)
            {
                setSize = _maxMailboxes;
            }

            // Activate the seed box and decrement the required box count
            int seedLetterCount = Random.Next(_mailboxData.MaxLettersPerBox);
            seedLetterCount = (seedLetterCount > setSize) ? 1 : seedLetterCount;
            seedBox.ActivateMailbox(seedLetterCount);
            _activeMailboxes = 1;
            setSize =- seedLetterCount;

            // Select & activate the rest of the required boxes
            while(setSize > 0)
            {
                // TODO: Update this quick and dirty NPE check. Should probably either shove all remaining letters into the last box or choose a new box at random from all boxes
                if(validBoxes.Count == 0) { break; }

                Mailbox box = Random.GetRandomEntry<Mailbox>(validBoxes);
                int letterCount = Random.Next(_mailboxData.MaxLettersPerBox);
                letterCount = (letterCount > setSize) ? setSize : letterCount;
                
                box.ActivateMailbox(letterCount);
                _activeMailboxes++;
                validBoxes.Remove(box);
                setSize =- letterCount;
            }
        }

        public void MailboxCompleted()
        {
            _activeMailboxes--;
            if(_activeMailboxes <= 0)
            {
                ActivateMailboxGroup(FindObjectOfType<Player>());
            }
        }
    }
}