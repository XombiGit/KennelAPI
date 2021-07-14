using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface ILocationEntity
    {
        string DogID { get; set; }
        float XCoord { get; set; }
        float YCoord { get; set; }
        DateTime Timestamp { get; set; }
    }
}
