using AutoFixture;
using AutoMapper;
using EvolentHealthExercise.Core.Interfaces;
using EvolentHealthExercise.Core.Models;
using EvolentHealthExercise.WebApi.Controllers;
using EvolentHealthExercise.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactStatus = EvolentHealthExercise.WebApi.Models.ContactStatus;

namespace EvolentHealthExercise.Tests.Controllers
{
    [TestFixture]
    public class ContactControllerTests
    {
        private Mock<IContactRepository> _contactRepositoryMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _contactRepositoryMock = new Mock<IContactRepository>();
            _mapperMock = new Mock<IMapper>();
        }

        [TearDown]
        public void TearDown()
        {
            _mapperMock = null;
            _contactRepositoryMock = null;
        }

        [Test]
        public void GetContacts_WhenRequestedAll_ShouldReturnAllActiveContacts()
        {
            //Arrange
            var contacts = GetContacts();
            var contactResponse = GetContactModelResponse(contacts);
            _contactRepositoryMock.Setup(x => x.GetContacts()).ReturnsAsync(contacts);
            _mapperMock.Setup(m => m.Map<List<Contact>, List<ContactModel>>(contacts)).Returns(contactResponse);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).GetContacts();
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(baseResponseModel.Result.Equals(contactResponse));
        }

        [Test]
        public void GetContact_WhenGivenContactId_ShouldReturnContact()
        {
            //Arrange
            var contact = new Fixture().Create<Contact>();
            contact.Id = 1;

            var contactModel = new ContactModel()
            {
                Id = contact.Id,
                Email = contact.Email,
                PhoneNumber = contact.PhoneNumber,
                FirstName = contact.FirstName,
                LastName = contact.LastName
            };

            _contactRepositoryMock.Setup(x => x.GetContact(1)).ReturnsAsync(contact);
            _mapperMock.Setup(m => m.Map<Contact, ContactModel>(contact)).Returns(contactModel);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).GetContact(contact.Id);
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(baseResponseModel.Result.Equals(contactModel));
        }

        [Test]
        public void GetContact_WhenGivenContactIdNotExists_ShouldReturnEmptyRecord()
        {
            //Arrange
            _contactRepositoryMock.Setup(x => x.GetContact(1)).ReturnsAsync((Contact)null);
            _mapperMock.Setup(m => m.Map<Contact, ContactModel>(null)).Returns((ContactModel)null);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).GetContact(1);
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(null, baseResponseModel.Result);
        }

        [Test]
        public void SaveContact_WhenGivenContact_ShouldSaveContact()
        {
            //Arrange
            var contactModel = new Fixture().Create<ContactModel>();
            var contact = new Contact()
            {
                Id = contactModel.Id,
                Email = contactModel.Email,
                PhoneNumber = contactModel.PhoneNumber,
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName
            };

            _contactRepositoryMock.Setup(x => x.InsertContact(contact)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ContactModel, Contact>(contactModel)).Returns(contact);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).SaveContact(contactModel);
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(baseResponseModel.Message.Equals("Contact saved successfully"));
        }

        [Test]
        public void UpdateContact_WhenGivenContact_ShouldUpdateContact()
        {
            //Arrange
            var contactModel = new Fixture().Create<ContactModel>();
            var contact = new Contact()
            {
                Id = contactModel.Id,
                Email = contactModel.Email,
                PhoneNumber = contactModel.PhoneNumber,
                FirstName = contactModel.FirstName,
                LastName = contactModel.LastName
            };

            _contactRepositoryMock.Setup(x => x.UpdateContact(contact)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map<ContactModel, Contact>(contactModel)).Returns(contact);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).UpdateContact(contactModel);
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(baseResponseModel.Message.Equals("Contact updated successfully"));
        }

        [Test]
        public void DeleteContact_WhenGivenContactId_ShouldDeleteContact()
        {
            //Arrange
            var id = 1;

            _contactRepositoryMock.Setup(x => x.DeleteContact(id)).Returns(Task.CompletedTask);

            //Act
            var result = new ContactController(_contactRepositoryMock.Object, _mapperMock.Object).DeleteContact(id);
            var okResult = result.Result as OkObjectResult;

            //Assert
            var baseResponseModel = okResult?.Value as BaseResponseModel ?? new BaseResponseModel();
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.IsTrue(baseResponseModel.Message.Equals("Contact deleted successfully"));
        }

        private List<Contact> GetContacts()
        {
            return new Fixture().CreateMany<Contact>().ToList();
        }

        private List<ContactModel> GetContactModelResponse(List<Contact> contacts)
        {
            return contacts.Select(contact => new ContactModel()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Id = contact.Id,
                Status = (ContactStatus)contact.Status,
                PhoneNumber = contact.PhoneNumber
            }).ToList();
        }
    }
}