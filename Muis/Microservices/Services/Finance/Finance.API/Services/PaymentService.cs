using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.API;
using Core.API.Provider;
using Core.EventBus;
using Core.Model;
using Finance.API.DTO;
using Finance.API.Services.Commands.Payment;
using Finance.Model;

namespace Finance.API.Services
{
  public class PaymentService : TransactionalService,
                                IPaymentService
  {
    readonly IEventBus _eventBus;

    public PaymentService(IDbConnectionProvider connectionProvider, IEventBus eventBus)
         : base(connectionProvider)
    {
      _eventBus = eventBus;
    }

    public async Task<IList<Payment>> Filter(PaymentFilter options)
    {
      return await Execute(new FilterPaymentCommand(options));
    }

    public async Task<IList<Payment>> GetCustomerRecentPayment(Guid customerID)
    {
      return await Execute(new GetCustomerRecentPaymentCommand(customerID));
    }

    public async Task<Payment> GetPaymentByID(long id)
    {
      return await Execute(new GetPaymentByIDCommand(id));
    }

    public async Task<Payment> PaymentAction(long id, PaymentStatus status, Bank bank,
      Officer officer)
    {
      return await Execute(new PaymentActionCommand(id, status, bank, officer, _eventBus));
    }

    public Task<Note> AddNote(long id, Note note, Officer user)
    {
      return Execute(new AddPaymentNoteCommand(id, note, user));
    }

    public Task<Payment> PaymentForComposition(PaymentForComposition param)
    {
      return Execute(new PaymentForCompositionCommand(param, _eventBus));
    }
  }
}
