using Domain.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Post : BaseEntity
{

    public int ServiceProviderProfileId { get; set; }
    public string Title { get; set; } = null!;
    public string? Content { get; set; }
    public string? ImageUrl { get; set; }

    public ServiceProviderProfile? ServiceProviderProfile { get; set; }

    //yeni elave 
    public bool IsActive { get; set; } 
}
