using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SmartTruckerApp5.Models
{
    public class DriverViewModel
    {

        [DisplayName("Estimated Travelling distance")]
        public int EstimatedTravellig { get; set; }
        [DisplayName("Destination")]
        public string Destination { get; set; }
        [DisplayName("Pick Up Location")]
        public string PickPoint { get; set; }
        [DisplayName("Estimated come back date")]
        public DateTime EstimatedDelivery { get; set; }
        [DisplayName("Starting date")]
        public DateTime StartDateTime { get; set; }
        [DisplayName("Truck assigned to you")]
        public int TrucksID { get; set; }
    }
}