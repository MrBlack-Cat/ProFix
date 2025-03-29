﻿using Application.CQRS.SubscriptionPlans.DTOs;
using Common.GlobalResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.CQRS.SubscriptionPlans.Queries.Responses;


public class GetListSubscriptionPlanResponse : ResponseModel<List<SubscriptionPlanListDto>>
{
    public GetListSubscriptionPlanResponse(List<SubscriptionPlanListDto> data)
    {
        Data = data;
        IsSuccess = true;
    }
}