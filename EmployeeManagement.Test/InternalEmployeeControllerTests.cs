using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test
{
    public class InternalEmployeeControllerTests
    {
        private readonly InternalEmployeesController _internalEmployeesController;
        private readonly InternalEmployee _firstEmployee;
        public InternalEmployeeControllerTests() 
        {
            _firstEmployee = new InternalEmployee("Megan", "Jones", 2, 3000, false, 2);

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(
                    new List<InternalEmployee>()
                    {
                        _firstEmployee,
                        new InternalEmployee("Jaimy", "Johnson", 3, 3000, true, 1),
                        new InternalEmployee("Anne", "Adams", 3, 3000, false, 3)
                    });

            /*var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(m => m.Map<InternalEmployee, Models.InternalEmployeeDto>(It.IsAny<InternalEmployee>()))
                .Returns(new Models.InternalEmployeeDto());*/

            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MapperProfiles.EmployeeProfile>());
            var mapper = new Mapper(mapperConfiguration);

            _internalEmployeesController = new InternalEmployeesController(employeeServiceMock.Object, mapper);
        }

        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult() 
        {
            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
            Assert.IsType<OkObjectResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnIEnumerableOfInternalEmployeeDtoAsModelType() 
        {
            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
            Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(((OkObjectResult)actionResult.Result).Value);
        }
        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnNumberOfInputtedInternalEmployees() 
        {
            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
            var actionResultOk = (OkObjectResult)actionResult.Result;
            var actionResultValue = (IEnumerable<Models.InternalEmployeeDto>)actionResultOk.Value;
            Assert.Equal(3, actionResultValue.Count());
        }

        [Fact]
        public async Task GetInternalEmployees_GetAction_ReturnsOkObjectResultWithCorrectAmountOfInternalEmployees() 
        {
            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.InternalEmployeeDto>>>(result);
            var okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var dtos = Assert.IsAssignableFrom<IEnumerable<Models.InternalEmployeeDto>>(okObjectResult.Value);
            Assert.Equal(3, dtos.Count());
            var firstEmployee = dtos.First();
            Assert.Equal(_firstEmployee.Id, firstEmployee.Id);
            Assert.Equal(_firstEmployee.FirstName, firstEmployee.FirstName);
            Assert.Equal(_firstEmployee.LastName, firstEmployee.LastName);
            Assert.Equal(_firstEmployee.Salary, firstEmployee.Salary);
            Assert.Equal(_firstEmployee.SuggestedBonus, firstEmployee.SuggestedBonus);
            Assert.Equal(_firstEmployee.YearsInService, firstEmployee.YearsInService);
        }
    }
}
