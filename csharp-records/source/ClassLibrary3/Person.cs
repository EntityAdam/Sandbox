namespace ClassLibrary3
{
    public class Person : IEquatable<Person>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static bool operator ==(Person person1, Person person2) => person1.Equals(person2);

        public static bool operator !=(Person person1, Person person2) => !person1.Equals(person2);

        public bool Equals(Person? other) => (object)this == other
                   || string.Equals(FirstName, other.FirstName, StringComparison.CurrentCultureIgnoreCase)
                   && string.Equals(LastName, other.LastName, StringComparison.CurrentCultureIgnoreCase);

        public override bool Equals(object? obj) => Equals(obj as Person);

        public override int GetHashCode() => HashCode.Combine(FirstName, LastName);
    }
}