using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FkThat.MediatorLite.Demo
{
    public enum OrderState
    {
        New,
        Completed,
        Canceled
    }

    // Define order message handlers
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

        public async Task HandleMessageAsync(CreateOrder message, CancellationToken cancellationToken)
        {
            _store.AddOrder(new Order(message.Id, OrderState.New));
            await _mediator.SendMessageAsync(new Notify("Order created."), cancellationToken);
        }

        public async Task HandleMessageAsync(CompleteOrder message, CancellationToken cancellationToken)
        {
            _store.UpdateOrder(new Order(message.Id, OrderState.Completed));
            await _mediator.SendMessageAsync(new Notify("Order completed."), cancellationToken);
        }

        public async Task HandleMessageAsync(CancelOrder message, CancellationToken cancellationToken)
        {
            _store.UpdateOrder(new Order(message.Id, OrderState.Canceled));
            await _mediator.SendMessageAsync(new Notify("Order canceled."), cancellationToken);
        }
    }

    // Define notification message handler
    public class NotificationHandler : IMessageHandler<Notify>
    {
        public Task HandleMessageAsync(Notify message, CancellationToken cancellationToken)
        {
            Console.WriteLine(message.Message);
            return Task.CompletedTask;
        }
    }

    public class OrderStore
    {
        private readonly HashSet<Order> _orders = new();

        public IReadOnlyCollection<Order> Orders => _orders;

        public void AddOrder(Order order)
        {
            var old = _orders.FirstOrDefault(o => o.Id == order.Id);

            if (old == null)
            {
                _orders.Add(order);
            }
        }

        public void UpdateOrder(Order order)
        {
            var old = _orders.FirstOrDefault(o => o.Id == order.Id);

            if (old != null)
            {
                _orders.Remove(old);
                _orders.Add(order);
            }
        }
    }

    internal static class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();

            services.AddSingleton<OrderStore>();

            // Register handlers
            services.AddTransient<OrderHandler>();
            services.AddTransient<NotificationHandler>();

            // Register mediator
            services.AddMediator();

            using var serviceProvider = services.BuildServiceProvider();

            // Use mediator
            var mediator = serviceProvider.GetRequiredService<IMediator>();
            await mediator.SendMessageAsync(new CreateOrder(1));
            await mediator.SendMessageAsync(new CreateOrder(2));
            await mediator.SendMessageAsync(new CreateOrder(3));
            await mediator.SendMessageAsync(new CompleteOrder(1));
            await mediator.SendMessageAsync(new CancelOrder(2));
            await mediator.SendMessageAsync(new CompleteOrder(3));
        }
    }

    // Define messages
    public record CreateOrder(int Id);
    public record CompleteOrder(int Id);
    public record CancelOrder(int Id);
    public record Notify(string Message);
    public record Order(int Id, OrderState State);
}
