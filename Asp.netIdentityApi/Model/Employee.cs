using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Model
{
    public class Employee
    {
        // by default because Id was prefix with the Class Name
        // efcore will chose it as the Primary kEY
        public int EmployeeId { get; set; }
        public string FirstName  { get; set; }
        public string LastName  { get; set; }
        public string Email  { get; set; }
        public string PhoneNumber { get; set; }
    }
}
