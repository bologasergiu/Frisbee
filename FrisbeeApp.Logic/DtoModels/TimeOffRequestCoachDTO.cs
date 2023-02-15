using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrisbeeApp.Logic.DtoModels
{
    public class TimeOffRequestCoachDTO
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeOffRequestType RequestType { get; set; }
    }
}
