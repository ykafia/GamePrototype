using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Stride.Core.Diagnostics;

namespace FPSgame.Player
{
    public class GravityPickUpScript : SyncScript
    {
        public float MaxPickUpDistance { get; set; } = 2f;
        public float Height {get;set;} = 1.0f;
        private bool isHolding = false;
        
        private Entity onHold = null;


        public override void Start() 
        {
            Log.ActivateLog(LogMessageType.Info);
        }


        /// <summary>
        /// Called on every frame update
        /// </summary>
        public override void Update()
        {
            var camForward = Entity.Transform.WorldMatrix.Forward * 2;
            var holdPosition = Entity.GetParent().Transform.Position + Vector3.UnitY*3 + camForward;
            if(Input.IsKeyPressed(Keys.R)) {
                
                if(!isHolding)
                {    

                    var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
                    var forward = Entity.Transform.WorldMatrix.Forward;
                    var raycastEnd = raycastStart + forward * MaxPickUpDistance;

                    var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);


                    if (result.Succeeded && result.Collider != null)
                    {
                        if (result.Collider is RigidbodyComponent rigidBody)
                        {
                            
                            onHold = Entity.Scene.Entities.First(e => e.Get<RigidbodyComponent>() == rigidBody);
                            isHolding = true;

                        }
                    }
                }
                else {
                    onHold = null;
                    isHolding = false;
                }
            }
            if(isHolding)
            {
                var onHand = Entity.Transform.Parent.Position + Entity.Transform.WorldMatrix.Forward*3 + Vector3.UnitY*1.5f;
                if(onHold != null)
                {
                    // var impulse = onHand - onHold.Transform.Position;
                    // impulse.Normalize();
                    // onHold
                    //     .Get<RigidbodyComponent>()?
                    //     .ApplyImpulse(
                    //         // Vector3.UnitY * onHold.Get<RigidbodyComponent>().Mass *
                    //         // (float) Game.UpdateTime.Elapsed.TotalSeconds *
                    //         // 10f *
                    //         // (1+(Height / onHold.Transform.Position.Y)%1)
                    //         (impulse.Length() < 1)?impulse*impulse:impulse
                    //     );
                    onHold.Transform.Position += Vector3.UnitY * 3 * (float)Game.UpdateTime.Elapsed.TotalSeconds;
                    
                }
                
                
            }
        }
    }
}
