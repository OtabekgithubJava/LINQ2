using System;
using System.Collections.Generic;
using System.Linq;
using LC.Model;

namespace LC
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<HR> hrList = new List<HR>()
            {
                new HR{ Id = 1, FirstName = "John", LastName = "Doe", Salary = 60000},
                new HR{ Id = 2, FirstName = "Jane", LastName = "Smith", Salary = 700000}
            };

            List<Marketing> marketingList = new List<Marketing>
            {
                new Marketing{ Id = 1, FirstName = "Alice", LastName = "Johnson", Campaign = "Spring Sale"},
                new Marketing{ Id = 2, FirstName = "Bob", LastName = "Williams", Campaign = "Holiday Special"}
            };

            List<Teacher> teacherList = new List<Teacher>
            {
                new Teacher{ Id = 1, FirstName = "Tom", LastName = "Anderson", Subject = "Math"},
                new Teacher{ Id = 2, FirstName = "Sara", LastName = "Clark", Subject = "Science"}
            };

            List<Student> studentList = new List<Student>
            {
                new Student{ Id = 1, FirstName = "Mike", LastName = "Davis", IsTuitionBased = true},
                new Student{ Id = 2, FirstName = "Emily", LastName = "Moore", IsTuitionBased = false}
            };

            // adding every list's sequences in to one sequence
            var employeeNames = hrList.Concat<Employee>(marketingList)
                .Concat(teacherList)
                .Concat(studentList)
                .Select(E => $"{E.FirstName} {E.LastName}");
            
            // Console.WriteLine("Employee names: ");
            // foreach (var name in employeeNames)
            // {
            //     Console.WriteLine(name);
            // }
            
            // SelectMany: Flatten the list of employees' full names
            var allNames = new List<string>();
            allNames.AddRange(hrList.Select(e => $"{e.FirstName} {e.LastName}"));
            allNames.AddRange(marketingList.Select(e => $"{e.FirstName} {e.LastName}"));
            allNames.AddRange(teacherList.Select(e => $"{e.FirstName} {e.LastName}"));
            allNames.AddRange(studentList.Select(e => $"{e.FirstName} {e.LastName}"));

            var topEmployees = allNames.Take(2); // the index inside “Take” means the index until itself
            var skippedEmployees = allNames.Skip(2); // the index inside “Skip” means the index from itself
            
            // Console.WriteLine("\nTop 2 Employee names: ");
            // foreach (var name in topEmployees)
            // {
            //     Console.WriteLine(name);
            // }
            //
            // Console.WriteLine("\nSkipped Employee names: ");
            // foreach (var name in skippedEmployees)
            // {
            //     Console.WriteLine(name);
            // }

            
            // ToLookup: Group employees by the first letter of their last name
            var employeeLookup = allNames.ToLookup(e => e[0]);
            
            // Console.WriteLine("\nEmployee Names Grouped by First Letter of Last Name:");
            // foreach (var group in employeeLookup)
            // {
            //     Console.WriteLine($"{group.Key}: {string.Join(", ", group)}");
            // }

            var tuitionGroups = studentList.GroupJoin(
                hrList.Concat<Employee>(marketingList).Concat(teacherList),
                student => student.IsTuitionBased,
                employee => employee is HR || employee is Marketing || employee is Teacher,
                (student, employees) => new { TuitionBased = student.IsTuitionBased, Employees = employees }
                );
            
            Console.WriteLine("\nStudents Grouped by Tuition Status:");
            foreach (var group in tuitionGroups)
            {
                Console.WriteLine($"{((bool)group.TuitionBased ? "Tuition-Based" : "Non Tuition-Based")}: " +
                                  $"{string.Join(", ", group.Employees.Select(e => $"{e.FirstName} {e.LastName}"))}");
            }
            
        }
    }
}

