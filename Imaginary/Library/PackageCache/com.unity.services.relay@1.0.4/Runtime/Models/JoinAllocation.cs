//-----------------------------------------------------------------------------
// <auto-generated>
//     This file was generated by the C# SDK Code Generator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//-----------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Unity.Services.Relay.Http;



namespace Unity.Services.Relay.Models
{
    /// <summary>
    /// An allocation created via a join code.
    /// </summary>
    [Preserve]
    [DataContract(Name = "JoinAllocation")]
    public class JoinAllocation
    {
        /// <summary>
        /// An allocation created via a join code.
        /// </summary>
        /// <param name="allocationId">The unique ID of the allocation.</param>
        /// <param name="serverEndpoints">Connection endpoints for the assigned Relay server.</param>
        /// <param name="relayServer">relayServer param</param>
        /// <param name="key">A base64-encoded key required for the HMAC signature of the &#x60;BIND&#x60; message.</param>
        /// <param name="connectionData">A base64-encoded representation of an encrypted connection data blob describing this allocation. This data is equired for establishing communication with other players.</param>
        /// <param name="allocationIdBytes">A base64-encoded form of the allocation ID. When decoded, this is the exact expected byte alignment to use when crafting Relay protocol messages that require the allocation ID. For example, &#x60;PING&#x60;, &#x60;CONNECT&#x60;, &#x60;RELAY&#x60;, and &#x60;CLOSE&#x60; message types.</param>
        /// <param name="region">The allocation region.</param>
        /// <param name="hostConnectionData">A base64-encoded representation of an encrypted connection data blob describing the allocation and Relay server of the player who created the join code. Connecting players can use this data to establish communication with the host player.</param>
        [Preserve]
        public JoinAllocation(System.Guid allocationId, List<RelayServerEndpoint> serverEndpoints, RelayServer relayServer, byte[] key, byte[] connectionData, byte[] allocationIdBytes, string region, byte[] hostConnectionData)
        {
            AllocationId = allocationId;
            ServerEndpoints = serverEndpoints;
            RelayServer = relayServer;
            Key = key;
            ConnectionData = connectionData;
            AllocationIdBytes = allocationIdBytes;
            Region = region;
            HostConnectionData = hostConnectionData;
        }

        /// <summary>
        /// The unique ID of the allocation.
        /// </summary>
        [Preserve]
        [DataMember(Name = "allocationId", IsRequired = true, EmitDefaultValue = true)]
        public System.Guid AllocationId{ get; }
        /// <summary>
        /// Connection endpoints for the assigned Relay server.
        /// </summary>
        [Preserve]
        [DataMember(Name = "serverEndpoints", IsRequired = true, EmitDefaultValue = true)]
        public List<RelayServerEndpoint> ServerEndpoints{ get; }
        /// <summary>
        /// 
        /// </summary>
        [Preserve]
        [DataMember(Name = "relayServer", IsRequired = true, EmitDefaultValue = true)]
        public RelayServer RelayServer{ get; }
        /// <summary>
        /// A base64-encoded key required for the HMAC signature of the &#x60;BIND&#x60; message.
        /// </summary>
        [Preserve]
        [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = true)]
        public byte[] Key{ get; }
        /// <summary>
        /// A base64-encoded representation of an encrypted connection data blob describing this allocation. This data is equired for establishing communication with other players.
        /// </summary>
        [Preserve]
        [DataMember(Name = "connectionData", IsRequired = true, EmitDefaultValue = true)]
        public byte[] ConnectionData{ get; }
        /// <summary>
        /// A base64-encoded form of the allocation ID. When decoded, this is the exact expected byte alignment to use when crafting Relay protocol messages that require the allocation ID. For example, &#x60;PING&#x60;, &#x60;CONNECT&#x60;, &#x60;RELAY&#x60;, and &#x60;CLOSE&#x60; message types.
        /// </summary>
        [Preserve]
        [DataMember(Name = "allocationIdBytes", IsRequired = true, EmitDefaultValue = true)]
        public byte[] AllocationIdBytes{ get; }
        /// <summary>
        /// The allocation region.
        /// </summary>
        [Preserve]
        [DataMember(Name = "region", IsRequired = true, EmitDefaultValue = true)]
        public string Region{ get; }
        /// <summary>
        /// A base64-encoded representation of an encrypted connection data blob describing the allocation and Relay server of the player who created the join code. Connecting players can use this data to establish communication with the host player.
        /// </summary>
        [Preserve]
        [DataMember(Name = "hostConnectionData", IsRequired = true, EmitDefaultValue = true)]
        public byte[] HostConnectionData{ get; }
    
    }
}
