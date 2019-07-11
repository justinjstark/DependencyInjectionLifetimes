# Dependency Injection Lifetimes
Demonstrations, Tests, and Examples of Dependency Injection Lifetimes in .NET Core

## Parts

### [Lifetimes](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/Lifetimes)

Want to learn about transient, scoped, and singleton lifetimes? Wondering about the difference between AddDbContext and AddDbContextPool? Start here.

### [Lifetime Tests](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/LifetimeTests)

Simple tests demonstrating transient, scoped, and singleton lifecycles. If you are new to dependency injection lifetimes, these test provide simple use cases to build an understanding.

### [Recurring Console Application](https://github.com/justinjstark/DependencyInjectionLifetimes/tree/master/src/RecurringConsoleApplication)

A console application that runs a job every five seconds. This example demonstrates how to use scopes to properly handle Entity Framework's DbContext and other dependencies where lifetime consideration is important.
