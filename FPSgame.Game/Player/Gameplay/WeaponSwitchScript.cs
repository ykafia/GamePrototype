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
using FPSgame.Core;

namespace FPSgame.Player.Gameplay
{
    public enum Weapons
    {
        PhysicsGun = 0,
        HeatGun = 1,
    }
    public class WeaponSwitchScript : SyncScript
    {
        private Weapons currentWeapon = Weapons.HeatGun;
        private int scriptUsed = (int) Weapons.HeatGun;
        private readonly SyncScript[] weaponScripts = {new MovePhysicsHandle(),new HeatGunScript()};
        public override void Start()
        {
            Entity.Add(weaponScripts[(int)currentWeapon]);
        }

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {
            DebugText.Print("Weapon used : " + currentWeapon, new Int2(x: 50, y: 50));
            DebugText.Print("Script index : " + scriptUsed, new Int2(x: 50, y: 75));
            DebugText.Print(
                "[PhysicsGun : " + ((Entity.Get<MovePhysicsHandle>()!=null)?1:0) +
                " ; HeatGun : " + ((Entity.Get<HeatGunScript>()!=null)?1:0) + " ]"
                , new Int2(x: 50, y: 25));

            if(Input.IsKeyPressed(Keys.E))
            {
                Entity.Remove(weaponScripts[scriptUsed]);
                scriptUsed = (scriptUsed + 1) % weaponScripts.Length;
                currentWeapon = (Weapons) scriptUsed;
                Entity.Add(weaponScripts[scriptUsed]);
            }
        }
    }
}
