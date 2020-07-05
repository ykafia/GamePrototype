using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Games;
using Stride.Physics;

namespace FPSgame.Chemistry.Electricity
{
    public class ElectricConductionScript : SyncScript
    {
        public readonly ElementType ElementalType = ElementType.Metal;
        public Current Conduct = Current.OFF;

        private RigidbodyComponent rb;

        public static EventKey currentChange = new EventKey("Electricity","currentChange");

        private int collisionNumber = 0;
        public override void Start()
        {
            try
            {
                rb = Entity.Get<RigidbodyComponent>();
                collisionNumber = CountCollision();
            }
            catch(Exception)
            {
                throw new Exception("A conductive object must have a Rigidbody component");
            }
        }

        public override void Update()
        {
            if(Conduct == Current.ON)
                DebugText.Print(Entity.Name + " has " + collisionNumber + "collisions", new Int2(900, 25 * (int)Entity.Name.Length));
            
                
            int temp = CountCollision();
            if(collisionNumber != temp)
            {
                currentChange.Broadcast();
                collisionNumber = temp;
            }
        }

        public int CountCollision()
        {
            return
                rb
                .Collisions
                .Select(c => c.ColliderB)
                .Select(r => Entity.Scene.Entities.FirstOrDefault(
                    e => e.Get<RigidbodyComponent>() != null && e.Get<RigidbodyComponent>() == r && e.Get<ElectricConductionScript>() != null)
                )
                .Where(x => x != null)
                .Count();
        }

        public void SwitchCurrent()
        {
            switch (Conduct)
            {
                case Current.ON:
                    Conduct = Current.OFF;
                    currentChange.Broadcast();
                    break;
                case Current.OFF:
                    currentChange.Broadcast();
                    Conduct = Current.ON;
                    break;
                default:
                    break;
            }

        }
    }
}