using System;
using pdxpartyparrot.Core.Util;
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
        private List<Mailbox> _foundBoxes;
        private Mailbox _seedBox;

        [SerializeField]
        private MailboxData _mailboxData;

#region Unity Lifecycle
        private void Awake()
        {
             GameManager.Instance.GameReadyEvent += InitilizeGameReady;
            // TODO: Uncomment when mail holding value is added to player data
            _maxMailboxes = /*PlayerData.Instance.MailHoldCount*/ 10;
            Random = new System.Random(_mailboxData.RandomizationSeed);
        }

        protected override void OnDestroy()
        {
            if(GameManager.HasInstance) 
            {
                GameManager.Instance.GameReadyEvent -= InitilizeGameReady;
            }

            base.OnDestroy();
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

        public void InitilizeGameReady(object sender, EventArgs e)
        {
            hits = new Collider[_mailboxes.Count];
            _foundBoxes = new List<Mailbox>(_mailboxes.Count);
            ActivateMailboxGroup(Players.PlayerManager.Instance.transform);
        }


        // TODO: Add weight for seed box based on previous activation count
            
        public void ActivateMailboxGroup(Transform origin)
        {
            
            // Find list of valid seed boxes          
            GetMailboxesInRange(origin, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange);

            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            _seedBox = (_foundBoxes.Count == 0) 
                ? Random.GetRandomEntry<Mailbox>(_mailboxes) 
                : Random.GetRandomEntry<Mailbox>(_foundBoxes);


            // Determine how many boxes we need for the set but don't go over remaining
            int setSize = Random.Next(_mailboxData.SetCountMin, _mailboxData.SetCountMax);
            if(setSize > _maxMailboxes) setSize = _maxMailboxes;

            // Activate the seed box and decrement the required box count
            int letterCount = Random.Next(1, _mailboxData.MaxLettersPerBox);
            letterCount = (letterCount > setSize) ? 1 : letterCount;
            _seedBox.ActivateMailbox(letterCount);
            _activeMailboxes = 1;
            setSize -= letterCount;

            // Get boxes in range of the seet for the set
            GetMailboxesInRange(_seedBox.transform, _mailboxData.SetMinRange, _mailboxData.SetMaxRange);

            // Select & activate the rest of the required boxes
            while(setSize > 0)
            {
                // TODO: Update this quick and dirty NPE check. Should probably either shove all remaining letters into the last box or choose a new box at random from all boxes
                if(_foundBoxes.Count == 0) { break; }

                Mailbox box = Random.GetRandomEntry<Mailbox>(_foundBoxes);
                letterCount = Random.Next(1, _mailboxData.MaxLettersPerBox);
                letterCount = (letterCount > setSize) ? setSize : letterCount;
                
                box.ActivateMailbox(letterCount);
                _activeMailboxes++;
                _foundBoxes.Remove(box);
                setSize -= letterCount;
            }
        }

        private void GetMailboxesInRange(Transform origin, float minimum, float maximum)
        {
            _foundBoxes.Clear();
            int totalHits;
            totalHits = Physics.OverlapSphereNonAlloc(origin.position, maximum, hits, LayerMask.GetMask("Mailboxes"));
            for(int i = 0 ; i < totalHits ; i++)
            {
                _foundBoxes.Add(hits[i].gameObject.GetComponent<Mailbox>());
            }

            totalHits = Physics.OverlapSphereNonAlloc(origin.position, minimum, hits, LayerMask.GetMask("Mailboxes"));
            for(int i = 0 ; i < totalHits ; i++)
            {
                _foundBoxes.Remove(hits[i].gameObject.GetComponent<Mailbox>());
            }
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