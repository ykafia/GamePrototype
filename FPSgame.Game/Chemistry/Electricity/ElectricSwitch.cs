using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Stride.Engine.Events;

namespace FPSgame.Chemistry.Electricity
{
    public class ElectricSwitch : SyncScript
    {
        // Event 
        public static EventKey Activate = new EventKey("Switch Activation","Activated");

        private bool activated = false;
        private ElectricConductionScript conduct;

        public Entity Target;


        public override void Start()
        {
            conduct = new ElectricConductionScript();
            Entity.Add(conduct);
        }

        public override void Update()
        {
            // Do stuff every new frame
            DebugText.Print("Switch has " + (activated?"":"not") + " been activated and current is " + conduct.Conduct, new Int2(500,100));
            if (conduct.Conduct == Current.ON && !activated)
                activated = true;
            else if (conduct.Conduct == Current.OFF && activated)
                activated = false;
        }
    }
}
