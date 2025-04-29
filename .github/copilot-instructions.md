# GitHub Copilot Instructions 

## Intensions

Use #C for client-side and server-side code.
Use the latest language features and latest version of .Net.
Use Visual Studio as my primary IDE.
Use VSCode as my backup IDE

Use **SQLite** for testing and local development.
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=localdb.db"));
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=localdb.db");
        }
    }
    ```

- **SQL Server** for production deployments.
    ```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("ProductionSqlServer")));
    }

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("ProductionSqlServer"));
        }
    }
    ```
    ```json
    {
      "ConnectionStrings": {
        "ProductionSqlServer": "Server=yourproductionserver;Database=yourdb;User Id=yourusername;Password=yourpassword;"
      }
    }
    ```

## Integration Testing

- Ensure that different modules or components of the application work together as expected.

### Setup

- Use a standard project structure with a separate folder for unit and integration tests.
    ```
    - Projct.sln
        - devops
        - documentation
        - databases
        - resoures
        - src
          - components
            - Access
              - Volatility
                - Contract
                - Orm (if needed)
                - Service
            - Engine
              - Volatility
                - Contract
                - Service
            - Manager
              - Volatility
                - Contract
                - Service
          - infra
            - Ifx
            - Utility
        - tests/
            - itegration
            - unit
    ```

### Tools and Frameworks

Use MSTest or xUnit for unit tests
Implement unit tests for every class created.
Use Moq to mock DI services.
Use an in-memory database like SQLite for testing to avoid side effects on the production database.
    ```csharp
    using Xunit;
    using MyProject.Services;
    using Microsoft.EntityFrameworkCore;
    using MyProject.Data;

    public class IntegrationTests
    {
        private readonly MyDbContext context;
        private readonly MyService service;

        public IntegrationTests()
        {
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseSqlite("Data Source=:memory:")
                .Options;
            
            context = new MyDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            service = new MyService(context);
        }

        [Fact]
        public void TestServiceMethod()
        {
            // Arrange
            var entity = new MyEntity { Id = 1, Name = "Test" };
            context.MyEntities.Add(entity);
            context.SaveChanges();

            // Act
            var result = service.GetEntity(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }
    }
    ```

### Best Practices
- Define clear interfaces for module interactions.
- Use incremental integration to isolate and fix defects more easily.
- Use realistic data for testing.
- Automate integration tests to run frequently.
- Integrate tests into the CI/CD pipeline.

## Client Development

- Use Maui for cross-platform applications

- Use WPF for building Windows desktop applications.
    ```csharp
    <Window x:Class="MyApp.MainWindow"
            xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
            xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
            Title="MainWindow" Height="350" Width="525">
        <Grid>
            <Button Content="Click Me" HorizontalAlignment="Left" VerticalAlignment="Top" Width="75"/>
        </Grid>
    </Window>
    ```

- Use WinUI for building modern Windows desktop applications for use in the Microsoft Store
    ```csharp
    <Page
        x:Class="MyApp.MainPage"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:local="using:MyApp"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        mc:Ignorable="d">
    
        <Grid>
            <Button Content="Click Me" HorizontalAlignment="Center" VerticalAlignment="Center"/>
        </Grid>
    </Page>
    ```
- Use Console for terminal based applications.
- Use Dependency Injection with Host Builder to configure services and run the application.
    ```csharp
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                })
                .Build();
            host.Run();
        }
    }
    ```
- Use Volatility-Based Decomposition (VBD) to structure the application for scalability and maintainability.
- Use the `Microsoft.Extensions.Configuration` package for configuration management.
- Use the `Microsoft.Extensions.DependencyInjection` package for dependency injection.`
- Use the `Microsoft.Extensions.Hosting` package for building console applications.
- Use the `Microsoft.Extensions.Logging` package for logging.

## Architecture Patterns

- Prefer Volatility-Based Decomposition (VBD) for building scalable and maintainable applications.
  
### Volatility-Based Decomposition

- **Manager Classes**: Use for workflow activities.
- **Engine Classes**: Use for applying business logic, aggregations, and transformations.
- **Access Classes**: Use to abstract data persistence.

#### Communication Restrictions

