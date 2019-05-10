using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using EasyExceptions.WritingRules;
using System.Threading.Tasks;
using System.Reflection;
using EasyExceptions.Utils.Io;

namespace EasyExceptions
{
    public static class ExceptionDumpUtil
    {
        public const string ServiceDataPrefix = "12F1CADC21A842EFAD20BF81F4B6117A";

        public static string Dump(Exception exception)
        {
            var resultBuilder = new StringBuilder();

            var dumpingException = DumpExceptionOnCurrentThread(exception, resultBuilder);

            if (dumpingException == null)
                return RemoveOmittedSourceDirectories(resultBuilder.ToString());

            var failedResultBuilder = new StringBuilder();
            failedResultBuilder.AppendLine("Exception was thrown while dumping exception.");
            failedResultBuilder.AppendLine();

            failedResultBuilder.AppendLine("Dumped exception:");
            failedResultBuilder.AppendLine(exception.ToString());
            failedResultBuilder.AppendLine();

            failedResultBuilder.AppendLine("Exception that was occured while dumping:");
            failedResultBuilder.AppendLine(dumpingException);
            failedResultBuilder.AppendLine();

            return RemoveOmittedSourceDirectories(failedResultBuilder.ToString());
        }

        private static string DumpExceptionOnCurrentThread(Exception exception, StringBuilder resultBuilder)
        {
            try
            {
                BuildAllExceptionsDump(resultBuilder, exception);
                return null;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public static string OmittedSourceDirectories { get; set; }

        private static string RemoveOmittedSourceDirectories(string exceptionText)
        {
            if (OmittedSourceDirectories == null)
                return exceptionText;

            var result = exceptionText;
            foreach (var sourceDirectoryToOmit in OmittedSourceDirectories.Split(';'))
            {
                result = result.Replace(" in " + sourceDirectoryToOmit + PathUtils.DirectorySeparatorChar, " in ");
            }
            return result;
        }

        private static void BuildAllExceptionsDump(StringBuilder resultBuilder, Exception exception)
        {
            var allExceptions = new List<ExceptionInfo>();
            FindAll(exception, allExceptions, "root");

            var exceptionMessages = allExceptions.Select(_ => _.Exception.Message).ToList();
            for (int i = 0; i < exceptionMessages.Count;)
            {
                var candidateToRemove = exceptionMessages[i];
                bool removeCurrentCandidate = false;
                for (int j = 0; j < i; j++)
                {
                    if (exceptionMessages[j].Contains(candidateToRemove))
                    {
                        removeCurrentCandidate = true;
                        break;
                    }
                }

                if (removeCurrentCandidate)
                {
                    exceptionMessages.RemoveAt(i);
                    continue;
                }

                i++;
            }
            resultBuilder.AppendLine(string.Join(" ", exceptionMessages));

            allExceptions.Reverse();

            for (int i = 0; i < allExceptions.Count; i++)
            {
                allExceptions[i].Exception.Data[ServiceDataPrefix + " ExceptionNumber"] = i + 1;
                allExceptions[i].Exception.Data[ServiceDataPrefix + " AllExceptionsCount"] = allExceptions.Count;
            }

            foreach (var exceptionInfo in allExceptions)
            {
                resultBuilder.AppendLine();
                BuildSingleExceptionDump(resultBuilder, exceptionInfo);
            }
        }

        private static void BuildSingleExceptionDump(StringBuilder resultBuilder, ExceptionInfo exceptionInfo)
        {
            resultBuilder.AppendFormat("=== EXCEPTION #{0}/{1}: {2}",
                exceptionInfo.Exception.Data[ServiceDataPrefix + " ExceptionNumber"],
                exceptionInfo.Exception.Data[ServiceDataPrefix + " AllExceptionsCount"],
                exceptionInfo.Exception.GetType().Name);
            resultBuilder.AppendLine();

            var writingRules = new List<IWritingRule>
            {
                new MessageRule(),
                new CalculatedPropertiesRule(),
                new ExcludeStackTraceRelatedPropertiesRule(),
                new ExcludeIPForWatsonBucketsPropertyRule(),
                new RegularPropertiesRule(),
                new StackTraceRule()
            };

            foreach (var writingRule in writingRules)
            {
                writingRule.Apply(resultBuilder, exceptionInfo.Exception, exceptionInfo.Properties);
            }
        }

        private static void FindAll(Exception exception, IList<ExceptionInfo> accumulatedExceptions, string path)
        {
            if (accumulatedExceptions.Any(_ => ReferenceEquals(_.Exception, exception)))
            {
                exception.Data[ServiceDataPrefix + " PathFromRootException"] += ", " + path;
                return;
            }

            exception.Data[ServiceDataPrefix + " PathFromRootException"] = path;

            if (accumulatedExceptions.Count >= 10)
                return;
            var exceptionInfo = new ExceptionInfo(exception);
            accumulatedExceptions.Add(exceptionInfo);

            foreach (var propertyInfo in exception.GetType().GetRuntimeProperties())
            {
                object value;
                string name = propertyInfo.Name;
                try
                {
                    value = propertyInfo.GetValue(exception, new object[0]);
                }
                catch (Exception ex)
                {
                    value = $"Exception of type {ex.GetType().FullName} was thrown while getting value.";
                }

                var innerException = value as Exception;
                if (innerException != null)
                {
                    FindAll(innerException, accumulatedExceptions, path + "." + propertyInfo.Name);
                }

                var enumerable = value as IEnumerable;
                if (enumerable != null && !(enumerable is string))
                {
                    var i = 0;
                    foreach (var item in enumerable)
                    {
                        var exceptionItem = item as Exception;
                        if (exceptionItem != null)
                        {
                            FindAll(exceptionItem, accumulatedExceptions, path + "." + propertyInfo.Name + "[" + i + "]");
                        }
                        i++;
                    }
                }

                exceptionInfo.Properties[name] = value;
            }
        }

        private class ExceptionInfo
        {
            public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();

            public Exception Exception { get; }

            public ExceptionInfo(Exception exception)
            {
                Exception = exception;
            }
        }
    }
}