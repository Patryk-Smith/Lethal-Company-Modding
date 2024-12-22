using GameNetcodeStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;

namespace FancyManMod.Items
{
    internal class BowlerHat : GrabbableObject
    {
        private PlayerControllerB previousPlayerHeldBy;

        [CompilerGenerated]
        private static class <>O
        {
            public static hook_SetHoverTipAndCurrentInteractTrigger<0> __PlayerControllerB_SetHoverTipAndCurrentInteractTrigger;

            public static hook_BeginGrabObject<1> __PlayerControllerB_BeginGrabObject;
        }

        public static void Initialize()
        {
            //IL_0010: Unknown result type (might be due to invalid IL or missing references)
            //IL_0015: Unknown result type (might be due to invalid IL or missing references)
            //IL_001b: Expected O, but got Unknown
            //IL_0030: Unknown result type (might be due to invalid IL or missing references)
            //IL_0035: Unknown result type (might be due to invalid IL or missing references)
            //IL_003b: Expected O, but got Unknown
            object obj = <> O.< 0 > __PlayerControllerB_SetHoverTipAndCurrentInteractTrigger;
            if (obj == null)
            {
                hook_SetHoverTipAndCurrentInteractTrigger val = PlayerControllerB_SetHoverTipAndCurrentInteractTrigger;

            <> O.< 0 > __PlayerControllerB_SetHoverTipAndCurrentInteractTrigger = val;
                obj = (object)val;
            }
            PlayerControllerB.SetHoverTipAndCurrentInteractTrigger += (hook_SetHoverTipAndCurrentInteractTrigger)obj;
            object obj2 = <> O.< 1 > __PlayerControllerB_BeginGrabObject;
            if (obj2 == null)
            {
                hook_BeginGrabObject val2 = PlayerControllerB_BeginGrabObject;

            <> O.< 1 > __PlayerControllerB_BeginGrabObject = val2;
                obj2 = (object)val2;
            }
            PlayerControllerB.BeginGrabObject += (hook_BeginGrabObject)obj2;
        }

        private static void PlayerControllerB_BeginGrabObject(orig_BeginGrabObject orig, PlayerControllerB self)
        {
            self.interactRay = new Ray(self.gameplayCamera.transform.position, self.gameplayCamera.transform.forward);
            if (!Physics.Raycast(self.interactRay, out self.hit, self.grabDistance, self.interactableObjectsMask) || self.hit.collider.gameObject.layer == 8 || !(self.hit.collider.tag == "PhysicsProp") || self.twoHanded || self.sinkingValue > 0.73f)
            {
                return;
            }
            self.currentlyGrabbingObject = self.hit.collider.transform.gameObject.GetComponent<GrabbableObject>();
            if ((!GameNetworkManager.Instance.gameHasStarted && !self.currentlyGrabbingObject.itemProperties.canBeGrabbedBeforeGameStart && !StartOfRound.Instance.testRoom.activeSelf) || self.currentlyGrabbingObject == null || self.inSpecialInteractAnimation || self.currentlyGrabbingObject.isHeld || self.currentlyGrabbingObject.isPocketed)
            {
                return;
            }
            NetworkObject networkObject = self.currentlyGrabbingObject.NetworkObject;
            if (!(networkObject == null) && networkObject.IsSpawned)
            {
                if (self.currentlyGrabbingObject is PouchyBelt && self.ItemSlots.Any((GrabbableObject x) => x != null && x is PouchyBelt))
                {
                    self.currentlyGrabbingObject.grabbable = false;
                }
                orig.Invoke(self);
                if (self.currentlyGrabbingObject is PouchyBelt)
                {
                    self.currentlyGrabbingObject.grabbable = true;
                }
            }
        }

        private static void PlayerControllerB_SetHoverTipAndCurrentInteractTrigger(orig_SetHoverTipAndCurrentInteractTrigger orig, PlayerControllerB self)
        {
            orig.Invoke(self);
            if (!Physics.Raycast(self.interactRay, out self.hit, self.grabDistance, self.interactableObjectsMask) || self.hit.collider.gameObject.layer == 8 || !(self.hit.collider.tag == "PhysicsProp"))
            {
                return;
            }
            if (self.FirstEmptyItemSlot() == -1)
            {
                self.cursorTip.text = "Inventory full!";
            }
            else if (self.hit.collider.gameObject.GetComponent<GrabbableObject>() is PouchyBelt)
            {
                if (self.ItemSlots.Any((GrabbableObject x) => x != null && x is PouchyBelt))
                {
                    self.cursorTip.text = "(Cannot hold more than 1 belt)";
                }
                else
                {
                    self.cursorTip.text = "Pick up belt";
                }
            }
        }

        public override void LateUpdate()
        {
            base.LateUpdate();
            if (previousPlayerHeldBy != null)
            {
                beltCosmetic.gameObject.SetActive(value: true);
                beltCosmetic.SetParent(null);
                beltCosmetic.GetComponent<MeshRenderer>().enabled = true;
                Transform root = previousPlayerHeldBy.lowerSpine.parent;
                beltCosmetic.position = root.position + beltCosmeticPositionOffset;
                Quaternion rotation = Quaternion.Euler(root.rotation.eulerAngles + beltCosmeticRotationOffset);
                beltCosmetic.rotation = rotation;
                mainObjectRenderer.enabled = false;
                base.gameObject.SetActive(value: true);
            }
            else
            {
                beltCosmetic.gameObject.SetActive(value: false);
                mainObjectRenderer.enabled = true;
                beltCosmetic.SetParent(base.transform);
            }
        }
    }
}
