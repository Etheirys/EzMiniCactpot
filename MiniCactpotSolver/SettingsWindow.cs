using Dalamud.Bindings.ImGui;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace MiniCactpotSolver;

public class SettingsWindow : Window
{

    public SettingsWindow(MiniCactpotPlugin plugin) : base("ezMiniCactpot Settings", ImGuiWindowFlags.NoResize)
    {
        Size = new Vector2(500, 300);
    }

    public Vector3 ButtonColor = new(1.0f, 1.0f, 1.0f);
    public Vector3 LaneColor = new(1.0f, 1.0f, 1.0f);

    int selected;
    public override void Draw()
    {
        ImEX.ButtonSelectorStrip("settings_filters_selector", new Vector2(ImEX.GetRemainingWidth(), ImEX.GetLineHeight()), ref selected, ["Colors", "Icon"]);

        if (selected == 0)
        {
            ImGui.Text("Button Colors:");
            ImEX.CenterNextElementWithPadding(25);
            if (ImGui.ColorEdit3("###ButtonColors", ref ButtonColor, ImGuiColorEditFlags.AlphaPreviewHalf))
            {

            }

            ImGui.Text("Lane Colors:");
            ImEX.CenterNextElementWithPadding(25);
            if (ImGui.ColorEdit3("##LaneColors", ref LaneColor, ImGuiColorEditFlags.AlphaPreviewHalf))
            {

            }
        }
        else if (selected == 1)
        {
            if (ImEX.GameIconButton(MiniCactpotPlugin.Instance!.TextureProvider, 61332))
            {

            }

            ImGui.SameLine();

            if (ImEX.GameIconButton(MiniCactpotPlugin.Instance!.TextureProvider, 90452))
            {

            }

            ImGui.SameLine();

            if (ImEX.GameIconButton(MiniCactpotPlugin.Instance!.TextureProvider, 234008))
            {

            }
        }
    }

}

public static partial class ImEX
{
    public static bool GameIconButton(ITextureProvider textureProvider, uint iconId)
    {
        var iconTexture = textureProvider.GetFromGameIcon(iconId);

        return ImGui.ImageButton(iconTexture.GetWrapOrEmpty().Handle, new Vector2(48.0f, 48.0f));
    }

    public static bool ButtonSelectorStrip(string id, Vector2 size, ref int selected, string[] options)
    {
        if (size == Vector2.Zero) size = new Vector2(GetRemainingWidth(), GetLineHeight());

        bool changed = false;
        float buttonWidth = size.X / options.Length;

        using (ImRaii.PushColor(ImGuiCol.ChildBg, ImGui.GetColorU32(ImGuiCol.Tab)))
        {
            using (ImRaii.PushStyle(ImGuiStyleVar.ChildRounding, ImGui.GetStyle().FrameRounding))
            {
                using var child = ImRaii.Child(id, size, false, ImGuiWindowFlags.NoScrollbar);
                if (child.Success)
                {
                    using (ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, new Vector2(0, 0)))
                    {
                        for (int i = 0; i < options.Length; i++)
                        {
                            if (i > 0)
                                ImGui.SameLine();

                            bool val = i == selected;
                            ToggleStripButton($"{options[i]}##{id}", new(buttonWidth, size.Y), ref val, false);

                            if (val && i != selected)
                            {
                                selected = i;
                                changed = true;
                            }
                        }
                    }
                }
            }
        }

        return changed;
    }

    public static bool ToggleStripButton(string label, Vector2 size, ref bool selected, bool canSelect = true)
    {
        bool clicked = false;

        using (ImRaii.Disabled(canSelect && selected))
        {
            using (ImRaii.PushColor(ImGuiCol.Button, ImGui.GetColorU32(selected ? ImGuiCol.TabActive : ImGuiCol.Tab)))
            using (ImRaii.PushStyle(ImGuiStyleVar.ItemSpacing, new Vector2(0, 0)))
                if (ImGui.Button(label, size))
                {
                    selected = !selected;
                    clicked = true;
                }
        }

        return clicked;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float GetRemainingWidth()
    {
        return ImGui.GetContentRegionAvail().X;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float GetRemainingHeight()
    {
        return ImGui.GetContentRegionAvail().Y;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static float GetLineHeight()
    {
        return ImGui.GetTextLineHeight() + (ImGui.GetStyle().FramePadding.Y * 2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void RightAlign(float width, int numItems)
    {
        RightAlign((width * numItems) + (ImGui.GetStyle().ItemSpacing.X * (numItems - 1)));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void RightAlign(float width)
    {
        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + (GetRemainingWidth() - width));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static void CenterNextElementWithPadding(float padding)
    {
        var elementWidth = GetRemainingWidth() - padding;

        float windowWidth = ImGui.GetContentRegionAvail().X;
        float offset = MathF.Max(0, (windowWidth - elementWidth) * 0.5f);

        ImGui.SetCursorPosX(ImGui.GetCursorPosX() + offset);
        ImGui.SetNextItemWidth(elementWidth);
    }
}
