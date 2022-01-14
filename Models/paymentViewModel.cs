using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace SmartTruckerApp5.Models
{
    public class paymentViewModel
    {
     
        [DisplayName("Cargo ID")]
        public int cargoID { get; set; }


        public virtual CargoDetails cargodetails { get; set; }

        public int paymentKey { get; set; }

        [Display(Name = "Slip Approval")]
        public string approval { get; set; }
        [DisplayName("Receipt Picture")]

        public byte[] File { get; set; }


        [Required]
        [DisplayName("Amount Payed")]
        public double amountpayed { get; set; }
        [Display(Name = "Uploaded Date")]
        [DataType(DataType.Date)]
        public DateTime DocumentDate { get; set; } 

       
        [Display(Name = "Driver's Email Address")]
        public string usersID { get; set; }
      


    }
}