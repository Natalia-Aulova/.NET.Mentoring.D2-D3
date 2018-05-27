Tracing calls to methods

Using as a basis one of the projects (e.g. "Windows services" or "Messaging" module)
add logging of method calls (i.e. any code except .NET Framework code or third-party libraries).

Additional conditions:
	1) The following should be logged: time, method name, parameter values in serialized form. 
	   The serializer should be any human-readable (xml, json, etc). 
	   If a type cannot be serialized for some reason, then insert "Not serializable" instead of the parameter value.
	   A return value should be logged after a method finishes.
	2) Implement logging aspect in 2 versions: 
	   - Based on Dynamic Proxy (e.g. Castle.Core);
	   - Using code rewriting (e.g. PostSharp).
	3) If possible, remove from classes explicit calls to the code of the AOP frameworks.
	   IoC can be used as one of the solutions.