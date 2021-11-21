using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitOceanParts : OceanParts
{
    public override void SetPollutionLevel(OceanAreas Area, EventButton Event)
    {
        parts[(int)Area].SetPollutionLevel(Event);

        if (Area == OceanAreas.Area_02)
        {
            parts[(int)OceanAreas.Area_03].SetPollutionLevel(Event);
        }
        else if (Area == OceanAreas.Area_03)
        {
            parts[(int)OceanAreas.Area_02].SetPollutionLevel(Event);
        }
    }
}
