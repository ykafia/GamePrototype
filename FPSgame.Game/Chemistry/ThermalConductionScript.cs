using System;
using Stride.Core;
using Stride.Core.Mathematics;
using Stride.Engine;
using Stride.Engine.Events;
using Stride.Physics;
using Stride.Input;
using System.Linq;
using Stride.Rendering;
using Stride.Rendering.Materials;
using FPSgame;
using Stride.Rendering.Materials.ComputeColors;

namespace FPSgame.Chemistry
{
    public class ThermalConductionScript : SyncScript
    {
        public float ThermalConductivity { get; set; } = 1.0f;
        public float Temperature { get; set; } = 0f;

        private float cooldown = ThermalSimulation.Timestep;
        public override void Update()
        {
            if (cooldown > 0)
                cooldown = (cooldown - (float)Game.UpdateTime.Elapsed.TotalSeconds > 0) ? cooldown - (float)Game.UpdateTime.Elapsed.TotalSeconds : 0;
            else
            {
                var rb = Entity.Get<RigidbodyComponent>();
                var collisions = rb.Collisions;
                var numCollisions = collisions.Count;
                Entity
                    .Scene
                    .Entities
                    .AsParallel()
                    .Where(e => e.Get<RigidbodyComponent>() != null && e.Get<ThermalConductionScript>() != null)?
                    .Where(e => collisions.AsParallel().Select(e => e.ColliderB).Contains(rb))
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
            var material = Entity.Get<ModelComponent>().GetMaterial(0);
            Entity.Get<ModelComponent>()
                .Materials
                .AsParallel()
                .ForAll(x => 
                    {
                        x.Value.Passes[0].Parameters.Set(ComputeEmissiveTemperatureKeys.Temperature, Temperature/25f);
                        x.Value.Passes[0].Parameters.Set(ComputeEmissiveIntensityKeys.Temperature,Temperature);
                    }
                );
            // material.Passes[0].Parameters.Set(ComputeEmissiveTemperatureKeys.Temperature, Temperature);
        }
        public float RadiateHeat()
        {
            Temperature -= ThermalConductivity * cooldown;
            return ThermalConductivity * cooldown;
        }
        public void HeatUp(float calor)
        {
            Temperature += ThermalConductivity * calor;
            static float getSign(float i)
            {
                if (i > 0)
                    return 1;
                else if (i < 0)
                    return -1;
                else
                    return 0;
            }

            static float getAbsolute(float i)
            {
                return getSign(i) switch
                {
                    1 => i,
                    -1 => -i,
                    _ => 0
                };
            }
            static float normalize(float i)
            {
                if (i > 25) return 25;
                else return i;
            }
            Temperature = getSign(Temperature) * normalize(getAbsolute(Temperature));
            // var material = Entity.Get<ModelComponent>().GetMaterial(0);
            DebugText.Print("Temperature target (Heating up) : " + Temperature, new Int2(x: 50, y: 100));
            Entity
                .Get<ModelComponent>()
                .Materials
                .AsParallel()
                .ForAll(x => 
                    {
                        x.Value.Passes[0].Parameters.Set(ComputeEmissiveTemperatureKeys.Temperature, Temperature/25f);
                        x.Value.Passes[0].Parameters.Set(ComputeEmissiveIntensityKeys.Temperature,Temperature);
                    }
                );
        }
    }
}