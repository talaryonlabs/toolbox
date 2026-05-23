using System.Text;
using Talaryon.Toolbox;
using Talaryon.Toolbox.Extensions;

namespace Talaryon.StackManager;

public interface ILogBuilder<out T>
{
    T AsError();
    T AsWarning();
    T AsSuccess();
    T AsColored(ConsoleColor color);
    T ResetFormatting();
    T WithPrefix(string prefix);
    T WithSuffix(string suffix);
    T WithTimestamp();
    T Indented(int level);
    T InBox();
    T InBox(int width);
    T NoNewLineAfter();
    T NewLineBefore();
}

public interface ILogBuilderTable : ITalaryonRunner, ILogBuilder<ILogBuilderTable>
{
    ILogBuilderTable WithHeaders(params string[] headers);
    ILogBuilderTable AddRow(params string[] cells);
    ILogBuilderTable WithColumnPadding(int padding);
}

public interface ILogBuilderProgress : ITalaryonRunner, ILogBuilder<ILogBuilderProgress>
{
    ILogBuilderProgress WithSpinner(params char[] frames);
    ILogBuilderProgress WithSpeed(int millisecondsPerFrame);
}

public interface ILogBuilderMessage : ITalaryonRunner, ILogBuilder<ILogBuilderMessage>
{
    ILogBuilderMessage WaitFor(Func<ILogBuilderMessage> predicate);
    ILogBuilderMessage WaitFor(Func<Task<ILogBuilderMessage>> predicate);
    ILogBuilderTable AsTable();
    ILogBuilderProgress AsProgress(string initialMessage = "");
}

public interface ILogBuilderQuestion : ITalaryonRunner<bool>, ILogBuilder<ILogBuilderQuestion>
{
    ILogBuilderQuestion AsYesNo();
    ILogBuilderQuestion WaitFor(Func<bool, ILogBuilderMessage> predicate);
    ILogBuilderQuestion WaitFor(Func<bool, Task<ILogBuilderMessage>> predicate);
}

public class LogBuilder(string content) : ILogBuilderMessage, ILogBuilderQuestion, ILogBuilderTable, ILogBuilderProgress
{
    public static ILogBuilderMessage Message(string message) => new LogBuilder(message);
    public static ILogBuilderQuestion Question(string question) => new LogBuilder(question);
    public static ILogBuilderTable Table() => new LogBuilder("");
    public static ILogBuilderProgress Progress(string message = "") => new LogBuilder(message);

    private bool _noNewLineAfter, _newLineBefore, _asError, _asWarning, _asSuccess, _asYesNo, _answer, 
        _useCustomColor, _useTimestamp, _inBox;
    private ConsoleColor _customColor;
    private int _indentLevel, _boxWidth = 80;
    private string? _prefix, _suffix;
    private Func<ILogBuilderMessage>? _messageFunction;
    private Func<Task<ILogBuilderMessage>>? _messageAsyncFunction;
    private Func<bool, ILogBuilderMessage>? _questionFunction;
    private Func<bool, Task<ILogBuilderMessage>>? _questionAsyncFunction;
    
    // Table state
    private readonly List<string> _tableHeaders = [];
    private readonly List<List<string>> _tableRows = [];
    private int _columnPadding = 2;
    private bool _isTable;
    
    // Progress state
    private char[] _spinnerFrames = ['|', '/', '-', '\\'];
    private int _spinnerSpeedMs = 100;
    private string _progressMessage = "";
    private CancellationTokenSource? _spinnerCtSource;

    private string _content = content;

    private string BuildBox(string content)
    {
        var lines = content.Split('\n');
        int maxLineLength = lines.Max(l => l.Length);
        int width = _boxWidth > 0 ? Math.Min(_boxWidth, maxLineLength + 4) : maxLineLength + 4;
        
        var border = new string('─', width);
        var sb = new StringBuilder();
        
        sb.AppendLine($"┌{border}┐");
        foreach (var line in lines)
        {
            sb.AppendLine($"│ {line.PadRight(width - 3)} │");
        }
        sb.Append($"└{border}┘");
        
        return sb.ToString();
    }

