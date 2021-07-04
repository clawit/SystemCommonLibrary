using System.Collections.Generic;
using SystemCommonLibrary.Data.Manager;
using SystemCommonLibrary.Enums;
using Xunit;

namespace UnitTest
{
    public class GenSqlTest
    {
        [Fact]
        public void GenFilterSql_Test()
        {
            QueryFilter filter = null;
            var sql = DbEntityManager.GenFilterSql(DbType.MySql, filter);
            Assert.Equal("", sql);

            filter = new QueryFilter() {
                Key = "Name",
                Value = "aaa",
                Comparison = QueryComparison.Equal
            };
            sql = DbEntityManager.GenFilterSql(DbType.MySql, filter);
            Assert.Equal("(`Name` = 'aaa')", sql);

            filter = new QueryFilter()
            {
                SubFilters = new List<QueryFilter> {
                    new QueryFilter(){
                        Key = "Name",
                        Value = "aaa",
                        Comparison = QueryComparison.Equal
                    },
                    new QueryFilter(){
                        Key = "Name",
                        Value = "bbb",
                        Comparison = QueryComparison.Equal,
                        Operator = QueryOperator.Or
                    },
                }
            };
            sql = DbEntityManager.GenFilterSql(DbType.MySql, filter);
            Assert.Equal("((`Name` = 'aaa') Or (`Name` = 'bbb'))", sql);

            filter = new QueryFilter()
            {
                SubFilters = new List<QueryFilter> {
                    new QueryFilter(){
                        Key = "Name",
                        Value = "aaa",
                        Comparison = QueryComparison.Equal
                    },
                    new QueryFilter(){
                        Operator = QueryOperator.And,
                        SubFilters = new List<QueryFilter>{ 
                            new QueryFilter(){
                                Key = "Status",
                                Value = 3,
                                Comparison = QueryComparison.LessEqual
                            },
                            new QueryFilter(){
                                Key = "Status",
                                Value = 5,
                                Comparison = QueryComparison.Greater,
                                Operator = QueryOperator.Or
                            }
                        }
                    },
                    new QueryFilter(){
                        Key = "OpenId",
                        Value = "oGi",
                        Comparison = QueryComparison.Like
                    },
                }
            };
            sql = DbEntityManager.GenFilterSql(DbType.MySql, filter);
            Assert.Equal("((`Name` = 'aaa') And ((`Status` <= 3) Or (`Status` > 5)) And (`OpenId` like '%oGi%'))", sql);

        }
    }
}
