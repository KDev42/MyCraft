using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Configuration 
{
    public List<Vector3Int> GetCoordinate(Vector3Int originCoordinates, ConfigurationData configurationData)
    {
        List<Vector3Int> coordinates = new List<Vector3Int>();
        int height = configurationData.height;
        int sing = Math.Sign(height);
        Configuration configuration = SelectConfiguration(configurationData.type);

        Vector3Int currentCoordinateMain = originCoordinates;
        Vector3Int currentCoordinateSecond = originCoordinates;

        for (int i = 0; i < Math.Abs(height); i++)
        {
            currentCoordinateMain.y = originCoordinates.y + sing * i;

            if (configurationData.isTwoDirection)
            {
                currentCoordinateSecond.y = originCoordinates.y - sing * i;
            }

            for (int j = 0; j < configurationData.distance; j++)
            {
                coordinates.AddRange(configuration.GetCoordinate(j, currentCoordinateMain, configurationData));

                if (configurationData.isTwoDirection)
                {
                    coordinates.AddRange(configuration.GetCoordinate(j, currentCoordinateSecond, configurationData));
                }
            }
        }

        return coordinates;
    }

    protected virtual List<Vector3Int> GetCoordinate(int indexWave, Vector3Int originCoordinates, ConfigurationData configurationData)
    {
        return null;
    }

    protected void AddCoordinate(int x, int y,int z, List<Vector3Int> coordinates)
    {
        Vector3Int coordinate = new Vector3Int(x,y,z);

        int sizeWorldBlock = WorldConstants.maxSizeWorld * WorldConstants.chunkWidth;

        if(y>0 && y<WorldConstants.chunkHeight && x>0 && x< sizeWorldBlock && z > 0 && z < sizeWorldBlock)
        coordinates.Add(coordinate);
    }

    private Configuration SelectConfiguration(ConfigurationType configurationType)
    {
        switch (configurationType)
        {
            case ConfigurationType.cylinder:
                return new CircleConfiguration();
        }

        return new CircleConfiguration();
    }
}
