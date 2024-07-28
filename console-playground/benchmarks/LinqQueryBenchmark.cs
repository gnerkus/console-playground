using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Benchmarks;

public class LinqQueryBenchmark
{
    public class Person
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }
        
    private readonly List<Person> _peopleList;

    public LinqQueryBenchmark()
    {
        _peopleList = new List<Person>();
        for (var index = 0; index < 10000; index++)
        {
            _peopleList.Add(new Person
            {
                FirstName = index % 2 == 0 ? "A" : "C",
                LastName = index % 3 == 0 ? "B" : "D"
            });
        }
    }

    [Benchmark]
    public List<Person> FilterPeopleWithoutLet()
    {
        return (from person in _peopleList
            where person.LastName.Contains('B')
                  && person.FirstName.Equals("A")
            select person).ToList();
    }

    [Benchmark]
    public List<Person> FilterPeopleUsingLet()
    {
        return (from person in _peopleList
            let isLastNameHaris = person.LastName.Contains('B')
            let isFirstNameAdmir = person.FirstName.Equals("A")
            where isLastNameHaris && isFirstNameAdmir
            select person).ToList();
    }
}