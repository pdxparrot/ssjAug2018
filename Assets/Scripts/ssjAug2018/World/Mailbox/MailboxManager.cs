using System;
using pdxpartyparrot.Core.Util;
using pdxpartyparrot.ssjAug2018.Data;

using System.Collections.Generic;
using UnityEngine;

namespace pdxpartyparrot.ssjAug2018.World
{
    public sealed class MailboxManager : SingletonBehavior<MailboxManager>
    {
        private readonly HashSet<Mailbox> _mailboxes = new HashSet<Mailbox>();
        private System.Random Random;

        private int _maxMailboxes;
        private int _activeMailboxes;
        
        // Used for finding valid mailboxes
        private List<Mailbox> _foundBoxes = new List<Mailbox>();
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
        public void RegisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Add(mailbox);
        }

        public void UnregisterMailbox(Mailbox mailbox)
        {
            _mailboxes.Remove(mailbox);
        }
#endregion

        public void InitilizeGameReady(object sender, EventArgs e)
        {
            ActivateMailboxGroup(Players.PlayerManager.Instance.transform);
        }


        // TODO: Add weight for seed box based on previous activation count
            
        public void ActivateMailboxGroup(Transform origin)
        {
            // Find list of valid seed boxes          
            GetMailboxesInRange(origin, _mailboxData.DistanceMinRange, _mailboxData.DistanceMaxRange);
            // Remove all that have been previously activated unless all found boxes have been previously activated
            if(!_foundBoxes.TrueForAll(Mailbox.PreviouslyActivated)) _foundBoxes.RemoveAll(Mailbox.PreviouslyActivated);
            // Choose seed box and continue activation. If there are no seeds in range, use a random box
            _seedBox = (_foundBoxes.Count == 0) 
                ? Random.GetRandomEntry(_mailboxes) 
                : Random.GetRandomEntry(_foundBoxes);
            if(null == _seedBox) {
                Debug.LogWarning("No seed mailbox found!");
                return;
            }

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
                Mailbox box = Random.GetRandomEntry<Mailbox>(_foundBoxes);
                letterCount = Random.Next(1, _mailboxData.MaxLettersPerBox);
                letterCount = (letterCount > setSize) ? setSize : letterCount;
                
                box.ActivateMailbox(letterCount);
                _activeMailboxes++;
                _foundBoxes.Remove(box);
                setSize -= letterCount;

                // If this is the last box, but we still need letters, just shove them all onto that box.
                if(_foundBoxes.Count == 0 && setSize > 0)
                {
                    letterCount += setSize;
                    box.ActivateMailbox(letterCount);
                    setSize = 0;
                }
            }
        }

        private void GetMailboxesInRange(Transform origin, float minimum, float maximum)
        {
            _foundBoxes.Clear();
            Collider[] hits = Physics.OverlapSphere(origin.position, maximum, LayerMask.GetMask("Mailboxes"));
            foreach(Collider hit in hits)
            {
                Mailbox box = hit.gameObject.GetComponent<Mailbox>();
                if((box.transform.position - origin.position).sqrMagnitude > minimum * minimum) 
                    { 
                    _foundBoxes.Add(box); 
                    }
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
