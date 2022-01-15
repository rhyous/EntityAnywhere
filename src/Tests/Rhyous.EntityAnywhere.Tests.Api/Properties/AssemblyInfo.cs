using Microsoft.VisualStudio.TestTools.UnitTesting;

[assembly: TestCategory("Api")]
[assembly: Parallelize(Workers = 4, Scope = ExecutionScope.ClassLevel)]