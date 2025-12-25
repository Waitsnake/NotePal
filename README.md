# NotesPal

**NotesPal** is a Dalamud plugin that allows you to make notes on players you meet in-game. This way, you can store information about players and retrieve it later for reference.

## Features
- **Create notes for players:** Save notes for any player you meet in the game.
- **Multiple views:** A clear overview of all notes with an editor to manage them.
- **Easy installation and usage:** Quick to install and ready to use.

## Installation

### 1. Install via Dalamud (Experimental)
You can easily install **NotesPal** through Dalamud by adding the plugin repository.

- Open **Dalamud** and go to **Experimental Plugins**.
- Add the following repository URL to the plugin list:
  https://raw.githubusercontent.com/Waitsnake/NotesPal/refs/heads/main/repo.json
- Search for **NotesPal** in the list and install the plugin.

## Usage

- **Open Notes:** Right-click on a player's name in the game and select "Open Note" or "New Note" to create a note.
- **Manage Notes:** All notes are accessible in the main plugin window. You can edit, delete, or add new notes.
- **In FFXIV Chat Terminal:**
- **/note**  Opens the note editor for the currently-targeted player. You can write or edit notes about the player you're targeting.
- **/notes**  Opens the main **NotesPal** window, where you can manage all of your notes.

## Developers

1. Clone the repository or download the source code:
git clone https://github.com/Waitsnake/NotesPal.git

2. Build the project in folder with `NotesPal.sln`:
dotnet build -c Release

## License

This project is licensed under the MIT License. See the [LICENSE file](LICENSE) for details.
