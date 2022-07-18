using System.Collections.Generic;
using System;

namespace OceanGame
{
    class DirectionTools
    {
        public static (int, int) GetDirectionOffsets(Direction direction)
        {
            switch (direction)
            {
                case Direction.Top:
                    return (0, -1);
                case Direction.Right:
                    return (1, 0);
                case Direction.Bottom:
                    return (0, 1);
                case Direction.Left:
                    return (-1, 0);
                default:
                    return (0, 0);
            }
        }

        public static Direction RandomDirection()
        {
            return (Direction)Globals.random.Next(Enum.GetValues(typeof(Direction)).Length);
        }

        public static void DirectionRandomIterate(DirectionAction action)
        {
            var directions = new List<Direction>();

            while (directions.Count < Enum.GetValues(typeof(Direction)).Length)
            {
                var direction = RandomDirection();
                if (!directions.Contains(direction))
                {
                    directions.Add(direction);
                }
            }

            foreach (var direction in directions)
            {
                (int nx, int ny) = GetDirectionOffsets(direction);
                if (!action(nx, ny))
                {
                    return;
                }
            }
        }
    }
}
