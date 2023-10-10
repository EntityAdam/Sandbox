using Xunit;

namespace TestProject1
{
    public class PersonTests1_Poco
    {
        [Fact]
        public void SameObject_ShouldHave_ReferentialEquality()
        {
            ClassLibrary1.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary1.Person person2 = person1;
            Assert.Same(person1, person2);
        }

        [Fact]
        public void SameObject_HasValueEqualityWithoutEqualityContract_BecauseShortCiruitedByReferentialEquality()
        {
            ClassLibrary1.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary1.Person person2 = person1;
            Assert.Equal(person1, person2);
        }

        [Fact]
        public void DifferentObjects_DoNotHaveValueEquality_WithoutEqualityContract()
        {
            ClassLibrary1.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary1.Person person2 = new() { FirstName = "John", LastName = "Doe" };
            Assert.NotEqual(person1, person2);
        }
    }
}