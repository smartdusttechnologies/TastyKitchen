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
    public class DailyExpenseRepository : IDailyExpenseRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public DailyExpenseRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<DailyExpense> Get()
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailyExpense>("Select * From [DailyExpense] where IsDeleted=0").ToList();
        }

        public List<DailyExpense> Get(DateTime from, DateTime to)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailyExpense>("Select * From [DailyExpense] where Date>=@from and Date<=@to and IsDeleted=0", new { from, to }).ToList();
        }

        public IPagedList<DailyExpense> GetPages(int pageIndex = 1, int pageSize = 10)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            
            var query = db.QueryMultiple("SELECT COUNT(*) FROM [DailyExpense] where isDeleted=0;SELECT* FROM [DailyExpense] where isDeleted=0 ORDER BY Date desc OFFSET ((@PageNumber - 1) * @Rows) ROWS FETCH NEXT @Rows ROWS ONLY", new { PageNumber = pageIndex, Rows = pageSize }, commandType: CommandType.Text);
            var row = query.Read<int>().First();
            var pageResult = query.Read<DailyExpense>().ToList();
            return new StaticPagedList<DailyExpense>(pageResult, pageIndex, pageSize, row);
            
        }
        public DailyExpense Get(int id)
        {
            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Query<DailyExpense>("Select top 1 * From [DailyExpense] where Id=@id and IsDeleted=0", new { id }).FirstOrDefault();
        }

        public int Insert(DailyExpense expense)
        {
            string query = @"Insert into [DailyExpense](Name, Amount, Quantity, Unit, Date) 
                values (@Name, @Amount, @Quantity, @Unit, @Date)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, expense);
        }

        public int InsertCollection(List<DailyExpense> expenses)
        {
            string query = @"Insert into [DailyExpense](Name, Amount, Quantity, Unit, Date) 
                values (@Name, @Amount, @Quantity, @Unit, @Date)";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, expenses);
        }

        public int Update(DailyExpense expense)
        {
            string query = @"update [DailyExpense] Set 
                                Name = @Name, 
                                Amount = @Amount,
                                Quantity = @Quantity,
                                Unit = @Unit,
                                Date = @Date
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            return db.Execute(query, expense);
        }

        public bool Delete(int id)
        {
            string query = @"update [DailyExpense] Set 
                                IsDeleted = @IsDeleted
                            Where Id = @Id";

            using IDbConnection db = _connectionFactory.GetConnection;
            db.Execute(query, new { IsDeleted = true, Id = id });
            return true;
        }
    }
}
