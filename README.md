# MediatorLite

[![Build Status](https://dev.azure.com/FkThat/CI/_apis/build/status/MediatorLite?branchName=develop)](https://dev.azure.com/FkThat/CI/_build/latest?definitionId=41&branchName=develop)
[![Azure DevOps tests (develop)](https://img.shields.io/azure-devops/tests/FkThat/CI/41/develop)](https://dev.azure.com/FkThat/CI/_build/latest?definitionId=41&branchName=develop)
[![Azure DevOps coverage (develop)](https://img.shields.io/azure-devops/coverage/FkThat/CI/41/develop)](https://dev.azure.com/FkThat/CI/_build/latest?definitionId=41&branchName=develop)

## Packages

[![MyGet (with prereleases)](https://img.shields.io/myget/fkthat/vpre/FkThat.MediatorLite?label=FkThat.MediatorLite)](https://www.myget.org/feed/fkthat/package/nuget/FkThat.MediatorLite)
[![MyGet (with prereleases)](https://img.shields.io/myget/fkthat/vpre/FkThat.MediatorLite.Abstractions?label=FkThat.MediatorLite.Abstractions)](https://www.myget.org/feed/fkthat/package/nuget/FkThat.MediatorLite.Abstractions)
[![MyGet (with prereleases)](https://img.shields.io/myget/fkthat/vpre/FkThat.MediatorLite.DependencyInjection?label=FkThat.MediatorLite.DependencyInjection)](https://www.myget.org/feed/fkthat/package/nuget/FkThat.MediatorLite.DependencyInjection)

## Sample usage

### Define messages

```csharp
public record CreateOrder(int Id);
public record CompleteOrder(int Id);
public record CancelOrder(int Id);
public record Notify(string Message);
```

### Define message handlers

```csharp
public class OrderHandler :
    IMessageHandler<CreateOrder>,
    IMessageHandler<CompleteOrder>,
    IMessageHandler<CancelOrder>
{
    private readonly OrderStore _store;
    private readonly IMediator _mediator;

    public OrderHandler(OrderStore store, IMediator mediator)
    {
        _store = store;
        _mediator = mediator;
    }

    public async Task HandleMessageAsync(CreateOrder message)
    {
        _store.AddOrder(new Order(message.Id, OrderState.New));
        await _mediator.SendMessageAsync(new Notify("Order created."));
    }

    public async Task HandleMessageAsync(CompleteOrder message)
    {
        _store.UpdateOrder(new Order(message.Id, OrderState.Completed));
        await _mediator.SendMessageAsync(new Notify("Order completed."));
    }

    public async Task HandleMessageAsync(CancelOrder message)
    {
        _store.UpdateOrder(new Order(message.Id, OrderState.Canceled));
        await _mediator.SendMessageAsync(new Notify("Order canceled."));
    }
}

public class NotificationHandler : IMessageHandler<Notify>
{
    public Task HandleMessageAsync(Notify message)
    {
        Console.WriteLine(message.Message);
        return Task.CompletedTask;
    }
}
```

### Register handlers

```csharp
services.AddTransient<OrderHandler>();
services.AddTransient<NotificationHandler>();
```

### Register mediator

```csharp
services.AddMediator();
```

### Use mediator

```csharp
await mediator.SendMessageAsync(new CreateOrder(1));
await mediator.SendMessageAsync(new CreateOrder(2));
await mediator.SendMessageAsync(new CreateOrder(3));
await mediator.SendMessageAsync(new CompleteOrder(1));
await mediator.SendMessageAsync(new CompleteOrder(2));
await mediator.SendMessageAsync(new CancelOrder(3));
```
