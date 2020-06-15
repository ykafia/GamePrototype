using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace FPSgame
{
    public class MovePhysicsHandle : SyncScript
    {

        public Entity target;
        public override void Start()
        {
            var rb = Entity.Get<RigidbodyComponent>();
            var trb = target.Get<RigidbodyComponent>();
            rb.LinearFactor = Vector3.Zero;
            rb.AngularFactor = Vector3.Zero;
            trb.LinearFactor = Vector3.One * 2;
            trb.AngularFactor = Vector3.One * 0.1f;

            var currentConstraint = Simulation.CreateConstraint(ConstraintTypes.ConeTwist, rb, trb,
                Matrix.Identity, Matrix.Translation(new Vector3(0, 0.1f, 0)));
            this.GetSimulation().AddConstraint(currentConstraint);

            var coneTwist = (ConeTwistConstraint)currentConstraint;
            coneTwist.SetLimit(0.5f, 0.5f, 0.5f);
        }

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {
           if(Input.IsKeyDown(Keys.Up))
           {
               Entity.Transform.Position -= Vector3.UnitZ * 2.0f * (float) Game.UpdateTime.Elapsed.TotalSeconds;
           }
           if(Input.IsKeyDown(Keys.Right))
           {
               Entity.Transform.Position += Vector3.UnitX * 2.0f * (float) Game.UpdateTime.Elapsed.TotalSeconds;
           }
           if(Input.IsKeyDown(Keys.Left))
           {
               Entity.Transform.Position -= Vector3.UnitX * 2.0f * (float) Game.UpdateTime.Elapsed.TotalSeconds;
           }
           if(Input.IsKeyDown(Keys.Down))
           {
               Entity.Transform.Position += Vector3.UnitZ * 2.0f * (float) Game.UpdateTime.Elapsed.TotalSeconds;
           }
        }
    }
}
