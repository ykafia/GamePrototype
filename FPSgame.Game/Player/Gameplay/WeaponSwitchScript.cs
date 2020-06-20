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
    public enum Weapons
    {
        PhysicsGun,
        HeatGun,
    }
    public class WeaponSwitchScript : SyncScript
    {
        private Weapons currentWeapon = Weapons.PhysicsGun;
        private readonly SyncScript[] weaponScripts = {new MovePhysicsHandle(),new HeatGunScript()};
        public override void Start()
        {
            Entity.Add(weaponScripts[(int)currentWeapon]);
        }

        // Declared public member fields and properties will show in the game studio
        public override void Update()
        {
           if(Input.IsKeyPressed(Keys.E))
           {
                Entity.Remove(weaponScripts[(int)currentWeapon]);
                currentWeapon = (Weapons)((int)currentWeapon + 1 % weaponScripts.Length);
                Entity.Add(weaponScripts[(int)currentWeapon]);
           }
        }
    }
}
