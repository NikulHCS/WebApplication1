using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EmployeeAPP.DAL.CModel
{
    public class EmployeeCModel
    {
        TestDBEntities context = new TestDBEntities();

        public long ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public int Country { get; set; }
        public bool Gender { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string FilePath { get; set; }
        public string FullName { get; set; }
        public HttpPostedFileBase file { get; set; }

        public long SaveEmployee(EmployeeCModel employeecModel)
        {
            Employee emp = new Employee();

            emp.Email = employeecModel.Email;
            emp.FirstName = employeecModel.FirstName;
            emp.LastName = employeecModel.LastName;
            emp.Birthdate = employeecModel.Birthdate;
            emp.Country = employeecModel.Country;
            emp.Gender = employeecModel.Gender;
            emp.Mobile = employeecModel.Mobile;
            emp.Password = employeecModel.Password;
            emp.FilePath = employeecModel.FilePath;

            context.Employees.Add(emp);
            context.SaveChanges();

            return emp.ID;
        }

        public List<EmployeeCModel> GetAllEmployeeList()
        {

            var employeelist =  (from emp in context.SP_GetEmployeeByBirthday()
                                  select new
                                  {
                                      ID = emp.ID,
                                      Birthdate = emp.Birthdate,
                                      FilePath = emp.FilePath,
                                      FullName = emp.FullName,
                                  }).AsEnumerable().Select(x => new EmployeeCModel()
                                  {
                                      ID = x.ID,
                                      Birthdate =x.Birthdate,
                                      FilePath = x.FilePath,
                                      FullName = x.FullName
                                  }).ToList();

            return employeelist;
        }

    }

}
