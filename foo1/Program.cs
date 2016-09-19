using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace foo1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            List<Company> companies = new List<Company>();
            List<Owner> owners = new List<Owner>();

            companies.Add(new Company(1, "Nokia", "Warszawa"));
            companies.Add(new Company(2, "Samsung", "Szczecin", 2));
            companies.Add(new Company(3, "Apple", "Warszawa", 1));

            employees.Add(new Employee(1, "Adam", "Nowak", 5000, 2));
            employees.Add(new Employee(2, "Daniel", "Kowalski", 7000, 1));          // 1 where // 2 where
            employees.Add(new Employee(3, "Franek", "Lewandowski", 8000, 3));       // 1 where
            employees.Add(new Employee(4, "Artur", "Kwiatkowski", 7000, 1));        // 1 where // 2 where
            employees.Add(new Employee(5, "Przemek", "Kowalczyk", 4000, 2));
            employees.Add(new Employee(6, "Bartosz", "Kowalczyk", 10000, 3));       // 1 where
            employees.Add(new Employee(7, "Filip", "Nowak", 8000, 2));              // 1 where // 2 where

            owners.Add(new Owner(1, "Marek", "Pawlicki", 80000));
            owners.Add(new Owner(2, "Filip", "Dawidowski", 40000));

            var chosenEmployee = employees.Join(companies, e => e.CompanyId, c => c.CompanyId, (e, c) =>
            new
            {
                EmployeeId = e.EmployeeId,
                EmployeeName = e.FirstName + " " + e.LastName,
                EmployeeSalary = e.Salary,
                CompanyName = c.Name,
                OwnerId = c.OwnerId
            })
                    .Where(x => x.EmployeeSalary > 5000)
                    .GroupJoin(owners,
                                e => e.OwnerId,
                                o => o.OwnerId,
                                (e, o) => new
                                {
                                    e,
                                    o
                                })
                    .SelectMany(a => a.o.DefaultIfEmpty(),
                                (a, o) => new
                                {
                                    EmployeeId = a.e.EmployeeId,
                                    EmployeeName = a.e.EmployeeName,
                                    EmployeeSalary = a.e.EmployeeSalary,
                                    CompanyName = a.e.CompanyName,
                                    OwnerName = o == null ? "No owner" : o.FirstName + " " + o.LastName,
                                    OwnerFunds = o == null ? 0 : o.Funds
                                })
                    .Where(a => a.OwnerFunds < 50000);

            foreach (var item in chosenEmployee)
            {
                Console.WriteLine(item.EmployeeId.ToString() + " " + item.CompanyName + " " + item.OwnerName);
            }
        }
    }

    class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Salary { get; set; }
        public int CompanyId { get; set; }

        public Employee()
        {

        }

        public Employee(int employeeId, string firstName, string lastName, int salary, int companyId)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Salary = salary;
            CompanyId = companyId;
        }

        public override string ToString()
        {
            return EmployeeId.ToString() + " " + FirstName + " " + CompanyId.ToString();
        }
    }

    class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int? OwnerId { get; set; }

        public Company()
        {

        }

        public Company(int companyId, string name, string city)
        {
            CompanyId = companyId;
            Name = name;
            City = city;
        }

        public Company(int companyId, string name, string city, int ownerId)
        {
            CompanyId = companyId;
            Name = name;
            City = city;
            OwnerId = ownerId;
        }

        public override string ToString()
        {
            return CompanyId.ToString() + " " + Name + " " + City;
        }
    }
    
    class Owner
    {
        public int OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Funds { get; set; }

        public Owner()
        {

        }

        public Owner(int ownerId, string firstName, string lastName, int funds)
        {
            OwnerId = ownerId;
            FirstName = firstName;
            LastName = lastName;
            Funds = funds;
        }
    }
}
