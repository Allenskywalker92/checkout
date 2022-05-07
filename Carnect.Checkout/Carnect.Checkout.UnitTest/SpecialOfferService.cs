using AutoFixture;
using Carnect.Checkout.Models;
using Carnect.Checkout.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Carnect.Checkout.UnitTest
{
    public class SpecialOfferServiceUnitTest
    {
        private Fixture _fixture;

        public SpecialOfferServiceUnitTest()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void AddSpecialOffer_allSpecialOffersIsNull_ThrowError()
        {
            // Arrange
            SpecialOffer specialOffers = _fixture.Build<SpecialOffer>().Create();

            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(null, specialOffers);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Bad data", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_specialOfferIsNull_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>().CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();

            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, null);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Bad data", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_FromDateGreaterThanToDate_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>().CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.FromDate = DateTime.Now.AddDays(1);
            specialOffer.ToDate = DateTime.Now;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("From date cannot be greater than to date", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_PriceIs0_AddSuccess()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>().CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.FromDate = DateTime.Now;
            specialOffer.ToDate = DateTime.Now.AddDays(10);
            specialOffer.SpecialPrice = 0;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Assert.Equal(4, allSpecialOffers.Count);
            Assert.Equal(specialOffer.Id, allSpecialOffers[3].Id);
        }

        [Fact]
        public void AddSpecialOffer_PriceLessThan0_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>().CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.FromDate = DateTime.Now;
            specialOffer.ToDate = DateTime.Now.AddDays(10);
            specialOffer.SpecialPrice = -10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Price cannot be less than 0", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_SameProductOffer_FromDateGreater_ToDateGreater__ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>()
                .With(x => x.FromDate, DateTime.Now)
                .With(x => x.ToDate, DateTime.Now.AddDays(7))
                .CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.ProductId = allSpecialOffers[0].ProductId;
            specialOffer.FromDate = allSpecialOffers[0].FromDate.AddDays(5);
            specialOffer.ToDate = allSpecialOffers[0].ToDate.AddDays(5);
            specialOffer.SpecialPrice = 10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Special offer is exist", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_SameProductOffer_FromDateLesser_ToDateLesser_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>()
                .With(x => x.FromDate, DateTime.Now)
                .With(x => x.ToDate, DateTime.Now.AddDays(7))
                .CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.ProductId = allSpecialOffers[0].ProductId;
            specialOffer.FromDate = allSpecialOffers[0].FromDate.AddDays(-5);
            specialOffer.ToDate = allSpecialOffers[0].ToDate.AddDays(-5);
            specialOffer.SpecialPrice = 10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Special offer is exist", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_SameProductOffer_FromDateLesser_ToDateGreater_ThrowError()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>()
                .With(x => x.FromDate, DateTime.Now)
                .With(x => x.ToDate, DateTime.Now.AddDays(7))
                .CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.ProductId = allSpecialOffers[0].ProductId;
            specialOffer.FromDate = allSpecialOffers[0].FromDate.AddDays(-5);
            specialOffer.ToDate = allSpecialOffers[0].ToDate.AddDays(10);
            specialOffer.SpecialPrice = 10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            Action act = () => specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Exception exception = Assert.Throws<Exception>(act);
            Assert.Equal("Special offer is exist", exception.Message);
        }

        [Fact]
        public void AddSpecialOffer_SameProduct_FromDateAndTodateGreater_ToDate_AddSuccess()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>()
                .With(x => x.FromDate, DateTime.Now)
                .With(x => x.ToDate, DateTime.Now.AddDays(7))
                .CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.ProductId = allSpecialOffers[0].ProductId;
            specialOffer.FromDate = allSpecialOffers[0].ToDate.AddDays(5);
            specialOffer.ToDate = allSpecialOffers[0].ToDate.AddDays(12);
            specialOffer.SpecialPrice = 10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Assert.Equal(4, allSpecialOffers.Count);
            Assert.Equal(specialOffer.Id, allSpecialOffers[3].Id);
        }

        [Fact]
        public void AddSpecialOffer_SameProduct_FromDateAndTodateLesser_FromDate_AddSuccess()
        {
            // Arrange
            List<SpecialOffer> allSpecialOffers = _fixture.Build<SpecialOffer>()
                .With(x => x.FromDate, DateTime.Now)
                .With(x => x.ToDate, DateTime.Now.AddDays(7))
                .CreateMany(3).ToList();
            SpecialOffer specialOffer = _fixture.Build<SpecialOffer>().Create();
            specialOffer.ProductId = allSpecialOffers[0].ProductId;
            specialOffer.FromDate = allSpecialOffers[0].FromDate.AddDays(-12);
            specialOffer.ToDate = allSpecialOffers[0].FromDate.AddDays(-5);
            specialOffer.SpecialPrice = 10;
            SpecialOfferService specialOfferService = new SpecialOfferService();
            // Act
            specialOfferService.AddSpecialOffer(allSpecialOffers, specialOffer);
            //assert
            Assert.Equal(4, allSpecialOffers.Count);
            Assert.Equal(specialOffer.Id, allSpecialOffers[3].Id);
        }
    }
}
