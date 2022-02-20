using System;

namespace xFrame.Core.Validation
{
    public class ValidationMessage
    {
        public string Message { get;}
        public Severity Severity { get; }

        public ValidationMessage(string message, Severity severity)
        {
            Message = message;
            Severity = severity;
        }

        public static ValidationMessage FromCondition(Severity severity,Func<bool> failingCondition, string message)
        {
            ValidationMessage component = null;

            if (failingCondition())
            {
                component = new ValidationMessage(message, severity);
            }

            return component;
        }

        public override string ToString()
        {
            return Message;
        }

        public static implicit operator string(ValidationMessage m) => m.Message;
    }
}