- Clients can communicate with Managers.
- Clients can not communicate with Engines or Accessors.
- Managers can communicate with Engines and Accessors.
- Managers can not communicate with Clients or other Managers.
- When a manager needs to communicate with another manager, it should do so through a message bus to avoid any coupling between the managers.
- Managers can call Engines and Accessors.
- Engines can call Accessors.
- Engines can not call Clients, Managers or other Engines.
- Accessors are the only way to interact with Data persistence.
- Accessors can not call Clients, Managers or Engines.
- Accessors can interact with the file system, databases, or external services such as remote APIs.

#### Cross-Cutting Concerns
- By definition, cross-cutting concerns are aspects of a program that affect multiple modules. These concerns are often difficult to separate from the rest of the system. Examples include logging, security, and error handling.  These libraries and utilities may be used by any component in the ecosystem.
 fs
##### Logging and Monitoring
- Implement centralized logging using a framework like Serilog or NLog.
- Capture, store, and analyze application logs and performance metrics to diagnose issues and monitor the health of the application.

##### Security
- Ensure secure access to the application, data protection, and compliance with security standards.
- Implement authentication and authorization using ASP.NET Core Identity or OAuth.

##### Error Handling
- Consistently manage and respond to errors and exceptions throughout the application.
- Use middleware to handle exceptions globally and return appropriate HTTP status codes.

##### Configuration Management
- Manage application settings and configurations in a centralized and consistent manner.
- Use appsettings.json in ASP.NET Core and environment variables for different deployment environments.

##### Caching
- Store frequently accessed data in memory to improve performance and reduce load on the database.
- Implement distributed caching with Redis or in-memory caching with IMemoryCache in ASP.NET Core.

##### Validation
- Ensure data integrity and consistency by validating inputs and outputs.
- Use DataAnnotations and FluentValidation to validate incoming requests.

##### Localization and Internationalization
- Make the application adaptable to different languages and cultural norms.
- Use resource files and localization middleware in ASP.NET Core.

##### Concurrency Management
- Handle multiple simultaneous requests or operations to ensure data consistency and avoid conflicts.
- Implement optimistic concurrency control with Entity Framework Core.

##### Auditing and Compliance
- Track changes and access to data for regulatory compliance and auditing purposes.
- Implement audit logs to track user activities and changes to critical data.

##### Transaction Management
- Ensure that a series of operations are completed successfully as a unit, maintaining data integrity.
- Use transactions in Entity Framework Core to ensure atomicity of operations.

## Patterns

When creating a component always create and implement an interface that matches the component with an "I" prefix.

Example: "Create Validating Engine" prompt.  
```csharp
  public interface IValidatingEngine
  {
    // Add activities to be supported
  }
  
  public class ValidatingEngine : IValidatingEngine
  {
    // Implement supported activities
  }

```

## Anti-Patterns

- Avoid mixing logical domains.
- Avoid Customer Specific domains.
- Avoid mixing user experience and domain logic.
- Avoid extra duplicative domains.
- Avoid orphan domains.
- Avoid non-customer centric APIs


## Unit of Work Pattern
- Implement the Unit of Work pattern to manage transactions and ensure data consistency.
    ```csharp
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;
        private readonly Dictionary<string, object> repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            repositories = new Dictionary<string, object>();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (IGenericRepository<TEntity>)repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
    ```


## Saga Pattern
- Use the Saga pattern to manage long-running transactions and ensure data consistency across multiple services.
- Implement compensating transactions to undo changes if part of the transaction fails.
    ```csharp
    public interface ISagaStep
    {
        Task ExecuteAsync();
        Task CompensateAsync();
    }

    public class OrderSaga : ISagaStep
    {
        private readonly IOrderService orderService;
        private readonly IPaymentService paymentService;

        public OrderSaga(IOrderService orderService, IPaymentService paymentService)
        {
            this.orderService = orderService;
            this.paymentService = paymentService;
        }

        public async Task ExecuteAsync()
        {
            await orderService.CreateOrderAsync();
            await paymentService.ProcessPaymentAsync();
        }

        public async Task CompensateAsync()
        {
            await paymentService.RefundPaymentAsync();
            await orderService.CancelOrderAsync();
        }
    }
    ```
