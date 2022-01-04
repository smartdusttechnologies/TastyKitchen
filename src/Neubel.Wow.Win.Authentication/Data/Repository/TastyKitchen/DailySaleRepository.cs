using Dapper;
using Neubel.Wow.Win.Authentication.Core.Model.TastyKitchen;
using Neubel.Wow.Win.Authentication.Data.Repository.Interfaces.TastyKitchen;
using Neubel.Wow.Win.Authentication.Infrastructure;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Neubel.Wow.Win.Authentication.Data.Repository
{
    public class DailySaleRepository : IDailySaleRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public DailySaleRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public bool Delete(int id)
        {
            string query = @"update [DailySale] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }

        public List<DailySale> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailySale>("Select * From [DailySale] where IsDeleted=0").ToList();
        }

        public DailySale Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailySale>("Select top 1 * From [DailySale] where Id=@id and IsDeleted=0", new { id }).FirstOrDefault();
        }

        public IPagedList<DailySale> GetPages(int pageIndex = 1, int pageSize = 10)
        {
            using IDbConnection db = _connectionFactory.GetConnection;

            var query = db.QueryMultiple("SELECT COUNT(*) FROM [DailySale] where isDeleted=0;SELECT* FROM [DailySale] where isDeleted=0 ORDER BY Id desc OFFSET ((@PageNumber - 1) * @Rows) ROWS FETCH NEXT @Rows ROWS ONLY", new { PageNumber = pageIndex, Rows = pageSize }, commandType: CommandType.Text);
            var row = query.Read<int>().First();
            var pageResult = query.Read<DailySale>().ToList();
            return new StaticPagedList<DailySale>(pageResult, pageIndex, pageSize, row);
        }

        public int Insert(DailySale dailySale)
        {
            string query = @"Insert into [DailySale](BillNumber, Amount, Date, Type) 
                values (@BillNumber, @Amount, @Date, @Type)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, dailySale);
        }

        public int InsertCollection(List<DailySale> dailySales)
        {
            string query = @"Insert into [DailySale](BillNumber, Amount, Date, Type) 
                values (@BillNumber, @Amount, @Date, @Type)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, dailySales);
        }

        public int InsertBillWiseSaleReport(BillWiseSaleReport billWiseSaleReport)
        {
            var p = new DynamicParameters();
            p.Add("Id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@Amount", billWiseSaleReport.Amount);
            p.Add("@Date", billWiseSaleReport.Date);

            string billWiseSaleReportQuery = @"Insert into [BillWiseSaleReport](Amount, Date) 
                values (@Amount ,@Date);
                SELECT @Id = @@IDENTITY";

            string billWiseSaleDataQuery = @"Insert into [BillWiseSaleData](BillNumber, Amount, BillWiseSaleReportId) 
                values (@BillNumber ,@Amount, @BillWiseSaleReportId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(billWiseSaleReportQuery, p, transaction);

            int insertedId = p.Get<int>("@Id");

            foreach (var item in billWiseSaleReport.BillWiseSales)
            {
                item.BillWiseSaleReportId = insertedId;
            }
            db.Execute(billWiseSaleDataQuery, billWiseSaleReport.BillWiseSales, transaction);

            transaction.Commit();
            return insertedId;
        }

        public int InsertMenuCategoryWiseSaleReport(MenuCategoryWiseSaleReport menuCategoryWiseSaleReport)
        {
            var p = new DynamicParameters();
            p.Add("Id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@Amount", menuCategoryWiseSaleReport.Amount);
            p.Add("@Date", menuCategoryWiseSaleReport.Date);

            string menuCategoryWiseSaleReportQuery = @"Insert into [MenuCategoryWiseSaleReport](Amount, Date) 
                values (@Amount ,@Date);
                SELECT @Id = @@IDENTITY";

            string menuCategoryWiseSaleDataQuery = @"Insert into [MenuCategoryWiseSaleData](Name, Quantity, Amount, MenuCategoryWiseSaleReportId) 
                values (@Name, @Quantity ,@Amount, @MenuCategoryWiseSaleReportId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(menuCategoryWiseSaleReportQuery, p, transaction);

            int insertedId = p.Get<int>("@Id");

            foreach (var item in menuCategoryWiseSaleReport.MenuCategoryWiseSales)
            {
                item.MenuCategoryWiseSaleReportId = insertedId;
            }
            db.Execute(menuCategoryWiseSaleDataQuery, menuCategoryWiseSaleReport.MenuCategoryWiseSales, transaction);

            transaction.Commit();
            return insertedId;
        }

        public int InsertMenuItemWiseSaleReport(MenuItemWiseSaleReport menuItemWiseSaleReport)
        {
            var p = new DynamicParameters();
            p.Add("Id", 0, DbType.Int32, ParameterDirection.Output);
            p.Add("@Amount", menuItemWiseSaleReport.Amount);
            p.Add("@Date", menuItemWiseSaleReport.Date);

            string menuItemWiseSaleReportQuery = @"Insert into [MenuItemWiseSaleReport](Amount, Date) 
                values (@Amount ,@Date);
                SELECT @Id = @@IDENTITY";

            string menuItemWiseSaleDataQuery = @"Insert into [MenuItemWiseSaleData](Name, Quantity, Amount, MenuItemWiseSaleReportId) 
                values (@Name, @Quantity ,@Amount, @menuItemWiseSaleReportId)";

            using IDbConnection db = _connectionFactory.GetConnection;
            using var transaction = db.BeginTransaction();
            db.Execute(menuItemWiseSaleReportQuery, p, transaction);

            int insertedId = p.Get<int>("@Id");

            foreach (var item in menuItemWiseSaleReport.MenuItemWiseSales)
            {
                item.MenuItemWiseSaleReportId = insertedId;
            }
            db.Execute(menuItemWiseSaleDataQuery, menuItemWiseSaleReport.MenuItemWiseSales, transaction);

            transaction.Commit();
            return insertedId;
        }

        public List<MenuItemWiseSaleReport> GetMenuItemWiseSaleReport(DateTime from, DateTime to)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            var menuItemWiseSaleReport = db.Query<MenuItemWiseSaleReport>("Select * From [MenuItemWiseSaleReport] where Date>=@from and Date<=@to and IsDeleted=0", new { from, to }).ToList();

            var menuItemWiseSaleData = db.Query<MenuItemWiseSaleData>("Select * From [MenuItemWiseSaleData] where MenuItemWiseSaleReportId IN @Ids", new { Ids = menuItemWiseSaleReport.Select(x => x.Id) }).ToList();

            menuItemWiseSaleReport.ForEach(x => x.MenuItemWiseSales = menuItemWiseSaleData.Where(y => y.MenuItemWiseSaleReportId == x.Id).ToList());
            return menuItemWiseSaleReport;
        }

        public List<BillWiseSaleReport> GetBillWiseSaleReport(DateTime from, DateTime to)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            var billWiseSaleReport = db.Query<BillWiseSaleReport>("Select * From [BillWiseSaleReport] where Date>=@from and Date<=@to and IsDeleted=0", new { from, to }).ToList();

            var billWiseSaleData = db.Query<BillWiseSaleData>("Select * From [BillWiseSaleData] where BillWiseSaleReportId IN @Ids", new { Ids = billWiseSaleReport.Select(x => x.Id) }).ToList();

            billWiseSaleReport.ForEach(x => x.BillWiseSales = billWiseSaleData.Where(y => y.BillWiseSaleReportId == x.Id).ToList());
            return billWiseSaleReport;
        }

        public List<MenuCategoryWiseSaleReport> GetMenuCategoryWiseSaleReport(DateTime from, DateTime to)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            var menuCategoryWiseSaleReport = db.Query<MenuCategoryWiseSaleReport>("Select * From [MenuCategoryWiseSaleReport] where Date>=@from and Date<=@to and IsDeleted=0", new { from, to }).ToList();
            
            var menuCategoryWiseSaleData = db.Query<MenuCategoryWiseSaleData>("Select * From [MenuCategoryWiseSaleData] where MenuCategoryWiseSaleReportId IN @Ids", new { Ids = menuCategoryWiseSaleReport.Select(x => x.Id) }).ToList();

            menuCategoryWiseSaleReport.ForEach(x => x.MenuCategoryWiseSales = menuCategoryWiseSaleData.Where(y => y.MenuCategoryWiseSaleReportId == x.Id).ToList());
            return menuCategoryWiseSaleReport;
        }

        public int Update(DailySale dailySale)
        {
            string query = @"update [DailySale] Set 
                                BillNumber = @BillNumber, 
                                Amount = @Amount,
                                Date = @Date,
                                Type = @Type
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, dailySale);
        }
    }
}
