using System;
using System.Linq;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace FPSgame.Player.Gameplay
{
    public class MovePhysicsHandle : SyncScript
    {

        private Entity target;
        private Constraint currentConstraint;

        private bool isHolding = false;

        public float MaxPickUpDistance = 4;

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {

            if (Input.IsKeyPressed(Keys.R))
            {

                if (!isHolding)
                {
                    var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
                    var forward = Entity.Transform.WorldMatrix.Forward;
                    var raycastEnd = raycastStart + forward * MaxPickUpDistance;

                    var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);


                    if (result.Succeeded && result.Collider != null)
                    {
                        if (result.Collider is RigidbodyComponent rigidBody)
                        {

                            target = Entity.Scene.Entities.FirstOrDefault(e => e.Get<RigidbodyComponent>() == rigidBody);
                            if(target != null)
                            {
                                isHolding = true;
                                CreateConstrait();
                            }
                        }
                    }
                }
                else
                {
                    RemoveConstrait();
                    target = null;
                    isHolding = false;
                }
            }
        }


        public void CreateConstrait()
        {
            var handle = Entity.FindChild("Lever");
            var rb = handle.Get<RigidbodyComponent>();
            var trb = target.Get<RigidbodyComponent>();
            trb.LinearFactor = Vector3.One * 0.8f;
            trb.AngularFactor = Vector3.Zero;

            currentConstraint = 
                Simulation.CreateConstraint(
                    ConstraintTypes.ConeTwist, 
                    rb, 
                    trb,
                    Matrix.Identity, 
                    Matrix.Identity);
            this.GetSimulation().AddConstraint(currentConstraint);

            var coneTwist = (ConeTwistConstraint)currentConstraint;
            coneTwist.SetLimit(0.5f, 0.5f, 0.5f);
        }

        public void RemoveConstrait()
        {
            var trb = target.Get<RigidbodyComponent>();
            trb.LinearFactor = Vector3.One;
            trb.AngularFactor = Vector3.One;
            this.GetSimulation().RemoveConstraint(currentConstraint);
        }
    }
}
