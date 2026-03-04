using ActionEffectRange.Actions.Data;
using ActionEffectRange.Actions.EffectRange;
using ActionEffectRange.Actions.Enums;
using System.Text;
using CornerCases = ActionEffectRange.Actions.Data.Predefined.EffectRangeCornerCases;

namespace ActionEffectRange.Actions
{
    internal static class EffectRangeDataManager
    {

        public static EffectRangeData? NewData(uint actionId)
        {
            var row = ActionData.GetActionExcelRow(actionId);
            return row == null ? null : NewData(row.Value);
        }

        public static EffectRangeData NewData(
            Lumina.Excel.Sheets.Action actionRow)
            => EffectRangeData.Create(actionRow.RowId, actionRow.ActionCategory.RowId,
                actionRow.TargetArea, ActionData.GetActionHarmfulness(actionRow),
                actionRow.Range, actionRow.EffectRange, actionRow.XAxisModifier,
                actionRow.CastType, isOriginal: true);

        public static string DescribeActionData(uint actionId)
        {
            var row = ActionData.GetActionExcelRow(actionId);
            if (row == null)
                return $"Action#{actionId}: no action-sheet row found.";

            var actionRow = row.Value;
            var rawData = NewData(actionRow);
            var customisedData = CustomiseEffectRangeData(rawData);
            var builder = new StringBuilder();

            builder.AppendLine($"Action#{actionRow.RowId}: {actionRow.Name}");
            builder.AppendLine(
                $"Category={ActionData.GetActionCategoryName((ActionCategory)actionRow.ActionCategory.RowId)} ({actionRow.ActionCategory.RowId}), " +
                $"CastType={actionRow.CastType}, TargetArea={actionRow.TargetArea}, Range={actionRow.Range}, " +
                $"EffectRange={actionRow.EffectRange}, XAxisModifier={actionRow.XAxisModifier}");
            builder.AppendLine(
                $"ClassJobCategory={actionRow.ClassJobCategory.RowId}, IsPvP={actionRow.IsPvP}, " +
                $"PlayerTriggered={ActionData.IsPlayerTriggeredAction(actionRow)}, " +
                $"PlayerCombat={ActionData.IsPlayerCombatAction(actionRow)}, " +
                $"Harmfulness={ActionData.GetActionHarmfulness(actionRow)}");
            builder.AppendLine($"RawData={rawData}");
            builder.AppendLine($"CustomisedData={customisedData}");

            if (ActionData.TryGetActionWithAdditionalEffects(actionId, out var additionalEffects))
                builder.AppendLine($"AdditionalEffects=[{string.Join(", ", additionalEffects)}]");

            if (ActionData.ShouldNotUseCachedSeq(actionId))
                builder.AppendLine("UsesCurrentPosition=true");

            return builder.ToString().TrimEnd();
        }

        public static EffectRangeData NewDataChangeHarmfulness(
            EffectRangeData original, ActionHarmfulness harmfulness)
            => EffectRangeData.Create(original.ActionId, (uint)original.Category,
                original.IsGTAction, harmfulness, original.Range, original.EffectRange,
                original.XAxisModifier, original.CastType, isOriginal: false);


        public static bool IsHarmfulAction(EffectRangeData data)
            => data.Harmfulness.HasFlag(ActionHarmfulness.Harmful);

        public static bool IsBeneficialAction(EffectRangeData data)
            => data.Harmfulness.HasFlag(ActionHarmfulness.Beneficial);


        internal static EffectRangeData CustomiseEffectRangeData(
            EffectRangeData erdata)
        {
            erdata = CheckAoETypeOverriding(erdata);
            erdata = CheckConeAoEAngleOverriding(erdata);
            erdata = CheckDonutAoERadiusOverriding(erdata);
            erdata = CheckAoEHarmfulnessOverriding(erdata);

            erdata = CornerCases.UpdateEffectRangeData(erdata);
            return erdata;
        }

        private static EffectRangeData CheckAoETypeOverriding(EffectRangeData original)
        {
            if (ActionData.TryGetModifiedAoEType(original.ActionId, out var data)
                && data != null)
                return EffectRangeData.Create(
                    original.ActionId, (uint)original.Category, original.IsGTAction,
                    data.Harmfulness, original.Range, original.EffectRange,
                    original.XAxisModifier, data.CastType, isOriginal: false);
            return original;
        }

        private static EffectRangeData CheckConeAoEAngleOverriding(
            EffectRangeData original)
        {
            if (original is not ConeAoEEffectRangeData) return original;

            if (ActionData.TryGetModifiedCone(original.ActionId, out var coneData)
                && coneData != null)
                return new ConeAoEEffectRangeData(
                    original, coneData.CentralAngleCycles, coneData.RotationOffset);

            if (ActionData.TryGetConeAoEDefaultAngle(
                original.EffectRange, out var angle))
                return new ConeAoEEffectRangeData(original, angle);

            return new ConeAoEEffectRangeData(
                original, ActionData.ConeAoEDefaultAngleCycles);
        }

        private static EffectRangeData CheckDonutAoERadiusOverriding(
            EffectRangeData original)
            => ActionData.TryGetDonutAoERadius(original.ActionId, out var radius)
            ? new DonutAoEEffectRangeData(original, radius) : original;

        private static EffectRangeData CheckAoEHarmfulnessOverriding(
            EffectRangeData original)
            => ActionData.TryGetHarmfulness(original.ActionId, out var harmfulness)
            ? NewDataChangeHarmfulness(original, harmfulness) : original;

    }
}
