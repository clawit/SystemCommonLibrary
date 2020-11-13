using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;
using SystemCommonLibrary.TinyMapper;

namespace UnitTest
{
    public enum TestState 
    { 
        Ok = 1,
        Fail = 0,
        Ready = 3
    }
    public class Order
    { 
        [Key]
        public int Id { get; set; }

        public string Creator { get; set; }

        public DateTime CreateAt { get; set; }

        public decimal Price { get; set; }

        public TestState State { get; set; }

        public string Desc { get; set; }

    }

    public class OrderExt : Order
    { 
        public string Address { get; set; }
    }

    public class OrderInfo
    {
        public int Id { get; set; }

        public string Creator { get; set; }

        public DateTime CreateAt { get; set; }

        public decimal Price { get; set; }

        public int State { get; set; }

        public string Address { get; set; }

        public string Invoice { get; set; }
    }

    public class MapperTest
    {
        [Fact]
        public void MapTest_Order_OrderExt()
        {
            Order order = new Order()
            {
                Id = 99,
                Creator = "AAA",
                CreateAt = new DateTime(2020, 12, 20),
                Price = 23.4M,
                State = TestState.Ready,
                Desc = "BBB",
            };

            var orderExt = order.MapTo<OrderExt>();

            Assert.Equal(99, orderExt.Id);
            Assert.Equal("AAA", orderExt.Creator);
            Assert.Equal(2020, orderExt.CreateAt.Year);
            Assert.Equal(12, orderExt.CreateAt.Month);
            Assert.Equal(20, orderExt.CreateAt.Day);
            Assert.Equal(23.4M, orderExt.Price);
            Assert.Equal(TestState.Ready, orderExt.State);
            Assert.Equal("BBB", orderExt.Desc);
            Assert.Null(orderExt.Address);
        }

        [Fact]
        public void MapTest_Order_OrderInfo()
        {
            Order order = new Order()
            {
                Id = 99,
                Creator = "AAA",
                CreateAt = new DateTime(2020, 12, 20),
                Price = 23.4M,
                State = TestState.Ready,
                Desc = "BBB",
            };

            var orderInfo = order.MapTo<OrderInfo>();

            Assert.Equal(99, orderInfo.Id);
            Assert.Equal("AAA", orderInfo.Creator);
            Assert.Equal(2020, orderInfo.CreateAt.Year);
            Assert.Equal(12, orderInfo.CreateAt.Month);
            Assert.Equal(20, orderInfo.CreateAt.Day);
            Assert.Equal(23.4M, orderInfo.Price);
            Assert.Equal(3, orderInfo.State);
            Assert.Null(orderInfo.Address);
        }

        [Fact]
        public void MapTest_OrderExt_Order()
        {
            OrderExt orderExt = new OrderExt()
            {
                Id = 99,
                Creator = "AAA",
                CreateAt = new DateTime(2020, 12, 20),
                Price = 23.4M,
                State = TestState.Ready,
                Desc = "BBB",
                Address = "CCC"
            };

            var order = orderExt.MapTo<Order>();

            Assert.Equal(99, order.Id);
            Assert.Equal("AAA", order.Creator);
            Assert.Equal(2020, order.CreateAt.Year);
            Assert.Equal(12, order.CreateAt.Month);
            Assert.Equal(20, order.CreateAt.Day);
            Assert.Equal(23.4M, order.Price);
            Assert.Equal(TestState.Ready, order.State);
            Assert.Equal("BBB", order.Desc);
        }

        [Fact]
        public void MapTest_OrderExt_OrderInfo()
        {
            OrderExt orderExt = new OrderExt()
            {
                Id = 99,
                Creator = "AAA",
                CreateAt = new DateTime(2020, 12, 20),
                Price = 23.4M,
                State = TestState.Ready,
                Desc = "BBB",
                Address = "CCC"
            };

            var orderInfo = orderExt.MapTo<OrderInfo>();

            Assert.Equal(99, orderInfo.Id);
            Assert.Equal("AAA", orderInfo.Creator);
            Assert.Equal(2020, orderInfo.CreateAt.Year);
            Assert.Equal(12, orderInfo.CreateAt.Month);
            Assert.Equal(20, orderInfo.CreateAt.Day);
            Assert.Equal(23.4M, orderInfo.Price);
            Assert.Equal(3, orderInfo.State);
            Assert.Null(orderInfo.Invoice);
        }

        [Fact]
        public void MapTest_OrderInfo_Order()
        {
            OrderInfo orderInfo = new OrderInfo()
            {
                Id = 99,
                Creator = "AAA",
                CreateAt = new DateTime(2020, 12, 20),
                Price = 23.4M,
                State = 3,
                Invoice = "BBB",
                Address = "CCC"
            };

            var order = orderInfo.MapTo<Order>();

            Assert.Equal(99, order.Id);
            Assert.Equal("AAA", order.Creator);
            Assert.Equal(2020, order.CreateAt.Year);
            Assert.Equal(12, order.CreateAt.Month);
            Assert.Equal(20, order.CreateAt.Day);
            Assert.Equal(23.4M, order.Price);
            Assert.Equal(TestState.Ready, order.State);
        }
    }
}
