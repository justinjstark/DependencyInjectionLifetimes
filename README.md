# Dependency Injection Lifetimes
Demonstrations, Tests, and Examples of Dependency Injection Lifetimes in .NET Core

## Parts

### [Lifetime Tests](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/LifetimeTests)

Simple tests demonstrating transient, scoped, and singleton lifecycles. If you are new to dependency injection lifetimes, start here.

### [Recurring Console Application](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/RecurringConsoleApplication)

A console application that runs a job every five seconds. This example demonstrates how to use scopes to properly handle Entity Framework's DbContext and other dependencies where lifetime consideration is important.

## Lifetimes

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
Z  Z

B
| \
C  Z
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
Z  Z

B
| \
C  Z
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
C  Z'
|
Z'

Scope 2

Z''

Z''

A
| \
Z''  Z''

B
| \
C  Z''
|
Z''
```

## AddDbContext vs AddDbContextPool

In a web application, when registering a DbContext via AddDbContext, a new DbContext is instantiated for each HTTP request.

Using DBContext pooling via AddDbContextPool, a new DbContext is not instantiated with each request. Instead, a pool of DbContexts is kept for reuse. The same DbContext may be used across HTTP requests but it is reset to its default state between requests.

In low volume situations, AddDbContext is the better choice as it allows DbContexts to be disposed of, freeing up resources. In high volume situations, AddDbContextPool allows DbContexts to be reused. This allows for better performance as fewer resources are dedicated to instantiating and disposing of DbContexts.

For applications with many concurring scopes (ex: web applications or applications with many concurrent, independent jobs), use AddDbContextPool. Otherwise use AddDbContext.
