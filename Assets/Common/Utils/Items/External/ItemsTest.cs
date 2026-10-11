using System;
using System.Collections.Generic;
using Common.Utils.Items.Runtime;
using Common.Utils.Items.Runtime.Actions;
using Common.Utils.Items.Runtime.Components;
using Common.Utils.Items.Runtime.States;
using Common.Utils.Items.Runtime.UseHandlers;
using UnityEngine;

namespace Common.Utils.Items.External
{
    public class ItemsTest : MonoBehaviour
    {
        private WeaponProxy _weaponProxy;
        
        private void Start()
        {
            List <IItemComponent> components = new()
            {
                new ShootItemComponent(10)
            };
            
            ItemDefinition def = new ItemDefinition(components);
            
            Dictionary<Type, IItemState> states = new();
            
            var shootState = new ShootItemState(){AmoAmount = 10};
            states[typeof(ShootItemComponent)] = shootState;
            states[typeof(ReloadItemComponent)] = shootState;
            
            ItemInstance pistol = new ItemInstance(def, states);

            ItemActionActionService actionActionService = new ItemActionActionService(
                new Dictionary<(Type, Type), IActionHandler>()
            {
                {(typeof(UseAction), typeof(ShootItemComponent)), new ShootHandler()},
                {(typeof(ReloadAction), typeof(ReloadItemComponent)), new ReloadHandler()}
            });

            var equipment = new Equipment();
            equipment.Equip(pistol);

            _weaponProxy = new WeaponProxy(equipment, actionActionService);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
        }

        private void Reload()
        {
            _weaponProxy.Reload();
        }

        private void Shoot()
        {
            _weaponProxy.Use();
        }
    }
}