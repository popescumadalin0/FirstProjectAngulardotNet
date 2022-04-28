using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebAPI.net3.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;

namespace WebAPI.net3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {   
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        DataTable RunSqlCommands(string query)
        {
            DataTable table= new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            return table;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                select EmployeeId, EmployeeName, Department,
                convert(varchar(10),DateOfJoining,120) as DateOfJoining
                ,PhotoFileName
                from dbo.Employee";

            var table = RunSqlCommands(query);

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"
                insert into dbo.Employee
                (EmployeeName, Department, DateOfJoining, PhotoFileName)
                values
                (
                '" + employee.EmployeeName + @"',
                '" + employee.Department + @"',
                '" + employee.DateOfJoining + @"',
                '" + employee.PhotoFileName + @"'
                )
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Added Succesfully");
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"
                update dbo.Employee set
                EmployeeName = '" + employee.EmployeeName + @"',
                Department = '" + employee.Department + @"',
                DateOfJoining = '" + employee.DateOfJoining + @"'
                where EmployeeId = " + employee.EmployeeId + @"
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Updated Succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                delete from dbo.Employee
                where EmployeeId = " + id + @"
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Deleted Succesfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("anonymous.png");
            }
        }

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            string query = @"
                select DepartmentName from dbo.Department
            ";
            
            var table=RunSqlCommands(query);

            return new JsonResult(table);
        }
    }
}
