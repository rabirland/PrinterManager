namespace UI;

/// <summary>
/// Manages the JS runtime.
/// </summary>
public interface IJsService
{
    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code);

    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to send for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code, object param1);

    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code, object param1, object param2);

    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code, object param1, object param2, object param3);

    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <param name="param4">A parameter to provide for the JS function. Must be serializable. Use the name "p4" in the js code to refer to it.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code, object param1, object param2, object param3, object param4);

    /// <summary>
    /// Execute as piece of code as the body of a function.
    /// </summary>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <param name="param4">A parameter to provide for the JS function. Must be serializable. Use the name "p4" in the js code to refer to it.</param>
    /// <param name="param5">A parameter to provide for the JS function. Must be serializable. Use the name "p5" in the js code to refer to it.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Execute(string code, object param1, object param2, object param3, object param4, object param5);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code, object param1);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code, object param1, object param2);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code, object param1, object param2, object param3);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <param name="param4">A parameter to provide for the JS function. Must be serializable. Use the name "p4" in the js code to refer to it.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code, object param1, object param2, object param3, object param4);

    /// <summary>
    /// Execute as piece of code as the body of a function with a return value.
    /// </summary>
    /// <typeparam name="T">The type to deserialized the returned JS value into.</typeparam>
    /// <param name="code">The code to execute.</param>
    /// <param name="param1">A parameter to provide for the JS function. Must be serializable. Use the name "p1" in the js code to refer to it.</param>
    /// <param name="param2">A parameter to provide for the JS function. Must be serializable. Use the name "p2" in the js code to refer to it.</param>
    /// <param name="param3">A parameter to provide for the JS function. Must be serializable. Use the name "p3" in the js code to refer to it.</param>
    /// <param name="param4">A parameter to provide for the JS function. Must be serializable. Use the name "p4" in the js code to refer to it.</param>
    /// <param name="param5">A parameter to provide for the JS function. Must be serializable. Use the name "p5" in the js code to refer to it.</param>
    /// <remarks>The code must contain a return statement to return a value.</remarks>
    /// <returns>The deserialized object.</returns>
    ValueTask<T> Execute<T>(string code, object param1, object param2, object param3, object param4, object param5);

    /// <summary>
    /// Executes a single JS statement.
    /// </summary>
    /// <param name="statement">The single statement to run.</param>
    /// <returns>An awaitable task.</returns>
    ValueTask Eval(string statement);
}
