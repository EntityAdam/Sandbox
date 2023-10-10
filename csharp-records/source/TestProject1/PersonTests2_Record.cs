using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class PersonTests2_Record
    {
        [Fact]
        public void SameObject_ShouldHave_ReferentialEquality()
        {
           
            ClassLibrary2.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary2.Person person2 = person1;
            Assert.Same(person1, person2);
        }

        [Fact]
        public void SameObject_HasValueEqualityWithoutEqualityContract_BecauseShortCiruitedByReferentialEquality()
        {
            ClassLibrary2.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary2.Person person2 = person1;
            Assert.Equal(person1, person2);
        }

        [Fact]
        public void DifferentObjects_DoNotHaveValueEquality_WithoutEqualityContract()
        {
            ClassLibrary2.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary2.Person person2 = new() { FirstName = "John", LastName = "Doe" };
            Assert.Equal(person1, person2);
        }

        [Fact]
        public void ValueEquality_ShouldBe_CaseSensitive()
        {
            ClassLibrary3.Person person1 = new() { FirstName = "John", LastName = "Doe" };
            ClassLibrary3.Person person2 = new() { FirstName = "john", LastName = "doe" };
            Assert.NotEqual(person1, person2);
        }
    }
}
