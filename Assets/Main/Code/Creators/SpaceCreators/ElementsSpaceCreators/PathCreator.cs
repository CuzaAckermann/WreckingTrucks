using System;
using System.Collections.Generic;

public class PathCreator
{
    public Path CreatePath(PathSettings pathSettings)
    {
        if (pathSettings == null)
        {
            throw new ArgumentNullException(nameof(pathSettings));
        }

        if (pathSettings.IndexCheckPointForStartShooting < 0 &&
            pathSettings.IndexCheckPointForStartShooting >= pathSettings.Path.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(pathSettings.IndexCheckPointForStartShooting));
        }

        List<CheckPoint> positions = new List<CheckPoint>();

        for (int i = 0; i < pathSettings.Path.Count; i++)
        {
            CheckPoint checkPoint = new CheckPoint(pathSettings.Path[i].position,
                                                   pathSettings.Path[i].forward);

            if (i == pathSettings.IndexCheckPointForStartShooting)
            {
                checkPoint.StayStarOfShooting();
            }
            else if (i == pathSettings.IndexCheckPointForFinishShooting)
            {
                checkPoint.StayFinishOfShooting();
            }

            positions.Add(checkPoint);
        }

        return new Path(positions);
    }
}