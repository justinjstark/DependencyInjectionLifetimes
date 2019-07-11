# Dependency Injection Lifetimes

Lifetimes are a basic building block of dependency injection. They tell the IoC Container when to instantiate a new object vs when to use one that already exists.

Even if you are not familiar with dependency injection, you are most likely still familiar with lifetimes. When not using dependency injection, we still have to worry about the lifetime of our services. This is usually done using static singletons, factories, and similar techniques. With dependency injection the responsibility of controlling service lifetime moves from the individual components to application startup when services are registered.

Microsoft.Extensions.DependencyInjection provides three lifetimes: transient, singleton, and scoped.

### Transient

A class is instantiated for every dependency. This is the standard lifetime and should be used unless another lifetime is required.

In the following object graphs, if Z has a transient lifetime, a new Z will be instantiated for each Z node.

```
Z

Z'

A
| \
Z'2 Z'3

B
| \
C   Z'5
|
Z'4
```

### Singleton

The same class instance is used across the application. Common uses for singletons are logging and caching.

When Z has a singleton lifetime, the same instance of Z is used in and across all object graphs.

```
Z

Z

A
| \
Z   Z

B
| \
C   Z
|
Z
```

### Scoped

Scoped is the most complex lifetime. The lifetime of each component is a singleton within a scope but transient across scopes.

The most common example of a scoped lifetime is using an Entity Framework DbContext within a web application. A DbContext represents a single unit of work and a single DbContext should be used across an entire HTTP request. In .NET web applications, there is a built in scope for each HTTP request. When you register a DbContext using AddDbContext, the lifetime of the DbContext is, by default, scoped. Therefore, for each HTTP request, a new DbContext is instantiated and used throughout the request.

```
Z

Z

A
| \
Z   Z

B
| \
C   Z
|
Z

Scope 1

Z'

Z'

A
| \
Z'  Z'

B
| \
C   Z'
|
Z'

Scope 2

Z''

Z''

A
| \
Z'' Z''

B
| \
C   Z''
|
Z''
```

## AddDbContext vs AddDbContextPool

In a web application, when registering a DbContext via AddDbContext, a new DbContext is instantiated for each HTTP request.

Using DBContext pooling via AddDbContextPool, a new DbContext is not instantiated with each request. Instead, a pool of DbContexts is kept for reuse. The same DbContext may be used across HTTP requests but it is reset to its default state between requests.

In low volume situations, AddDbContext is the better choice as it allows DbContexts to be disposed of, freeing up resources. In high volume situations, AddDbContextPool allows DbContexts to be reused. This allows for better performance as fewer resources are dedicated to instantiating and disposing of DbContexts.

For applications with many concurring scopes (ex: web applications or applications with many concurrent, independent jobs), use AddDbContextPool. Otherwise use AddDbContext.
