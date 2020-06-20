using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public float MaxPickUpDistance = 3;

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.R))
            {

                if (!isHolding)
                {

                    var raycastStart = Entity.Transform.Parent.WorldMatrix.TranslationVector;
                    var forward = Entity.Transform.Parent.WorldMatrix.Forward;
                    var raycastEnd = raycastStart + forward * MaxPickUpDistance;

                    var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);


                    if (result.Succeeded && result.Collider != null)
                    {
                        if (result.Collider is RigidbodyComponent rigidBody)
                        {

                            target = Entity.Scene.Entities.First(e => e.Get<RigidbodyComponent>() == rigidBody);
                            isHolding = true;
                            CreateConstrait();

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
            var rb = Entity.Get<RigidbodyComponent>();
            var trb = target.Get<RigidbodyComponent>();
            trb.LinearFactor = Vector3.One * 0.8f;
            trb.AngularFactor = Vector3.One * 0.01f;

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
