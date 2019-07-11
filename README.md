# Dependency Injection Lifetimes
Demonstrations, Tests, and Examples of Dependency Injection Lifetimes in .NET Core

## Parts

### Lifetime Tests

Simple tests demonstrating transient, scoped, and singleton lifecycles. If you are new to dependency injection lifetimes, start here.

### Recurring Console Application

A console application that runs a job every five seconds. This example demonstrates how to use scopes to properly handle Entity Framework's DbContext and other dependencies where lifetime consideration is important.

## Lifetimes

### Transient

A class is instantiated for every dependency. This includes multiple dependencies in a single class as well as a dependency used across classes.

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

The same class instance is used across the application. When Z has a singleton lifetime, the same instance of Z is used in and across all object graphs.

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

The most common example of a scoped lifetime is using an Entity Framework DbContext within a web application. Since a DbContext represents a single unit of work and a single DbContext should be used across an entire HTTP request, there is a built in scope for each HTTP request. When you register a DbContext using AddDbContext or AddDbContextPool, the lifetime of the DbContext is, by default, scoped. Therefore, for each HTTP request, a new DbContext is instantiated [1].

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

[1] When using AddDbContextPool, a new DbContext is not instantiated with each request. The same DbContext may be used across HTTP requests but it is reset to its default state between requests. This allows for better request-volume scalability of web applications as fewer resources are dedicated to instantiating and disposing of DbContexts.
