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
        public float Temperature { get; set; } = 25f;

        private float cooldown = ThermalSimulation.Timestep;
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
            var material = Entity.Get<ModelComponent>().GetMaterial(0);
            material.Passes[0].Parameters.Set(ComputeEmissiveTemperatureKeys.Temperature,Temperature);
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
                if (i > 100) return 100;
                else return i;
            }
            Temperature = getSign(Temperature) * normalize(getAbsolute(Temperature));
        }
    }
}