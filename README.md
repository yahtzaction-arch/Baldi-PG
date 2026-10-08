Baldi PG — Part Generator

Baldi PG is a Unity Editor toolkit by Yahtz for creating Baldi's Basics-style levels using parts and NPCs from your own game/decompile files.

Features
🏫 Part Generator
Classrooms
Rooms
Hallways
Walls
Doors
Windows
Stairs
Props
One-way wall parts
👤 NPC Dragger
Browse detected NPC prefabs
Search NPCs
Place NPCs directly into the Unity Scene
🔎 Game Asset Scanner
Select your Baldi's Basics game/decompile folder
Scan for prefabs and models
Automatically build the Baldi PG asset list
🎯 Scene Placement
Select a part or NPC
Place it directly into the current Scene
Uses the Scene camera position for quick placement
Installation

Baldi PG is installed through Unity Package Manager using a Git URL.

Open your Unity project.
Go to Window → Package Manager.
Click the + button.
Select Add package from git URL...
Enter the Baldi PG Git repository URL.
Click Add.
Wait for Unity to finish installing the package.

After installation, you'll find:

Yahtz
└── Baldi PG
    ├── Setup
    ├── NPC Dragger
    ├── Part Generator
    └── About
First-Time Setup

Open:

Yahtz → Baldi PG → Setup

Click Select... and choose the folder containing your Baldi's Basics installation.

For a Steam installation, this will normally be somewhere under:

SteamLibrary
└── steamapps
    └── common
        └── Baldi's Basics

Depending on your Steam library, the exact location may be different.

You can also select a Baldi's Basics decompile/project folder containing the assets you want to use.

Then click:

Scan Game

If the folder is valid, Baldi PG will report the assets it found.

Part Generator

Open:

Yahtz → Baldi PG → Part Generator

Select a category:

All
Rooms
Hallways
Stairs
Walls
Props

You can search for a specific part and use Place to put it into the Scene.

NPC Dragger

Open:

Yahtz → Baldi PG → NPC Dragger

Click Scan Prefabs to find available prefabs.

You can search for an NPC and place it into the Scene.

Important

Baldi PG does not include or redistribute Baldi's Basics game assets.

You must provide your own game/decompile folder. Baldi PG only scans the folder you select.

Make sure you have the appropriate rights to use the files you're providing to the tool.

Requirements
Unity 2021.3 or newer
A Unity project
Baldi's Basics game/decompile files containing the assets you want to use
Credits

Baldi PG
Created by Yahtz

Part Generator + NPC Dragger for Unity.
