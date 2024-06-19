﻿using MediatR;
using Out_of_Office.Domain.Entities;
using Out_of_Office.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Application.Employee.Command.CreateEmployee
{
    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public CreateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(request.Status, out EmployeeStatus employeeStatus))
            {
                throw new ArgumentException("Invalid employee status", nameof(request.Status));
            }
            var employee = new Domain.Entities.Employee
            {
                FullName = request.FullName,
                Subdivision = request.Subdivision,
                Position = request.Position,
                Status = employeeStatus,
                PeoplePartnerID = request.PeoplePartnerID,
                OutOfOfficeBalance = request.OutOfOfficeBalance,
                Photo = request.Photo
            };

            await _employeeRepository.AddEmployeeAsync(employee);

            return Unit.Value;
        }
    }
}
