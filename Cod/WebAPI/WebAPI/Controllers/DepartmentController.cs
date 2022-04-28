using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WebAPI.Models;
using System.Data.SqlClient;
using System.Net;
using WebAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly WebAPIDbContext _webAPIDbContext;

        public DepartmentController(WebAPIDbContext webAPIDbContext)
        {
            _webAPIDbContext = webAPIDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var _Departments =await _webAPIDbContext.Department.ToListAsync();
            return Ok(_Departments.ToArray());
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(Department department)
        {
            _webAPIDbContext.Department.Add(department);
            await _webAPIDbContext.SaveChangesAsync();
            return Created($"/get-department-by-id?id={department.DepartmentId}", department);
        }
        [HttpPut]
        public async Task<IActionResult> PutAsync(Department department)
        {
            _webAPIDbContext.Department.Update(department);
            await _webAPIDbContext.SaveChangesAsync();
            return NoContent();
        }
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var departmentToDelete = await _webAPIDbContext.Department.FindAsync(id);
            if (departmentToDelete == null)
            {
                return NotFound();
            }
            _webAPIDbContext.Department.Remove(departmentToDelete);
            await _webAPIDbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
