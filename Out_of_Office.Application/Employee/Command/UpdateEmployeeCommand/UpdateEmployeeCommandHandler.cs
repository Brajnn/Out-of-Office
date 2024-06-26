﻿using MediatR;
using Out_of_Office.Domain.Entities;
using Out_of_Office.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Out_of_Office.Application.Employee.Command.UpdateEmployeeCommand
{
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public UpdateEmployeeCommandHandler(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task<Unit> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(request.Status, out EmployeeStatus employeeStatus))
            {
                throw new ArgumentException("Invalid employee status", nameof(request.Status));
            }

            var employee = await _employeeRepository.GetEmployeeByIdAsync(request.Id);
            if (employee == null)
            {
                throw new ArgumentException("Employee not found", nameof(request.Id));
            }

            employee.FullName = request.FullName;
            employee.Subdivision = request.Subdivision;
            employee.Position = request.Position;
            employee.Status = employeeStatus;
            employee.PeoplePartnerID = request.PeoplePartnerID;
            employee.OutOfOfficeBalance = request.OutOfOfficeBalance;
            employee.Photo = request.Photo;

            await _employeeRepository.UpdateEmployeeAsync(employee);

            return Unit.Value;
        }
    }
}
