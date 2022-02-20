using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace xFrame.Core.Validation
{
    public interface IValidatable
    {
        void OnValidated(ValidationResult result);
    }
}
