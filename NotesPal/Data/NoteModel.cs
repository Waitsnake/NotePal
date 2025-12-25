using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace NotesPal.Data
{
    public class NoteModel
    {
        public string Id => GetId(Name, WorldId);
        public required string Name { get; set; }
        public required uint WorldId { get; set; }
        public string? NoteText { get; set; }
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        public static string GetId(string name, uint worldId) 
            => string.Join(
                "",
                MD5.Create().ComputeHash(
                    Encoding.ASCII.GetBytes($"{name}@{worldId}")
                ).Select(b => $"{b:X2}")
            );
    }
}