    private string BuildTable()
    {
        if (_tableHeaders.Count == 0 && _tableRows.Count == 0)
            return _content;
        
        int colCount = Math.Max(_tableHeaders.Count, _tableRows.DefaultIfEmpty([]).Max(r => r.Count));
        if (colCount == 0) return _content;
        
        var colWidths = new int[colCount];
        
        for (int i = 0; i < _tableHeaders.Count; i++)
            colWidths[i] = _tableHeaders[i].Length;
        
        foreach (var row in _tableRows)
        {
            for (int i = 0; i < row.Count && i < colCount; i++)
                colWidths[i] = Math.Max(colWidths[i], row[i].Length);
        }
        
        for (int i = 0; i < colWidths.Length; i++)
            colWidths[i] += _columnPadding;
        
        var sb = new StringBuilder();
        
        // Header separator
        var separatorParts = new string[colCount];
        for (int i = 0; i < colCount; i++)
        {
            separatorParts[i] = new string('─', colWidths[i]);
        }
        string headerSeparator = "─" + string.Join("─┼─", separatorParts) + "─";
        
        // Headers
        if (_tableHeaders.Count > 0)
        {
            sb.Append("│ ");
            for (int i = 0; i < _tableHeaders.Count; i++)
            {
                sb.Append(_tableHeaders[i].PadRight(colWidths[i] - 1));
                if (i < _tableHeaders.Count - 1) sb.Append(" │ ");
            }
            for (int i = _tableHeaders.Count; i < colCount; i++)
            {
                sb.Append(new string(' ', colWidths[i] - 1));
                if (i < colCount - 1) sb.Append(" │ ");
            }
            sb.AppendLine(" │");
            sb.AppendLine($"├{headerSeparator}┤");
        }
        
        // Rows
        foreach (var row in _tableRows)
        {
            sb.Append("│ ");
            for (int i = 0; i < colCount; i++)
            {
                string cell = i < row.Count ? row[i] : "";
                sb.Append(cell.PadRight(colWidths[i] - 1));
                if (i < colCount - 1) sb.Append(" │ ");
            }
            sb.AppendLine(" │");
        }
        
        // Footer
        if (_tableHeaders.Count > 0 && _tableRows.Count > 0)
        {
            sb.AppendLine($"└{headerSeparator}┘");
        }
        else if (_tableRows.Count > 0)
        {
            sb.AppendLine($"└{headerSeparator}┘");
        }
        
        return sb.ToString().TrimEnd();
    }

    async Task ITalaryonRunner.RunAsync(CancellationToken cancellationToken)
    {
        // Handle progress spinner
        if (_progressMessage.Length > 0)
        {
            await AnimateProgress(cancellationToken);
            return;
        }
        
        await Task.Run(() => (this as ITalaryonRunner).Run(), cancellationToken);
    }

    void ITalaryonRunner.Run()
    {
        string finalContent = _content;
        
        if (_isTable)
        {
            finalContent = BuildTable();
        }
        else if (_inBox)
        {
            finalContent = BuildBox(finalContent);
        }
        
        if (_useTimestamp)
        {
            finalContent = $"[{DateTime.Now:HH:mm:ss}] {finalContent}";
        }
        
        if (_indentLevel > 0)
        {
            var indent = new string(' ', _indentLevel * 2);
            if (_inBox || _isTable)
            {
                finalContent = indent + finalContent.Replace("\n", "\n" + indent);
            }
            else
            {
                finalContent = indent + finalContent;
            }
        }
        
        if (_prefix != null) finalContent = _prefix + finalContent;
        if (_suffix != null) finalContent = finalContent + _suffix;

        if (_useCustomColor)
        {
            Console.ForegroundColor = _customColor;
        }
        else
        {
            if (_asError) Console.ForegroundColor = ConsoleColor.Red;
            if (_asWarning) Console.ForegroundColor = ConsoleColor.Yellow;
            if (_asSuccess) Console.ForegroundColor = ConsoleColor.Green;
        }

        if (_newLineBefore) Console.WriteLine();
        if (_noNewLineAfter)
        {
            Console.Write(finalContent);
        }
        else
        {
            Console.WriteLine(finalContent);
        }
        Console.ResetColor();

        _messageFunction?.Invoke().Run();
        _messageAsyncFunction?.Invoke().RunSynchronouslyWithResult().Run();
    }

    private async Task AnimateProgress(CancellationToken cancellationToken)
    {
        _spinnerCtSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var spinCt = _spinnerCtSource.Token;
        
        int frameIndex = 0;
        
        try
        {
            // Display initial message
            if (_newLineBefore) Console.WriteLine();
            Console.Write($"{_spinnerFrames[0]} {_progressMessage}");
            
            while (!spinCt.IsCancellationRequested)
            {
                frameIndex = (frameIndex + 1) % _spinnerFrames.Length;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{_spinnerFrames[frameIndex]} {_progressMessage}");
                await Task.Delay(_spinnerSpeedMs, spinCt);
            }
        }
        finally
        {
            Console.WriteLine(); // Move to next line when done
            Console.ResetColor();
            _spinnerCtSource?.Dispose();
        }
    }

