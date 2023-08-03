using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;

namespace EmployeeManagement.Test
{
    public class EmployeeFactoryTests : IDisposable
    {
        private EmployeeFactory _employeeFactory;

        // constructor and Dispose method will run for each test
        public EmployeeFactoryTests() 
        {
            _employeeFactory = new EmployeeFactory();
        }

        public void Dispose() 
        {
            // clean up the setup code, if required.
        }

        // [Fact(Skip = "Skipping this one for demo purposes.")] // skip example
        [Fact] 
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
        {
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee("Kevin", "Dockx");

            Assert.Equal(2500, employee.Salary);
        }
        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
        {
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee("Kevin", "Dockx");

            Assert.True(employee.Salary >= 2500 && employee.Salary <= 3500, "Salary must be between 2500 and 3500.");
        }
        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Alternative()
        {
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee("Kevin", "Dockx");

            Assert.True(employee.Salary >= 2500);
            Assert.True(employee.Salary <= 3500);
        }
        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_AlternativeWithInRange()
        {
            // Act
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee("Kevin", "Dockx");

            // Assert
            Assert.InRange(employee.Salary, 2500, 3500);
        }
        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
        public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500_PrecisionExample()
        {
            // Act
            var employee = (InternalEmployee)_employeeFactory.CreateEmployee("Kevin", "Dockx");
            employee.Salary = 2500.123m;  // 'm' is used to initialize the number as decimal instead of double (standard)

            // Assert
            Assert.Equal(2500, employee.Salary, 0);
        }
        [Fact]
        [Trait("Category", "EmployeeFactory_CreateEmployee_ReturnType")]
        public void CreateEmployee_IsËxternalIsTrue_ReturnTypeMustBeExternalEmployee()
        {
            // Act
            var employee = _employeeFactory.CreateEmployee("Kevin", "Dockx", "Marvin", true);

            // Assert
            Assert.IsType<ExternalEmployee>(employee);
            Assert.IsNotType<InternalEmployee>(employee);
            Assert.IsAssignableFrom<Employee>(employee);  // ExternalEmployee is a subclass of Employee
        }
    }
}
