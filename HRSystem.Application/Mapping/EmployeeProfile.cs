using AutoMapper;
using HRSystem.Application.DTOs.Position;
using HRSystem.Application.DTOs.Employees.Requests;
using HRSystem.Application.DTOs.Employees.Responses;
using HRSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRSystem.Application.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeCreateRequest, Employee>();
            CreateMap<EmployeeUpdateRequest, Employee>();
            CreateMap<Position, PositionResponse>().ReverseMap();
            CreateMap<Employee, EmployeeResponse>()
                        .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position.Name))
                        .ForMember(dest => dest.PositionId, opt => opt.MapFrom(src => src.PositionId));
        }
    }
}
