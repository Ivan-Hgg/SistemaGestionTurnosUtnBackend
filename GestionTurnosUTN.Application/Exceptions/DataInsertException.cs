using System;
using System.Collections.Generic;

namespace GestionTurnosUTN.Application.Exceptions;

public class DataInsertException(string message) : Exception(message)
{

}
