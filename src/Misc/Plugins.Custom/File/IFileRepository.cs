using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;

namespace Rhyous.WebFramework.Services
{
    public interface IFileRepository : IRepository<File, IFile, Guid>
    {
        bool CheckFileExists(string path);
        List<IFile> ListFiles(string path);
    }
}
