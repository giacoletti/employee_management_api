﻿using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Test.Fixtures;

namespace EmployeeManagement.Test
{
    [Collection("EmployeeServiceCollection")]
    public class DataDrivenEmployeeServiceTests // : IClassFixture<EmployeeServiceFixture>
    {
        private readonly EmployeeServiceFixture _employeeServiceFixture;

        public DataDrivenEmployeeServiceTests(
            EmployeeServiceFixture employeeServiceFixture)
        {
            _employeeServiceFixture = employeeServiceFixture;
        }

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithProperty 
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {100, true},
                    new object[] {200, false},
                };
            }
        }

        public static IEnumerable<object[]> ExampleTestDataForGiveRaise_WithMethod(int testDataInstancesToProvide)
        {
            var testData = new List<object[]>
                {
                    new object[] {100, true}, 
                    new object[] {200, false},
                };

            return testData.Take(testDataInstancesToProvide);
        }

        [Theory]
        [InlineData("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e")]
        [InlineData("37e03ca7-c730-4351-834c-b66f280cdb01")]
        public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedObligatoryCourse(Guid courseId)
        {
            // Act
            var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

            // Assert
            Assert.Contains(internalEmployee.AttendedCourses, course => course.Id == courseId);
        }

        [Fact]
        public async Task GiveRaise_MinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeTrue()
        {
            // Arrange  
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act
            await _employeeServiceFixture
                .EmployeeService.GiveRaiseAsync(internalEmployee, 100);

            // Assert
            Assert.True(internalEmployee.MinimumRaiseGiven);
        }


        [Fact]
        public async Task GiveRaise_MoreThanMinimumRaiseGiven_EmployeeMinimumRaiseGivenMustBeFalse()
        {
            // Arrange  
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act 
            await _employeeServiceFixture.EmployeeService
                .GiveRaiseAsync(internalEmployee, 200);

            // Assert
            Assert.False(internalEmployee.MinimumRaiseGiven);
        }

        [Theory]
        [MemberData(
            nameof(DataDrivenEmployeeServiceTests.ExampleTestDataForGiveRaise_WithMethod),
            1,
            MemberType = typeof(DataDrivenEmployeeServiceTests))]
        public async Task GiveRaise_RaiseGiven_EmployeeMinimumRaiseGivenMatchesValue(int raiseGiven, bool expectedValueForMinimumRaiseGiven)
        {
            // Arrange  
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);

            // Act 
            await _employeeServiceFixture.EmployeeService
                .GiveRaiseAsync(internalEmployee, raiseGiven);

            // Assert
            Assert.Equal(expectedValueForMinimumRaiseGiven, internalEmployee.MinimumRaiseGiven);
        }
    }
}
