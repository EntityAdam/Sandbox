# Everything you need to know about C# records

---

# Intro

---

Records were introduced in C# 9  
Records are fancy classes

```cs
internal record Dog(string Name, int Age);
```

record class and record struct were introduced in C# 10

```cs
//Fancy class
internal record Dog(string Name, int Age);
internal record class Dog(string Name, int Age);

//Fancy struct
internal record struct Cat(string Name, int Age)
```

---

# Features and characteristics with examples

---

## Shorthand declaration with positional parameters

```cs
internal record Dog(string Name, int Age);

//Usage
Dog dog1 = new("Ed", 5);
```

---

## Records are reference types by default

```cs
//same same
internal record Dog(string Name, int Age);
internal record class Dog(string Name, int Age);

//value type
internal record struct Dog(string Name, int Age);
```

---

## Declaration with constructor

```cs
internal record Dog
{
    public Dog(string Name, int Age)
    {
        this.Name = Name;
        this.Age = Age;
    }
    public string Name { get; init; }
    public int Age { get; init; }
}

//Usage
Dog dog1 = new("Ed", 5);
var dog2 = new Dog() = { Name = "Ed", Age = 5 };
```

---

## Long Declaration without positional parameters

```cs
internal record Dog
{
    public required string Name { get; init; }
    public required int Age { get; init; }
}

//Usage
Dog dog1 = new() { Name = "Ed", Age = 5 };
```

---
## Modifying an auto property

```cs
internal record Dog(string Name, int Age)
{
    public int Age { get; set; } = Age;
}

//Usage
Dog dog1 = new("Ed", 5);
dog1.Age = 10;
```

---
## With mutable properties

```cs
internal record Dog
{
    public string Name { get; init; }
    public int Age { get; set; }
}

//Usage
Dog dog1 = new("Ed", 5);
dog1.Age = 10;
```

---
## Applying attributes without positional parameters

```cs
internal record Dog
{
    [Required, StringLength(50, MinimumLength = 2)]
    public required string Name { get; init; }

    [Required]
    public required int Age { get; init; }
}
```

---
## Applying attributes with positional parameters

```cs
internal record Dog(
    [property: Required, StringLength(50, MinimumLength = 2)] string Name,
    [property: Required] int Age
    );
```

---
## Non-destructive mutation

```cs
internal record Dog(string Name, int Age);

//Usage
Dog dog1 = new("Ed", 5);
Dog dog2 = dog1 with { Name = "Bob"};
```

---
## Inheritance

```cs
internal abstract record Pet(string Name, int Age);
internal sealed record Dog(string Name, int Age) : Pet(Name, Age);
internal sealed record Cat(string Name, int Age) : Pet(Name, Age);

//Usage
Pet dog = new Dog("Ed", 5);
Pet cat = new Cat("Ed", 5);

//GOTCHA - Type matters! Even though cat and dog are both pets and are equal in value, if you compare them they are not equal.
```

---
## Derived records

- Records can only derive from records

```cs
internal abstract record Pet(string Name, int Age);
internal sealed record Dog(string Name, int Age, bool IsGoodBoy) : Pet(Name, Age);

//Usage - Valid
Pet dog1 = new Dog("Ed", 5, true);
Pet dog2 = dog1 with { Name = "Hank", Age = 2 };

//Usage - Invalid.  dog1 is a Pet and you can't mutate a pet into a dog
Dog dog2 = dog1 with { Name = "Hank", Age = 2 };

//Usage - Gotcha.  
//dog1 is a Pet and you can't set the IsGoodBoy using with keyword
//you can only set the properties for the compile time type
Dog dog2 = dog1 with { Name = "Hank", Age = 2 /*, IsGoodBoy = false */ };
```

---
## Deconstruction

```cs
internal record Dog(string Name, int Age);

//Usage
Dog dog1 = new Dog("Ed", 5);
var (name, age) = dog1;
Console.WriteLine($"Dog Name: {name}");
Console.WriteLine($"Dog Age: {age}"); 
```

---
## You can only deconstruct positional parameters

```cs
internal record Dog(string Name, int Age)
{
    public bool IsGoodBoy { get; set; } = true;
}

Dog dog = new("Ed", 5);
var (name, age) = dog;
/* invalid
   var (name, age, isGoodBoy) = dog
*/

```

---
## Deconstruction for derived types

```cs
internal abstract record Pet(string Name, int Age);
internal sealed record Dog(string Name, int Age, bool IsGoodBoy) : Pet(Name, Age);

//Usage - Can only deconstruct the compile time type here:
Pet dog1 = new Dog("Ed", 5, true);
var (name, age) = dog1;
Console.WriteLine($"Dog Name: {name}");
Console.WriteLine($"Dog Age: {age}");

//Usage - But we can explicitly cast dog1 to the runtime type
Pet dog1 = new Dog("Ed", 5, true);
var (name, age, isGoodBoy) = dog1 as Dog;
Console.WriteLine($"Dog Name: {name}");
Console.WriteLine($"Dog Age: {age}");
Console.WriteLine($"{name} {(isGoodBoy ? "is" : "is not")} a good boy");
```

---
## Record struct - It's properties are mutable!

```cs
internal record struct Dog(string Name, int Age);

//Usage 
Dog dog1 = new("Ed", 5);
dog1.Name = "Hank";
dog1.Age = 2;
```

---
## Read-only record struct

```cs
internal readonly record struct Dog(string Name, int Age);

//Usage
Dog dog1 = new("Ed", 5);
```

---
## Gotcha - Records immutability is shallow

