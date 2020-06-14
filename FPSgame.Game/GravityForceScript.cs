using System;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Input;
using Stride.Physics;
using Stride.Rendering.Sprites;
using System.Linq;

namespace FPSgame.Player
{
    public class GravityForceScript : SyncScript
    {
        public Force ForceDirection = Force.Push;
        public float MaxPullDistance { get; set; } = 20f;

        public float PullImpulse { get; set; } = 50f;

        public float Cooldown { get; set; } = 1.0f;
        private float cooldownRemaining = 0;

        public float PullDuration = 5.0f;

        private float pullRemainingDuration = 0;

        
        private Entity onHold;

        /// <summary>
        /// Called on every frame update
        /// </summary>
        public override void Update()
        {
            var camForward = Entity.Transform.WorldMatrix.Forward * 2;
            var holdPosition = Entity.GetParent().Transform.Position + Vector3.UnitY*3 + camForward;
            if(Input.IsKeyPressed(Keys.E)) {
                
                if(cooldownRemaining == 0)
                {    
                    pullRemainingDuration = PullDuration;
                    cooldownRemaining = Cooldown;

                    var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
                    var forward = Entity.Transform.WorldMatrix.Forward;
                    var raycastEnd = raycastStart + forward * MaxPullDistance;

                    var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);

                    var weaponFired = new WeaponFiredResult { HitResult = result, DidFire = true, DidHit = false };

                    if (result.Succeeded && result.Collider != null)
                    {
                        weaponFired.DidHit = true;

                        if (result.Collider is RigidbodyComponent rigidBody)
                        {
                            onHold = Entity.Scene.Entities.First(e => e.Get<RigidbodyComponent>() == rigidBody);
                        }
                    }
                }
            }
            if(pullRemainingDuration>0)
            {
                var direction = (holdPosition - onHold.Transform.Position) * (float)ForceDirection;
                direction.Normalize();
                onHold?.Get<RigidbodyComponent>()?.ApplyImpulse(direction * (float) Game.UpdateTime.Elapsed.TotalSeconds * PullImpulse);
                pullRemainingDuration = (pullRemainingDuration - (float) Game.UpdateTime.Elapsed.TotalSeconds >= 0) ? pullRemainingDuration - (float) Game.UpdateTime.Elapsed.TotalSeconds : 0;
            }
            if(pullRemainingDuration==0 && cooldownRemaining > 0)
            {
                cooldownRemaining =  (cooldownRemaining - (float) Game.UpdateTime.Elapsed.TotalSeconds >= 0) ? cooldownRemaining - (float) Game.UpdateTime.Elapsed.TotalSeconds : 0;
            }


        }
    }
}