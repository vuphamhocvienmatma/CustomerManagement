using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bussiness.Entities
{
    public class EntityBase
    {
        [Key] public Guid Id { get; set; } = Guid.NewGuid();
        [Required] public DateTime Created { get; set; } = DateTime.UtcNow;
        public Guid? CreatedBy { get; set; }
        public DateTime Updated { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
}
