using Common.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces.Services
{
    public interface ILocationService
    {
        float MeasureDistance(ILocationEntity myDog, ILocationEntity otherDog);
    }
}
