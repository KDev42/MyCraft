using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleConfiguration : Configuration
{
    protected override List<Vector3Int> GetCoordinate(int indexWave, Vector3Int originCoordinates, ConfigurationData configurationData)
    {
        List<Vector3Int> coordinates = new List<Vector3Int>();
        int x = 0, z = indexWave, gap = 0, delta = (2 - 2 * indexWave);
        while (z >= 0)
        {
            AddCoordinate(originCoordinates.x + x, originCoordinates.y, originCoordinates.z + z, coordinates);
            AddCoordinate(originCoordinates.x + x, originCoordinates.y, originCoordinates.z - z, coordinates);
            AddCoordinate(originCoordinates.x - x, originCoordinates.y, originCoordinates.z - z, coordinates);
            AddCoordinate(originCoordinates.x - x, originCoordinates.y, originCoordinates.z + z, coordinates);

            AddCoordinate(originCoordinates.x + z, originCoordinates.y, originCoordinates.z + x, coordinates);
            AddCoordinate(originCoordinates.x + z, originCoordinates.y, originCoordinates.z - x, coordinates);
            AddCoordinate(originCoordinates.x - z, originCoordinates.y, originCoordinates.z - x, coordinates);
            AddCoordinate(originCoordinates.x - z, originCoordinates.y, originCoordinates.z + x, coordinates);

            gap = 2 * (delta + z) - 1;
            if (delta < 0 && gap <= 0)
            {
                x++;
                delta += 2 * x + 1;
                continue;
            }
            if (delta > 0 && gap > 0)
            {
                z--;
                delta -= 2 * z + 1;
                continue;
            }
            x++;
            z--;
            delta += 2 * (x - z);
        }
        return coordinates;
    }
}
