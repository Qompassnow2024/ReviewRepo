using AutoMapper;
using MasterDataServices.DBServices;
using MasterDataServices.DTOModels.Employee;
using MasterDataServices.IControlServices;
using MasterDataServices.Models.Employee;
using MasterDataServices.Models.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MasterDataServices.Controllers
{
    // Author: Prashanth
    // Date: <20-Sep-2024>
    // Description: Employees Controller.

    [Authorize]
    [Route("QOM_API_V1/CMEmployees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly BaseDbContex DbContextAccess;
        private readonly IMapper _mapper;
        private readonly Respons_M response;
        private readonly IEmployeeService _employeeService;

        // Constructor for the EmployeesController class, which accepts dependencies via Dependency Injection.
        public EmployeesController(BaseDbContex dbContextAccess, IMapper mapper, IEmployeeService employeeService)
        {
            DbContextAccess = dbContextAccess;
            _mapper = mapper;
            _employeeService = employeeService;

            // Initialize the response object.
            response = new();
        }


        // Get Employees
        [HttpGet]
        [Route("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);

                // Retrieve all records from Employee
                var employees = await _employeeService.GetAllEmployeesAsync(orgId);
                if (!employees.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employees);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee By Id
        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);

                // Retrieve record from Employee
                var employeeExists = await _employeeService.GetEmployeeByIdAsync(id, orgId);
                if (!employeeExists.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeExists);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employees Pagination
        [HttpGet]
        [Route("GetEmployeesPagination")]
        public async Task<IActionResult> GetEmployeesPagination(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);

                // Get Employee Pagination
                var employeePagination = await _employeeService.GetEmployeePaginationAsync(pageNumber, pageSize, orgId);
                if (!employeePagination.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeePagination);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee
        [HttpPost]
        [Route("CreateEmployee")]
        public async Task<IActionResult> CreateEmployee([FromBody] Employees employees)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Create Employee
                var createEmployee = await _employeeService.CreateEmployeeAsync(employees, orgId, UserId);
                if (!createEmployee.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee
        [HttpPut]
        [Route("UpdateEmployeeById/{id}")]
        public async Task<IActionResult> UpdateEmployeeById(int id, [FromBody] Employees employees)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee
                var updateEmployee = await _employeeService.UpdateEmployeeByIdAsync(employees, orgId, UserId);
                if (!updateEmployee.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee
        [HttpDelete]
        [Route("DeleteEmployeeById/{id}")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);

                // Delete Employee
                var deleteEmployee = await _employeeService.DeleteEmployeeByIdAsync(orgId, id);
                if (!deleteEmployee.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Role
        [HttpGet]
        [Route("GetAllEmployeeRole")]
        public async Task<IActionResult> GetAllEmployeeRole()
        {
            try
            {
                // Retieve all records from Employee Role
                var employeeRole = await _employeeService.GetAllEmployeeRoleAsync();
                if (!employeeRole.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeRole);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Role By Id
        [HttpGet]
        [Route("GetEmployeeRoleById/{id}")]
        public async Task<IActionResult> GetEmployeeRoleById(int id)
        {
            try
            {
                // Retieve record from Employee Role
                var employeeRole = await _employeeService.GetEmployeeRoleByIdAsync(id);
                if (!employeeRole.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeRole);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee
        [HttpPost]
        [Route("CreateEmployeerole")]
        public async Task<IActionResult> CreateEmployeerole([FromBody] EmployeeRole employeeRole)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // 1. Fetch valid roles from the database (Roles table)
                var validRoles = await DbContextAccess.EmployeeRole.Select(r => r.role).ToListAsync();

                // 2. Check if the role is valid
                if (!validRoles.Contains(employeeRole.role))
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Invalid role. Please select a valid role.";
                    return BadRequest(response);
                }

                // 3. Check if employee already has a role
                var existingEmployeeRole = await DbContextAccess.EmployeeRole.FirstOrDefaultAsync(e => e.employee_id == employeeRole.employee_id);
                if (existingEmployeeRole == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee already has a role assigned.";
                    return Conflict(response);
                }

                // Create Employee Role
                var createEmployeeRole = await _employeeService.CreateEmployeeRoleAsync(employeeRole, UserId);
                if (!createEmployeeRole.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee Role
        [HttpPut]
        [Route("UpdateEmployeeRoleById/{id}")]
        public async Task<IActionResult> UpdateEmployeeRoleById(int id, [FromBody] EmployeeRole employeeRole)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee Role
                var updateEmployeeRole = await _employeeService.UpdateEmployeeRoleByIdAsync(employeeRole, id, UserId);
                if (!updateEmployeeRole.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee Role
        [HttpDelete]
        [Route("DeleteEmployeeRoleById/{id}")]
        public async Task<IActionResult> DeleteEmployeeRoleById(int id)
        {
            try
            {
                // Delete Employee Role
                var employeeRole = await _employeeService.DeleteEmployeeRoleByIdAsync(id);
                if (!employeeRole.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get All Employee Skill
        [HttpGet]
        [Route("GetAllEmployeeskill")]
        public async Task<IActionResult> GetAllEmployeeSkill()
        {
            try
            {
                // Retrieve all records from Employee Skill 
                var employeeSkill = await _employeeService.GetAllEmployeeSkillAsync();
                if (!employeeSkill.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeSkill);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Skill By Id
        [HttpGet]
        [Route("GetEmployeeSkillById/{id}")]
        public async Task<IActionResult> GetEmployeeSkillById(int id)
        {
            try
            {
                // Check if the Employee Skill exists
                var employeeSkill = await _employeeService.GetEmployeeSkillByIdAsync(id);
                if (!employeeSkill.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeSkill);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee Skill
        [HttpPost]
        [Route("CreateEmployeeSkill")]
        public async Task<IActionResult> CreateEmployeeSkill([FromBody] EmployeeSkill employeeSkill)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Create Employee Skill
                var createEmployeeSkill = await _employeeService.CreateEmployeeSkillAsync(employeeSkill, UserId);
                if
                (!createEmployeeSkill.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee Skill
        [HttpPut]
        [Route("UpdateEmployeeSkillById/{id}")]
        public async Task<IActionResult> UpdateEmployeeSkillById(int id, [FromBody] EmployeeSkill employeeSkill)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee Skill
                var updateEmployeeSkill = await _employeeService.UpdateEmployeeSkillAsync(employeeSkill, id, UserId);
                if (!updateEmployeeSkill.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee Skill
        [HttpDelete]
        [Route("DeleteEmployeeSkillById/{id}")]
        public async Task<IActionResult> DeleteEmployeeSkillById(int id)
        {
            try
            {
                // Delete Employee Skill
                var deleteEmployeeSkill = await _employeeService.DeleteEmployeeSkillAsync(id);
                if (!deleteEmployeeSkill.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Course
        [HttpGet]
        [Route("GetEmployeeCourse")]
        public async Task<IActionResult> GetEmployeeCourse()
        {
            try
            {
                // Retrieve all records from Employee Course
                var employeeCourse = await _employeeService.GetAllEmployeeCourseAsync();
                if (!employeeCourse.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeCourse);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Course By Id
        [HttpGet]
        [Route("GetEmployeeCourseById/{id}")]
        public async Task<IActionResult> GetEmployeeCourseById(int id)
        {
            try
            {
                // Retrieve record from Employee Course
                var employeeEmployeecourse = await _employeeService.GetEmployeeCourseByIdAsync(id);
                if (!employeeEmployeecourse.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeEmployeecourse);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee Course
        [HttpPost]
        [Route("CreateEmployeecourse")]
        public async Task<IActionResult> CreateEmployeecourse([FromBody] EmployeeCourse employeeCourse)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Create Employee Course
                var createEmployeeCourse = await _employeeService.CreateEmployeeCourseAsync(employeeCourse, UserId);
                if (!createEmployeeCourse.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee Course
        [HttpPut]
        [Route("UpdateEmployeeCourseById/{id}")]
        public async Task<IActionResult> UpdateEmployeeCourseById(int id, [FromBody] EmployeeCourse employeeCourse)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee Course
                var updateEmployeeCourse = await _employeeService.UpdateEmployeeCourseByIdAsync(employeeCourse, id, UserId);
                if (!updateEmployeeCourse.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee Course
        [HttpDelete]
        [Route("DeleteEmployeeCourseById/{id}")]
        public async Task<IActionResult> DeleteEmployeeCourseById(int id)
        {
            try
            {
                // Delete Employee Course 
                var deleteEmployeeCourse = await _employeeService.DeleteEmployeeCourseByIdAsync(id);
                if (!deleteEmployeeCourse.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee License
        [HttpGet]
        [Route("GetAllEmployeeLicense")]
        public async Task<IActionResult> GetAllEmployeeLicense()
        {
            try
            {
                // Retrieve all records from Employee License
                var employeeLicense = await _employeeService.GetAllEmployeeLicenseAsync();
                if (!employeeLicense.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeLicense);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee License By Id
        [HttpGet]
        [Route("GetEmployeeLicenseById/{id}")]
        public async Task<IActionResult> GetEmployeeLicenseById(int id)
        {
            try
            {
                // Retrieve record from Employee License
                var employeeLicense = await _employeeService.GetEmployeeLicenseByIdAsync(id);
                if (!employeeLicense.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeLicense);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee License
        [HttpPost]
        [Route("CreateEmployeeLicense")]
        public async Task<IActionResult> CreateEmployeeLicense([FromBody] EmployeeLicense employeeLicense)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Create Employee License
                var createEmployeeLicense = await _employeeService.CreateEmployeeLicenseAsync(employeeLicense, UserId);
                if (!createEmployeeLicense.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee License
        [HttpPut]
        [Route("UpdateEmployeeLicenseById/{id}")]
        public async Task<IActionResult> UpdateEmployeeLicenseById(int id, [FromBody] EmployeeLicense employeeLicense)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "Invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee License
                var updateEmployeeLicense = await _employeeService.UpdateEmployeeLicenseByIdAsync(employeeLicense, id, UserId);
                if (!updateEmployeeLicense.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee License
        [HttpDelete]
        [Route("DeleteEmployeeLicenseById/{id}")]
        public async Task<IActionResult> DeleteEmployeeLicenseById(int id)
        {
            try
            {
                // Delete Employee License
                var deleteEmployeeLicense = await _employeeService.DeleteEmployeeLicenseByIdAsync(id);
                if (!deleteEmployeeLicense.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Shift
        [HttpGet]
        [Route("GetAllEmployeeShift")]
        public async Task<IActionResult> GetAllEmployeeShift()
        {
            try
            {
                // Retrieve all records from Employee Shift
                var employeeShift = await _employeeService.GetAllEmployeeShiftAsync();
                if (!employeeShift.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeShift);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Get Employee Shift By Id
        [HttpGet]
        [Route("GetEmployeeShiftById/{id}")]
        public async Task<IActionResult> GetEmployeeShiftById(int id)
        {
            try
            {
                // Check if the Employee Shift exists
                var employeeShift = await _employeeService.GetEmployeeShiftByIdAsync(id);
                if (!employeeShift.Status)
                {
                    return BadRequest(response);
                }

                return Ok(employeeShift);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Create Employee Shift
        [HttpPost]
        [Route("CreateEmployeeshift")]
        public async Task<IActionResult> CreateEmployeeshift([FromBody] EmployeeShiftDto employeeShiftDto)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Create Employee Shift
                var createEmployeeShift = await _employeeService.CreateEmployeeShiftAsync(employeeShiftDto, UserId);
                if (!createEmployeeShift.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server errors
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Update Employee Shift
        [HttpPut]
        [Route("UpdateEmployeeShiftById/{id}")]
        public async Task<IActionResult> UpdateEmployeeShiftById(int id, [FromBody] EmployeeShiftDto employeeShiftDto)
        {
            // Model Validation
            if (!ModelState.IsValid)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = "invalid data.";
                return BadRequest(response);
            }

            try
            {
                // Retrieve the organization ID from the authenticated user's claims
                int orgId = Convert.ToInt32(User.FindFirst("COrgid")?.Value);
                int UserId = Convert.ToInt32(User.FindFirst("CUserid")?.Value);

                // Update Employee Shift
                var updateEmployeeShift = await _employeeService.UpdateEmployeeShiftByIdAsync(employeeShiftDto, id, UserId);
                if (!updateEmployeeShift.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }


        // Delete Employee Shift
        [HttpDelete]
        [Route("DeleteEmployeeShiftById/{id}")]
        public async Task<IActionResult> DeleteEmployeeShiftById(int id)
        {
            try
            {
                // Delete Employee Shift
                var updateEmployeeShift = await _employeeService.DeleteEmployeeShiftByIdAsync(id);
                if (!updateEmployeeShift.Status)
                {
                    return BadRequest(response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return an internal server error
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}.";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }
    }
}
