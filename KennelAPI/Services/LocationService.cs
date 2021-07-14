using Common.Entities;
using Common.Interfaces.Services;
using MongoPersistence.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class LocationService : ILocationService
    {
        public LocationService()
        {

        }
        public float MeasureDistance(ILocationEntity myDog, ILocationEntity otherDog)
        {
            return 0;
        }
    }
}
