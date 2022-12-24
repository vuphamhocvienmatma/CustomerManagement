using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Bussiness.Entities
{

    [Table("Customer")]
    public class Customer : EntityBase
    {
        [StringLength(150)] public string FullName { get; set; }
        [StringLength(50)] public string CustomerCode { get; set; }
        [StringLength(100)] public string ShortName { get; set; }
        [StringLength(100)] public string Aliases { get; set; }
        public string CitizenID { get; set; }
        [StringLength(250)] public string IssuedBy { get; set; }
        public Gender Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int? NumberChildren { get; set; }
        [StringLength(20)] public string PhoneNumber { get; set; }
        [StringLength(20)] public string OtherPhoneNumber { get; set; }
        [StringLength(150)] public string Email { get; set; }
        [StringLength(150)] public string Facebook { get; set; }
        [StringLength(150)] public string Skype { get; set; }
        [StringLength(150)] public string Zalo { get; set; }
        [StringLength(150)] public string Twitter { get; set; }
        [StringLength(150)] public string WorkPlace { get; set; }
        [StringLength(150)] public string HouseholdRegistrationNumber { get; set; }
        public decimal? TotalMonthlyIncome { get; set; }
        public decimal? MainIncome { get; set; }
        public decimal? SideIncome { get; set; }
        public decimal? TotalMonthlySpending { get; set; }
        public decimal? Income { get; set; }
        public ICollection<Relationship> Relationships { get; set; }

    }
    
    // Giới tính
    public enum Gender
    {
        [Description("Nam")]
        MALE,
        [Description("Nữ")]
        FEMALE,
        [Description("Khác")]
        OTHER
    }
}
