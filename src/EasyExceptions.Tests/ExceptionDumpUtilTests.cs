using System;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading;
using NUnit.Framework;

namespace EasyExceptions.Tests
{
    public class ExceptionDumpUtilTests
    {
        private static void DoTest(string expectation, Func<Exception> action, CultureInfo uiCulture)
        {
            var savedCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = uiCulture;
            var savedUiCulture = Thread.CurrentThread.CurrentUICulture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;
            ExceptionDumpUtil.OmittedSourceDirectories = Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory);
            try
            {
                var ex = action();
                var dumped = ExceptionDumpUtil.Dump(ex);
                Assert.AreEqual(expectation.Replace("\r\n", "\n"), CutStackTraces(dumped).Replace("\r\n", "\n"));
            }
            finally
            {
                ExceptionDumpUtil.OmittedSourceDirectories = null;
                Thread.CurrentThread.CurrentUICulture = savedUiCulture;
                Thread.CurrentThread.CurrentCulture = savedCulture;
            }
        }

        private static string CutStackTraces(string dumped)
        {
            var resultBuilder = new StringBuilder();
            bool add = true;
            bool firstLine = true;
            foreach (var line in dumped.Split(new [] {Environment.NewLine}, StringSplitOptions.None))
            {
                if (add)
                {
                    if (!firstLine)
                        resultBuilder.AppendLine();
                    firstLine = false;
                    resultBuilder.Append(line);
                    if (line == "StackTrace: ``")
                    {
                        add = false;
                    }
                }
                else
                {
                    if (line == "``")
                    {
                        resultBuilder.AppendLine();
                        resultBuilder.Append(line);
                        add = true;
                    }
                }
            }
            return resultBuilder.ToString();
        }

        private static void DoTest(string expectation, Func<Exception> action)
        {
            DoTest(expectation, action, new CultureInfo("en-US"));
        }

        private const string JustExceptionExpectation =
@"Exception of type 'System.Exception' was thrown.

=== EXCEPTION #1/1: Exception
Message: Exception of type 'System.Exception' was thrown.
@PathFromRootException: root
@GetType().FullName: System.Exception
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void JustException()
        {
            DoTest(JustExceptionExpectation, () => new Exception());
        }

        private const string NestedExpectation =
@"Outer Exception. Inner exception.

=== EXCEPTION #1/2: Exception
Message: Inner exception.
@PathFromRootException: root.InnerException
@GetType().FullName: System.Exception
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #2/2: Exception
Message: Outer Exception.
@PathFromRootException: root
@GetType().FullName: System.Exception
InnerException: root.InnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void Nested()
        {
            DoTest(NestedExpectation, () => new Exception("Outer Exception.", new Exception("Inner exception.")));
        }

        private const string DictionaryExpectation =
@"Exception of type 'System.Exception' was thrown.

=== EXCEPTION #1/1: Exception
Message: Exception of type 'System.Exception' was thrown.
@PathFromRootException: root
@GetType().FullName: System.Exception
Data:
  foo: bar
  42: buz
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void Dictionary()
        {
            DoTest(DictionaryExpectation, () => new Exception { Data = { { "foo", "bar" }, { 42, "buz" } } });
        }

        private const string ListExpectation =
@"Exception of type 'EasyExceptions.Tests.ListException' was thrown.

=== EXCEPTION #1/1: ListException
Message: Exception of type 'EasyExceptions.Tests.ListException' was thrown.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.ListException
List:
- foo
- - bar
  - buz
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void List()
        {
            DoTest(ListExpectation, () => new ListException());
        }

        private const string AggregateExceptionExpectation =
@"One or more errors occurred. Exception of type 'System.Exception' was thrown. Value does not fall within the expected range.

=== EXCEPTION #1/3: ArgumentException
Message: Value does not fall within the expected range.
@PathFromRootException: root.InnerExceptions[1]
@GetType().FullName: System.ArgumentException
HResult: -2147024809
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #2/3: Exception
Message: Exception of type 'System.Exception' was thrown.
@PathFromRootException: root.InnerExceptions[0], root.InnerException
@GetType().FullName: System.Exception
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #3/3: AggregateException
Message: One or more errors occurred.
@PathFromRootException: root
@GetType().FullName: System.AggregateException
InnerExceptions[0]: root.InnerExceptions[0], root.InnerException
InnerExceptions[1]: root.InnerExceptions[1]
InnerExceptionCount: 2
InnerException: root.InnerExceptions[0], root.InnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void AggregateException()
        {
            DoTest(AggregateExceptionExpectation, () => new AggregateException(new Exception(), new ArgumentException()));
        }

