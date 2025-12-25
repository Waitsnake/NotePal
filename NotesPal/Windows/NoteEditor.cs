using Dalamud.Interface.Windowing;
using System;
using NotesPal.Data;
using Dalamud.Bindings.ImGui;

namespace NotesPal.Windows
{
    public class NoteEditor : Window
    {
        private NoteModel? note;
        private string noteText = string.Empty;

        private bool isFirstOpen = true;
		
		private const int SavedHintTimeMs = 2000;
		private long lastSaveTime;

        public static NoteEditor? Instance { get; private set; }

        private NoteEditor() : base("Note Editor", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize) { }

        public static void OpenNoteForPlayer(string playerName, uint worldId)
        {
            var note = NoteDb.Get(playerName, worldId);
            if (note != null)
            {
                OpenNote(note);
            }
        }


        public static void OpenNote(NoteModel note)
        {
            if (Instance == null)
            {
                Instance = new NoteEditor();
                Services.Instance.WindowSystem.AddWindow(Instance);
            }

            Instance.note = note;
            Instance.noteText = note.NoteText ?? string.Empty;
            Instance.IsOpen = true;
        }

        public override void Draw()
        {
            if (note == null) return;

            if (isFirstOpen)
            {
                var screenSize = ImGui.GetIO().DisplaySize;
                var windowSize = ImGui.GetWindowSize();
                ImGui.SetWindowPos(screenSize / 2 - windowSize / 2);
                isFirstOpen = false;
            }

            ImGui.Text($"Editing Note: {note.Name}");
            ImGui.InputTextMultiline("##note-text", ref noteText, 1000, new System.Numerics.Vector2(800, 300));

			var inSaveCooldown =
				DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastSaveTime <= SavedHintTimeMs;

			if (inSaveCooldown)
			{
				ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(0, 1, 0, 1));
				ImGui.BeginDisabled();
				ImGui.Button("Saved!");
				ImGui.EndDisabled();
				ImGui.PopStyleColor();
			}
			else if (ImGui.Button("Save Note"))
			{
				SaveNote();
			}

            ImGui.SameLine();

			var ctrlIsHeld =
				ImGui.IsKeyDown(ImGuiKey.LeftCtrl) ||
				ImGui.IsKeyDown(ImGuiKey.RightCtrl);

			if (!ctrlIsHeld)
				ImGui.BeginDisabled();

			if (ImGui.Button("Delete Note"))
			{
				DeleteNote();
			}

			if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
				ImGui.SetTooltip("Hold Ctrl to enable this button.");

			if (!ctrlIsHeld)
				ImGui.EndDisabled();
        }

        private void SaveNote()
        {
            if (note != null)
            {
                note.NoteText = noteText;
                NoteDb.Upsert(note);
				
				lastSaveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();

                if (MainWindow.Instance.IsOpen)
                {
                    MainWindow.Instance.ReloadNotes(); // Notizen im Hauptfenster neu laden
                }
            }
        }

        private void DeleteNote()
        {
            if (note != null)
            {
                NoteDb.Delete(note.Name, note.WorldId);
                IsOpen = false;
                if (MainWindow.Instance.IsOpen)
                {
                    MainWindow.Instance.ReloadNotes(); // Notizen im Hauptfenster neu laden
                }
            }
        }
    }
}

