﻿using pdxpartyparrot.Core.Util;

using UnityEngine;

namespace pdxpartyparrot.Game.Actors.ControllerComponents
{
    public sealed class AimControllerComponent : CharacterActorControllerComponent
    {
        public class AimAction : CharacterActorControllerAction
        {
            public static AimAction Default = new AimAction();
        }

        [SerializeField]
        [ReadOnly]
        private bool _isAiming;

        public bool IsAiming => _isAiming;

        public override bool OnAnimationMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            return true;
        }

        public override bool OnPhysicsMove(Vector3 axes, float dt)
        {
            if(!IsAiming) {
                return false;
            }

            // TODO: this is bullshit, but until we get slopes fixed in CharacterActorController
            // we need this in order to not slide down stuff
            Vector3 velocity = Controller.Rigidbody.velocity;
            velocity.x = velocity.z = 0.0f;
            Controller.Rigidbody.velocity = velocity;

            return true;
        }

        public override bool OnStarted(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = true;

            Debug.Log("TODO: zoom and aim!");

            return true;
        }

        public override bool OnCancelled(CharacterActorControllerAction action)
        {
            if(!(action is AimAction)) {
                return false;
            }

            _isAiming = false;

            return true;
        }
    }
}
