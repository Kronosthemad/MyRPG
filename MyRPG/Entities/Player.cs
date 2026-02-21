using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyRPG.Core;
using MyRPG.Data;
using MyRPG.Entities;
using MyRPG.World;

public class Player
{
    public Texture2D Texture;
    public Vector2 Position;
    public float Speed;
    public Stats Stats { get; set; }
    public int Width => Texture?.Width ?? 32;
    public int Height => Texture?.Height ?? 32;

    public Player(Texture2D texture, Vector2 startPosition, Stats stats)
    {
        Texture = texture;
        Position = startPosition;
        Speed = 250f;
        Stats = stats;
    }

    public void Update(GameTime gameTime)
    {
        // Get the current state of the keyboard
        KeyboardState kState = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        Vector2 direction = Vector2.Zero;

        if (kState.IsKeyDown(Keys.W)) direction.Y -= 1;
        if (kState.IsKeyDown(Keys.S)) direction.Y += 1;
        if (kState.IsKeyDown(Keys.A)) direction.X -= 1;
        if (kState.IsKeyDown(Keys.D)) direction.X += 1;

        // Normalize direction so diagonal movement isn't faster (C++ math logic!)
        if (direction != Vector2.Zero)
        {
            direction.Normalize();
            Position += direction * Speed * dt;
        }
    }
    public void Update(Room room, List<NPC> npcs)
    {
        if (InputManager.Direction != Vector2.Zero)
        {
            float dt = Globals.Time;

            // Calculate the "Potential" new position for X
            Vector2 newPos = Position;
            newPos.X += InputManager.Direction.X * Speed * dt;

            // If the new X position is NOT blocked by room or NPCs, apply it
            if (!room.IsRectangleBlocked(newPos, Width, Height) && !CheckNPCCollision(newPos, npcs))
            {
                Position.X = newPos.X;
            }

            // Calculate the "Potential" new position for Y
            newPos = Position;
            newPos.Y += InputManager.Direction.Y * Speed * dt;

            // If the new Y position is NOT blocked by room or NPCs, apply it
            if (!room.IsRectangleBlocked(newPos, Width, Height) && !CheckNPCCollision(newPos, npcs))
            {
                Position.Y = newPos.Y;
            }
        }
    }

    private bool CheckNPCCollision(Vector2 newPos, List<NPC> npcs)
    {
        foreach (var npc in npcs)
        {
            if (newPos.X < npc.Position.X + npc.Width &&
                newPos.X + Width > npc.Position.X &&
                newPos.Y < npc.Position.Y + npc.Height &&
                newPos.Y + Height > npc.Position.Y)
            {
                return true;
            }
        }
        return false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.White);
    }
}