using RoleJwtApi1.Context;
using RoleJwtApi1.Models;

namespace RoleJwtApi1.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly JwtContext _context;

        public EmployeeService(JwtContext context)
        {
            _context = context;
        }

        public List<Employee> GetEmployeeDetails()
        {
            return _context.Employees.ToList();
        }

        public Employee AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }
    }
}
