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

        // Author: Prashanth
        // Date: <02-Sep-2024>
        // Description: Location Master.

        public DbSet<Locations> Location { get; set; }
        public DbSet<LocationCustomer> LocationCustomer { get; set; }
        public DbSet<LocationFinance> LocationFinance { get; set; }
        public DbSet<LocationEmployee> LocationEmployee { get; set; }
        public DbSet<LocationVehicle> LocationVehicle { get; set; }
        public DbSet<LocationEquipment> LocationEquipment { get; set; }
        public DbSet<LocationHoliday> LocationHoliday { get; set; }
        public DbSet<LocationOperationday> LocationOperationday { get; set; }
        public DbSet<LocationService> LocationService { get; set; }
        public DbSet<LocationShippoint> LocationShippoint { get; set; }



        // Author: Prashanth
        // Date: <03-Sep-2024>
        // Description: Shippoint Master.

        public DbSet<Shippoints> Shippoint { get; set; }
        public DbSet<ShippointVehicle> ShippointVehicle { get; set; }
        public DbSet<ShippointPoc> ShippointPoc { get; set; }
        public DbSet<ShippointPickup> ShippointPickup { get; set; }
        public DbSet<ShippointOperatingday> ShippointOperatingday { get; set; }
        public DbSet<ShippointHoliday> ShippointHoliday { get; set; }
        public DbSet<ShippointGate> ShippointGate { get; set; }
        public DbSet<ShippointDocument> ShippointDocument { get; set; }
        public DbSet<ShippointDockdoor> ShippointDockdoor { get; set; }
        public DbSet<ShippointDelivery> ShippointDelivery { get; set; }
        public DbSet<ShippointCustomer> ShippointCustomer { get; set; }
        public DbSet<ShippointConsignee> ShippointConsignee { get; set; }



        // Author: Prashanth
        // Date: <05-Sep-2024>
        // Description: Customer Master.

        public DbSet<Customers> Customer { get; set; }
        public DbSet<CustomerTaxation> CustomerTaxation { get; set; }
        public DbSet<CustomerPoc> CustomerPoc { get; set; }
        public DbSet<CustomerPayment> CustomerPayment { get; set; }
        public DbSet<CustomerOperatingday> CustomerOperatingday { get; set; }
        public DbSet<CustomerHoliday> CustomerHoliday { get; set; }
        public DbSet<CustomerDocument> CustomerDocument { get; set; }



        // Author: Prashanth
        // Date: <07-Sep-2024>
        // Description: Geography Master.

        public DbSet<Countries> Country { get; set; }
        public DbSet<States> State { get; set; }
        public DbSet<Districts> District { get; set; }
        public DbSet<Towns> Town { get; set; }
        public DbSet<Pincodes> Pincode { get; set; }
        public DbSet<Suburbs> Suburb { get; set; }
        public DbSet<Regions> Region { get; set; }
        public DbSet<RegionComponent> RegionComponent { get; set; }
        public DbSet<RegionFunction> RegionFunction { get; set; }
        public DbSet<RegionCustomer> RegionCustomer { get; set; }
        public DbSet<RegionVendor> RegionVendor { get; set; }
        public DbSet<Zones> Zone { get; set; }
        public DbSet<ZoneComponent> ZoneComponent { get; set; }
        public DbSet<ZoneFunction> ZoneFunction { get; set; }
        public DbSet<ZoneCustomer> ZoneCustomer { get; set; }
        public DbSet<ZoneVendor> ZoneVendor { get; set; }
        public DbSet<Clusters> Cluster { get; set; }
        public DbSet<ClusterComponent> ClusterComponent { get; set; }
        public DbSet<ClusterFunction> ClusterFunction { get; set; }
        public DbSet<ClusterCustomer> ClusterCustomer { get; set; }
        public DbSet<ClusterVendor> ClusterVendor { get; set; }



        // Author: Prashanth
        // Date: <07-Sep-2024>
        // Description: Employee Master.

        public DbSet<Employees> Employee { get; set; }
        public DbSet<EmployeeRole> EmployeeRole { get; set; }
        public DbSet<EmployeeSkill> EmployeeSkill { get; set; }
        public DbSet<EmployeeCourse> EmployeeCourse { get; set; }
        public DbSet<EmployeeLicense> EmployeeLicense { get; set; }
        public DbSet<EmployeeShift> EmployeeShift { get; set; }



        // Author: Prashanth
        // Date: <21-Sep-2024>
        // Description: Division Master.

        public DbSet<Divisions> Division { get; set; }
        public DbSet<DivisionLocation> DivisionLocation { get; set; }
        public DbSet<DivisionKAM> DivisionKAM { get; set; }
        public DbSet<DivisionService> DivisionService { get; set; }
        public DbSet<DivisionInfoDto> DivisionInfoDto { get; set; }



        // Author: Prashanth
        // Date: <23-Sep-2024>
        // Description: Leg Master.

        public DbSet<Legs> Leg { get; set; }
        public DbSet<LegFrom> LegFrom { get; set; }
        public DbSet<LegTo> LegTo { get; set; }
        public DbSet<LegService> LegService { get; set; }
        public DbSet<LegVendor> LegVendor { get; set; }
        public DbSet<LegParameter> LegParameter { get; set; }
        public DbSet<LegInfoDto> LegDto { get; set; }



        // Author: Prashanth
        // Date: <24-Sep-2024>
        // Description: Network Master.

        public DbSet<Networks> Network { get; set; }
        public DbSet<NetworkLeg> NetworkLeg { get; set; }
        public DbSet<NetworkService> NetworkService { get; set; }
        public DbSet<NetworkCustomer> NetworkCustomer { get; set; }
        public DbSet<NetworkAttribute> NetworkAttribute { get; set; }
        public DbSet<NetworkDto> NetworkDto { get; set; }



        // Author: Prashanth
        // Date: <25-Sep-2024>
        // Description: UOM Master.

        public DbSet<Uom> UOM { get; set; }
        public DbSet<UomGeo> UOMGeo { get; set; }



        // Author: Prashanth
        // Date: <25-Sep-2024>
        // Description: SKU Master.

        public DbSet<SKU> SKU { get; set; }
        public DbSet<SKUAttribute> SKUAttribute { get; set; }
        public DbSet<SKUPackingBay> SKUPackingBay { get; set; }



        // Author: Prashanth
        // Date: <27-Sep-2024>
        // Description: Incompatibility Master.

        public DbSet<Incompatibility> Incompatibility { get; set; }
        public DbSet<IncompatibilityService> IncompatibilityService { get; set; }
        public DbSet<IncompatibilityFromGeo> IncompatibilityFromGeo { get; set; }
        public DbSet<IncompatibilityToGeo> IncompatibilityToGeo { get; set; }
        public DbSet<IncompatibilityFunction> IncompatibilityFunction { get; set; }
        public DbSet<IncompatibilityCustomer> IncompatibilityCustomer { get; set; }
        public DbSet<IncompatibilityVendor> IncompatibilityVendor { get; set; }



        // Author: Prashanth
        // Date: <01-Oct-2024>
        // Description: Service Master.

        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceProductManager> ServiceProductManager { get; set; }
        public DbSet<ServiceExclustionType> ServiceExclustionType { get; set; }
        public DbSet<ServiceBehaviour> ServiceBehaviour { get; set; }



        // Author: Prashanth
        // Date: <07-Oct-2024>
        // Description: VehicleType Master.

        public DbSet<Vehicletype> Vehicletype { get; set; }


        // Author: Prashanth
        // Date: <08-Oct-2024>
        // Description: User Master.

        public DbSet<Users> User { get; set; }
        public DbSet<UserRoleUser> UserRoleUser { get; set; }
        public DbSet<UserSso> UserSso { get; set; }



        // Author: Prashanth
        // Date: <09-Oct-2024>
        // Description: UserRole Master.

        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<UserRoleModule> UserRoleModule { get; set; }
        public DbSet<UserRoleFunction> UserRoleFunction { get; set; }
        public DbSet<UserRoleUI> UserRoleUI { get; set; }
        public DbSet<Modules> Modules { get; set; }
        public DbSet<Functions> Functions { get; set; }
        public DbSet<UI> UI { get; set; }
        public DbSet<Usertype> Usertype { get; set; }
        public DbSet<ModuleFunctions> ModuleFunction { get; set; }
        public DbSet<PermissionComponent> PermissionComponent { get; set; }
        public DbSet<Permissions> Permission { get; set; }
        public DbSet<UsertypeComponent> UsertypeComponent { get; set; }
        public DbSet<UsertypePermission> UsertypePermission { get; set; }


        // Author: Prashanth
        // Date: <15-Nov-2024>
        // Description: Booking Creation Master.

        public DbSet<BookingPickup> BookingPickup { get; set; }
        public DbSet<BookingDelivery> BookingDelivery { get; set; }
        public DbSet<BookingProduct> BookingProduct { get; set; }
        public DbSet<BookingService> BookingService { get; set; }
        public DbSet<BookingDocument> BookingDocument { get; set; }


        // Author: Prashanth
        // Date: <20-Nov-2024>
        // Description: Vehicle Master.

        public DbSet<Vehicles> Vehicle { get; set; }
        public DbSet<VehicleOwner> VehicleOwner { get; set; }
        public DbSet<VehicleGeo> VehicleGeo { get; set; }
        public DbSet<VehicleUnavailability> VehicleUnavailability { get; set; }


        // Author: Prashanth
        // Date: <25-Nov-2024>
        // Description: Vendor Master.

        public DbSet<Vendor> Vendor { get; set; }
        public DbSet<VendorContactPerson> VendorContactPerson { get; set; }
        public DbSet<VendorTax> VendorTax { get; set; }
        public DbSet<VendorCertification> VendorCertification { get; set; }
        public DbSet<VendorPayment> VendorPayment { get; set; }
        public DbSet<VendorService> VendorService { get; set; }
        public DbSet<VendorAsset> VendorAsset { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ensure LocationMaster is properly configured
            modelBuilder.Entity<Locations>()
                .ToTable("locations", "COM")
                .HasKey(l => l.location_id);

            modelBuilder.Entity<Locations>()
                .HasOne(lm => lm.Address)
                .WithMany(lm => lm.Locations)
                .HasForeignKey(lm => lm.address_id);

            modelBuilder.Entity<Address>()
                .ToTable("addresses", "COM")
                .HasKey(am => am.address_id);

            modelBuilder.Entity<Address>()
                .HasKey(am => am.address_id);

            modelBuilder.Entity<LocationCustomer>()
                .ToTable("location_customer", "COM")
                .HasKey(lc => lc.location_customer_id);

            //modelBuilder.Entity<LocationCustomer>()
            //.HasOne(lc => lc.Customer)
            //.WithMany() // Specify navigation property here if any
            //.HasForeignKey(lc => lc.CustomerId);

            //modelBuilder.Entity<LocationCustomer>()
            //    .HasOne(lc => lc.locationMP)
            //    .WithMany()
            //    .HasForeignKey(lc => lc.location_id);

            modelBuilder.Entity<LocationFinance>()
                .ToTable("location_finance", "COM")
                .HasKey(lf => lf.location_finance_id);

            modelBuilder.Entity<LocationEmployee>()
                .ToTable("employee_location", "COM")
                .HasKey(le => le.employee_location_id);

            modelBuilder.Entity<LocationEquipment>()
                .ToTable("equipment_location", "COM")
                .HasKey(le => le.equipment_location_id);

            modelBuilder.Entity<LocationVehicle>()
                .ToTable("vehicle_location", "COM")
                .HasKey(le => le.vehicle_location_id);

            modelBuilder.Entity<LocationOperationday>()
                .HasOne(l => l.LocationMaster)
                .WithMany() // Use WithMany if LocationMaster has no inverse navigation property
                .HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior


            modelBuilder.Entity<LocationCustomer>()
                .HasOne(l => l.LocationMaster)
                .WithMany() // Use WithMany if LocationMaster has no inverse navigation property
                .HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior

            modelBuilder.Entity<LocationEmployee>()
                .HasOne(l => l.LocationMaster)
                .WithMany() // Use WithMany if LocationMaster has no inverse navigation property
                .HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior


            modelBuilder.Entity<LocationEquipment>()
                .HasOne(l => l.LocationMaster)
                .WithMany() // Use WithMany if LocationMaster has no inverse navigation property
                .HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior

            modelBuilder.Entity<LocationFinance>()
                .HasOne(l => l.LocationMaster)
                .WithMany() // Use WithMany if LocationMaster has no inverse navigation property
                .HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
                .OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior

            //modelBuilder.Entity<LocationHoliday>()
            //.HasOne(l => l.LocationMaster)
            //.WithMany() // Use WithMany if LocationMaster has no inverse navigation property
            //.HasForeignKey(l => l.location_id) // Specify foreign key in LocationOperationday
            //.OnDelete(DeleteBehavior.Restrict); // Optional: specify delete behavior


            // Author: Prashanth
            // Date: <07-Sep-2024>
            // Description: Holiday.

            //  Configure the Holiday entity to map to the "holidays" table in the "COM" schema 
            modelBuilder.Entity<Holidays>()
                .ToTable("holidays", "COM");


            // Configure the LocationHoliday entity to map to the "location_holiday" table in the "COM" schema
            modelBuilder.Entity<LocationHoliday>()
                .ToTable("location_holiday", "COM");


            // Set up a relationship between LocationHoliday and Holiday entities
            //modelBuilder.Entity<LocationHoliday>()
            //    .HasOne(lh => lh.Holiday)
            //    .WithMany(lh => lh.LocationHolidays)
            //    .HasForeignKey(lh => lh.holiday_id);
            //base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<LocationOperationday>()
                .ToTable("location_operatingday", "COM")
                .HasKey(lo => lo.location_operatingday_id);

            modelBuilder.Entity<LocationService>()
                .ToTable("location_service", "COM")
                .HasKey(ls => ls.location_service_id);

            modelBuilder.Entity<LocationShippoint>()
                .ToTable("location_shippoint", "COM")
                .HasKey(ls => ls.location_shippoint_id);


            // Author: Prashanth
            // Date: <03-Sep-2024>
            // Description: Shippoint.
            /* Configure entities for ShippointVehicle, ShippointPoc, ShippointPickup, ShippointOperatingday, ShippointHoliday, ShippointGate, ShippointDocument, ShippointDockdoor, ShippointDelivery, ShippointCustomer, ShippointConsignee, Holiday, ShippointHoliday (with foreign key to Holiday)
             all mapped to respective tables in the "COM" schema with keys and specific configurations as defined. */

            modelBuilder.Entity<Shippoints>()
                .ToTable("shippoints", "COM")
                .HasKey(sm => sm.ship_point_id);

            modelBuilder.Entity<ShippointVehicle>()
                .ToTable("shippoint_vehicle", "COM")
                .HasKey(sv => sv.shippoint_vehicle_id);

            modelBuilder.Entity<ShippointVehicle>()
                .Property(sv => sv.from_time)
                .HasColumnType("time(7)");

            modelBuilder.Entity<ShippointVehicle>()
                .Property(s => s.to_time)
                .HasColumnType("time(7)");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShippointPoc>()
                .ToTable("shippoint_poc", "COM")
                .HasKey(sp => sp.shippoint_poc_id);

            modelBuilder.Entity<ShippointPickup>()
                .ToTable("shippoint_pickup", "COM")
                .HasKey(sp => sp.shippoint_pickup_id);

            modelBuilder.Entity<ShippointOperatingday>()
                .ToTable("shippoint_operating_day", "COM")
                .HasKey(so => so.shippoint_operating_day_id);

            modelBuilder.Entity<ShippointHoliday>()
                .ToTable("shippoint_holiday", "COM")
                .HasKey(sh => sh.shippoint_holiday_id);

            modelBuilder.Entity<ShippointGate>()
                .ToTable("shippoint_gate", "COM")
                .HasKey(sg => sg.shippoint_gate_id);

            modelBuilder.Entity<ShippointDocument>()
                .ToTable("shippoint_document", "COM")
                .HasKey(sd => sd.shippoint_document_id);

            modelBuilder.Entity<ShippointDockdoor>()
                .ToTable("shippoint_dockdoor", "COM")
                .HasKey(sd => sd.shippoint_dockdoor_id);

            modelBuilder.Entity<ShippointDelivery>()
                .ToTable("shippoint_delivery", "COM")
                .HasKey(sd => sd.shippoint_delivery_id);

            modelBuilder.Entity<ShippointCustomer>()
                .ToTable("shippoint_customer", "COM")
                .HasKey(sc => sc.shippoint_customer_id);

            modelBuilder.Entity<ShippointConsignee>()
                .ToTable("shippoint_consignee", "COM")
                .HasKey(sc => sc.shippoint_consignee_id);

            modelBuilder.Entity<Holidays>()
                .ToTable("holidays", "COM");

            modelBuilder.Entity<ShippointHoliday>()
                .ToTable("shippoint_holiday", "COM");

            //modelBuilder.Entity<ShippointHoliday>()
            //    .HasOne(sh => sh.Holiday)
            //    .WithMany(sh => sh.ShippointHolidays)
            //    .HasForeignKey(sh => sh.holiday_id);



            // Author: Prashanth
            // Date: <05-Sep-2024>
            // Description: Customers.
            // Configure entities for Customers, CustomerTaxation, CustomerPoc, CustomerPayment, CustomerOperatingday, CustomerHoliday, CustomerDocument all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Customers>()
                .ToTable("customers", "COM")
                .HasKey(c => c.customer_id);

            modelBuilder.Entity<CustomerTaxation>()
                .ToTable("customer_taxation", "COM")
                .HasKey(ct => ct.customer_taxation_id);

            modelBuilder.Entity<CustomerPoc>()
                .ToTable("customer_poc", "COM")
                .HasKey(cp => cp.customer_poc_id);

            modelBuilder.Entity<CustomerPayment>()
                .ToTable("customer_payment", "COM")
                .HasKey(cp => cp.customer_payment_id);

            modelBuilder.Entity<CustomerOperatingday>()
                .ToTable("customer_operating_day", "COM")
                .HasKey(co => co.customer_operating_day_id);

            modelBuilder.Entity<CustomerHoliday>()
                .ToTable("customer_holiday", "COM")
                .HasKey(ch => ch.customer_holiday_id);

            modelBuilder.Entity<CustomerDocument>()
                .ToTable("customer_documents", "COM")
                .HasKey(cd => cd.customer_documents_id);



            // Author: Prashanth
            // Date: <07-Sep-2024>
            // Description: Geography.
            // Configure entities for Country, states, districs, towns, pincodes, suburbs, regions, region_component, region_function, region_customer, region_vendor, zones, zone_component, zone_function, zone_customer, zone_vendor, clusters, cluster_component, cluster_function, cluster_customer, cluster_vendor all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.
            modelBuilder.Entity<Countries>()
                .ToTable("countries", "COM")
                .HasKey(c => c.country_id);

            modelBuilder.Entity<States>()
                .ToTable("states", "COM")
                .HasKey(s => s.state_id);

            modelBuilder.Entity<Districts>()
                .ToTable("districts", "COM")
                .HasKey(d => d.district_id);

            modelBuilder.Entity<Towns>()
                .ToTable("towns", "COM")
                .HasKey(t => t.town_id);

            modelBuilder.Entity<Pincodes>()
                .ToTable("pincodes", "COM")
                .HasKey(p => p.pincode_id);

            modelBuilder.Entity<Suburbs>()
                .ToTable("suburbs", "COM")
                .HasKey(s => s.suburb_id);

            modelBuilder.Entity<Regions>()
                .ToTable("regions", "COM")
                .HasKey(r => r.region_id);

            modelBuilder.Entity<RegionComponent>()
                .ToTable("region_component", "COM")
                .HasKey(rc => rc.region_component_id);

            modelBuilder.Entity<RegionFunction>()
                .ToTable("region_function", "COM")
                .HasKey(rf => rf.region_function_id);

            modelBuilder.Entity<RegionCustomer>()
                .ToTable("region_customer", "COM")
                .HasKey(rc => rc.region_customer_id);

            modelBuilder.Entity<RegionVendor>()
                .ToTable("region_vendor", "COM")
                .HasKey(rv => rv.region_vendor_id);

            modelBuilder.Entity<Zones>()
                .ToTable("zones", "COM")
                .HasKey(z => z.zone_id);

            modelBuilder.Entity<ZoneComponent>()
                .ToTable("zone_component", "COM")
                .HasKey(zc => zc.zone_component_id);

            modelBuilder.Entity<ZoneFunction>()
                .ToTable("zone_function", "COM")
                .HasKey(zf => zf.zone_function_id);

            modelBuilder.Entity<ZoneCustomer>()
                .ToTable("zone_customer", "COM")
                .HasKey(l => l.zone_customer_id);

            modelBuilder.Entity<ZoneVendor>()
                .ToTable("zone_vendor", "COM")
                .HasKey(zv => zv.zone_vendor_id);

            modelBuilder.Entity<Clusters>()
                .ToTable("clusters", "COM")
                .HasKey(c => c.cluster_id);

            modelBuilder.Entity<ClusterComponent>()
                .ToTable("cluster_component", "COM")
                .HasKey(cc => cc.cluster_component_id);

            modelBuilder.Entity<ClusterFunction>()
                .ToTable("cluster_function", "COM")
                .HasKey(cf => cf.cluster_function_id);

            modelBuilder.Entity<ClusterCustomer>()
                .ToTable("cluster_customer", "COM")
                .HasKey(cc => cc.cluster_customer_id);

            modelBuilder.Entity<ClusterVendor>()
                .ToTable("cluster_vendor", "COM")
                .HasKey(cv => cv.cluster_vendor_id);



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



            // Author: Prashanth
            // Date: <21-Sep-2024>
            // Description: Division.
            // Configure entities for Division, DivisionLocation, DivisionKAM, DivisionService all mapped to respective tables in the "COM" schema with keys and specific configurations as defined. 

            modelBuilder.Entity<Divisions>()
                .ToTable("divisions", "COM")
                .HasKey(d => d.division_id);

            modelBuilder.Entity<DivisionLocation>()
                .ToTable("division_location", "COM")
                .HasKey(dl => dl.division_location_id);

            modelBuilder.Entity<DivisionKAM>()
                .ToTable("division_kam", "COM")
                .HasKey(dk => dk.division_kam_id);

            modelBuilder.Entity<DivisionService>()
                .ToTable("division_service", "COM")
                .HasKey(ds => ds.division_service_id);



            // Author: Prashanth
            // Date: <23-Sep-2024>
            // Description: Leg.
            // Configure entities for Leg, LegFrom, LegTo, LegService, LegVendor, LegParameter all mapped to respective tables in the "COM" schema with keys and specific configurations as defined. 

            modelBuilder.Entity<Legs>()
                .ToTable("legs", "COM")
                .HasKey(l => l.leg_id);

            modelBuilder.Entity<LegFrom>()
                .ToTable("leg_from", "COM")
                .HasKey(lf => lf.leg_from_id);

            modelBuilder.Entity<LegTo>()
                .ToTable("leg_to", "COM")
                .HasKey(lt => lt.leg_to_id);

            modelBuilder.Entity<LegService>()
                .ToTable("leg_service", "COM")
                .HasKey(ls => ls.leg_service_id);

            modelBuilder.Entity<LegVendor>()
                .ToTable("leg_vendor", "COM")
                .HasKey(lv => lv.leg_vendor_id);

            modelBuilder.Entity<LegParameter>()
                .ToTable("leg_parameter", "COM")
                .HasKey(lp => lp.leg_parameter_id);



            // Author: Prashanth
            // Date: <24-Sep-2024>
            // Description: Networks.
            // Configure entities for Networks, NetworkLeg, NetworkService, NetworkCustomer, NetworkAttribute all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Networks>()
                .ToTable("networks", "COM")
                .HasKey(n => n.network_id);

            modelBuilder.Entity<NetworkLeg>()
                .ToTable("network_leg", "COM")
                .HasKey(nl => nl.network_leg_id);

            modelBuilder.Entity<NetworkService>()
                .ToTable("network_service", "COM")
                .HasKey(ns => ns.network_service_id);

            modelBuilder.Entity<NetworkCustomer>()
                .ToTable("network_customer", "COM")
                .HasKey(nc => nc.network_customer_id);

            modelBuilder.Entity<NetworkAttribute>()
                .ToTable("network_attribute", "COM")
                .HasKey(na => na.network_attribute_id);

            // Author: Prashanth
            // Date: <25-Sep-2024>
            // Description: UOM.
            // Configure entities for UOM, UOM_Geo all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Uom>()
                .ToTable("uom", "COM")
                .HasKey(u => u.uom_id);

            modelBuilder.Entity<UomGeo>()
                .ToTable("uom_geo", "COM")
                .HasKey(ug => ug.uom_geo_id);


            // Author: Prashanth
            // Date: <25-Sep-2024>
            // Description: SKU.
            // Configure entities for SKU, SKUAttribute, SKUPackingBay all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<SKU>()
                .ToTable("sku", "WMS")
                .HasKey(s => s.item_id);

            modelBuilder.Entity<SKUAttribute>()
                .ToTable("sku_attribute", "WMS")
                .HasKey(sa => sa.sku_attribute_id);

            modelBuilder.Entity<SKUPackingBay>()
                .ToTable("sku_packing_bay", "WMS")
                .HasKey(sp => sp.sku_packing_bay_id);

            // Author: Prashanth
            // Date: <27-Sep-2024>
            // Description: Incompatibility.
            // Configure entities for Incompatibility, IncompatibilityCustomer, IncompatibilityFromGeo, IncompatibilityToGeo, IncompatibilityFunction, IncompatibilityService, IncompatibilityVendor all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Incompatibility>()
                .ToTable("incompatibility", "TMS")
                .HasKey(i => i.incompatibility_id);

            modelBuilder.Entity<IncompatibilityCustomer>()
                .ToTable("incompatibility_customer", "TMS")
                .HasKey(i => i.incompatibility_customer_id);

            modelBuilder.Entity<IncompatibilityFromGeo>()
                .ToTable("incompatibility_from_geo", "TMS")
                .HasKey(i => i.incompatibility_from_geo_id);

            modelBuilder.Entity<IncompatibilityToGeo>()
                .ToTable("incompatibility_to_geo", "TMS")
                .HasKey(i => i.incompatibility_to_geo_id);

            modelBuilder.Entity<IncompatibilityFunction>()
                .ToTable("incompatibility_function", "TMS")
                .HasKey(i => i.incompatibility_function_id);

            modelBuilder.Entity<IncompatibilityService>()
                .ToTable("incompatibility_service", "TMS")
                .HasKey(i => i.incompatibility_service_id);

            modelBuilder.Entity<IncompatibilityVendor>()
                .ToTable("Incompatibility_vendor", "TMS")
                .HasKey(I => I.incompatibility_vendor_id);


            // Author: Prashanth
            // Date: <01-Oct-2024>
            // Description: Services.
            // Configure entities for Services, ServiceProductManager, ServiceExclusionType, ServiceBehavior all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Service>()
                .ToTable("services", "COM")
                .HasKey(s => s.service_id);

            modelBuilder.Entity<ServiceProductManager>()
                .ToTable("service_product_manager", "COM")
                .HasKey(sp => sp.service_product_manager_id);

            modelBuilder.Entity<ServiceExclustionType>()
                .ToTable("service_exclusion_type", "COM")
                .HasKey(se => se.services_exclusion_type_id);

            modelBuilder.Entity<ServiceBehaviour>()
                .ToTable("service_behaviour", "COM")
                .HasKey(sb => sb.service_behaviour_id);


            // Author: Prashanth
            // Date: <07-Oct-2024>
            // Description: VehicleType.
            // Configure entities for Vehicletype all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Vehicletype>()
                .ToTable("vehicle_type", "COM")
                .HasKey(v => v.vehicle_type_id);


            // Author: Prashanth
            // Date: <08-Oct-2024>
            // Description: User.
            // Configure entities for Users, UserRoleUser, UserSso all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Users>()
                .ToTable("users", "COM")
                .HasKey(u => u.user_id);

            modelBuilder.Entity<UserRoleUser>()
                .ToTable("userrole_user", "COM")
                .HasKey(u => u.userrole_user_id);

            modelBuilder.Entity<UserSso>()
                .ToTable("user_sso", "COM")
                .HasKey(u => u.user_sso_id);


            // Author: Prashanth
            // Date: <09-Oct-2024>
            // Description: Usertypes.
            // Configure entities for Usertype, Permissions, UsertypePermission, UsertypeComponent, PermissionComponent, Modules, ModuleFunctions, UI all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Usertype>()
                .ToTable("user_types", "COM")
                .HasKey(u => u.user_type_id);

            modelBuilder.Entity<Permissions>()
                .ToTable("permissions", "COM")
                .HasKey(u => u.permission_id);

            modelBuilder.Entity<UsertypePermission>()
                .ToTable("user_type_permission", "COM")
                .HasKey(u => u.user_type_permission_id);

            modelBuilder.Entity<UsertypeComponent>()
                .ToTable("user_type_permissions", "COM")
                .HasKey(u => u.user_type_permission_id);

            modelBuilder.Entity<PermissionComponent>()
                .ToTable("permission_component", "COM")
                .HasKey(u => u.permission_component_id);

            modelBuilder.Entity<Modules>()
                .ToTable("modules", "COM")
                .HasKey(u => u.module_id);

            modelBuilder.Entity<ModuleFunctions>()
                .ToTable("module_functions", "COM")
                .HasKey(u => u.function_id);

            modelBuilder.Entity<UI>()
                .ToTable("ui", "COM")
                .HasKey(u => u.ui_id);


            // Author: Prashanth
            // Date: <15-Nov-2024>
            // Description: Booking Creation Master.
            // Configure entities for BookingPickup, BookingDelivery, BookingProduct, BookingService, BookingDocument all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<BookingPickup>()
                .ToTable("booking_pickup", "COM")
                .HasKey(bc => bc.booking_pickup_id);

            modelBuilder.Entity<BookingDelivery>()
                .ToTable("booking_delivery", "COM")
                .HasKey(bc => bc.booking_delivery_id);

            modelBuilder.Entity<BookingProduct>()
                .ToTable("booking_product", "COM")
                .HasKey(bc => bc.booking_product_id);

            modelBuilder.Entity<BookingService>()
                .ToTable("booking_service", "COM")
                .HasKey(bc => bc.booking_service_id);

            modelBuilder.Entity<BookingDocument>()
                .ToTable("booking_document", "COM")
                .HasKey(bc => bc.booking_document_id);


            // Author: Prashanth
            // Date: <20-Nov-2024>
            // Description: Vehicle Master.
            // Configure entities for Vehicle, VehicleOwner, VehicleGeo, VehicleUnavailability all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Vehicles>()
                .ToTable("vehicles", "COM")
                .HasKey(v => v.vehicle_id);

            modelBuilder.Entity<VehicleOwner>()
                .ToTable("vehicle_owner", "COM")
                .HasKey(v => v.vehicle_owner_id);

            modelBuilder.Entity<VehicleGeo>()
                .ToTable("vehicle_geo", "COM")
                .HasKey(v => v.vehicle_geo_id);

            modelBuilder.Entity<VehicleUnavailability>()
                .ToTable("vehicle_unavailability", "COM")
                .HasKey(v => v.vehicle_unavailability_id);

            // Author: Prashanth
            // Date: <25-Nov-2024>
            // Description: Vendor Master.
            // Configure entities for Vendor, VendorContactPerson, VendorTax, VendorCertification, VendorPayment, VendorService, VendorAsset all mapped to respective tables in the "COM" schema with keys and specific configurations as defined.

            modelBuilder.Entity<Vendor>()
                .ToTable("vendor", "COM")
                .HasKey(v => v.vendor_id);

            modelBuilder.Entity<VendorContactPerson>()
                .ToTable("vendor_contactperson", "COM")
                .HasKey(v => v.vendor_contactperson_id);

            modelBuilder.Entity<VendorTax>()
                .ToTable("vendor_tax", "COM")
                .HasKey(v => v.vendor_tax_id);

            modelBuilder.Entity<VendorCertification>()
                .ToTable("vendor_certification", "COM")
                .HasKey(v => v.vendor_certification_id);

            modelBuilder.Entity<VendorPayment>()
                .ToTable("vendor_payment", "COM")
                .HasKey(v => v.vendor_payment_id);

            modelBuilder.Entity<VendorService>()
                .ToTable("vendor_service", "COM")
                .HasKey(v => v.vendor_service_id);

            modelBuilder.Entity<VendorAsset>()
                .ToTable("vendor_asset", "COM")
                .HasKey(v => v.vendor_asset_id);
        }
    }
}