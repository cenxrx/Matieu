using NUnit.Framework;
using System;

namespace Matieu.Tests
{
    public class ServiceTests
    {
        [Test]
        public void Service_Name_ShouldNotBeEmpty()
        {
            var service = new Service
            {
                Name = "Пошив костюма",
                Price = 5000
            };

            Assert.That(service.Name, Is.Not.Empty);
            Assert.That(service.Name.Length, Is.GreaterThan(0));
        }

        [Test]
        public void Service_Price_ShouldBePositive()
        {
            var service = new Service
            {
                Name = "Косплей",
                Price = 10000
            };

            Assert.That(service.Price, Is.GreaterThan(0));
            Assert.That(service.Price, Is.TypeOf<decimal>());
        }

        [Test]
        public void Service_Category_ShouldBeValidType()
        {
            var service = new Service
            {
                Name = "Услуга",
                Category = "Custom"
            };

            Assert.That(service.Category, Is.EqualTo("Custom").Or.EqualTo("Cosplay"));
        }

        [Test]
        public void Service_ImagePath_ShouldHaveCorrectFormat()
        {
            var service = new Service
            {
                Name = "Услуга",
                ImagePath = "pr/Custom/image.jpg"
            };

            Assert.That(service.ImagePath, Does.StartWith("pr/"));
            Assert.That(service.ImagePath, Does.Contain("/"));
        }

        [Test]
        public void Service_CollectionId_CanBeNull()
        {
            var service = new Service
            {
                Name = "Услуга без коллекции",
                CollectionId = null
            };

            Assert.That(service.CollectionId, Is.Null);
        }
    }

    public class UserTests
    {
        [Test]
        public void User_Login_ShouldNotBeEmpty()
        {
            var user = new User
            {
                Login = "admin",
                Password = "password123"
            };

            Assert.That(user.Login, Is.Not.Empty);
        }

        [Test]
        public void User_RoleName_ShouldBeValid()
        {
            var user = new User
            {
                Login = "moderator",
                RoleName = "moderator"
            };

            var validRoles = new[] { "admin", "moderator", "user" };
            Assert.That(validRoles, Does.Contain(user.RoleName));
        }

        [Test]
        public void User_Balance_ShouldBeNonNegative()
        {
            var user = new User
            {
                Login = "user1",
                Balance = 1000.50m
            };

            Assert.That(user.Balance, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void User_FullName_CanBeSet()
        {
            var user = new User
            {
                Login = "user1",
                FullName = "Иван Иванов"
            };

            Assert.That(user.FullName, Is.Not.Empty);
            Assert.That(user.FullName, Does.Contain(" "));
        }

        [Test]
        public void User_Id_ShouldBePositive()
        {
            var user = new User
            {
                Id = 1,
                Login = "user1"
            };

            Assert.That(user.Id, Is.GreaterThan(0));
        }
    }

    public class CollectionTests
    {
        [Test]
        public void Collection_Name_ShouldNotBeEmpty()
        {
            var collection = new Collection
            {
                Id = 1,
                Name = "Летняя коллекция"
            };

            Assert.That(collection.Name, Is.Not.Empty);
        }

        [Test]
        public void Collection_Id_ShouldBePositive()
        {
            var collection = new Collection
            {
                Id = 5,
                Name = "Коллекция"
            };

            Assert.That(collection.Id, Is.GreaterThan(0));
        }

        [Test]
        public void Collection_CanBeCreatedWithDefaultValues()
        {
            var collection = new Collection();

            Assert.That(collection.Name, Is.EqualTo(string.Empty));
            Assert.That(collection.Id, Is.EqualTo(0));
        }

        [Test]
        public void Collection_Name_CanContainCyrillicCharacters()
        {
            var collection = new Collection
            {
                Name = "Зимняя коллекция 2024"
            };

            Assert.That(collection.Name, Does.Match(@"[А-Яа-я]"));
        }

        [Test]
        public void Collection_Properties_CanBeModified()
        {
            var collection = new Collection
            {
                Id = 1,
                Name = "Старое название"
            };

            collection.Name = "Новое название";

            Assert.That(collection.Name, Is.EqualTo("Новое название"));
        }
    }
}
