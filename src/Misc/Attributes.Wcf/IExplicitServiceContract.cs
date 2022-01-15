using System;

namespace Rhyous.EntityAnywhere.Attributes
{
    /// <summary>
    /// WCF has a limitation in allowing only one service contract.
    /// The WebService generic code fixes that for you, however, it guesses
    /// on which Service Contract to consolidate to. If you want to explicitly
    /// define the service contract to consolidate to, use this attribute.
    /// </summary>
    public interface IExplicitServiceContract
    {
        /// <summary>
        /// The type of the ServiceContract. Only one ServiceContract can be used.
        /// If a web service inherits multiple interfaces that are decorated with
        /// the ServiceContractAttribute, perhaps due to WebService inheritance,
        /// the service contract to use can be specified. If a service contract
        /// is not specified, once is guess upon using inheritane.
        /// </summary>
        Type ServiceContract { get; set; }
    }
}