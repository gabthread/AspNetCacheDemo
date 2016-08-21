using AspNetCacheDemo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Test
{
    [TestClass]
    public class CacheDemoTests
    {
        private ICacheManager _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new AspNetCacheManager();
        }

        [TestMethod]
        public void AddOrUpdate_ShouldAddAValueToTheCache_WhenIsCalled()
        {

            var cacheItem = new CacheItem()
            {
                CacheDuration = new TimeSpan(0, 0, 0, 120),
                Key = "testKey",
                Value = "12345"
            };

            _sut.AddOrUpdate(cacheItem);

            Assert.AreEqual("12345", _sut.Get<string>("testKey"));
        }

        [TestMethod]
        public void Remove_ShouldRemoveValueFromTheCache_WhenIsCalled()
        {
            //Arrange
            var cacheItem = new CacheItem()
            {
                CacheDuration = new TimeSpan(0, 0, 0, 120),
                Key = "testKey",
                Value = "12345"
            };

            _sut.AddOrUpdate(cacheItem);

            //Act
            _sut.Remove(cacheItem);

            //Assert
            Assert.AreEqual(null, _sut.Get<string>("testKey"));
        }

        [TestMethod]
        public void RemoveAll_ShouldRemoveAllValueFromTheCache_WhenIsCalled()
        {
            //Arrange
            var cacheItem = new CacheItem()
            {
                CacheDuration = new TimeSpan(0, 0, 0, 120),
                Key = "testKey",
                Value = "12345"
            };

            var cacheItem2 = new CacheItem()
            {
                CacheDuration = new TimeSpan(0, 0, 0, 120),
                Key = "testKey2",
                Value = "6789"
            };

            _sut.AddOrUpdate(cacheItem);
            _sut.AddOrUpdate(cacheItem2);

            //Act
            _sut.RemoveAll();

            //Assert
            Assert.AreEqual(null, _sut.Get<string>("testKey"));
            Assert.AreEqual(null, _sut.Get<string>("testKey2"));
        }


        [TestMethod]
        public void GetWithFallback_ShouldCallTheFallbackFunctionToGetTheValueAndStoreIt_WhenTheValueIsNotStoredInTheCache()
        {

            //Act
            _sut.GetWithFallback("testKey", 2, () => GetFakeValue());

            //Assert
            Assert.AreEqual("Fake Value", _sut.Get<string>("testKey"));
        }

        [TestMethod]
        public void GetWithFallback_ShouldNotCallTheFallbackFunctionToGetTheValue_WhenTheValueIsStoredInTheCache()
        {
            //Arrange
            var cacheItem = new CacheItem()
            {
                CacheDuration = new TimeSpan(0, 0, 0, 120),
                Key = "testKey",
                Value = "12345"
            };

            _sut.AddOrUpdate(cacheItem);

            //Act
            _sut.GetWithFallback("testKey", 2, () => GetFakeValue());

            //Assert
            Assert.AreEqual("12345", _sut.Get<string>("testKey"));
        }

        private string GetFakeValue()
        {
            return "Fake Value";
        }



    }
}
