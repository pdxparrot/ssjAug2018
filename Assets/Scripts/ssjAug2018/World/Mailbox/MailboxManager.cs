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
        protected System.Random Random;

        private int _maxMailboxes;
        private int _activeMailboxes;
        
        // Used for finding valid mailboxes
        private Collider[] hits;
        private Mailbox _seedBox;

        [SerializeField]
        private MailboxData _mailboxData;

#region Unity Lifecycle

        private void Awake()
        {
            // TODO: Uncomment when mail holding value is added to player data
            _maxMailboxes = /*PlayerData.Instance.MailHoldCount*/ 10;
            Random = new System.Random(_mailboxData.RandomizationSeed);

            // TODO: Update this allocation size when we have a better idea of how many mailboxes are going in the game
            hits = new Collider[50];
        }

#endregion

#region Registration
        public virtual void RegisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Add(mailbox);
            Debug.Log("!!Mailbox added: " + mailbox.name);
        }

        public virtual void UnregisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Remove(mailbox);
        }
#endregion

        
        // TODO: Add weight for seed box based on previous activation count
            
        public void ActivateMailboxGroup(Transform origin)
        {
            // Find list of valid seed boxes          
            List<Mailbox> validSeeds = GetMailboxesInRange(origin, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange);
            
            // Sort potential seeds by times activated
            validSeeds.Sort();
            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            _seedBox = (validSeeds.Count == 0) 
                ? Random.GetRandomEntry<Mailbox>(_mailboxes) 
                : Random.GetRandomEntry<Mailbox>(validSeeds);


            // Determine how many boxes we need for the set but don't go over remaining
            int setSize = Random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax);
            if(setSize > _maxMailboxes) setSize = _maxMailboxes;

            // Activate the seed box and decrement the required box count
            int seedLetterCount = Random.Next(_mailboxData.MaxLettersPerBox);
            seedLetterCount = (seedLetterCount > setSize) ? 1 : seedLetterCount;
            _seedBox.ActivateMailbox(seedLetterCount);
            _activeMailboxes = 1;
            setSize =- seedLetterCount;

            // Get boxes in range of the seet for the set
            List<Mailbox> validBoxes = GetMailboxesInRange(_seedBox.transform, _mailboxData.SetMinRange, _mailboxData.SetMaxRange);

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

        private List<Mailbox> GetMailboxesInRange(Transform origin, float minimum, float maximum)
        {
            List<Mailbox> validBoxes = new List<Mailbox>();
            Physics.OverlapSphereNonAlloc(origin.position, maximum, hits, LayerMask.GetMask("Mailboxes"));
            foreach(Collider box in hits)
            {
                validBoxes.Add(box.gameObject.GetComponent<Mailbox>());
            }

            Physics.OverlapSphereNonAlloc(origin.position, maximum, hits, LayerMask.GetMask("Mailboxes"));
            foreach(Collider box in hits)
            {
                validBoxes.Remove(box.gameObject.GetComponent<Mailbox>());
            }
            return validBoxes;
        }

        public void MailboxCompleted()
        {
            _activeMailboxes--;
            if(_activeMailboxes <= 0)
            {
                ActivateMailboxGroup(_seedBox.transform);
            }
        }
    }
}