    bool ITalaryonRunner<bool>.Run()
    {
        string finalContent = _content;
        
        if (_inBox)
        {
            finalContent = BuildBox(finalContent);
        }
        
        if (_useTimestamp)
        {
            finalContent = $"[{DateTime.Now:HH:mm:ss}] {finalContent}";
        }
        
        if (_indentLevel > 0)
        {
            finalContent = new string(' ', _indentLevel * 2) + finalContent;
        }
        
        if (_prefix != null) finalContent = _prefix + finalContent;
        if (_suffix != null) finalContent = finalContent + _suffix;

        if (_useCustomColor)
        {
            Console.ForegroundColor = _customColor;
        }
        else
        {
            if (_asError) Console.ForegroundColor = ConsoleColor.Red;
            if (_asWarning) Console.ForegroundColor = ConsoleColor.Yellow;
            if (_asSuccess) Console.ForegroundColor = ConsoleColor.Green;
        }

        if (_asYesNo)
        {
            finalContent += " [y/N]: ";
        }
        
        if (_newLineBefore) Console.WriteLine();
        if (_noNewLineAfter)
        {
            Console.Write(finalContent);
        }
        else
        {
            Console.WriteLine(finalContent);
        }
        Console.ResetColor();

        if (_asYesNo)
        {
            _answer = Console.ReadLine()?.ToLower() == "y";
        }

        _questionFunction?.Invoke(_answer).Run();
        _questionAsyncFunction?.Invoke(_answer).RunSynchronouslyWithResult().Run();

        return _answer;
    }

    Task<bool> ITalaryonRunner<bool>.RunAsync(CancellationToken cancellationToken)
        => Task.Run(() => (this as ITalaryonRunner<bool>).Run(), cancellationToken);

