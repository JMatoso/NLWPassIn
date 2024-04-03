namespace PassIn.Exceptions;

public class ErrorOnValidationException(string message) : PassInException(message) {}