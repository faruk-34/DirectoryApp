using DirectoryApi.Application.Services;
using DirectoryApi.Domain.Entities;
using DirectoryApi.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Directory = DirectoryApi.Domain.Entities.Directory;



namespace DirectoryApi.Test.Application
{
    public class ContactInfoTest
    {
        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
                  {"DirectoryApp", "DirectoryAppTest"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return configuration;
        }

        private DirectoryDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<DirectoryDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + System.Guid.NewGuid())
                .Options;


            var configuration = GetTestConfiguration();
            return new DirectoryDbContext(options, configuration);
        }

        [Fact]
        public async Task Insert_Should_Add_ContactInfo_And_Return_Directory_With_Contacts()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var directory = new Directory
            {
                Id = 1,
                Name = "Test Directory",
                Company = "Test Company",
                Surname = "Test Surname",
                ContactInfos = new List<ContactInfo>()
            };
            context.Directorys.Add(directory);
            await context.SaveChangesAsync();

            var contactInfo = new ContactInfo
            {
                DirectoryId = 1,
                Email = "test@example.com",
                Address = "Test adres",
                Location = "İstanbul",
                Phone = "5686996587"
            };

            var service = new ContactInfoService(context);

            // Act
            var result = await service.Insert(contactInfo);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Single(result.ContactInfos);
            Assert.Equal("test@example.com", result.ContactInfos.First().Email);
        }

        [Fact]
        public async Task Delete_Should_Remove_ContactInfo_And_Return_Directory_Without_That_Contact()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var contactInfo = new ContactInfo
            {
                Id = 1,
                DirectoryId = 1,
                Email = "test@example.com",
                Address = "Test adres",
                Location = "İstanbul",
                Phone = "5686996587"
            };
            var directory = new Directory
            {
                Id = 1,
                Name = "Test Directory",
                Company = "Test Company",
                Surname = "Test Surname",
                ContactInfos = new List<ContactInfo> { contactInfo }
            };

            context.Directorys.Add(directory);
            context.ContactInfos.Add(contactInfo);
            await context.SaveChangesAsync();

            var service = new ContactInfoService(context);

            // Act
            var result = await service.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Empty(result.ContactInfos);
            Assert.Null(await context.ContactInfos.FindAsync(1));
        }
    }
}
