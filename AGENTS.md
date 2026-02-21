# AGENTS.md - Agent Guidelines for MyRPG

## Project Overview

This is a MonoGame-based RPG game engine written in C# targeting .NET 8.0. The project uses a room-based dungeon system with entities, collision detection, and a stats system.

---

## Build & Development Commands

### Building the Project
```bash
# Build the project (from MyRPG directory)
dotnet build

# Build with verbose output
dotnet build -v detailed

# Clean and rebuild
dotnet clean && dotnet build
```

### Running the Game
```bash
# Run in debug mode
dotnet run
```

### Testing
```bash
# Run all tests (if tests exist)
dotnet test

# Run a single test by name
dotnet test --filter "FullyQualifiedName~TestMethodName"

# Run tests with detailed output
dotnet test --logger "console;verbosity=detailed"
```

---

## Code Style Guidelines

### General Principles
- Keep code concise and readable - avoid unnecessary complexity
- Follow the existing patterns in the codebase
- Avoid adding comments unless explicitly requested
- Use existing libraries and utilities rather than reinventing

### Imports & Namespaces
- Use explicit namespaces; avoid `using` statements for types not used in the file
- Order imports: System libraries first, then third-party (MonoGame), then project-specific
- Example order:
  ```csharp
  using System;
  using System.Collections.Generic;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Graphics;
  using MyRPG.Core;
  using MyRPG.Entities;
  using MyRPG.World;
  ```

### Naming Conventions
- **Classes/Types**: PascalCase (e.g., `Player`, `RoomManager`, `DialogueSystem`)
- **Methods**: PascalCase (e.g., `Update`, `Draw`, `CheckDoorTransition`)
- **Properties**: PascalCase (e.g., `Position`, `Width`, `CurrentRoom`)
- **Private fields**: PascalCase or _camelCase (e.g., `_player`, `_direction`)
- **Constants**: PascalCase (e.g., `DirectionChangeInterval`)
- **Parameters**: camelCase (e.g., `gameTime`, `spriteBatch`)

### File Organization
- One public class per file
- Files should match the class name
- Order within file: fields, constructor, public methods, private methods
- Keep related functionality together

### Type Usage
- Use `var` when type is obvious from right side of assignment
- Prefer explicit types for method parameters and return types
- Use nullable reference types appropriately (`string?`, `Vector2?`)
- Use `List<T>` for collections that change, `T[]` for fixed-size

### Error Handling
- Use `Math.Clamp()` for value range limiting
- Check for null before accessing objects (`if (obj?.Property == value)`)
- Handle edge cases in boundary checks (array bounds, division by zero)
- Use guard clauses early in methods

### Game-Specific Patterns

#### Update Loop Pattern
```csharp
protected override void Update(GameTime gameTime)
{
    // Input handling first
    InputManager.Update();
    
    // Update globals
    Globals.Update(gameTime);
    
    // Entity updates
    _player.Update(_roomManager.CurrentRoom, _npcs);
    
    base.Update(gameTime);
}
```

#### Draw Loop Pattern
```csharp
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);
    
    _spriteBatch.Begin(transformMatrix: _camera.Transform);
    // Draw world objects
    _spriteBatch.End();
    
    _spriteBatch.Begin();
    // Draw UI elements (no camera transform)
    _spriteBatch.End();
    
    base.Draw(gameTime);
}
```

#### Collision Detection
- Check corners for rectangle collision, not just center point
- Allow door passage by checking door collision before wall collision
- Separate X and Y movement for smooth sliding along walls

#### Entity Pattern
```csharp
public class Entity
{
    public Vector2 Position { get; set; }
    public int Width => _texture?.Width ?? 32;
    public int Height => _texture?.Height ?? 32;
    
    public virtual void Update() { }
    public virtual void Draw() { }
}
```

---

## Content Pipeline

### Adding New Content
- Add spritefonts to `Content/Fonts/`
- Add texture files to content folder
- Register in `Content.mgcb` for build pipeline
- Example entry:
  ```
  #begin Fonts/default.spritefont
  /importer:FontDescriptionImporter
  /processor:FontDescriptionProcessor
  /build:Fonts/default.spritefont
  ```

---

## Common Patterns in This Codebase

### Stats System
- Stats are stored as properties on Player/NPC entities
- Use clamping for dynamic values (health, stamina, magic)
- Example:
  ```csharp
  public int CurrentHealth
  {
      get => _currentHealth;
      set => _currentHealth = Math.Clamp(value, 0, MaxHealth);
  }
  ```

### Room/Door System
- Rooms contain tiles and doors
- Doors define target room and spawn position
- Check door collision before wall collision
- Room name used to identify current room

### Input Handling
- Use `InputManager` for shared input state
- `InputManager.IsKeyPressed()` for single-press detection
- `InputManager.Direction` for movement vectors

---

## When in Doubt
- Look at similar files for patterns (e.g., check `NPC.cs` for entity patterns)
- Keep changes minimal and focused
- Test locally before committing changes
