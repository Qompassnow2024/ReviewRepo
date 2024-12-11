using MasterDataServices.DTOModels.Employee;
using MasterDataServices.Models.Employee;
using MasterDataServices.Models.ResponseModels;

namespace MasterDataServices.IControlServices
{
    
    public interface IEmployeeService
    {
        // Employee
        Task<Respons_M> GetAllEmployeesAsync(int orgId);
        Task<Respons_M> GetEmployeeByIdAsync(int orgId, int employeeId);
        Task<Respons_M> GetEmployeePaginationAsync(int pageNumber, int pageSize, int orgId);
        Task<Respons_M> CreateEmployeeAsync(Employees employees, int orgId, int userId);
        Task<Respons_M> UpdateEmployeeByIdAsync(Employees employees, int employeeId, int userId);
        Task<Respons_M> DeleteEmployeeByIdAsync(int orgId, int employeeId);


        // Employee Role
        Task<Respons_M> GetAllEmployeeRoleAsync();
        Task<Respons_M> GetEmployeeRoleByIdAsync(int employeeRoleId);
        Task<Respons_M> CreateEmployeeRoleAsync(EmployeeRole employeeRole, int userId);
        Task<Respons_M> UpdateEmployeeRoleByIdAsync(EmployeeRole employeeRole, int employeeRoleId, int userId);
        Task<Respons_M> DeleteEmployeeRoleByIdAsync(int employeeRoleId);


        // Employee Skill
        Task<Respons_M> GetAllEmployeeSkillAsync();
        Task<Respons_M> GetEmployeeSkillByIdAsync(int employeeSkillId);
        Task<Respons_M> CreateEmployeeSkillAsync(EmployeeSkill employeeSkill, int userId);
        Task<Respons_M> UpdateEmployeeSkillAsync(EmployeeSkill employeeSkill, int employeeSkillId, int userId);
        Task<Respons_M> DeleteEmployeeSkillAsync(int employeeSkillId);


        // Employee Course
        Task<Respons_M> GetAllEmployeeCourseAsync();
        Task<Respons_M> GetEmployeeCourseByIdAsync(int employeeCourse);
        Task<Respons_M> CreateEmployeeCourseAsync(EmployeeCourse employeeCourse, int userId);
        Task<Respons_M> UpdateEmployeeCourseByIdAsync(EmployeeCourse employeeCourse, int employeeCourseId, int userId);
        Task<Respons_M> DeleteEmployeeCourseByIdAsync(int employeeCourseId);


        // Employee License
        Task<Respons_M> GetAllEmployeeLicenseAsync();
        Task<Respons_M> GetEmployeeLicenseByIdAsync(int employeLicenseId);
        Task<Respons_M> CreateEmployeeLicenseAsync(EmployeeLicense employeeLicense, int userId);
        Task<Respons_M> UpdateEmployeeLicenseByIdAsync(EmployeeLicense employeeLicense, int employeeLicenseId, int userId);
        Task<Respons_M> DeleteEmployeeLicenseByIdAsync(int employeLicenseId);


        // Employee Shift
        Task<Respons_M> GetAllEmployeeShiftAsync();
        Task<Respons_M> GetEmployeeShiftByIdAsync(int employeeShiftId);
        Task<Respons_M> CreateEmployeeShiftAsync(EmployeeShiftDto employeeShiftDto, int userId);
        Task<Respons_M> UpdateEmployeeShiftByIdAsync(EmployeeShiftDto employeeShiftDto, int employeeShiftId, int userId);
        Task<Respons_M> DeleteEmployeeShiftByIdAsync(int employeeShiftId);
    }
}
