using System;
using System.Collections.Generic;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DodgeThis {

  [HarmonyPatch(typeof(EntityInLiquid))]
  [HarmonyPatch("DoApply")]
  public class Patch_EntityInLiquid_DoApply {
    static void Prefix(float dt, Entity entity, EntityPos pos, EntityControls controls,
      EntityInLiquid __instance,
      ref float ___push,
      ref long ___lastPush
    ) {

      //IL_0022: Unknown result type (might be due to invalid IL or missing references)
      if (entity.Swimming && entity.Alive) {
        string text = ((entity is EntityPlayer) ? ((EntityPlayer)entity).PlayerUID : null);
        // if ((controls.TriesToMove || controls.Jump) && entity.World.ElapsedMilliseconds - ___lastPush > 2000 && text != null) {
        if ((controls.TriesToMove || controls.Jump) && entity.World.ElapsedMilliseconds - ___lastPush > 2000) {
            ___push = 8f;
          ___lastPush = entity.World.ElapsedMilliseconds;
          if (text != null) {
            entity.PlayEntitySound("swim", entity.World.PlayerByUid(text), true, 24f);
          }
        }
        else {
          ___push = Math.Max(1f, ___push - 0.1f * dt * 60f);
        }
        Block block = entity.World.BlockAccessor.GetBlock((int)pos.X, (int)pos.Y, (int)pos.Z);
        Block block2 = entity.World.BlockAccessor.GetBlock((int)pos.X, (int)(pos.Y + 1.0), (int)pos.Z);
        Block block3 = entity.World.BlockAccessor.GetBlock((int)pos.X, (int)(pos.Y + 2.0), (int)pos.Z);
        float num = GameMath.Clamp((float)(int)pos.Y + (float)block.LiquidLevel / 8f + (((CollectibleObject)block2).IsLiquid() ? 1.125f : 0f) + (((CollectibleObject)block3).IsLiquid() ? 1.125f : 0f) - (float)pos.Y - (float)entity.SwimmingOffsetY, 0f, 1f);
        num = Math.Min(1f, num + 0.075f);
        double num2 = 0.0;
        num2 = ((!controls.Jump) ? (controls.FlyVector.Y * (double)(1f + ___push) * 0.029999999329447746 * (double)num) : ((double)(0.005f * num * dt * 60f)));
        pos.Motion.Add(controls.FlyVector.X * (double)(1f + ___push) * 0.029999999329447746, num2, controls.FlyVector.Z * (double)(1f + ___push) * 0.029999999329447746);
      }
      Block block4 = entity.World.BlockAccessor.GetBlock((int)pos.X, (int)pos.Y, (int)pos.Z);
      if (block4.PushVector != null) {
        pos.Motion.Add(block4.PushVector);
      }
    }
  }
}