    // ILogBuilder<ILogBuilderMessage> implementations
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.AsError() { _asError = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.AsWarning() { _asWarning = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.AsSuccess() { _asSuccess = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.AsColored(ConsoleColor color) { _customColor = color; _useCustomColor = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.ResetFormatting() { _asError = _asWarning = _asSuccess = _useCustomColor = false; _useTimestamp = _inBox = false; _indentLevel = 0; _prefix = _suffix = null; _customColor = ConsoleColor.Gray; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.WithPrefix(string prefix) { _prefix = prefix; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.WithSuffix(string suffix) { _suffix = suffix; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.WithTimestamp() { _useTimestamp = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.Indented(int level) { _indentLevel = Math.Max(0, level); return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.InBox() { _inBox = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.InBox(int width) { _inBox = true; _boxWidth = width; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.NoNewLineAfter() { _noNewLineAfter = true; return this; }
    ILogBuilderMessage ILogBuilder<ILogBuilderMessage>.NewLineBefore() { _newLineBefore = true; return this; }

    // ILogBuilderMessage implementations
    ILogBuilderMessage ILogBuilderMessage.WaitFor(Func<ILogBuilderMessage> predicate) { _messageFunction = predicate; return this; }
    ILogBuilderMessage ILogBuilderMessage.WaitFor(Func<Task<ILogBuilderMessage>> predicate) { _messageAsyncFunction = predicate; return this; }
    ILogBuilderTable ILogBuilderMessage.AsTable() { _isTable = true; return this; }
    ILogBuilderProgress ILogBuilderMessage.AsProgress(string initialMessage) { _progressMessage = initialMessage; return this; }

    // ILogBuilder<ILogBuilderQuestion> implementations
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.AsError() => (ILogBuilderQuestion)(this as ILogBuilderMessage).AsError();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.AsWarning() => (ILogBuilderQuestion)(this as ILogBuilderMessage).AsWarning();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.AsSuccess() => (ILogBuilderQuestion)(this as ILogBuilderMessage).AsSuccess();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.AsColored(ConsoleColor color) => (ILogBuilderQuestion)(this as ILogBuilderMessage).AsColored(color);
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.ResetFormatting() => (ILogBuilderQuestion)(this as ILogBuilderMessage).ResetFormatting();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.WithPrefix(string prefix) => (ILogBuilderQuestion)(this as ILogBuilderMessage).WithPrefix(prefix);
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.WithSuffix(string suffix) => (ILogBuilderQuestion)(this as ILogBuilderMessage).WithSuffix(suffix);
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.WithTimestamp() => (ILogBuilderQuestion)(this as ILogBuilderMessage).WithTimestamp();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.Indented(int level) => (ILogBuilderQuestion)(this as ILogBuilderMessage).Indented(level);
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.InBox() => (ILogBuilderQuestion)(this as ILogBuilderMessage).InBox();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.InBox(int width) => (ILogBuilderQuestion)(this as ILogBuilderMessage).InBox(width);
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.NoNewLineAfter() => (ILogBuilderQuestion)(this as ILogBuilderMessage).NoNewLineAfter();
    ILogBuilderQuestion ILogBuilder<ILogBuilderQuestion>.NewLineBefore() => (ILogBuilderQuestion)(this as ILogBuilderMessage).NewLineBefore();

    // ILogBuilderQuestion implementations
    ILogBuilderQuestion ILogBuilderQuestion.AsYesNo() { _asYesNo = true; return this; }
    ILogBuilderQuestion ILogBuilderQuestion.WaitFor(Func<bool, ILogBuilderMessage> predicate) { _questionFunction = predicate; return this; }
    ILogBuilderQuestion ILogBuilderQuestion.WaitFor(Func<bool, Task<ILogBuilderMessage>> predicate) { _questionAsyncFunction = predicate; return this; }

    // ILogBuilder<ILogBuilderTable> implementations
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.AsError() => (ILogBuilderTable)(this as ILogBuilderMessage).AsError();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.AsWarning() => (ILogBuilderTable)(this as ILogBuilderMessage).AsWarning();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.AsSuccess() => (ILogBuilderTable)(this as ILogBuilderMessage).AsSuccess();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.AsColored(ConsoleColor color) => (ILogBuilderTable)(this as ILogBuilderMessage).AsColored(color);
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.ResetFormatting() => (ILogBuilderTable)(this as ILogBuilderMessage).ResetFormatting();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.WithPrefix(string prefix) => (ILogBuilderTable)(this as ILogBuilderMessage).WithPrefix(prefix);
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.WithSuffix(string suffix) => (ILogBuilderTable)(this as ILogBuilderMessage).WithSuffix(suffix);
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.WithTimestamp() => (ILogBuilderTable)(this as ILogBuilderMessage).WithTimestamp();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.Indented(int level) => (ILogBuilderTable)(this as ILogBuilderMessage).Indented(level);
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.InBox() => (ILogBuilderTable)(this as ILogBuilderMessage).InBox();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.InBox(int width) => (ILogBuilderTable)(this as ILogBuilderMessage).InBox(width);
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.NoNewLineAfter() => (ILogBuilderTable)(this as ILogBuilderMessage).NoNewLineAfter();
    ILogBuilderTable ILogBuilder<ILogBuilderTable>.NewLineBefore() => (ILogBuilderTable)(this as ILogBuilderMessage).NewLineBefore();

    // ILogBuilderTable implementations
    ILogBuilderTable ILogBuilderTable.WithHeaders(params string[] headers) { _tableHeaders.Clear(); _tableHeaders.AddRange(headers); return this; }
    ILogBuilderTable ILogBuilderTable.AddRow(params string[] cells) { _tableRows.Add([.. cells]); return this; }
    ILogBuilderTable ILogBuilderTable.WithColumnPadding(int padding) { _columnPadding = padding; return this; }

    // ILogBuilder<ILogBuilderProgress> implementations
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.AsError() => (ILogBuilderProgress)(this as ILogBuilderMessage).AsError();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.AsWarning() => (ILogBuilderProgress)(this as ILogBuilderMessage).AsWarning();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.AsSuccess() => (ILogBuilderProgress)(this as ILogBuilderMessage).AsSuccess();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.AsColored(ConsoleColor color) => (ILogBuilderProgress)(this as ILogBuilderMessage).AsColored(color);
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.ResetFormatting() => (ILogBuilderProgress)(this as ILogBuilderMessage).ResetFormatting();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.WithPrefix(string prefix) => (ILogBuilderProgress)(this as ILogBuilderMessage).WithPrefix(prefix);
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.WithSuffix(string suffix) => (ILogBuilderProgress)(this as ILogBuilderMessage).WithSuffix(suffix);
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.WithTimestamp() => (ILogBuilderProgress)(this as ILogBuilderMessage).WithTimestamp();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.Indented(int level) => (ILogBuilderProgress)(this as ILogBuilderMessage).Indented(level);
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.InBox() => (ILogBuilderProgress)(this as ILogBuilderMessage).InBox();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.InBox(int width) => (ILogBuilderProgress)(this as ILogBuilderMessage).InBox(width);
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.NoNewLineAfter() => (ILogBuilderProgress)(this as ILogBuilderMessage).NoNewLineAfter();
    ILogBuilderProgress ILogBuilder<ILogBuilderProgress>.NewLineBefore() => (ILogBuilderProgress)(this as ILogBuilderMessage).NewLineBefore();

    // ILogBuilderProgress implementations
    ILogBuilderProgress ILogBuilderProgress.WithSpinner(params char[] frames) { _spinnerFrames = frames; return this; }
    ILogBuilderProgress ILogBuilderProgress.WithSpeed(int millisecondsPerFrame) { _spinnerSpeedMs = millisecondsPerFrame; return this; }
}
