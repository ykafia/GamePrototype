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
    public class InitPhysics : StartupScript
    {
        // Declared public member fields and properties will show in the game studio
        public float FrictionForAll = 1;
        public override void Start()
        {
            Entity.Scene
                .Entities
                .Where(e => e.Get<RigidbodyComponent>() != null)
                .AsParallel()
                .ForAll(e => e.Get<RigidbodyComponent>().Friction = FrictionForAll);
        }
    }
}
