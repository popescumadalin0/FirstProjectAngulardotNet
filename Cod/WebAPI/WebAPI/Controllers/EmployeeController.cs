using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entities;
using WebAPI.Models;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly WebAPIDbContext _webAPIDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EmployeeController(WebAPIDbContext webAPIDbContext, IWebHostEnvironment webHostEnvironment)
        {
            _webAPIDbContext = webAPIDbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var _Employees = await _webAPIDbContext.Employee.Select(s => new {
                EmployeeId = s.EmployeeId,
                EmployeeName = s.EmployeeName,
                Department = s.Department,
                DateOfJoining = s.DateOfJoining.ToString(),
                PhotoFileName = s.PhotoFileName
            })
                .ToListAsync();
            return Ok(_Employees);
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(Employee Employee)
        {
            _webAPIDbContext.Employee.Add(Employee);
            await _webAPIDbContext.SaveChangesAsync();
            return Created($"/get-Employee-by-id?id={Employee.EmployeeId}", Employee);
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(Employee Employee)
        {
            _webAPIDbContext.Employee.Update(Employee);
            await _webAPIDbContext.SaveChangesAsync();
            return NoContent();
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var EmployeeToDelete = await _webAPIDbContext.Employee.FindAsync(id);
            if (EmployeeToDelete == null)
            {
                return NotFound();
            }
            _webAPIDbContext.Employee.Remove(EmployeeToDelete);
            await _webAPIDbContext.SaveChangesAsync();
            return NoContent();
        }
        [Route("GetAllDepartmentNames")]
        [HttpGet]
        public async Task<IActionResult> GetAllDepartmentNamesAsync()
        {
            var _Departments = await _webAPIDbContext.Department.Select(s => new { DepartmentName = s.DepartmentName })
                .ToListAsync();
            return Ok(_Departments);
        }

        [Route("SaveFile")]
        [HttpPost]
        public IActionResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                if (postedFile != null)
                {
                    string filename = postedFile.FileName;
                    var physicalPath = _webHostEnvironment.ContentRootPath + "/Photos/" + filename;

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                    }

                    return Ok(filename);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception)
            {
                return Ok("anonymous.png");
            }
        }
    }
}
