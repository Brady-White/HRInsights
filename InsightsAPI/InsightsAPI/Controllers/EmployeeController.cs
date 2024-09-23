﻿using InsightsAPI.Data;
using InsightsAPI.Entities;
using InsightsAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsightsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository) {
            _employeeRepository = employeeRepository;
        }

        [Authorize(Roles = "HR Administrator, Employee")]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetEmployeesAsync();

            if (employees == null || !employees.Any())
            {
                return NotFound(new { message = "No employees found." });
            }

            return Ok(new
            {
                message = "Employees retrieved successfully.",
                employees = employees
            });

        }
        [Authorize(Roles = "HR Administator, Employee")]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<Employee>>> GetEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(new
            {
                message = "Employee retrieved successfully.",
                employee = employee
            });

        }
        [Authorize(Roles = "HR Administator")]
        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee([FromBody] Employee employee)
        {
            var addedEmployee = await _employeeRepository.AddEmployeeAsync(employee);

            if (addedEmployee == null)
            {
                return BadRequest("Invalid employee data.");
            }

            return CreatedAtAction(nameof(GetEmployee), new { id = addedEmployee.EmployeeId }, new
            {
                message = "Employee successfully created.",
                employee = addedEmployee
            });
        }
        [Authorize(Roles = "HR Administator")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return BadRequest(new { message = "Employee ID mismatch." });
            }

            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);

            if (updatedEmployee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            return Ok(new { 
                message = "Employee updated successfully.", 
                employee = updatedEmployee 
            }
            );
        }
        [Authorize(Roles = "HR Administator")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.GetEmployeeAsync(id);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            bool deleteResult = await _employeeRepository.DeleteEmployeeAsync(id);

            if (!deleteResult)
            {
                return BadRequest(new { message = "Error deleting employee." });
            }

            return Ok(new { message = "Employee deleted successfully." });
        }

    }
}
