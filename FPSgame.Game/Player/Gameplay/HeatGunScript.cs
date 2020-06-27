using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using FPSgame.Chemistry;

namespace FPSgame.Player.Gameplay
{
    
    public class HeatGunScript : SyncScript
    {
        public float CalorPerSec {get;set;} = 1;
        public float MaxHeatUpDistance = 3;

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {
            if (Input.IsKeyDown(Keys.R))
            {


                var raycastStart = Entity.Transform.Parent.WorldMatrix.TranslationVector;
                var forward = Entity.Transform.Parent.WorldMatrix.Forward;
                var raycastEnd = raycastStart + forward * MaxHeatUpDistance;

                var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);
                

                if (result.Succeeded && result.Collider != null)
                {
                    if (result.Collider is RigidbodyComponent rigidBody)
                    {
                        var target = Entity.Scene.Entities.First(e => e.Get<RigidbodyComponent>() == rigidBody && e.Get<ThermalConductionScript>()!=null);
                        if(target != null)
                            DebugText.Print("Temperature target : " + target, new Int2(x: 50, y: 125));
                        target?.Get<ThermalConductionScript>()?.HeatUp(CalorPerSec * (float)Game.UpdateTime.Elapsed.TotalSeconds);
                    }
                }
            }
        }
    }
}
