# Dependency Injection Lifetimes

Lifetimes are a basic building block of dependency injection. They tell the IoC Container when to instantiate a new object vs when to use one that already exists.

You most likely deal with service lifetimes even if you are not familiar with dependency injection. This is usually done using static singletons, factories, and similar techniques. With dependency injection the responsibility of controlling service lifetimes moves from code patterns and service internals to the application root where services are registered at application startup.

Microsoft.Extensions.DependencyInjection provides three lifetimes: transient, singleton, and scoped.

### Transient

A new service is instantiated every time one is requested. This is the standard lifetime and should be used unless another lifetime is required.

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

Scoped is the most complex lifetime. The lifetime of a scoped service is a singleton within a scope but transient across scopes. Scoped lifetimes permit lower layers of the application to control the lifetime of scoped services by creating scopes.

Some third-party containers allow for named scopes which give the application more flexibility to control the lifetime of services.

The most common example of a scoped lifetime is using an Entity Framework DbContext within a web application. A DbContext should not be reused across HTTP requests but should be reused for all services within any HTTP request. In .NET web applications, a new scope is created for each HTTP request. When registering a DbContext using AddDbContext, the lifetime of the DbContext is, by default, scoped. Therefore a new DbContext is instantiated for each HTTP request and used throughout the request.

```
Z

Z

A
| \
B   Z
|
Z

BEGIN SCOPE

Z'

Z'

A'
| \
B'  Z'
|
Z'

A'2
| \
B'2 Z'
|
Z'

END SCOPE

Z

A'3
| \
B'3 Z
|
Z
```
