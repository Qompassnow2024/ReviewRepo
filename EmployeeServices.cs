using AutoMapper;
using MasterDataServices.DBServices;
using MasterDataServices.DTOModels.Employee;
using MasterDataServices.IControlServices;
using MasterDataServices.Models.Employee;
using MasterDataServices.Models.ResponseModels;
using Microsoft.EntityFrameworkCore;

namespace MasterDataServices.ControlServices
{
    // Author: Prashanth
    // Date: <20-Sep-2024>
    // Description: EmployeeServices.
    public class EmployeeServices : IEmployeeService
    {
        private readonly BaseDbContex DbContextAccess;
        private readonly IMapper _mapper;
        private readonly Respons_M response;

        public EmployeeServices(BaseDbContex dbContextAccess, IMapper mapper)
        {
            DbContextAccess = dbContextAccess;
            _mapper = mapper;

            // Initialize the response object.
            response = new();
        }


        // Get All Employees
        public async Task<Respons_M> GetAllEmployeesAsync(int orgId)
        {
            try
            {
                // Retrieve all records from Employee
                var employees = await DbContextAccess.Employee
                    .Where(e => e.organization_id == orgId).ToListAsync();

                response.Status = true;
                response.Data = employees;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee By Id
        public async Task<Respons_M> GetEmployeeByIdAsync(int orgId, int employeeId)
        {
            try
            {
                // Check if the Employee exists
                var employeeExists = await DbContextAccess.Employee
                    .Where(e => e.employee_id == employeeId).SingleOrDefaultAsync();

                if (employeeExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee not found.";
                    response.Data = null;
                }

                response.Status = true;
                response.Data = employeeExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee Pagination
        public async Task<Respons_M> GetEmployeePaginationAsync(int pageNumber, int pageSize, int orgId)
        {
            try
            {
                // Get the total number of Employee records from the database
                var totalEmployees = await DbContextAccess.Employee
                    .Where(e => e.organization_id == orgId)
                    .CountAsync();

                // Retrieve the Employee records for the current page, skipping the previous records and taking the next set based on pageSize
                var employees = await DbContextAccess.Employee
                    .Where(e => e.organization_id == orgId)
                    .Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                // Prepare the paginated response
                var paginationResult = new
                {
                    totalEmployees = totalEmployees,
                    pageSize = pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling((double)totalEmployees / pageSize),
                    employees = employees
                };

                response.Status = true;
                response.Data = paginationResult;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Create Employee
        public async Task<Respons_M> CreateEmployeeAsync(Employees employees, int orgId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee exists
                    var validEmployeeTypes = await DbContextAccess.Employee
                        .Select(e => e.employee_type).ToListAsync();

                    if (!validEmployeeTypes.Contains(employees.employee_type))
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Invalid employee type. Please select a valid employee type.";
                        response.Data = null;
                    }

                    // Create Employee
                    var employee = new Employees
                    {
                        employee_type = employees.employee_type,
                        vendor_id = employees.vendor_id,
                        first_name = employees.first_name,
                        middle_name = employees.middle_name,
                        last_name = employees.last_name,
                        work_email = employees.work_email,
                        date_of_birth = employees.date_of_birth,
                        gender = employees.gender,
                        marital_status = employees.marital_status,
                        blood_group = employees.blood_group,
                        physically_handicapped = employees.physically_handicapped,
                        mobile_phone = employees.mobile_phone,
                        home_phone = employees.home_phone,
                        personal_email = employees.personal_email,
                        current_address_id = employees.current_address_id,
                        permanent_address_id = employees.permanent_address_id,
                        job_title = employees.job_title,
                        reporting_to_employee_id = employees.reporting_to_employee_id,
                        join_date = employees.join_date,
                        availability = employees.availability,
                        organization_id = orgId,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow,
                        others = employees.others,
                    };

                    // Create the Employee and save changes
                    DbContextAccess.Employee.Add(employees);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee created successfully.";
                    response.Data = employees;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
            }

            return response;
        }


        // Update Employee By Id
        public async Task<Respons_M> UpdateEmployeeByIdAsync(Employees employees, int employeeId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee exists
                    var employeeExists = await DbContextAccess.Employee
                        .Where(e => e.employee_id == employeeId).SingleOrDefaultAsync();

                    if (employeeExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee not found.";
                        response.Data = null;
                    }

                    // Update Employee
                    employeeExists.employee_type = employees.employee_type;
                    employeeExists.vendor_id = employees.vendor_id;
                    employeeExists.first_name = employees.first_name;
                    employeeExists.middle_name = employees.middle_name;
                    employeeExists.last_name = employees.last_name;
                    employeeExists.work_email = employees.work_email;
                    employeeExists.date_of_birth = employees.date_of_birth;
                    employeeExists.gender = employees.gender;
                    employeeExists.marital_status = employees.marital_status;
                    employeeExists.blood_group = employees.blood_group;
                    employeeExists.physically_handicapped = employees.physically_handicapped;
                    employeeExists.mobile_phone = employees.mobile_phone;
                    employeeExists.home_phone = employees.home_phone;
                    employeeExists.personal_email = employees.personal_email;
                    employeeExists.current_address_id = employees.current_address_id;
                    employeeExists.permanent_address_id = employees.permanent_address_id;
                    employeeExists.job_title = employees.job_title;
                    employeeExists.reporting_to_employee_id = employees.reporting_to_employee_id;
                    employeeExists.join_date = employees.join_date;
                    employeeExists.availability = employees.availability;
                    employeeExists.last_modified_by = userId;
                    employeeExists.last_modified_on = DateTime.UtcNow;
                    employeeExists.others = employees.others;

                    // Update the Employee and save changes
                    DbContextAccess.Employee.Update(employeeExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction 
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee updated successfully.";
                    response.Data = employeeExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Delete Employee By Id
        public async Task<Respons_M> DeleteEmployeeByIdAsync(int orgId, int employeeId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee exists
                    var employeeExists = await DbContextAccess.Employee
                        .Where(e => e.employee_id == employeeId & e.organization_id == orgId)
                        .SingleOrDefaultAsync();

                    if (employeeExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee not found.";
                        response.Data = null;
                    }

                    // Remove the Employee and save changes
                    DbContextAccess.Employee.Remove(employeeExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the tranaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee deleted successfully.";
                    response.Data = employeeExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Get All Employee Role
        public async Task<Respons_M> GetAllEmployeeRoleAsync()
        {
            try
            {
                // Retieve all records from Employee Role
                var employeeRole = await DbContextAccess.EmployeeRole.ToListAsync();

                response.Status = true;
                response.Data = employeeRole;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee Role By Id
        public async Task<Respons_M> GetEmployeeRoleByIdAsync(int employeeRoleId)
        {
            try
            {
                // Check if the Employee Role exists
                var employeeRoleExists = await DbContextAccess.EmployeeRole
                    .Where(e => e.employee_role_id == employeeRoleId)
                    .SingleOrDefaultAsync();

                if (employeeRoleExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee Role not found.";
                    response.Data = null;
                }

                response.Status = true;
                response.Data = employeeRoleExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Create Employee 
        public async Task<Respons_M> CreateEmployeeRoleAsync(EmployeeRole employeeRole, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Fetch valid roles from the database (Roles table)
                    var validRoles = await DbContextAccess.EmployeeRole.Select(r => r.role).ToListAsync();

                    // 2. Check if the role is valid
                    if (!validRoles.Contains(employeeRole.role))
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Invalid role. Please select a valid role.";
                        response.Data = null;
                    }

                    // 3. Check if employee already has a role
                    var existingEmployeeRole = await DbContextAccess.EmployeeRole
                        .SingleOrDefaultAsync(e => e.employee_id == employeeRole.employee_id);

                    if (existingEmployeeRole == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee already has a role assigned.";
                        response.Data = null;
                    }

                    // Create Employee Role
                    var employeerole = new EmployeeRole
                    {
                        employee_id = employeeRole.employee_id,
                        role = employeeRole.role,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow
                    };

                    // Create the EmployeeRole and save changes
                    DbContextAccess.EmployeeRole.Add(employeeRole);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee role created successfully.";
                    response.Data = employeerole;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Update Employee Role By Id
        public async Task<Respons_M> UpdateEmployeeRoleByIdAsync(EmployeeRole employeeRole, int employeeRoleId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Role exists
                    var employeeRoleExists = await DbContextAccess.EmployeeRole
                        .Where(e => e.employee_role_id == employeeRoleId)
                        .SingleOrDefaultAsync();

                    if (employeeRoleExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Role not found.";
                        response.Data = null;
                    }

                    // Update Employee Role
                    employeeRoleExists.employee_id = employeeRole.employee_id;
                    employeeRoleExists.role = employeeRole.role;
                    employeeRoleExists.last_modified_by = userId;
                    employeeRoleExists.last_modified_on = DateTime.UtcNow;

                    // Update the EmployeeRole and save changes
                    DbContextAccess.EmployeeRole.Update(employeeRoleExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Role updated successfully.";
                    response.Data = employeeRoleExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Delete Employee Role By Id
        public async Task<Respons_M> DeleteEmployeeRoleByIdAsync(int employeeRoleId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Role exists
                    var employeeroleExists = await DbContextAccess.EmployeeRole
                        .Where(e => e.employee_role_id == employeeRoleId)
                        .SingleOrDefaultAsync();

                    if (employeeroleExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Role not found.";
                        response.Data = null;
                    }

                    // Remove the EmployeeRole and save changes
                    DbContextAccess.EmployeeRole.Remove(employeeroleExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Role deleted successfully.";
                    response.Data = employeeroleExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Get All Employee Skill 
        public async Task<Respons_M> GetAllEmployeeSkillAsync()
        {
            try
            {
                // Retrieve all records from Employee Skill 
                var employeeSkill = await DbContextAccess.EmployeeSkill.ToListAsync();

                response.Status = true;
                response.Data = employeeSkill;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee Skill By Id
        public async Task<Respons_M> GetEmployeeSkillByIdAsync(int employeeSkillId)
        {
            try
            {
                // Check if the Employee Skill exists
                var employeeSkillExists = await DbContextAccess.EmployeeSkill
                    .Where(e => e.employee_skill_id == employeeSkillId)
                    .SingleOrDefaultAsync();

                if (employeeSkillExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee Skill not found.";
                    response.Data = null;
                }

                response.Status = true;
                response.Data = employeeSkillExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Create Employee Skill
        public async Task<Respons_M> CreateEmployeeSkillAsync(EmployeeSkill employeeSkill, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Employee Skill
                    var employeeskill = new EmployeeSkill
                    {
                        employee_id = employeeSkill.employee_id,
                        skill = employeeSkill.skill,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow
                    };

                    // Create the Employee Skill and save changes
                    DbContextAccess.EmployeeSkill.Add(employeeSkill);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Skill created successfully.";
                    response.Data = employeeskill;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Update Employee Skill
        public async Task<Respons_M> UpdateEmployeeSkillAsync(EmployeeSkill employeeSkill, int employeeSkillId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Skill exists
                    var employeeSkillExists = await DbContextAccess.EmployeeSkill
                        .Where(e => e.employee_skill_id == employeeSkillId)
                        .SingleOrDefaultAsync();

                    if (employeeSkillExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Skill not found.";
                        response.Data = null;
                    }

                    // Update Employee Skill
                    employeeSkillExists.employee_id = employeeSkill.employee_id;
                    employeeSkillExists.skill = employeeSkill.skill;
                    employeeSkillExists.last_modified_by = userId;
                    employeeSkillExists.last_modified_on = DateTime.UtcNow;

                    // Update the Employee Skill and save changes
                    DbContextAccess.EmployeeSkill.Update(employeeSkillExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee skill updated successfully.";
                    response.Data = employeeSkillExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Delete Employee Skill
        public async Task<Respons_M> DeleteEmployeeSkillAsync(int employeeSkillId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Skill exists
                    var employeeSkillExists = await DbContextAccess.EmployeeSkill
                        .Where(e => e.employee_skill_id == employeeSkillId)
                        .SingleOrDefaultAsync();

                    if (employeeSkillExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Skill not found.";
                        response.Data = null;
                    }

                    // Remove the Employee Skill and save changes
                    DbContextAccess.EmployeeSkill.Remove(employeeSkillExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Skill deleted successfully.";
                    response.Data = employeeSkillExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Get All Employee Course 
        public async Task<Respons_M> GetAllEmployeeCourseAsync()
        {
            try
            {
                // Retrieve all records from Employee Course
                var employeeCourse = await DbContextAccess.EmployeeCourse.ToListAsync();

                response.Status = true;
                response.Data = employeeCourse;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee Course By Id
        public async Task<Respons_M> GetEmployeeCourseByIdAsync(int employeeCourse)
        {
            try
            {
                // Check if the Employee Course exists
                var employeeCourseExists = await DbContextAccess.EmployeeCourse
                    .Where(e => e.employee_course_id == employeeCourse)
                    .SingleOrDefaultAsync();

                if (employeeCourseExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee Course not found.";
                    response.Data = null;
                }

                response.Status = true;
                response.Data = employeeCourseExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Create Employee Course
        public async Task<Respons_M> CreateEmployeeCourseAsync(EmployeeCourse employeeCourse, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Employee Course
                    var employeecourse = new EmployeeCourse
                    {
                        employee_id = employeeCourse.employee_id,
                        course = employeeCourse.course,
                        trained_from = employeeCourse.trained_from,
                        trained_to = employeeCourse.trained_to,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow
                    };

                    // Create the Employee Course and save changes
                    DbContextAccess.EmployeeCourse.Add(employeecourse);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Course created successfully.";
                    response.Data = employeecourse;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Update Employee Course By Id
        public async Task<Respons_M> UpdateEmployeeCourseByIdAsync(EmployeeCourse employeeCourse, int employeeCourseId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Course exists
                    var employeeCourseExists = await DbContextAccess.EmployeeCourse
                        .Where(e => e.employee_course_id == employeeCourseId)
                        .SingleOrDefaultAsync();

                    if (employeeCourseExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Course not found.";
                        response.Data = null;
                    }

                    // Update Employee Course
                    employeeCourseExists.employee_id = employeeCourse.employee_id;
                    employeeCourseExists.course = employeeCourse.course;
                    employeeCourseExists.trained_from = employeeCourse.trained_from;
                    employeeCourseExists.trained_to = employeeCourse.trained_to;
                    employeeCourseExists.last_modified_by = userId;
                    employeeCourseExists.last_modified_on = DateTime.UtcNow;

                    // Update the Employee Course and save changes
                    DbContextAccess.EmployeeCourse.Update(employeeCourseExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Course updated successfully.";
                    response.Data = employeeCourseExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }


            return response;
        }


        // Delete Employee Skill By Id 
        public async Task<Respons_M> DeleteEmployeeCourseByIdAsync(int employeeCourseId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Course exists
                    var employeeCourseExists = await DbContextAccess.EmployeeCourse
                        .Where(e => e.employee_course_id == employeeCourseId)
                        .SingleOrDefaultAsync();

                    if (employeeCourseExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found 
                        response.StatusMsg = "Employee Course not found.";
                        response.Data = null;
                    }

                    // Remove the Employee Course and save changes
                    DbContextAccess.EmployeeCourse.Remove(employeeCourseExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Course deleted successfully.";
                    response.Data = employeeCourseExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }


            return response;
        }


        // Get All Employee License
        public async Task<Respons_M> GetAllEmployeeLicenseAsync()
        {
            try
            {
                // Retrieve all records from Employee License
                var employeeLicense = await DbContextAccess.EmployeeLicense.ToListAsync();

                response.Status = true;
                response.Data = employeeLicense;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee License By Id
        public async Task<Respons_M> GetEmployeeLicenseByIdAsync(int employeLicenseId)
        {
            try
            {
                // Check if the Employee License exists
                var employeeLicenseExists = await DbContextAccess.EmployeeLicense
                    .Where(e => e.employee_license_id == employeLicenseId)
                    .SingleOrDefaultAsync();

                if (employeeLicenseExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee License not found";
                    response.Data = employeeLicenseExists;
                }

                response.Status = true;
                response.Data = employeeLicenseExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }

        public async Task<Respons_M> CreateEmployeeLicenseAsync(EmployeeLicense employeeLicense, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Employee License
                    var employeelicense = new EmployeeLicense
                    {
                        employee_id = employeeLicense.employee_id,
                        license_type = employeeLicense.license_type,
                        issue_date = employeeLicense.issue_date,
                        due_date = employeeLicense.due_date,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow
                    };

                    // Create the Employee License and save changes
                    DbContextAccess.EmployeeLicense.Add(employeeLicense);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee License created successfully.";
                    response.Data = employeelicense;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Update Employee License By Id
        public async Task<Respons_M> UpdateEmployeeLicenseByIdAsync(EmployeeLicense employeeLicense, int employeeLicenseId, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee License exists
                    var employeeLicenseExists = await DbContextAccess.EmployeeLicense
                        .Where(e => e.employee_license_id == employeeLicenseId)
                        .SingleOrDefaultAsync();

                    if (employeeLicenseExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found 
                        response.StatusMsg = "Employee License not found.";
                        response.Data = null;
                    }

                    // Update Employee License
                    employeeLicenseExists.employee_id = employeeLicense.employee_id;
                    employeeLicenseExists.license_type = employeeLicense.license_type;
                    employeeLicenseExists.issue_date = employeeLicense.issue_date;
                    employeeLicenseExists.due_date = employeeLicense.due_date;
                    employeeLicenseExists.last_modified_by = userId;
                    employeeLicenseExists.last_modified_on = DateTime.UtcNow;

                    // Update the Employee License and save changes
                    DbContextAccess.EmployeeLicense.Update(employeeLicenseExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee License updated successfully.";
                    response.Data = employeeLicenseExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Delete Employee License By Id
        public async Task<Respons_M> DeleteEmployeeLicenseByIdAsync(int employeLicenseId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee License exists
                    var employeeLicenseExists = await DbContextAccess.EmployeeLicense
                        .Where(e => e.employee_license_id == employeLicenseId)
                        .SingleOrDefaultAsync();

                    if (employeeLicenseExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found 
                        response.StatusMsg = "Employee License not found.";
                        response.Data = null;
                    }

                    // Remove the Employee License and save changes
                    DbContextAccess.EmployeeLicense.Remove(employeeLicenseExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee License deleted successfully.";
                    response.Data = employeeLicenseExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Get All Employee Shift
        public async Task<Respons_M> GetAllEmployeeShiftAsync()
        {
            try
            {
                // Retrieve all records from Employee Shift
                var employeeShift = await DbContextAccess.EmployeeShift.ToListAsync();

                response.Status = true;
                response.Data = employeeShift;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Get Employee Shift By Id
        public async Task<Respons_M> GetEmployeeShiftByIdAsync(int employeeShiftId)
        {
            try
            {
                // Check if the Employee Shift exists
                var employeeShiftExists = await DbContextAccess.EmployeeShift
                    .Where(e => e.employee_shift_id == employeeShiftId)
                    .SingleOrDefaultAsync();

                if (employeeShiftExists == null)
                {
                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = "Employee Shift not found.";
                    response.Data = employeeShiftExists;
                }

                response.Status = true;
                response.Data = employeeShiftExists;
            }
            catch (Exception ex)
            {
                response.Status = false; // Set to false since it wasn't found
                response.StatusMsg = $"Error: {ex.Message}";
                response.Data = null;
            }

            return response;
        }


        // Create Employee Shift
        public async Task<Respons_M> CreateEmployeeShiftAsync(EmployeeShiftDto employeeShiftDto, int userId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Create Employee Shift
                    var employeeShift = new EmployeeShift
                    {
                        employee_id = employeeShiftDto.employee_id,
                        shift_type = employeeShiftDto.shift_type,
                        shift_from = employeeShiftDto.shift_from.TimeOfDay,
                        shift_to = employeeShiftDto.shift_to.TimeOfDay,
                        maker_id = userId,
                        make_time = DateTime.UtcNow,
                        last_modified_by = userId,
                        last_modified_on = DateTime.UtcNow
                    };

                    // Create the Employee Shift and save changes
                    DbContextAccess.EmployeeShift.Add(employeeShift);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Shift created successfully.";
                    response.Data = employeeShiftDto;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
            }

            return response;
        }


        // Update Employee Shift By Id
        public async Task<Respons_M> UpdateEmployeeShiftByIdAsync(EmployeeShiftDto employeeShiftDto, int employeeShiftId, int userId)
        {
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Shift exists
                    var employeeShiftExists = await DbContextAccess.EmployeeShift
                        .Where(e => e.employee_shift_id == employeeShiftId)
                        .SingleOrDefaultAsync();

                    if (employeeShiftExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found
                        response.StatusMsg = "Employee Shift not found.";
                        response.Data = null;
                    }

                    // Update Employee Shift
                    employeeShiftExists.employee_id = employeeShiftDto.employee_id;
                    employeeShiftExists.shift_type = employeeShiftDto.shift_type;
                    employeeShiftExists.shift_from = employeeShiftDto.shift_from.TimeOfDay;
                    employeeShiftExists.shift_to = employeeShiftDto.shift_to.TimeOfDay;
                    employeeShiftExists.last_modified_by = userId;
                    employeeShiftExists.last_modified_on = DateTime.UtcNow;

                    // Update the Employee Shift and save changes
                    DbContextAccess.EmployeeShift.Update(employeeShiftExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Shift updated successfully.";
                    response.Data = employeeShiftExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }


        // Delete Employee Shift By Id
        public async Task<Respons_M> DeleteEmployeeShiftByIdAsync(int employeeShiftId)
        {
            // Start a database transaction
            using (var transaction = await DbContextAccess.Database.BeginTransactionAsync())
            {
                try
                {
                    // Check if the Employee Shift exists
                    var employeeShiftExists = await DbContextAccess.EmployeeShift
                        .Where(e => e.employee_shift_id == employeeShiftId)
                        .SingleOrDefaultAsync();

                    if (employeeShiftExists == null)
                    {
                        response.Status = false; // Set to false since it wasn't found 
                        response.StatusMsg = "Employee Shift not found.";
                        response.Data = null;
                    }

                    // Remove the Employee Shift and save changes
                    DbContextAccess.EmployeeShift.Remove(employeeShiftExists);
                    await DbContextAccess.SaveChangesAsync();

                    // Commit the transaction
                    await transaction.CommitAsync();

                    response.Status = true;
                    response.StatusMsg = "Employee Shift deleted successfully.";
                    response.Data = employeeShiftExists;
                }
                catch (Exception ex)
                {
                    // Rollback the transaction on any error
                    await transaction.RollbackAsync();

                    response.Status = false; // Set to false since it wasn't found
                    response.StatusMsg = $"Error: {ex.Message}";
                    response.Data = null;
                }
                finally
                {
                    // Dispose of the transaction resources
                    await transaction.DisposeAsync();
                }
            }

            return response;
        }
    }
}
