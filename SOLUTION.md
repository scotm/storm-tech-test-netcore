# Solution Notes

## Answer 1

Straightforward enough. Installed the .NET tools for MacOS, cloned the repository, restored the project, ran the provided tools to create the database.

Ended up using the command line, mostly. `dotnet watch --project Todo` gives me hot reloading and a fast development experience.

## Answer 2

I ended up converting the Items Collection into an Array, then used the `Array.Sort` method.

```csharp
    // I'm sure there's a better, more idiomatic method of doing this. I'm not a C# dev. Yet.
    var items = Model.Items.ToArray();

    // Use Array sort method to sort items by Importance
    Array.Sort(items, (x, y) => x.Importance.CompareTo(y.Importance));
```

I'm sure there's a better way of doing this using the Entity framework. It might work within the SingleTodoList method in `ApplicationDbContextConvenience.cs` with the OrderBy chained method.

Example:

```csharp
OrderBy(x => x.Col1).ThenBy(x => x.Col2)
```

But the problem specified to make the changes to `Views/TodoList/Detail.cshtml`. So I've left that as is for now.

## Answer 3

Ran the unit tests, noted the failing method, then followed the setup method up to the faulty `TodoItemEditFields` constructor. I then added the correct `Importance` field as passed into the constructor and the unit test passed.
