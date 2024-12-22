using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq; 
using static System.Net.Mime.MediaTypeNames;
using UnityEngine;
using Unity.Netcode; 
using UnityEngine.UI;
using FancyManMod;

namespace FancyManMod.Items
{
    [HarmonyLib.HarmonyPatch]
    internal class TopHat : GrabbableObject
    {
         
        public override void DiscardItem()
        {
            RemoveItemSlots();
            previousPlayerHeldBy = null;
            base.DiscardItem();
        }

        public new void GrabItemOnClient()
        {
            base.GrabItemOnClient();
        }

        public override void EquipItem()
        {
            base.EquipItem();
            if (playerHeldBy != null)
            {
                if (playerHeldBy != previousPlayerHeldBy)
                {
                    AddItemSlots();
                }
                previousPlayerHeldBy = playerHeldBy;
            }
        }

        protected override void __initializeVariables()
        {
            base.__initializeVariables();
        }

        protected internal override string __getTypeName()
        {
            return "PouchyBelt";
        }
    }
}
