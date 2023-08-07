﻿using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace EmployeeManagement.Test
{
    public class DemoInternalEmployeesControllerTests
    {
        [Fact]
        public async Task CreateInternalEmployee_InvalidInput_MustReturnBadRequest()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();
            var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object, mapperMock.Object);

            var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto();
            demoInternalEmployeesController.ModelState.AddModelError("FirstName", "Required");

            // Act
            var result = await demoInternalEmployeesController.CreateInternalEmployee(internalEmployeeForCreationDto);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.InternalEmployeeDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }
        [Fact]
        public void GetProtectedInternalEmployees_GetActionForUserInAdminrole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees() 
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();
            var demoInternalEmployeesController = new DemoInternalEmployeesController(employeeServiceMock.Object, mapperMock.Object);

            var userClaims = new List<Claim>()
            { 
                new Claim(ClaimTypes.Name, "Karen"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var claimsIdentity = new ClaimsIdentity(userClaims, "UnitTest");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            var httpContext = new DefaultHttpContext()
            { 
                User = claimsPrincipal
            };

            demoInternalEmployeesController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            // Act
            var result = demoInternalEmployeesController.GetProtectedInternalEmployees();


            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("GetInternalEmployees", redirectToActionResult.ActionName);
            Assert.Equal("ProtectedInternalEmployees", redirectToActionResult.ControllerName);
        }
    }
}
