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
            try
            {
                var legacyId = NoteModel.GetId(name, worldId);

                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");

                return col.Exists(n => n.LegacyId == legacyId);
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.Exists failed");
                return false; // fallback = false
            }
        }

        public static NoteModel Get(string name, uint worldId)
        {
            try
            {
                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");
                var legacyId = NoteModel.GetId(name, worldId);
                var note = col.FindOne(n => n.LegacyId == legacyId);
                return note ?? new NoteModel { Name = name, WorldId = worldId };
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.Get failed");
                return new NoteModel { Name = name, WorldId = worldId }; // fallback = empty NoteModel
            }
        }

        public static void Upsert(NoteModel noteModel, bool updateTimestamp = true)
        {
            try
            {
                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");
                
                if (updateTimestamp)
                    noteModel.LastModified = DateTime.UtcNow;

                col.Upsert(noteModel);
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.Upsert failed");
            }
        }
		
		public static int Count()
		{
            try
            {
                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");
                return col.Count();
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.Count failed");
                return 0; // fallback = 0
            }
		}

        public static List<NoteModel> GetAll()
        {
            try
            {
                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");
                return col.FindAll().ToList();
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.GetAll failed");
                return new List<NoteModel>(); // fallback = empty list
            }
        }

        public static void Delete(string name, uint worldId)
        {
            try
            {
                var legacyId = NoteModel.GetId(name, worldId);

                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");

                var note = col.FindOne(n => n.LegacyId == legacyId);
                if (note != null)
                    col.Delete(note.DbId);
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.Delete failed");
            }
        }

        public static void DeleteAll()
        {
            try
            {
                using var db = new LiteDatabase(ConnectionString);
                var col = db.GetCollection<NoteModel>("notes");
                col.DeleteAll();
            }
            catch (Exception ex)
            {
                Services.Instance.PluginLog.Error(ex, "NoteDb.DeleteAll failed");
            }
        }
    }
}

