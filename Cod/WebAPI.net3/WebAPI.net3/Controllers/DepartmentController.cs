using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebAPI.net3.Models;

namespace WebAPI.net3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        DataTable RunSqlCommands(string query)
        {
            DataTable table = new DataTable();

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

        public HttpResponseMessage Get()
        {
            string query = @"
                select DepartmentId, DepartmentName from dbo.Department";
            
            var table = RunSqlCommands(query);

            return Request.CreateResponse(HttpStatusCode.OK, table);
        }
        

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"
                insert into dbo.Department values ('" + department.DepartmentName + @"')
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Added Succesfully");
        }
        
        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"
                update dbo.Department 
                set DepartmentName = '" + department.DepartmentName + @"'
                where DepartmentId = "+ department.DepartmentId+@"
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Updated Succesfully");
        }
        
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                delete from dbo.Department 
                where DepartmentId = "+ id + @"
                ";

            _ = RunSqlCommands(query);

            return new JsonResult("Deleted Succesfully");
        }
        
    }
}