```cs
internal record Dog(string Name, int Age)
{
    public string[] Toys { get; init; } = Array.Empty<string>();
}

//Usage 
var toys = new string[] { "biscuit", "newspaper" };
Dog dog1 = new("Ed", 5) { Toys = toys };
toys[0] = "ball";
Console.WriteLine(string.Join(" - ", dog1.Toys));

//Output
ball - newspaper
```

---
# When to use them?
---
- Always use a class
- Unless
	- You are sure you want to define a data model that depends on value equality
	- You are sure you want to define a type for which objects are immutable.
- Or unless
	- You are sure you want to use a struct
---
## Domain Objects

Should I use records as domain objects?

> It depends.

Gotcha's:
- Using records instead of classes is a functional paradigm. If you are not familiar with functional programming
	- OOP suggest that data and processes are combined: ref: **Martin Fowler, Anemic Domain Model, 2003**
	- Using records instead of classes will make your models 'anemic' (contains no business logic). This moves your business logic to the 'manager' classes. This is OK as long as you are aware of it.

Yes, where domain objects require value equality. 
Example; business object, value object

```cs
internal sealed record class Vehicle(string Vin, string Make, string Model, int Year);
internal sealed record class DriversLicense(string LiscenceNumber, string StateIssued)
internal sealed record class LiscensePlate(string PlateNumber)

public class VehicleRegistrationManager
{
    //my immutable records
	public Vehicle Vehicle { get; set; }
	public DriversLicense DriversLicense { get; set; }
	public LicensePlate LicensePlate { get; set; }
    
    //my mutable properties and methods to process a registration
	public DateTime RegisteredOn { get; set; }
	public bool HasAllPaperwork { get; set; } = false;
}
```

---
## DTOs

It depends.

External Data Contracts: Yes -- Example, REST API GET
  - Immutable - You can pass it around.
  - Deconstruction is great here too.
  - Cache Hit / Miss - This might be great for cache hit, because `GetHashcode()` is value based?

---
## Domain <--> Services & Projection - Yes

```cs
var _petService = new PetService();
Dog dog = new("Ed", 5);
var (name, age) = dog;
await _petService.CreateDog(new DogEntity() { Name = name, Age = age });

public record Dog(string Name, int Age);
public class DogEntity
{
    public string? Name { get; set; }
    public int Age { get; set; }
}
public class PetService
{
    public List<DogEntity> Dogs { get;set; } = new List<DogEntity>();

    public async Task CreateDog(DogEntity entity) => Dogs.Add(entity);
    public async Task<IEnumerable<DogEntity>> GetAllDogs() => Dogs;
}
```

---
## Json - Yes (System.Text.Json)

```cs
public record Dog(string Name, int Age);

Dog dog = new("Hank", 2);
var serialized = JsonSerializer.Serialize(dog);
Dog deserilizedDog = JsonSerializer.Deserialize<Dog>(serialized);
Console.WriteLine(deserilizedDog);
```

---
## XML - No

```cs
internal record Dog(string Name, int Age);

Dog dog = new("Hank", 2);
//Fail - Needs parameter less constructor
XmlSerializer xmlSerializer = new XmlSerializer(dog.GetType()); 
xmlSerializer.Serialize(Console.Out, dog);
```

---
## Entity Framework

NO - EF depends on reference equality for entities to do it's job.

---
## Dapper (ORM)

NO - Requires public parameter-less constructor

---
# DO NOT DO
---
## Hack: Force public parameter-less constructor

Records were not designed with a public parameter-less constructor.
If you need a public parameter-less constructor, use a class.

```cs
public record Dog(string Name, int Age)
{
    public Dog() : this(default, default) { }
}
```

---
## Misuse

This example 
 - is mutable. (set vs init)
 - does not have a constructor so it will not have a synthesized deconstructor
 - can not be value equal
- completely defeats the purpose of a record
- should be a class.

```cs
public record Dog
{
	public string Name { get; set; }
	public int Age { get; set; }
}
```




## A record lowered to C# 8

```cs
    internal sealed class Dog : IEquatable<Dog>
    {
        public Dog(string Name, int Age)
        {
            this.Name = Name;
            this.Age = Age;
        }

        public string Name { get; set; /* init */ }

        public int Age { get; set; /* init */ }

        private Dog(Dog original)
        {
            Name = original.Name;
            Age = original.Age;
        }

        public void Deconstruct(out string Name, out int Age)
        {
            Name = this.Name;
            Age = this.Age;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Dog");
            stringBuilder.Append(" { ");
            if (PrintMembers(stringBuilder))
            {
                stringBuilder.Append(' ');
            }
            stringBuilder.Append('}');
            return stringBuilder.ToString();
        }

        public static bool operator !=(Dog? left, Dog? right)
        {
            return !(left == right);
        }


        public static bool operator ==(Dog? left, Dog? right)
        {
            return (object)left == right || (left?.Equals(right) ?? false);
        }


        public override int GetHashCode()
        {
            return (EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(Age);
        }


        public override bool Equals(object? obj)
        {
            return Equals(obj as Dog);
        }


        public bool Equals(Dog? other)
        {
            return (object)this == other || ((object)other != null && EqualityContract == other!.EqualityContract && EqualityComparer<string>.Default.Equals(Name, other!.Name) && EqualityComparer<int>.Default.Equals(Age, other!.Age));
        }

        private Type EqualityContract
        {
            get
            {
                return typeof(Dog);
            }
        }

        private bool PrintMembers(StringBuilder builder)
        {
            RuntimeHelpers.EnsureSufficientExecutionStack();
            builder.Append("Name = ");
            builder.Append((object?)Name);
            builder.Append(", Age = ");
            builder.Append(Age.ToString());
            return true;
        }

    }
```

## History

- Presented @ AIS on 2023-04-06