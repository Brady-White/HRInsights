﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InsightsAPI.Entities
{
    public enum EmployeeStatus
    {
        Onboarding,
        Active,
        OnLeave
    }

    public class Employee
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Position { get; set; }
        public required DateTime DateOfHire { get; set; }
        public required decimal Salary { get; set; }
        public required EmployeeStatus Status { get; set; }
        public string Role { get; set; }
    }

}
