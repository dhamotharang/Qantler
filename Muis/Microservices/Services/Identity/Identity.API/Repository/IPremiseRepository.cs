using Identity.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.API.Repository
{
  public interface IPremiseRepository
  {
    public Task<IList<Premise>> Select();
  }
}
