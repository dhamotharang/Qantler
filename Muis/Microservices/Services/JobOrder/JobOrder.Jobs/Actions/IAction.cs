using System;
using System.Threading.Tasks;

namespace JobOrder.Jobs.Actions
{
  public interface IAction
  {
    Task Invoke();
  }
}
