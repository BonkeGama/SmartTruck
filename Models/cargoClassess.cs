using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTruckerApp5.Models

{
    public class CargoDetails
    {

        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CargoDetailsKey { get; set; }
        [DisplayName("Cargo Status")]
        public string CargoStatus { get; set; }
        [DisplayName("Estimated Travelling distance")]
        public int EstimatedTravellig { get; set; }
       
        [DisplayName("Destination")]
        public int Destination { get; set; }
        [DisplayName("Pick Up Location")]
        public int PickPoint { get; set; }

        public double CargoPayment { get; set; }
        [DisplayName("Customer Email Address")]
        public string CustomerID { get; set; }
     
        public int TransactionId { get; set; }
        public virtual Transactions transaction { get; set; }
       
        //public virtual Location Locations { get; set; }
        public int cargNo { get; set; }
        public virtual CargoType CrgType { get; set; }
        public ICollection<payment> payments { get; set; }
        ApplicationDbContext db = new ApplicationDbContext();
        public double calccargo()
        { var crgvalue = (from gh in db.cargoTypes
                          where gh.CargoTypeKey == cargNo
                          select gh.CargoValue).FirstOrDefault();

            return (crgvalue+(EstimatedTravellig*100));
        }

       
    }
    public class CargoType
    {


        public int CargoTypeKey { get; set; }

        public string CargoName { get; set; }

        public double CargoValue { get; set; }

        public virtual ICollection<CargoDetails> crgDetails { get; set; }

    }
    
    public class DriverTransactions
    {
        public string UserDetailsKey { get; set; }

       


        public int TransactionsKey { get; set; }

        public virtual Transactions transactions { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public bool DriverAvailabilityChecker()
        {
            bool check = false;
            var comebackdate = (from gh in db.driverTransactionsObj
                                 join jk in db.transactions
                                on gh.TransactionsKey equals jk.TransactionsKey
                                where gh.UserDetailsKey==UserDetailsKey && gh.TransactionsKey==TransactionsKey
                                select gh.UserDetailsKey).Count();

            if (comebackdate == 1)
            {
                check = true;
            }

            return (check);
        }
        public bool DriverChecker()
        {
            bool check = false;
            var comebackdate = (from gh in db.driverTransactionsObj
                                join jk in db.transactions
                               on gh.TransactionsKey equals jk.TransactionsKey
                                where gh.UserDetailsKey == UserDetailsKey
                                select jk.EstimatedDelivery).FirstOrDefault();

            var transDate = (from gh in db.transactions
                             where gh.TransactionsKey == TransactionsKey
                             select gh.EstimatedDelivery).FirstOrDefault();

            if (comebackdate > transDate)
            {
                check = true;
            }

            return (check);
        }
        public DateTime DriverAvailabilityDate()
        {
            var comebackdate = (from gh in db.driverTransactionsObj
                                join jk in db.transactions
                               on gh.TransactionsKey equals jk.TransactionsKey
                                where gh.UserDetailsKey == UserDetailsKey
                                select jk.EstimatedDelivery).FirstOrDefault();
            return (comebackdate);
        }

    }
    public class Location
    {
        public int LocationKey { get; set; }

        public string LocationName { get; set; }


        public virtual ICollection<CargoDetails> cargoDetails { get; set; }
    }
    

    public class Trucks
    {
        public int TrucksKey { get; set; }
        public string RegistrationNo { get; set; }

        // public string TruckModel { get; set; }

        //  public string TruckColour { get; set; }


        public string TruckStatus { get; set; }

        public string TruckPrice { get; set; }

        //public int MaxSpeed { get; set; }
        public virtual ICollection<Transactions> transactions { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public bool TruckRegChecker()
        {
            bool check = false;
            var comebackdate = (from gh in db.trucks
                                where gh.RegistrationNo == RegistrationNo
                                select gh.RegistrationNo).Count();

            if (comebackdate == 1)
            {
                check = true;
            }

            return (check);
        }


    }
  

    public class userRole
    {
        public string userKey { get; set; }
        public string roleKey { get; set; }
        
    }
    public class Transactions
    {

        public int TransactionsKey { get; set; }
       
        public DateTime EstimatedDelivery { get; set; }

        
       
        public DateTime StartDateTime { get; set; }

        

        public virtual ICollection<DriverTransactions> driverTransactions { get; set; }

        public virtual ICollection<CargoDetails> Cargos { get; set; }
        public int TrucksID { get; set; }
        public virtual Trucks trucks { get; set; }

        ApplicationDbContext db = new ApplicationDbContext();
        public bool TruckAvailabilityChecker()
        {
            bool check = false;
            var comebackdate = (from gh in db.transactions
                                where gh.EstimatedDelivery >= StartDateTime && gh.TrucksID == TrucksID
                                select gh.StartDateTime).Count();

            if (comebackdate > 1)
            {
                check = true;
            }

            return (check);
        }
        public DateTime TruckAvailabilidate()
        {

            var comebackdate = (from gh in db.transactions
                                where gh.EstimatedDelivery >= StartDateTime && gh.TrucksID == TrucksID
                                select gh.StartDateTime).FirstOrDefault();



            return (comebackdate);
        }

    }

    public class payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int paymentKey { get; set; }
      
        public int cargoID{ get; set; }

        
        public virtual CargoDetails cargodetails { get; set; }
        
        [Display(Name = "Payment Approval")]
        public string approval { get; set; }

        public byte[] File { get; set; }

       
       [Required]
        public double amountpayed { get; set; }
        [Display(Name = "Uploaded Date")]
        [DataType(DataType.Date)]
        public DateTime DocumentDate { get; set; } = DateTime.Now.Date;

        //public virtual MemberAccountDetails MemberAccount { get; set; }
        [Display(Name = "User Email")]
        public string usersID { get; set; }
      public enum Ptype
        {
            The_Slip_Is_not_Clear,
            Values_Do_not_Match,
            Approved
        }

    }



    
}