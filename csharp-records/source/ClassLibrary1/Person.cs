namespace ClassLibrary1
{
    public sealed class Person : IEquatable<Person>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Equals(Person? other)
        {
            throw new NotImplementedException();
        }
    }
}