        private const string FallingExceptionExpectation =
@"Exception of type 'EasyExceptions.Tests.FallingException' was thrown.

=== EXCEPTION #1/1: FallingException
Message: Exception of type 'EasyExceptions.Tests.FallingException' was thrown.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.FallingException
FallingProperty: Exception of type System.NotSupportedException was thrown
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void FallingException()
        {
            DoTest(FallingExceptionExpectation, () => new FallingException());
        }

        private const string DynamicExceptionExpectation =
@"Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.

=== EXCEPTION #1/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #2/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #3/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #4/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #5/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #6/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #7/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #8/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #9/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root.DynamicInnerException
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``

=== EXCEPTION #10/10: DynamicException
Message: Exception of type 'EasyExceptions.Tests.DynamicException' was thrown.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.DynamicException
DynamicInnerException: root.DynamicInnerException
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void DynamicException()
        {
            DoTest(DynamicExceptionExpectation, () => new DynamicException());
        }

        private const string ComplexPropertyExpectation =
@"Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.

=== EXCEPTION #1/1: ComplexPropertyException
Message: Validation failed for one or more entities. See 'EntityValidationErrors' property for more details.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.ComplexPropertyException
EntityValidationErrors:
- Entry:
    Entity:
      Id: 240b10f4-11dc-4e75-b268-da922fa6d781
    ValidationErrors:
    - PropertyName: Author
      ErrorMessage: The Author field is required.
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void ComplexProperty()
        {
            DoTest(ComplexPropertyExpectation, () => new ComplexPropertyException());
        }

        private const string WrongSerialPortExpectation =
            @"The port 'COM999' does not exist.

=== EXCEPTION #1/1: IOException
Message: The port 'COM999' does not exist.
@PathFromRootException: root
@GetType().FullName: System.IO.IOException
HResult: -2146232800
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void WrongSerialPort()
        {
            DoTest(WrongSerialPortExpectation, () =>
            {
                var serialPort = new SerialPort("COM999");
                try
                {
                    serialPort.Open();
                }
                catch (IOException exception)
                {
                    return exception;
                }
                Assert.Fail();
                return null;
            });
        }

        private const string ReferenceLoopExpectation =
            @"Exception of type 'EasyExceptions.Tests.ReferenceLoopObjectException' was thrown.

=== EXCEPTION #1/1: ReferenceLoopObjectException
Message: Exception of type 'EasyExceptions.Tests.ReferenceLoopObjectException' was thrown.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.ReferenceLoopObjectException
ReferenceLoopObject: &o0
  LoopedReference: *o0
  Text: Hello
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void ReferenceLoop()
        {
            DoTest(ReferenceLoopExpectation, () => new ReferenceLoopObjectException());
        }
        
        private const string ThrowingPropertyExpectation =
            @"Exception of type 'EasyExceptions.Tests.ThrowingPropertyException' was thrown.

=== EXCEPTION #1/1: ThrowingPropertyException
Message: Exception of type 'EasyExceptions.Tests.ThrowingPropertyException' was thrown.
@PathFromRootException: root
@GetType().FullName: EasyExceptions.Tests.ThrowingPropertyException
ThrowingProperty: Exception of type System.ObjectDisposedException was thrown
ObjectWithThrowingProperty:
  ThrowingProperty: Exception of type System.ObjectDisposedException was thrown
HResult: -2146233088
IsTransient: false
StackTrace: ``
``
";

        [Test]
        public void ThrowingProperty()
        {
            DoTest(ThrowingPropertyExpectation, () => new ThrowingPropertyException());
        }
    }
}
