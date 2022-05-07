using Xunit;
using AutoFixture;
using Carnect.Checkout.Services;
using Carnect.Checkout.Models;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Carnect.Checkout.UnitTest
{
    public class CheckoutServiceUnitTest
    {
        private Fixture _fixture;
        private List<Product> _allProducts;

        public CheckoutServiceUnitTest()
        {
            _fixture = new Fixture();
            Random rand = new Random();
            _allProducts = _fixture.Build<Product>().With(p => p.Price, rand.Next(10, 101)).CreateMany(3).ToList();
            
        }

        [Fact]
        public void Calculate_AppliedOffer_ReturnCorrectly()
        {
            // Arrange
            List<Product> boughtProduct = new List<Product>();
            boughtProduct.Add(_allProducts[1]);
            boughtProduct.Add(_allProducts[0]);
            boughtProduct.Add(_allProducts[1]);
            boughtProduct.Add(_allProducts[1]);

            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(-10), ToDate = DateTime.Now.Date.AddDays(10), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            decimal expectedNumber = _allProducts[1].Price + _allProducts[0].Price + allSpecialOffers[0].SpecialPrice;
            // Act
            decimal result = checkoutService.Calculate(_allProducts, allSpecialOffers, boughtProduct);
            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Fact]
        public void Calculate_NotAppliedOffer_ReturnCorrectly()
        {
            // Arrange
            List<Product> boughtProduct = new List<Product>();
            boughtProduct.Add(_allProducts[1]);
            boughtProduct.Add(_allProducts[0]);
            boughtProduct.Add(_allProducts[1]);
            boughtProduct.Add(_allProducts[1]);

            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            decimal expectedNumber = _allProducts[1].Price * 3 + _allProducts[0].Price;
            // Act
            decimal result = checkoutService.Calculate(_allProducts, allSpecialOffers, boughtProduct);
            // Assert
            Assert.Equal(expectedNumber, result);
        }

        [Fact]
        public void Calculate_ProductNotExist_ThrowError()
        {
            // Arrange
            List<Product> boughtProduct = new List<Product>();
            boughtProduct.Add(_allProducts[1]);
            boughtProduct.Add(_allProducts[0]);
            boughtProduct.Add(_fixture.Create<Product>());
            boughtProduct.Add(_fixture.Create<Product>());

            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            // Act
            Action act = () => checkoutService.Calculate(_allProducts, allSpecialOffers, boughtProduct);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Product is not exist", exception.Message);
        }

        [Fact]
        public void Calculate_BoughtProductNotExist_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            // Act
            Action act = () => checkoutService.Calculate(_allProducts, allSpecialOffers, null);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Bad data", exception.Message);
        }

        [Fact]
        public void Calculate_allProductsIsNull_ThrowError()
        {
            // Arrange
            List<Product> boughtProducts = new List<Product>();
            boughtProducts.Add(_allProducts[1]);
            boughtProducts.Add(_allProducts[0]);
            boughtProducts.Add(_fixture.Create<Product>());
            boughtProducts.Add(_fixture.Create<Product>());
            // Arrange
            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            // Act
            Action act = () => checkoutService.Calculate(null, allSpecialOffers, boughtProducts);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Bad data", exception.Message);
        }

        [Fact]
        public void Calculate_allSpecialOffersIsNull_ThrowError()
        {
            // Arrange
            List<Product> boughtProducts = new List<Product>();
            boughtProducts.Add(_allProducts[1]);
            boughtProducts.Add(_allProducts[0]);
            boughtProducts.Add(_fixture.Create<Product>());
            boughtProducts.Add(_fixture.Create<Product>());
            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            // Act
            Action act = () => checkoutService.Calculate(_allProducts, null, null);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Bad data", exception.Message);
        }

        [Fact]
        public void Calculate_BoughtProductIsEmpty_Return0()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = new List<SpecialOffer>();
            allSpecialOffers.Add(new SpecialOffer() { Id = 1, ProductId = _allProducts[1].Id, FromDate = DateTime.Now.Date.AddDays(10), ToDate = DateTime.Now.Date.AddDays(20), NumberOfProduct = 2, SpecialPrice = _allProducts[1].Price - 5 });

            CheckoutService checkoutService = new CheckoutService();
            // Act
            decimal result = checkoutService.Calculate(_allProducts, allSpecialOffers, new List<Product>());
            //assert
            Assert.Equal(0, result);
        }
    }
}