﻿using AutoMapper;
using Out_of_Office.Application.Approval_Request;
using Out_of_Office.Application.Employee;
using Out_of_Office.Application.Leave_Request;
using Out_of_Office.Application.Project;
using Out_of_Office.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Application.Mapping
{
    public class OfficeMappingProfile: Profile
    {
        public OfficeMappingProfile()
        {
            CreateMap<Domain.Entities.Employee, EmployeeDto>();

            CreateMap<ApprovalRequest, ApprovalRequestDto>()
           .ForMember(dest => dest.ApproverFullName, opt => opt.MapFrom(src => src.Approver != null ? src.Approver.FullName : string.Empty));
            CreateMap<LeaveRequest, LeaveRequestDto>()
           .ForMember(dest => dest.EmployeeFullName, opt => opt.MapFrom(src => src.Employee.FullName))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Domain.Entities.Project, ProjectDto>();
        }
    }
}
