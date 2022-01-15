using Rhyous.WebFramework.Entities;
using Rhyous.WebFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rhyous.WebFramework.Services
{
    public interface IFileService : IServiceCommon<File, IFile, Guid>
    {
        Task<IFile> GetFileByOrg(string orgId, string fileId);
        Task<IFile> GetFileByOrgAndProduct(string orgId, string productId, string fileId);
        Task<IFile> GetFileByOrgOrder(string orgId, string orderId, string fileId);
        Task<IFile> GetFileByOrgOrderAndProduct(string orgId, string orderId, string productId, string fileId);
        Task<IFile> GetFileByProduct(string productId, string fileId);
        Task<List<IFile>> GetFilesByOrg(string orgId);
        Task<List<IFile>> GetFilesByOrgAndProduct(string orgId, string productId);
        Task<List<IFile>> GetFilesByOrgOrder(string orgId, string orderId);
        Task<List<IFile>> GetFilesByOrgOrderAndProduct(string orgId, string orderId, string productId);
        Task<List<IFile>> GetFilesByProduct(string productId);
        Task<IFile> GetFilesZipped(string productId);
        Task<List<IFile>> PostFilesByOrg(string orgId, List<IFile> files);
        Task<List<IFile>> PostFilesByOrgAndProduct(string orgId, string productId, List<IFile> files);
        Task<List<IFile>> PostFilesByOrgOrder(string orgId, string orderId, List<IFile> files);
        Task<List<IFile>> PostFilesByOrgOrderAndProduct(string orgId, string orderId, string productId, List<IFile> files);
        Task<List<IFile>> PostFilesByOrgOrderProductAndRelease(string orgId, string orderId, string productId, string version, List<IFile> files);
        Task<List<IFile>> PostFilesByProduct(string productId, List<IFile> files);
        Task<bool> CheckFileExistsByProduct(string fileName, string productId);
        Task<bool> CheckFileExistsByOrg(string fileName, string orgId);
        Task<bool> CheckFileExistsByOrgOrder(string fileName, string orgId, string orderId);
        Task<bool> CheckFileExistsByOrgOrderAndProduct(string fileName, string orgId, string orderId, string productId);
        Task<bool> CheckFileExistsByOrgAndProduct(string fileName, string orgId, string prodId);

        Task<bool> CheckFileExistsByOrgOrderProductAndRelease(string fileName, string orgId, string orderId, string productId, string version);
    }
}