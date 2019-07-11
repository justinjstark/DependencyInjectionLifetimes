# Dependency Injection Lifetimes
Demonstrations, Tests, and Examples of Dependency Injection Lifetimes in .NET Core

## Parts

### [Lifetimes](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/Lifetimes)

Want to learn about transient, scoped, and singleton lifetimes? Start here.

### [Lifetime Tests](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/LifetimeTests)

Simple tests demonstrating transient, scoped, and singleton lifecycles. If you are new to dependency injection lifetimes, these tests provide simple use cases to build an understanding.

### [Best Practices](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/BestPractices)

A list of best practices including
- When to use AddDbContext vs AddDbContextPool
- How to properly use HttpClient
- How to use scopes within services

### [Recurring Console Application](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/RecurringConsoleApplication)

A console application that runs a job every five seconds. This example demonstrates how to use scopes to properly handle Entity Framework's DbContext and other dependencies where lifetime consideration is important.
