using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;

namespace FPSgame.Chemistry
{
    public class ElectricConductionScript : SyncScript
    {
        public readonly ElementType ElementalType = ElementType.Metal;
        public Current Conduct = Current.OFF;
        private RigidbodyComponent rb;

        public EventKey<Current> currentChange = new EventKey<Current>();

        public override void Start()
        {
            rb = Entity.Get<RigidbodyComponent>();
        }
        public override void Update(){}
        public void SwitchCurrentOn()
        {
            Conduct = Current.ON;

            rb
                .Collisions
                .AsParallel()
                .Select(c => c.ColliderB)
                .Select(cb => 
                    
                        Entity
                            .Scene
                            .Entities
                            .First(i => i.Get<ElectricConductionScript>() != null && i.Get<RigidbodyComponent>() != null && i.Get<RigidbodyComponent>() == cb)
                    
                ).ForAll(e => e.Get<ElectricConductionScript>().SwitchCurrentOn());
        
        }
    }
}