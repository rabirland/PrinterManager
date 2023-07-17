using Microsoft.JSInterop;

namespace UI;

public class JsService : IJsService, IService
{
    /// <summary>
    /// Whether the supplementary features are initialized yet or not.
    /// </summary>
    private static bool initialized = false;

    private readonly IJSRuntime js;

    public JsService(IJSRuntime js)
    {
        this.js = js;
    }

    private async Task TryInitialize()
    {
        if (initialized)
        {
            return;
        }

        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP0 = (code) => (new Function(code)())");
        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP1 = (code, p1) => (new Function('p1', code)(p1))");
        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP2 = (code, p1, p2) => (new Function('p1', 'p2', code)(p1, p2))");
        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP3 = (code, p1, p2, p3) => (new Function('p1', 'p2', 'p3', code)(p1, p2, p3))");
        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP4 = (code, p1, p2, p3, p4) => (new Function('p1', 'p2', 'p3', 'p4', code)(p1, p2, p3, p4))");
        await js.InvokeVoidAsync("eval", $"window.CustomFunctionRunnerP5 = (code, p1, p2, p3, p4, p5) => (new Function('p1', 'p2', 'p3', 'p4', 'p5', code)(p1, p2, p3, p4, p5))");

        initialized = true;
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP0", code);
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code, object param1)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP1", code, param1);
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code, object param1, object param2)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP2", code, param1, param2);
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code, object param1, object param2, object param3)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP3", code, param1, param2, param3);
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code, object param1, object param2, object param3, object param4)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP4", code, param1, param2, param3, param4);
    }

    /// <inheritdoc />
    public async ValueTask Execute(string code, object param1, object param2, object param3, object param4, object param5)
    {
        await TryInitialize();
        await js.InvokeVoidAsync("CustomFunctionRunnerP5", code, param1, param2, param3, param4, param5);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP0", code);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code, object param1)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP1", code, param1);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code, object param1, object param2)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP2", code, param1, param2);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code, object param1, object param2, object param3)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP3", code, param1, param2, param3);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code, object param1, object param2, object param3, object param4)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP4", code, param1, param2, param3, param4);
    }

    /// <inheritdoc />
    public async ValueTask<T> Execute<T>(string code, object param1, object param2, object param3, object param4, object param5)
    {
        await TryInitialize();
        return await js.InvokeAsync<T>("CustomFunctionRunnerP5", code, param1, param2, param3, param4, param5);
    }

    /// <inheritdoc />
    public ValueTask Eval(string statement)
    {
        return js.InvokeVoidAsync("eval", statement);
    }
}
