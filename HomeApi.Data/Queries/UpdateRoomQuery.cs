using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeApi.Data.Queries
{
    public class UpdateRoomQuery
    {
        public string NewName { get; }
        public int Area { get; set; }
        public bool GasConnected { get; set; }
        public int Voltage { get; set; }

        public UpdateRoomQuery(string newName = null, int area = 0, bool gasConnected = false, int voltage = 0)
        {
            this.NewName = newName;
            this.Area = area;
            this.GasConnected = gasConnected;
            this.Voltage = voltage;
        }
    }
}
