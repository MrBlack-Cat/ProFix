using Domain.BaseEntities;
using Domain.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class ServiceBooking : BaseEntity
{
    public int ClientProfileId { get; set; }
    public int ServiceProviderProfileId { get; set; }
    public string? Description { get; set; }
    public int StatusId { get; set; }
    public DateTime? ScheduledDate { get; set; }

    public ClientProfile ClientProfile { get; set; }
    public ServiceProviderProfile ServiceProviderProfile { get; set; }
    public ServiceBookingStatus Status { get; set; }
}
