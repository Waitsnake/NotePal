using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using LiteDB;

namespace NotesPal.Data
{
    public static class NoteDb
    {
        private static string FilePath => Path.Join(
                Services.Instance.PluginInterface.GetPluginConfigDirectory(),
                "NotesPal.db"
            );

        private static string ConnectionString => $"Filename={FilePath}; Connection=shared";

        public static bool Exists(string name, uint worldId)
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            return col.Exists(n => n.Id == NoteModel.GetId(name, worldId));
        }

        public static NoteModel Get(string name, uint worldId)
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            return col.FindOne(n => n.Id == NoteModel.GetId(name, worldId)) ?? new NoteModel { Name = name, WorldId = worldId };
        }

        public static void Upsert(NoteModel noteModel, bool updateTimestamp = true)
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            
            if (updateTimestamp)
                noteModel.LastModified = DateTime.UtcNow;

            if (Exists(noteModel.Name, noteModel.WorldId))
            {
                col.Update(noteModel);
            }
            else
            {
                col.Insert(noteModel);
            }
        }
		
		public static int Count()
		{			
			using var db = new LiteDatabase(ConnectionString);
			var col = db.GetCollection<NoteModel>("notes");
			return col.Count();
		}

        public static List<NoteModel> GetAll()
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            return col.FindAll().ToList();
        }

        public static void Delete(string name, uint worldId)
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            col.Delete(NoteModel.GetId(name, worldId));
        }

        public static void DeleteAll()
        {
            using var db = new LiteDatabase(ConnectionString);
            var col = db.GetCollection<NoteModel>("notes");
            col.DeleteAll();
        }
    }
}

