using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyRPG.World;
using MyRPG.Core;

namespace MyRPG.Entities
{
    public class Boss : Entity
    {
        public string Dialogue { get; set; }
        public string RoomName { get; set; }
        private Vector2 _direction;
        private float _directionTimer;
        private const float DirectionChangeInterval = 2f;

        public Boss(Texture2D texture, Vector2 startPosition, string dialogue, string roomName = "bossroom", int v = 0) 
            : base(texture, startPosition)
        {
            Dialogue = dialogue;
            RoomName = roomName;
            Speed = 50f;
            _direction = Vector2.Zero;
            _directionTimer = 0;
        }

        public void Update(Room room, Player player, List<Boss> otherNPCs)
        {
            _directionTimer += Globals.Time;
            
            if (_directionTimer >= DirectionChangeInterval)
            {
                _directionTimer = 0;
                if (_direction == Vector2.Zero || RandomHelper.RandomNext(2) == 0)
                {
                    int dir = RandomHelper.RandomNext(4);
                    _direction = dir switch
                    {
                        0 => new Vector2(0, -1),
                        1 => new Vector2(0, 1),
                        2 => new Vector2(-1, 0),
                        _ => new Vector2(1, 0)
                    };
                }
                else
                {
                    _direction = Vector2.Zero;
                }
            }

            if (_direction != Vector2.Zero)
            {
                float dt = Globals.Time;
                
                Vector2 newPos = Position;
                newPos.X += _direction.X * Speed * dt;
                if (!room.IsRectangleBlocked(newPos, Width, Height) && !CheckEntityCollision(newPos, player, otherNPCs))
                {
                    Position.X = newPos.X;
                }

                newPos = Position;
                newPos.Y += _direction.Y * Speed * dt;
                if (!room.IsRectangleBlocked(newPos, Width, Height) && !CheckEntityCollision(newPos, player, otherNPCs))
                {
                    Position.Y = newPos.Y;
                }
            }
        }

        private bool CheckEntityCollision(Vector2 newPos, Player player, List<Boss> otherNPCs)
        {
            if (CheckCollision(newPos, player.Position, player.Width, player.Height))
                return true;

            foreach (var mfother in otherNPCs)
            {
                if (mfother == this) continue;
                if (CheckCollision(newPos, mfother.Position, mfother.Width, mfother.Height))
                    return true;
            }

            return false;
        }

        private bool CheckCollision(Vector2 pos1, Vector2 pos2, int width2, int height2)
        {
            return pos1.X < pos2.X + width2 &&
                   pos1.X + Width > pos2.X &&
                   pos1.Y < pos2.Y + height2 &&
                   pos1.Y + Height > pos2.Y;
        }
    }

    
}
