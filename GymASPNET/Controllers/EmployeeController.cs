using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GymASPNET.Models;

namespace GymASPNET.Controllers
{
    [RoutePrefix("api/EmployeeUser")]
    public class EmployeeController : ApiController
    {
        //DbContext instance 
        GYMEntitiesGlobal dbContext = new GYMEntitiesGlobal();


        [HttpPost]
        [Route("createEmployee")]
        public IHttpActionResult CreateEmployee(Employee employee)
        {
            try
            {
                employee.CreatedOn = DateTime.Now;
                dbContext.Employees.Add(employee);
                dbContext.SaveChanges();
                return Content(HttpStatusCode.OK, new { obj = employee, request = "Process completed success" });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.OK, new { obj = employee, request = ex.Message });
            }
        }

        [HttpGet]
        [Route("employeeList")]
        public IHttpActionResult EmployeeList()
        {
            try
            {
                var result = dbContext.Employees.ToList();
                return Content(HttpStatusCode.OK, new { obj = result, request = "Process completed success" });
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.OK, new { obj = "", request = ex.Message });
            }
        }
    }
}