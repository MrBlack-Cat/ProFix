using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.BaseEntities
{
    public abstract class BaseEntity
    {

     
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }


        //yeni deyishiklikler 
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public string? DeletedReason { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
