using Stride.Engine;
using Stride.Engine.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FPSgame.Chemistry.Electricity
{
   
    class GlobalConductionScript : AsyncScript
    {
        private EventReceiver change = new EventReceiver(ElectricConductionScript.currentChange);
        public static EventKey rebuildGraph = new EventKey("Electricity","rebuild graph");
        public override async Task Execute()
        {
            await change.ReceiveAsync();
            rebuildGraph.Broadcast();
        }
    }
}
