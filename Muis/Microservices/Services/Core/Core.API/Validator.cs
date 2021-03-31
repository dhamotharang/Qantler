using System;
using System.Collections.Generic;
using System.Text;

namespace Core.API
{
  public abstract class Validator
  {
    Validator _nextValidator;
    public Validator NextValidator { set => _nextValidator = value; }

    /// <summary>
    /// Dos the validate.
    /// </summary>
    protected abstract void DoValidate();

    /// <summary>
    /// Validate this instance.
    /// </summary>
    public void Validate()
    {
      DoValidate();
      _nextValidator?.DoValidate();
    }
  }
}
