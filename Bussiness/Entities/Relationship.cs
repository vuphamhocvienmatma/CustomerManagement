using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Entities
{
    public class Relationship : EntityBase
    {
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }
        public RelationshipEnum Relationships { get; set; }
        public Guid CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public Customer Customer
        {
            get; set;
        }
        [Column(TypeName = "varchar(20)")]
        public string Phone { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "varchar(4)")]
        public string YearofBirth { get; set; }
        public string CurentAddress { get; set; }
    }

    // Quan hệ
    public enum RelationshipEnum
    {
        [Description("Vợ")]
        WIFE,
        [Description("Chồng")]
        HUSBAND,
        [Description("Bố đẻ")]
        FATHER,
        [Description("Mẹ đẻ")]
        MOTHER,
        [Description("Con")]
        CHILD,
        [Description("Ông nội")]
        GRANDFATHER,
        [Description("Ông ngoại")]
        GRANDMOTHER
    }

}
