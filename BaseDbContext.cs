/// Developer:DEVBYDIBIN
/// Developer:DEVBYDIBIN
// Author:DIBIN
// Description: DbContext.

using MasterDataServices.DTOModels.Division;
using MasterDataServices.DTOModels.Leg;
using MasterDataServices.DTOModels.Networks;
using MasterDataServices.IServices;
using MasterDataServices.Models;
using MasterDataServices.Models.Address;
using MasterDataServices.Models.BookingCreation;
using MasterDataServices.Models.Customer;
using MasterDataServices.Models.Division;
using MasterDataServices.Models.Employee;
using MasterDataServices.Models.Geography;
using MasterDataServices.Models.Holidays;
using MasterDataServices.Models.Incompatibility;
using MasterDataServices.Models.Leg;
using MasterDataServices.Models.Location;
using MasterDataServices.Models.Networks;
using MasterDataServices.Models.Services;
using MasterDataServices.Models.Shippoint;
using MasterDataServices.Models.SKU;
using MasterDataServices.Models.UOM;
using MasterDataServices.Models.User;
using MasterDataServices.Models.Userrole;
using MasterDataServices.Models.UserRole;
using MasterDataServices.Models.Users;
using MasterDataServices.Models.Usertype;
using MasterDataServices.Models.Usertypes;
using MasterDataServices.Models.Vehicle;
using MasterDataServices.Models.Vehicletype;
using MasterDataServices.Models.Vendor;
using MasterDataServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
namespace MasterDataServices.DBServices
{
    /// Developer:DEVBYDIBIN <summary>
    /// Developer:DEVBYDIBIN
    /// </summary>
    /// /// Developer:DEVBYDIBIN
    /// Developer:DEVBYDIBIN
    // Author:DIBIN
    // Description: DbContext.
    public class BaseDbContex : DbContext
    {
        private readonly ITenantProvider _tenantProvider;
        private readonly IConfiguration _config;
        //private readonly IConnectionMultiplexer _redis;
        private readonly ILogger<BaseDbContex> _logger;
        private readonly IMemoryCache _memoryCache;
        public readonly IndexProvider _indexProvider;
        public BaseDbContex(DbContextOptions options, IConfiguration config, ITenantProvider tenantProvider, IndexProvider indexProvider, ILogger<BaseDbContex> logger, IMemoryCache memoryCache)
            : base(options)
        {
            _tenantProvider = tenantProvider;
            _config = config;
            //_redis = connectionMultiplexer;
            _logger = logger;
            _memoryCache = memoryCache;
            _indexProvider = indexProvider;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                IndexModel cachedData = new IndexModel();
                // Only configure if not already configured
                //if (!optionsBuilder.IsConfigured)
                //{
                var tenantId = _tenantProvider.GetTenantId();
                if (string.IsNullOrEmpty(tenantId))
                {
                    throw new ArgumentException("Tenant ID cannot be null or empty.");
                }

                if (_memoryCache.TryGetValue(tenantId, out IndexModel cachedItem))
                {
                    // cachedItem contains the value from the cache if found
                    cachedData = cachedItem;
                    // You can now use cachedData as needed
                }
                else
                {
                    cachedData = null;
                }
                if (cachedData == null)
                {
                    _indexProvider.SetCacheData();
                    if (_memoryCache.TryGetValue(tenantId, out IndexModel cachedItem1))
                    {
                        // cachedItem contains the value from the cache if found
                        cachedData = cachedItem1;
                        // You can now use cachedData as needed
                    }
                    else
                    {
                        cachedData = null;
                    }

                }

                //var db = _redis.GetDatabase();
                //    var cachedData = db.StringGet(tenantId.Trim());

                //    if (cachedData.IsNullOrEmpty)
                //    {
                //        cachedData = db.StringGet(tenantId);
                //    }

                if (cachedData != null)
                {
                    //var Idata = JsonSerializer.Deserialize<IndexModel>(cachedData);
                    //if (Idata == null || string.IsNullOrEmpty(Idata.mainconstring))
                    //{
                    //    throw new Exception("Failed to retrieve or deserialize cached data.");
                    //}

                    optionsBuilder.UseSqlServer(cachedData.mainconstring);
                }
                else
                {
                    throw new KeyNotFoundException("Connection string not found in cache.");
                }
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while configuring the DbContext.");
                throw;
            }
        }


        // Author: Prashanth
        // Date: <02-Sep-2024>
        // Description: Address Master.

        public DbSet<Address> Address { get; set; }

        // Author: Prashanth
        // Date: <02-Sep-2024>
        // Description: Holiday Master.

        public DbSet<Holidays> Holiday { get; set; }

        
      

        public DbSet<Employees> Employee { get; set; }
        public DbSet<EmployeeRole> EmployeeRole { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkill { get; set; }
        public DbSet<EmployeeCourse> EmployeeCourse { get; set; }
        public DbSet<EmployeeLicense> EmployeeLicense { get; set; }
        public DbSet<EmployeeShift> EmployeeShift { get; set; }



        

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           


            // Author: Prashanth
            // Date: <07-Sep-2024>
            // Description: Employees.
            // Configure entities for Employees, EmployeeRole, EmployeeSkill, EmployeeCourse, EmployeeLicense, EmployeeShift all mapped to respective tables in the "COM" schema with keys and specific configurations as defined. 

            modelBuilder.Entity<Employees>()
                .ToTable("employees", "COM")
                .HasKey(e => e.employee_id);

            modelBuilder.Entity<EmployeeRole>()
                .ToTable("employee_role", "COM")
                .HasKey(er => er.employee_role_id);

            modelBuilder.Entity<EmployeeSkill>()
                .ToTable("employee_skill", "COM")
                .HasKey(es => es.employee_skill_id);

            modelBuilder.Entity<EmployeeCourse>()
                .ToTable("employee_course", "COM")
                .HasKey(ec => ec.employee_course_id);

            modelBuilder.Entity<EmployeeLicense>()
                .ToTable("employee_license", "COM")
                .HasKey(el => el.employee_license_id);

            modelBuilder.Entity<EmployeeShift>()
                .ToTable("employee_shift", "COM")
                .HasKey(es => es.employee_shift_id);



            
        }
    }
}
