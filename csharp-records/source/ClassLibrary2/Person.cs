namespace ClassLibrary2
{
    public record Person : IEquatable<Person>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
}