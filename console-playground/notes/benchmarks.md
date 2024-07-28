[1]: steven-giesel.com/blogPost/98458f74-5205-4b2b-9f5b-535e34ec2fea

[2]: https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/introduction-to-linq-queries#immediate

- Remember to run the application as `Release | Any CPU`

# LINQ `let` [1]

A LINQ query without a call to methods like `ToList`, `ToArray` which force immediate execution of
the query [2], returns
an `IEnumerable<T>`which is only a forward collection. This means the LINQ query is not executed at
all.