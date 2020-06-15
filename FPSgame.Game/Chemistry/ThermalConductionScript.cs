using System;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using Stride.Input;
using System.Linq;

namespace FPSgame.Chemistry
{
    public class ThermalConductionScript : SyncScript
    {
        public float ThermalConductivity { get; set; } = 1.0f;
        public float Temperature { get; set; } = 25f;

        private float cooldown = ThermalSimulation.timestep;
        public override void Update()
        {
            if (cooldown > 0)
                cooldown = (cooldown - (float)Game.UpdateTime.Elapsed.TotalSeconds > 0) ? cooldown - (float)Game.UpdateTime.Elapsed.TotalSeconds : 0;
            else
            {
                var collisions = Entity.Get<RigidbodyComponent>().Collisions;
                var numCollisions = collisions.Count;
                Entity
                    .Scene
                    .Entities
                    .Where(e => e.Get<RigidbodyComponent>() != null && e.Get<ThermalConductionScript>() != null)
                    .AsParallel()
                    .ForAll(
                        e => 
                        {
                            
                            var thermB = e.Get<ThermalConductionScript>();
                            if (thermB.Temperature > Temperature)
                                Temperature += thermB.RadiateHeat();
                            else
                                thermB.Temperature += RadiateHeat();

                        }
                    );
                
            }

        }
        public float RadiateHeat()
        {
            Temperature -= ThermalConductivity * cooldown;
            return ThermalConductivity * cooldown;
        }
    }
}