using MelonLoader;
using HarmonyLib;
using System.Reflection.Emit;
using ScheduleOne.UI.Items;

[assembly: MelonInfo(typeof(LargerMoneyStacks_Alternate.LargerMoneyStacks), "LargerMoneyStacks_Alternate", "1.0.1", "Apatel78", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace LargerMoneyStacks_Alternate
{
    public class LargerMoneyStacks : MelonMod
    {
        public override void OnInitializeMelon()
        {
            var harmony = new HarmonyLib.Harmony("com.apatel.schedule1.xppatch");

            harmony.Patch(
                AccessTools.Method(typeof(ItemUIManager), "UpdateCashDragAmount"),
                transpiler: new HarmonyMethod(typeof(LargerMoneyStacks), nameof(TranspilerPatch))
            );
            harmony.Patch(
                AccessTools.Method(typeof(ItemUIManager), "StartDragCash"),
                transpiler: new HarmonyMethod(typeof(LargerMoneyStacks), nameof(TranspilerPatch))
            );
            harmony.Patch(
                AccessTools.Method(typeof(ItemUIManager), "EndCashDrag"),
                transpiler: new HarmonyMethod(typeof(LargerMoneyStacks), nameof(TranspilerPatch))
            );

            MelonLogger.Msg("Schedule1 XP Patch initialized!");
        }
        static IEnumerable<CodeInstruction> TranspilerPatch(IEnumerable<CodeInstruction> instructions)
        {
            MelonLogger.Msg("Transpiler called");
            foreach (var instruction in instructions)
            {
                MelonLogger.Msg($"Opcode: {instruction.opcode}, Operand: {instruction.operand}");
                if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 1000f)
                    yield return new CodeInstruction(OpCodes.Ldc_R4, (float)99999);
                else
                    yield return instruction;
            }
        }

    }
}