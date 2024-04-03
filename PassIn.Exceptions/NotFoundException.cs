namespace PassIn.Exceptions;

public class NotFoundException(string message) : PassInException(message) {}