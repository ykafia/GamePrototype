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
using Stride.Engine.Events;

namespace FPSgame.Player
{
    public class SimplePickUpScript : SyncScript
    {
        public float MaxPickUpDistance { get; set; } = 2f;
        public float Height {get;set;} = 1.0f;
        private bool isHolding = false;
        
        private Entity onHold = null;


        public override void Start() 
        {
            // maxMoveSpeed = Entity.GetParent().Get<PlayerController>().MaxRunSpeed;
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
                if(onHold!=null)
                {
                    onHold.Transform.Position -= onHold.Transform.Position;
                    onHold.Transform.Position += Vector3.Transform(Entity.Transform.Parent.Position + Entity.Transform.WorldMatrix.Forward * 3 + Vector3.UnitY *1.5f, onHold.Transform.Rotation);
                    
                }
            }
        }
    }
}