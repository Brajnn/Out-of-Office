﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Out_of_Office.Application.Employee;
using Out_of_Office.Application.Employee.Command.AssignProject;
using Out_of_Office.Application.Employee.Command.CreateEmployee;
using Out_of_Office.Application.Employee.Command.UpdateEmployeeCommand;
using Out_of_Office.Application.Employee.Command.UpdateEmployeeStatus;
using Out_of_Office.Application.Employee.Queries.GetAllEmployees;
using Out_of_Office.Application.Employee.Queries.GetEmployeeById;
using Out_of_Office.Application.Models.AssignProjectViewModel;
using Out_of_Office.Application.Project.Query.GetAllProjectsQuery;
using Out_of_Office.Domain.Interfaces;
using Out_of_Office.Infrastructure.Presistance;
using System.Security.Claims;
namespace Out_of_Office.Controllers
{
    [Authorize(Roles = "HR Manager,Project Manager,Administrator")]
    public class EmployeeController :Controller
    {
        private readonly IMediator _mediator;

        public EmployeeController(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.PositionSortParm = sortOrder == "Position" ? "position_desc" : "Position";


            ViewBag.CurrentFilter = searchString;

            var employees = await _mediator.Send(new GetAllEmployeesQuery());

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(e => e.FullName.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            employees = sortOrder switch
            {             
                "Name" => employees.OrderBy(e => e.FullName).ToList(),
                "name_desc" => employees.OrderByDescending(e => e.FullName).ToList(),             
                "Position" => employees.OrderBy(e => e.Position).ToList(),
                "position_desc" => employees.OrderByDescending(e => e.Position).ToList(),   
                _=>employees
            };

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var employee = await _mediator.Send(new GetEmployeeByIdQuery { Id = id });
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("CreateEmployee");
        }

        
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeCommand command)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View("CreateEmployee", command);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, bool isActive)
        {
            var command = new UpdateEmployeeStatusCommand
            {
                Id = id,
                Status = isActive ? "Active" : "Inactive"
            };
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var query = new GetEmployeeByIdQuery { Id = id };
            var employee = await _mediator.Send(query);
            if (employee == null)
            {
                return NotFound();
            }
            return View("EditEmployee", employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeDto employeeDto)
        {
            if (ModelState.IsValid)
            {
                var command = new UpdateEmployeeCommand
                {
                    Id = employeeDto.Id,
                    FullName = employeeDto.FullName,
                    Subdivision = employeeDto.Subdivision,
                    Position = employeeDto.Position,
                    Status = employeeDto.Status,
                    PeoplePartnerID = employeeDto.PeoplePartnerID,
                    OutOfOfficeBalance = employeeDto.OutOfOfficeBalance,
                    Photo = employeeDto.Photo
                };

                await _mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View("EditEmployee", employeeDto);
        }

       
    }
}
