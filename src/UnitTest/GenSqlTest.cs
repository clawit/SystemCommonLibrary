using System;
using System.Collections.Generic;
using SystemCommonLibrary.Data.DataEntity;
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

        [Fact]
        public void GenInsertSql_CreateAt_Test()
        {
            // 未赋值的 CreateAt 不写入, 交给数据库默认值
            var article = new Article() { Title = "aaa" };
            var sql = DbEntityManager.GenInsertSql(DbType.MySql, article, out bool hasIdentity);
            Assert.True(hasIdentity);
            Assert.DoesNotContain("CreateAt", sql);
            Assert.DoesNotContain("0001-01-01", sql);
            Assert.Contains("Title", sql);
            Assert.EndsWith(";Select @@IDENTITY;", sql);

            sql = DbEntityManager.GenInsertSql(DbType.PostgreSql, article, out hasIdentity);
            Assert.True(hasIdentity);
            Assert.DoesNotContain("CreateAt", sql);
            Assert.EndsWith("RETURNING Id;", sql);

            // 显式赋值的 CreateAt 照常写入
            article.CreateAt = new DateTime(2020, 12, 20, 8, 30, 0);
            sql = DbEntityManager.GenInsertSql(DbType.MySql, article, out hasIdentity);
            Assert.True(hasIdentity);
            Assert.Contains("CreateAt", sql);
            Assert.Contains("'2020-12-20 08:30:00'", sql);

            // 非 Entity 子类保持原行为
            var log = new PlainLog() { Message = "bbb" };
            sql = DbEntityManager.GenInsertSql(DbType.MySql, log, out hasIdentity);
            Assert.False(hasIdentity);
            Assert.Contains("CreateAt", sql);
            Assert.Contains("'0001-01-01 00:00:00'", sql);
        }

        public class Article : Entity
        {
            public string Title { get; set; }
        }

        public class PlainLog
        {
            public string Message { get; set; }
            public DateTime CreateAt { get; set; }
        }
    }
}
